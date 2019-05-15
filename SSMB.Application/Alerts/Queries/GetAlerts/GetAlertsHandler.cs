namespace SSMB.Application.Alerts.Queries.GetAlerts
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    class GetAlertsHandler : IRequestHandler<GetAlertsQuery, Alert[]>
    {
        private readonly ISsmbDbContext dbContext;

        public GetAlertsHandler(ISsmbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Alert[]> Handle(GetAlertsQuery request, CancellationToken cancellationToken)
        {
            return await this.dbContext.Alerts
                             .Where(i => (!request.UserId.HasValue || i.UserId.Equals(request.UserId)) &&
                                         (!request.ItemId.HasValue || i.ItemId.Equals(request.ItemId)))
                             .Include(a => a.Conditions)
                             .ToArrayAsync(cancellationToken);
        }
    }
}
