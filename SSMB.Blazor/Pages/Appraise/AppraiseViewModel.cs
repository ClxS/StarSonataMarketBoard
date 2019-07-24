namespace SSMB.Blazor.Pages.Appraise
{
    using System;
    using System.Collections.Generic;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using ViewServices;

    public class AppraiseViewModel : IAppraiseViewModel
    {
        private readonly IItemsService itemsService;
        public string AppraiseText { get; set; }

        private readonly Subject<Unit> appraisalCompleted = new Subject<Unit>();
        private ItemAppraisal[] appraisals;
        public IObservable<Unit> WhenAppraisalCompleted => this.appraisalCompleted.AsObservable();

        public ItemAppraisal[] Appraisals
        {
            get => this.appraisals;
            set
            {
                this.appraisals = value; 
                this.appraisalCompleted.OnNext(Unit.Default);
            }
        }

        public AppraiseViewModel(IItemsService itemsService)
        {
            this.itemsService = itemsService;
            this.AppraiseText = @"";
        }

        public void OnAppraiseClick()
        {
            var text = this.AppraiseText.Replace("\r", string.Empty);
            var lines = text.Split("\n");
            var items = new List<(string name, int count)>();
            foreach (var line in lines)
            {
                try
                {
                    var entry = line.Split("	");
                    var count = int.Parse(entry[0]);
                    var name = entry[1];
                    items.Add((name, count));
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            _ = Task.Run(async () => { this.Appraisals = await this.itemsService.GetAppraisal(items.ToArray()); });
        }
    }
}
