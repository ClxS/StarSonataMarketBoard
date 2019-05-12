namespace SSMB.Blazor.Pages.CreateAlert
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using Application.Items.Models;
    using Models;
    using ViewServices;

    class CreateAlertViewModel : ICreateAlertViewModel
    {
        private readonly Subject<FullDetailItem> itemDetailsSubject;

        private readonly IItemsService itemsService;

        private List<AlertCondition> conditions = new List<AlertCondition>();

        private FullDetailItem itemDetail;

        private int? itemId;

        public CreateAlertViewModel(IItemsService itemsService)
        {
            this.itemsService = itemsService;
            this.itemDetailsSubject = new Subject<FullDetailItem>();
        }

        public IList<AlertCondition> Conditions => this.conditions;

        public FullDetailItem ItemDetail
        {
            get => this.itemDetail;
            set
            {
                this.itemDetail = value;
                this.itemDetailsSubject.OnNext(value);
            }
        }

        public int? ItemId
        {
            get => this.itemId;
            set
            {
                this.itemId = value;
                if (value.HasValue)
                {
                    this.itemsService.GetItemDetails(value.Value).ContinueWith(t => { this.ItemDetail = t.Result; });
                }
            }
        }

        public IObservable<FullDetailItem> WhenItemDetailsUpdated => this.itemDetailsSubject.AsObservable();

        public void AddConditionClicked()
        {
            this.conditions.Add(new AlertCondition());
        }

        public void AddAlertClicked()
        {
        }

        public void RemoveCondition(int conditionIndex)
        {
            this.conditions.RemoveAt(conditionIndex);
        }
    }
}
