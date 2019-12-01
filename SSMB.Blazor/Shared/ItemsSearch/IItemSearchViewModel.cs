namespace SSMB.Blazor.Shared.ItemsSearch
{
    using Microsoft.AspNetCore.Components;

    public interface IItemSearchViewModel
    {
        string SearchValue { get; set; }

        void OnClick();

        void OnInput(ChangeEventArgs args);

        void OnInputFocus();
    }
}
