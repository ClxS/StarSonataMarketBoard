namespace SSMB.Server.API.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Items.Models;
    using Application.Items.Queries.GetBulkAppraisal;
    using Application.Items.Queries.GetFullItemDetails;
    using Application.Items.Queries.GetHotItems;
    using Application.Items.Queries.GetItems;
    using Application.Items.Queries.GetProfitableItems;
    using Application.Items.Queries.GetRecentItems;
    using Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class ItemsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> Index([FromQuery] string filter, [FromQuery] int count = 20)
        {
            return await this.mediator.Send(new GetItemsQuery { Filter = filter, Count = count });
        }

        [HttpGet]
        public async Task<FullDetailItem> Detail([FromQuery] int id)
        {
            return await this.mediator.Send(new GetFullItemDetailsQuery { ItemId = id });
        }

        [HttpGet]
        public async Task<IEnumerable<RecentItem>> Recent()
        {
            return await this.mediator.Send(new GetRecentItemsQuery());
        }

        [HttpGet]
        public async Task<IEnumerable<Item>> Hot()
        {
            return await this.mediator.Send(new GetHotItemsQuery());
        }

        [HttpGet]
        public async Task<IEnumerable<ItemProfit>> Profitable()
        {
            return await this.mediator.Send(new GetProfitableItemsQuery());
        }

        [HttpGet]
        public async Task<ItemAppraisal[]> Appraise((string Name, int Count)[] items)
        {
            return await this.mediator.Send(new GetBulkAppraisalQuery()
            {
                Items = items
            });
        }

        [HttpGet]
        public async Task<ItemRecommendedPrice[]> UnderCut(string[] items)
        {
            return await this.mediator.Send(new GetBulkUnderCutQuery()
            {
                Items = items
            });
        }

        [HttpGet]
        public async Task<Scrapable[]> Scrapable(double factor)
        {
            return await this.mediator.Send(new GetScrapableQuery()
            {
                Factor = factor
            });
        }
    }
}
