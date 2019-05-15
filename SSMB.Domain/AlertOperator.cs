namespace SSMB.Domain
{
    using System.ComponentModel;

    public enum AlertOperator : byte
    {
        [Description("<")]
        Below = 0,
        [Description("<=")]
        BelowEqual = 1,
        [Description("=")]
        Equal = 2,
        [Description(">=")]
        AboveEqual = 3,
        [Description(">")]
        Above = 4,
    }
}
