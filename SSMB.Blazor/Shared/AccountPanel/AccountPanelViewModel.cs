namespace SSMB.Blazor.Shared.AccountPanel
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Http;

    class AccountPanelViewModel : IAccountPanelViewModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUriHelper uriHelper;

        public AccountPanelViewModel(IHttpContextAccessor httpContextAccessor, IUriHelper uriHelper)
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
