namespace SSMB.Blazor.Pages.CreateAlert.Models
{
    public class AlertCondition
    {
        public AlertField AlertField { get; set; }

        public AlertOperator AlertOperator { get; set; }

        public long Value { get; set; }
    }
}
