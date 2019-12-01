namespace SSMB.Blazor.Shared.AccountPanel
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Http;

    class AccountPanelViewModel : IAccountPanelViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly NavigationManager uriHelper;

        public AccountPanelViewModel(IHttpContextAccessor httpContextAccessor, NavigationManager uriHelper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.uriHelper = uriHelper;
        }

        public bool IsUserAuthenticated => this.httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string UserName => this.httpContextAccessor.HttpContext.User.Identity.Name;

        public void OnLoginClicked()
        {
            this.uriHelper.NavigateTo("/Login", true);
        }

        public void OnAlertsClicked()
        {
            this.uriHelper.NavigateTo("/Alerts", true);
        }
    }
}
