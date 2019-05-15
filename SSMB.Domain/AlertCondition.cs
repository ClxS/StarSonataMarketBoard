namespace SSMB.Domain
{
    public class AlertCondition
    {
        public Alert Alert { get; set; }

        public int AlertId { get; set; }

        public AlertField Field { get; set; }

        public int Id { get; set; }

        public AlertOperator Operator { get; set; }

        public long Value { get; set; }
    }
}
