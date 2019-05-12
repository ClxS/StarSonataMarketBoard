namespace SSMB.Blazor.Pages.CreateAlert.Models
{
    using System.ComponentModel;

    public enum AlertOperator
    {
        [Description("<")]
        Below,
        [Description("<=")]
        BelowEqual,
        [Description("=")]
        Equal,
        [Description(">=")]
        AboveEqual,
        [Description(">")]
        Above,
    }
}
