namespace Securibox.FacturX.Models.Basic
{
    public class LineDetails
    {
        public string LineId { get; private set; }

        public LineNote? Note { get; private set; }

        public LineDetails(string lineId)
        {
            LineId = lineId;
        }

        internal void AddBasicNote(LineNote? note) => Note = note;
    }
}
