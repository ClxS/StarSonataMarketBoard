namespace SSMB.Server.API.V1
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Application.Interfaces;
    using Application.Items.Models;
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
        public IEnumerable<Item> Index([FromQuery] string filter)
        {
            //var items = this.dbContext.Items.Where(x => EF.Functions.Like(x.Name, $"%{filter}%")).ToArray();
            //return items;
            return null;
        }

        [System.Web.Http.HttpGet]
        public async Task<IEnumerable<RecentItem>> Recent()
        {
            return await this.mediator.Send(new GetRecentItemsQuery());
        }
    }
}
