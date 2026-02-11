namespace Securibox.FacturX.Models.Basic
{
    public class QuantityUnit
    {
        public decimal Quantity { get; internal set; }

        public string? UnitCode { get; internal set; }

        public QuantityUnit() { }

        public QuantityUnit(decimal quantity)
        {
            Quantity = quantity;
        }

        public QuantityUnit(decimal quantity, string? unitCode)
            : this(quantity)
        {
            UnitCode = unitCode;
        }

        internal void AddQuantity(decimal quantity) => Quantity = quantity;

        internal void AddUnitCode(string? unitCode) => UnitCode = unitCode;
    }
}
