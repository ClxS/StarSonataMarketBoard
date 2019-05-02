namespace SSMB.Blazor.Shared.ItemsSearch
{
    using Microsoft.AspNetCore.Components;
    using Services;
    using ViewServices;

    public class ItemsSearchViewModel : IItemSearchViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ISearchService searchService;

        public ItemsSearchViewModel(ISearchService searchService, INavigationService navigationService)
        {
            this.searchService = searchService;
            this.navigationService = navigationService;
        }

        public string SearchValue
        {
            get => this.searchService.SearchValue;
            set => this.searchService.SearchValue = value;
        }

        public void OnClick()
        {
            this.navigationService.Click(this);
            this.searchService.MarkSearchVisible();
        }

        public void OnInput(UIChangeEventArgs args)
        {
            this.SearchValue = (string)args.Value;
        }

        public void OnInputFocus()
        {
            this.navigationService.Click(this);
            this.searchService.MarkSearchVisible();
        }
    }
}
