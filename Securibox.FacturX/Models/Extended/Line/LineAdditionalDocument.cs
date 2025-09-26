using Securibox.FacturX.Models.EN16931;

namespace Securibox.FacturX.Models.Extended
{
    public class LineAdditionalDocument : IReference
    {
        public string AssignedId { get; private set; }

        public string? ExternalLocation { get; private set; }

        public string? LineId { get; private set; }

        public string TypeCode { get; private set; }

        public IEnumerable<string>? NameList { get; private set; }

        public Attachment? AttachedDocument { get; private set; }

        public string? ReferenceTypeCode { get; private set; }

        public DateTime? IssueDate { get; private set; }

        internal void AddAssignedId(string assignedId) => AssignedId = assignedId;
        internal void AddTypeCode(string typeCode) => TypeCode = typeCode;
        internal void AddExternalLocation(string? externalLocation) => ExternalLocation = externalLocation;
        internal void AddLineId(string? lineId) => LineId = lineId;
        internal void AddNameList(IEnumerable<string>? nameList) => NameList = nameList;
        internal void AddAttachedDocument(Attachment? attachedDocument) => AttachedDocument = attachedDocument;
        internal void AddReferenceTypeCode(string? referenceTypeCode) => ReferenceTypeCode = referenceTypeCode;
        internal void AddIssueDate(DateTime? issueDate) => IssueDate = issueDate;
    }
}