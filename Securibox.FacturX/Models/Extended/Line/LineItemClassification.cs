using Securibox.FacturX.Models.EN16931;

namespace Securibox.FacturX.Models.Extended
{
    public class LineItemClassification : EN16931.LineItemClassification
    {
        public string? Name { get; internal set; }

        internal LineItemClassification(LineItemClassificationIdentifier? classificationIdentifier, string? name) 
            : base(classificationIdentifier) => Name = name;
    }
}
