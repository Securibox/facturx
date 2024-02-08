namespace Securibox.FacturX.Models.Extended
{
    public class Note : BasicWL.Note
    {
        public string? ContentCode { get; internal set; }

        public Note(string content, string? subjectCode, string? contentCode) : base(content, subjectCode) => ContentCode = contentCode;
    }
}
