namespace SSMB.Application.Alerts.Commands.CreateAlert
{
    using System.Threading;
    using System.Threading.Tasks;
    using Domain;
    using Interfaces;
    using MediatR;

    class CreateAlertHandler : IRequestHandler<CreateAlertCommand, Alert>
    {
        private readonly ISsmbDbContext context;

        public CreateAlertHandler(ISsmbDbContext context)
        {
            this.context = context;
        }

        public async Task<Alert> Handle(CreateAlertCommand request, CancellationToken cancellationToken)
        {
            var alert = await this.context.Alerts.AddAsync(new Alert()
            {
                UserId = request.UserId,
                ItemId = request.ItemId,
                Name = request.AlertName,
                Conditions = request.Conditions
            }, cancellationToken);

            await this.context.SaveChangesAsync(cancellationToken);
            return alert.Entity;
        }
    }
}
