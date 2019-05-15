namespace SSMB.Application.Items.Queries.GetHotItems
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    class GetHotItemsQueryHandler : IRequestHandler<GetHotItemsQuery, Item[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetHotItemsQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Item[]> Handle(GetHotItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await this.dbContext.Items
                                  .Where(i => i.Orders.Any())
                                  .Take(request.Count)
                                  .ToArrayAsync(cancellationToken);
            Array.ForEach(items, i => i.StructuredDescription = string.Empty);
            return items;
        }
    }
}
