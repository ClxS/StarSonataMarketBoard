namespace SSMB.Application.Items.Queries.GetItems
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    class GetItemsQueryHandler : IRequestHandler<GetItemsQuery, Item[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetItemsQueryHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Item[]> Handle(GetItemsQuery request, CancellationToken cancellationToken)
        {
            return await this.dbContext.Items.Where(x => EF.Functions.Like(x.Name, $"%{request.Filter}%"))
                             .ToArrayAsync(cancellationToken);
        }
    }
}
