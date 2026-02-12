namespace Securibox.FacturX.Models.BasicWL
{
    public class DeliveryPeriod
    {
        public DateTime? StartDate { get; internal set; }

        public DateTime? EndDate { get; internal set; }

        public DeliveryPeriod(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
        }
    }
}
