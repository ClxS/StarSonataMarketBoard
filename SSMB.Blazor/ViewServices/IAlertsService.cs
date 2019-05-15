namespace SSMB.Blazor.ViewServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain;

    public interface IAlertsService
    {
        Task<Alert[]> GetAlerts(int itemId);

        Task<Alert[]> GetAlerts(long userId);

        Task<Alert[]> GetAlerts(int itemId, long userId);

        Task<Alert> CreateAlert(long idValue, int itemId, string alertName, IEnumerable<AlertCondition> conditions);
    }
}
