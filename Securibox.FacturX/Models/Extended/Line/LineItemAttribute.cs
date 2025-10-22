namespace Securibox.FacturX.Models.Extended
{
    public class LineItemAttribute : EN16931.LineItemAttribute
    {
        public string? Type { get; internal set; }

        public LineAttributeValueMeasure? ValueMeasure { get; internal set; }

        internal LineItemAttribute(string name, string value) : base(name, value) { }

        internal void AddType(string? type) => Type = type;
        internal void AddValueMeasure(LineAttributeValueMeasure? valueMeasure) => ValueMeasure = valueMeasure;
    }
}