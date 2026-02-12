using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class ReferencedDocumentExtended
    {
        public Minimum.ID IssuerAssignedID { get; set; }
        public Minimum.ID URIID { get; set; }
        public Minimum.ID LineID { get; set; }
        public string TypeCode { get; set; }

        [XmlElement]
        public string[] Name { get; set; }
        public EN16931.BinaryObject AttachmentBinaryObject { get; set; }
        public string ReferenceTypeCode { get; set; }
        public BasicWL.FormattedIssueDateTime FormattedIssueDateTime { get; set; }
    }
}
