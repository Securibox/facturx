namespace Securibox.FacturX.Models.Extended
{
    public class LineAttributeValueMeasure
    {
        public decimal Value { get; internal set; }

        public string Measure { get; internal set; }

        public LineAttributeValueMeasure(decimal value, string measure)
        {
            Value = value;
            Measure = measure;
        }
    }
}
