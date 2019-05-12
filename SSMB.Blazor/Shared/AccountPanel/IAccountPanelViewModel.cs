namespace SSMB.Blazor.Shared.AccountPanel
{
    public interface IAccountPanelViewModel
    {
        bool IsUserAuthenticated { get; }

        string UserName { get; }

        void OnLoginClicked();

        void OnAlertsClicked();
    }
}
