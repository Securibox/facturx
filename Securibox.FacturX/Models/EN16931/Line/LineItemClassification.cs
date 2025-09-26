namespace Securibox.FacturX.Models.EN16931
{
    public class LineItemClassification
    {
        public LineItemClassificationIdentifier? ClassificationIdentifier { get; internal set; }

        public LineItemClassification(LineItemClassificationIdentifier? classificationIdentifier) => ClassificationIdentifier = classificationIdentifier;
    }
}