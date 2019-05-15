namespace SSMB.Blazor.Pages.CreateAlert
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using System.Reactive.Subjects;
    using System.Security.Claims;
    using Application.Items.Models;
    using Domain;
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Http;
    using ViewServices;

    class CreateAlertViewModel : ICreateAlertViewModel
    {
        private readonly IAlertsService alertsService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUriHelper uriHelper;
        private readonly Subject<FullDetailItem> itemDetailsSubject;

        private readonly IItemsService itemsService;

        private List<AlertCondition> conditions = new List<AlertCondition>();

        private FullDetailItem itemDetail;

        private int? itemId;

        public CreateAlertViewModel(
            IItemsService itemsService,
            IAlertsService alertsService,
            IHttpContextAccessor httpContextAccessor,
            IUriHelper uriHelper)
        {
            this.itemsService = itemsService;
            this.alertsService = alertsService;
            this.httpContextAccessor = httpContextAccessor;
            this.uriHelper = uriHelper;
            this.itemDetailsSubject = new Subject<FullDetailItem>();
        }

        public string AlertName { get; set; }

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

        public void AddAlertClicked()
        {
            if (!this.ItemId.HasValue)
            {
                return;
            }

            var identity = this.httpContextAccessor?.HttpContext?.User?.Identity as ClaimsIdentity;
            if (identity?.IsAuthenticated ?? false)
            {
                var id = identity.Claims.FirstOrDefault(c => c.Type.EndsWith("nameidentifier"));
                if (long.TryParse(id?.Value ?? "-", out var idValue))
                {
                    this.alertsService.CreateAlert(idValue, this.ItemId.Value, this.AlertName, this.Conditions)
                        .ContinueWith(
                            t =>
                            {
                                if (t.IsCompletedSuccessfully)
                                {
                                    this.uriHelper.NavigateTo($"Item?id={this.ItemId}&showAlerts=1");
                                }
                            });
                }
            }
        }

        public void AddConditionClicked()
        {
            this.conditions.Add(new AlertCondition());
        }

        public void RemoveCondition(int conditionIndex)
        {
            this.conditions.RemoveAt(conditionIndex);
        }
    }
}
