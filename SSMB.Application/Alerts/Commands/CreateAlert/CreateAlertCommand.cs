namespace SSMB.Application.Alerts.Commands.CreateAlert
{
    using Domain;
    using MediatR;

    public class CreateAlertCommand : IRequest<Alert>
    {
        public string AlertName { get; set; }

        public AlertCondition[] Conditions { get; set; }

        public int ItemId { get; set; }
        public long UserId { get; set; }
    }
}
