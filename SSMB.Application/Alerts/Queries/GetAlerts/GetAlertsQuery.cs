namespace SSMB.Application.Alerts.Queries.GetAlerts
{
    using Domain;
    using MediatR;

    public class GetAlertsQuery : IRequest<Alert[]>
    {
        public long? UserId { get; set; }

        public int? ItemId { get; set; }
    }
}
