namespace SSMB.Blazor.Shared
{
    using ViewServices;

    public class MainLayoutViewModel : IMainLayoutViewModel
    {
        private readonly INavigationService navigationService;

        public MainLayoutViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public void OnRootClicked()
        {
            this.navigationService.Click(this);
        }
    }
}
