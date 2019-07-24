namespace SSMB.DataCollection
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Model;
    using StarSonata.API;
    using StarSonata.API.Messages.Incoming;

    public interface IMarketCheckService
    {
        Task<OrderEntry[]> RequestMarketCheck(string itemName);

        Task<(string description, long scrapValue, OrderEntry[] orders)> RequestMarketCheckWithDescription(
            string itemName);
    }

    internal class MarketCheckService : IDisposable, IMarketCheckService
    {
        private readonly StarSonataApi api;

        private readonly ConcurrentQueue<TextMessage> incomingMessages;

        private readonly ConcurrentQueue<MarketRequest> pendingRequests;
        private bool shouldExitMcTask;

        public MarketCheckService(StarSonataApi api, ILoginCredentials credentials)
        {
            this.api = api;
            this.pendingRequests = new ConcurrentQueue<MarketRequest>();
            this.incomingMessages = new ConcurrentQueue<TextMessage>();
            this.shouldExitMcTask = false;
            this.api.WhenConnected.Subscribe(_ => { this.TryGameLoginAsync(credentials.Username, credentials.Password); });
            this.api.WhenDisconnected.Subscribe(_ => { this.api.Connect(); });
            this.api.WhenMessageReceived.Where(msg => msg is Disconnect).Subscribe(msg =>
            {
                this.TryGameLoginAsync(credentials.Username, credentials.Password);
            });
            this.api.WhenMessageReceived.Where(msg => msg is TextMessage).Subscribe(msg =>
            {
                this.incomingMessages.Enqueue((TextMessage)msg);
            });

            this.api.Connect();
            Task.Run(this.AnalyseRequests);
        }

        public MarketCheckService(StarSonataApi api, ILoginCredentials credentials, CancellationToken token)
            : this(api, credentials)
        {
            token.Register(() => { this.shouldExitMcTask = true; });
        }

        private enum EAppState
        {
            WaitingForLogin,

            ManuallyStopped,

            LoginFailed,

            Ready,
        }

        public TimeSpan Throttle { get; } = TimeSpan.FromSeconds(0.8);

        private EAppState AppState { get; set; } = EAppState.WaitingForLogin;

        public void Dispose()
        {
            this.shouldExitMcTask = true;
        }

        public Task<OrderEntry[]> RequestMarketCheck(string itemName)
        {
            var request = new MarketRequest(itemName, false);
            this.pendingRequests.Enqueue(request);
            return Task.Run(() =>
            {
                var doneEvent = new ManualResetEvent(false);
                OrderEntry[] data = null;
                using var _ = request.UpdateSubject.Subscribe(md =>
                {
                    data = md.orders;
                    doneEvent.Set();
                });
                doneEvent.WaitOne();
                Debug.Assert(data != null, "Market data cannot be null");
                return data;
            });
        }

        public Task<(string description, long scrapValue, OrderEntry[] orders)> RequestMarketCheckWithDescription(
            string itemName)
        {
            var request = new MarketRequest(itemName, true);
            this.pendingRequests.Enqueue(request);
            return Task.Run(() =>
            {
                var doneEvent = new ManualResetEvent(false);
                (string description, long scrapValue, OrderEntry[] orders)? data = null;
                using var subscribe = request.UpdateSubject.Subscribe(md =>
                {
                    data = md;
                    doneEvent.Set();
                });
                doneEvent.WaitOne();
                Debug.Assert(data.HasValue, "Market data cannot be null");
                return data.Value;
            });
        }

        private async Task AnalyseRequests()
        {
            while (!this.shouldExitMcTask)
            {
                if (!this.api.IsConnected || !this.pendingRequests.TryDequeue(out var request))
                {
                    this.EmptyIncoming();
                    await Task.Delay(500).ConfigureAwait(true);
                    continue;
                }

                Console.WriteLine($"Sending Request for {request.ItemName}");
                this.api.SendChatAsync($"/mc {request.ItemName}", StarSonata.API.Objects.MessageChannel.Event);

                var sw = new Stopwatch();
                MarketDataState? foundState = null;
                TextMessage msg = null;
                sw.Start();
                while (sw.ElapsedMilliseconds < 10000)
                {
                    if (!this.incomingMessages.TryDequeue(out msg))
                    {
                        continue;
                    }

                    // More important than it looks, wouldn't want the bot to be able to be
                    // pm-DOS'd!
                    if (msg.Message.Channel != StarSonata.API.Objects.MessageChannel.Event)
                    {
                        continue;
                    }

                    if (msg.Message.Message.IndexOf($"'{request.ItemName}' is not a valid item name.",
                            StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        foundState = MarketDataState.ItemDoesNotExist;
                        break;
                    }

                    if (Regex.IsMatch(msg.Message.Message, $"{request.ItemName.Replace("+", "\\+")} was not found.*"))
                    {
                        foundState = MarketDataState.NotOnMarket;
                        break;
                    }

                    if (Regex.IsMatch(msg.Message.Message, $"{request.ItemName.Replace("+", "\\+")}(.|\\n)*Average selling price(.|\\n)*"))
                    {
                        foundState = MarketDataState.Available;
                        break;
                    }
                }

                if (foundState.HasValue)
                {
                    Console.WriteLine($"{request.ItemName} found");
                }
                else
                {
                    Console.WriteLine($"Timeout {request.ItemName}");
                }

                request.UpdateSubject.OnNext((
                    foundState.HasValue && foundState == MarketDataState.Available
                        ? request.ShouldGetDescription
                            ? MarketDataStringExtractUtility.DescriptionFromMarketCheckString(
                                msg.Message.Message)
                            : null
                        : null,
                    foundState.HasValue && foundState == MarketDataState.Available
                        ? request.ShouldGetDescription
                            ? MarketDataStringExtractUtility.ScrapValueFromMarketCheckString(
                                msg.Message.Message)
                            : -1
                        : -1,
                    foundState.HasValue && foundState == MarketDataState.Available
                        ? MarketDataStringExtractUtility.OrderDataFromMarketCheckString(msg.Message.Message)
                        : new OrderEntry[0]));
                request.UpdateSubject.OnCompleted();

                //await Task.Delay(this.Throttle).ConfigureAwait(true);
            }
        }

        private void EmptyIncoming()
        {
            while (this.incomingMessages.TryDequeue(out _))
            {
            }
        }

        private Task TryGameLoginAsync(string username, string password)
        {
            return Task.Run(
                async () =>
                {
                    for (var i = 0; i < 5; ++i)
                    {
                        if (this.AppState == EAppState.ManuallyStopped)
                        {
                            return;
                        }

                        this.AppState = EAppState.WaitingForLogin;

                        try
                        {
                            this.api.TryLoginAsync(username, password).Wait();
                            this.AppState = EAppState.Ready;
                            break;
                        }
                        catch (Exception)
                        {
                            // ignored
                            this.AppState = EAppState.LoginFailed;
                            await Task.Delay(1000);
                        }
                    }
                });
        }

        private class MarketRequest
        {
            public MarketRequest(string itemName, bool shouldGetDescription)
            {
                this.ItemName = itemName;
                this.ShouldGetDescription = shouldGetDescription;
                this.UpdateSubject = new Subject<(string, long, OrderEntry[])>();
            }

            public string ItemName { get; }

            public bool ShouldGetDescription { get; }

            public Subject<(string description, long scrapValue, OrderEntry[] orders)> UpdateSubject { get; }
        }
    }
}
