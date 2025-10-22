using Securibox.FacturX.Models.EN16931.Enum;

namespace Securibox.FacturX.Models.EN16931
{
    public abstract class AdditionalDocument : IReference
    {
        public string? AssignedId { get; set; }

        public AdditionalDocumentReferenceType Type { get; set; }
    }
}
