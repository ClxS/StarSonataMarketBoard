namespace StarSonata.API
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading;
    using System.Threading.Tasks;
    using Communication;
    using Messages;
    using Messages.Incoming;
    using Messages.Outgoing;
    using Objects;

    public class StarSonataApi : IDisposable
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly ConcurrentQueue<ICommand> commandQueue =
            new ConcurrentQueue<ICommand>();

        private readonly Subject<Unit> connectedSubject;

        private readonly Subject<Unit> disconnectedSubject;

        private readonly Subject<IIncomingMessage> messageReceivedSubject;

        private Dictionary<byte, Func<byte[], IIncomingMessage>> registeredMessageTypes;

        public StarSonataApi(string apiUrl)
        {
            this.SetServerUrl(apiUrl);
            this.messageReceivedSubject = new Subject<IIncomingMessage>();
            this.connectedSubject = new Subject<Unit>();
            this.disconnectedSubject = new Subject<Unit>();
            this.WhenMessageReceived = this.messageReceivedSubject.AsObservable();
            var token = this.cancellationTokenSource.Token;
            Task.Run(async () =>
            {
                this.Initialise();
                while (!token.IsCancellationRequested)
                {
                    if (!this.commandQueue.TryDequeue(out var message))
                    {
                        await Task.Delay(100);
                        continue;
                    }

                    await message.Do(StarSonataCommClient.Client);
                }
            });
        }

        private interface ICommand
        {
            Task Do(StarSonataCommClient commClient);
        }

        public bool HasStarted { get; private set; }

        public bool IsConnected => StarSonataCommClient.Client?.Socket?.Connected ?? false;

        public IObservable<Unit> WhenConnected => this.connectedSubject.AsObservable();

        public IObservable<Unit> WhenDisconnected => this.disconnectedSubject.AsObservable();

        public IObservable<IIncomingMessage> WhenMessageReceived { get; }

        public void Connect()
        {
            while (true)
            {
                try
                {
                    StarSonataCommClient.Client.Connect();
                    break;
                }
                catch (Exception)
                {
                    Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                }
            }

            this.connectedSubject.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            this.messageReceivedSubject?.Dispose();
            this.cancellationTokenSource.Dispose();
        }

        public void SendAsync(IOutgoingMessage message)
        {
            this.commandQueue.Enqueue(new OutgoingCommand(message));
        }

        public void SendChatAsync(string message, MessageChannel channel)
        {
            var command = new ChatCommand(message, channel);
            this.commandQueue.Enqueue(command);
        }

        public Task TryLoginAsync(string username, string password)
        {
            var command = new LoginCommand(username, password);
            this.commandQueue.Enqueue(command);
            return command.Task;
        }

        private static MessageChannel GetChannelForString(string channel)
        {
            if (channel == MessageChannel.All.ToString())
            {
                return MessageChannel.All;
            }

            if (channel == MessageChannel.Trade.ToString())
            {
                return MessageChannel.Trade;
            }

            return MessageChannel.Event;
        }

        private void AddDefaultSubscriptions()
        {
            // Received a ping, send a pong
            this.WhenMessageReceived.Where(msg => msg is Ping).Subscribe(
                msg =>
                {
                    var ping = (Ping)msg;
                    StarSonataCommClient.Client.SendMessage(new Pong(ping.Sec, ping.USec));
                });

            // Login as the first available character
            this.WhenMessageReceived.Where(msg => msg is CharacterList).Subscribe(
                msg =>
                {
                    var characterList = (CharacterList)msg;
                    StarSonataCommClient.Client.SendMessage(new SelectCharacter(characterList.Characters.First()));
                });
        }

        private void Initialise()
        {
            if (this.HasStarted)
            {
                throw new Exception("Start has already been called.");
            }

            this.HasStarted = true;
            this.registeredMessageTypes = new Dictionary<byte, Func<byte[], IIncomingMessage>>();

            this.AddDefaultSubscriptions();

            this.registeredMessageTypes[MessageConstants.SC_characterlist] = bytes => new CharacterList(bytes);
            this.registeredMessageTypes[MessageConstants.SC_ping] = bytes => new Ping(bytes);
            this.registeredMessageTypes[MessageConstants.SC_textmessage] = bytes => new TextMessage(bytes);
            this.registeredMessageTypes[MessageConstants.SC_loginfail] = bytes => new LoginFail(bytes);
            this.registeredMessageTypes[MessageConstants.SC_disconnect] = bytes => new Disconnect();

            StarSonataCommClient.Client.WhenDataReceived.Subscribe(
                e =>
                {
                    if (e.Bytes.Length <= 3)
                    {
                        return;
                    }

                    if (!this.registeredMessageTypes.ContainsKey(e.Bytes[2]))
                    {
                        return;
                    }

                    var constructor = this.registeredMessageTypes[e.Bytes[2]];
                    var msg = constructor(e.Bytes.Skip(3).ToArray());
                    this.messageReceivedSubject.OnNext(msg);
                });
        }

        private void SetServerUrl(string serverUrl)
        {
            StarSonataCommClient.ServerUrl = serverUrl;
        }

        private class ChatCommand : ICommand
        {
            public ChatCommand(string message, MessageChannel channel)
            {
                this.Message = message;
                this.Channel = channel;
            }

            private MessageChannel Channel { get; }

            private string Message { get; }

            public Task Do(StarSonataCommClient commClient)
            {
                StarSonataCommClient.Client.SendMessage(new TextMessageOut(new ChatMessage
                    { Channel = this.Channel, Message = this.Message }));
                return Task.CompletedTask;
            }
        }

        private class LoginCommand : ICommand
        {
            private readonly TaskCompletionSource<object> taskCompletionSource;

            public LoginCommand(string username, string password)
            {
                this.Username = username;
                this.Password = password;
                this.taskCompletionSource = new TaskCompletionSource<object>();
            }

            public Task Task => this.taskCompletionSource.Task;

            private string Password { get; }

            private string Username { get; }

            public async Task Do(StarSonataCommClient commClient)
            {
                StarSonataCommClient.Client.SendMessage(
                    new ChatClientLogin(new User
                        { Username = this.Username, Password = this.Password }));
                this.taskCompletionSource.SetResult(null);
            }
        }

        private class OutgoingCommand : ICommand
        {
            private readonly IOutgoingMessage message;

            public OutgoingCommand(IOutgoingMessage message)
            {
                this.message = message;
            }

            public Task Do(StarSonataCommClient commClient)
            {
                StarSonataCommClient.Client.SendMessage(this.message);
                return Task.CompletedTask;
            }
        }
    }
}
