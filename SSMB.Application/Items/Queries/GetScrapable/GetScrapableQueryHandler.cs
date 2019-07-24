namespace SSMB.Application.Items.Queries.GetRecentItems
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Models;

    class GetScrapableQueryHandler : IRequestHandler<GetScrapableQuery, Scrapable[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetScrapableQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Scrapable[]> Handle(GetScrapableQuery request, CancellationToken cancellationToken)
        {
            var items = await this.dbContext.Items
                                  .Where(i => i.ScrapValue > 10000 && i.ScrapValue < long.MaxValue)
                                  .ToArrayAsync(cancellationToken);
            return items.Select(i => new Scrapable
            {
                Item = i,
                Price = (long)(i.ScrapValue * request.Factor)
            }).ToArray();
        }
    }
}
