namespace SSMB.Blazor.ViewServices.Defaults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Server.API.V1;

    class AlertsServiceServerBased : IAlertsService
    {
        private readonly AlertsController alertsController;

        public AlertsServiceServerBased(AlertsController alertsController)
        {
            this.alertsController = alertsController;
        }

        public Task<Alert[]> GetAlerts(int itemId)
        {
            return Task.Run(async () => (await this.alertsController.Index(itemId, null)).ToArray());
        }

        public Task<Alert[]> GetAlerts(long userId)
        {
            return Task.Run(async () => (await this.alertsController.Index(null, userId)).ToArray());
        }

        public Task<Alert[]> GetAlerts(int itemId, long userId)
        {
            return Task.Run(async () => (await this.alertsController.Index(itemId, userId)).ToArray());
        }

        public Task<Alert> CreateAlert(long idValue, int itemId, string alertName, IEnumerable<AlertCondition> conditions)
        {
            return Task.Run(async () => (await this.alertsController.Index(itemId, idValue, alertName, conditions.ToArray())));
        }
    }
}