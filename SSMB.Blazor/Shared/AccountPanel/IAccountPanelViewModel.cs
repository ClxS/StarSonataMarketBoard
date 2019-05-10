namespace SSMB.Blazor.Shared.AccountPanel
{
    public interface IAccountPanelViewModel
    {
        bool IsUserAuthenticated { get; }

        void OnLoginClicked();
    }
}
