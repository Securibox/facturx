namespace Securibox.FacturX.Models.Extended
{
    public class DeliveryPeriod : BasicWL.DeliveryPeriod
    {
        public string? Description { get; internal set; }

        public DeliveryPeriod(DateTime? startDate, DateTime? endDate, string? description) 
            : base(startDate, endDate) => Description = description;
    }
}
