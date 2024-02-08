namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class ReferencedDocumentEN16931
    {
        public Minimum.ID IssuerAssignedID { get; set; }
        public Minimum.ID URIID { get; set; }
        public Minimum.ID LineID { get; set; }
        public string TypeCode { get; set; }
        public string Name { get; set; }
        public BinaryObject AttachmentBinaryObject { get; set; }
        public string ReferenceTypeCode { get; set; }
        public BasicWL.FormattedIssueDateTime FormattedIssueDateTime { get; set; }
    }
}
