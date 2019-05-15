namespace SSMB.Server.API.V1
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Application.Alerts.Commands.CreateAlert;
    using Application.Alerts.Queries.GetAlerts;
    using Domain;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    public class AlertsController : ControllerBase
    {
        private readonly IMediator mediator;

        public AlertsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<Alert>> Index([FromQuery] int? item, [FromQuery] long? user)
        {
            return await this.mediator.Send(new GetAlertsQuery { ItemId = item, UserId = user });
        }

        [HttpPost]
        public async Task<Alert> Index([FromQuery] int item, [FromQuery] long user, [FromQuery]string name, AlertCondition[] conditions)
        {
            return await this.mediator.Send(new CreateAlertCommand
            {
                ItemId = item,
                UserId = user,
                AlertName = name,
                Conditions = conditions
            });
        }
    }
}
