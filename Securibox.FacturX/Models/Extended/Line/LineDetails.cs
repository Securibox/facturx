namespace Securibox.FacturX.Models.Extended
{
    public class LineDetails : Models.Basic.LineDetails
    {
        public string? ParentLineId { get; internal set; }

        public string? StatusCode { get; internal set; }

        public string? StatusReasonCode { get; internal set; }

        public new LineNote? Note { get; internal set; }

        public LineDetails(
            string lineId,
            string? parentLineId,
            string? statusCode,
            string? statusReasonCode
        )
            : base(lineId)
        {
            ParentLineId = parentLineId;
            StatusCode = statusCode;
            StatusReasonCode = statusReasonCode;
        }

        internal void AddExtendedNote(LineNote? note) => Note = note;
    }
}
