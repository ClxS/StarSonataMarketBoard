namespace SSMB.Server.API.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Application.Interfaces;
    using Application.Items.Models;
    using Application.Items.Queries.GetFullItemDetails;
    using Application.Items.Queries.GetHotItems;
    using Application.Items.Queries.GetItems;
    using Application.Items.Queries.GetRecentItems;
    using Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class ItemsController : ApiController
    {
        private readonly IMediator mediator;

        public ItemsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [System.Web.Http.HttpGet]
        public async Task<IEnumerable<Item>> Index([FromQuery] string filter, [FromQuery] int count = 20)
        {
            return await this.mediator.Send(new GetItemsQuery { Filter = filter, Count = count });
        }

        [System.Web.Http.HttpGet]
        public async Task<FullDetailItem> Detail([FromQuery] int id)
        {
            return await this.mediator.Send(new GetFullItemDetailsQuery { ItemId = id });
        }

        [System.Web.Http.HttpGet]
        public async Task<IEnumerable<RecentItem>> Recent()
        {
            return await this.mediator.Send(new GetRecentItemsQuery());
        }

        [System.Web.Http.HttpGet]
        public async Task<IEnumerable<Item>> Hot()
        {
            return await this.mediator.Send(new GetHotItemsQuery());
        }
    }
}
