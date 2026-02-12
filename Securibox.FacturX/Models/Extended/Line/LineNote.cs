namespace Securibox.FacturX.Models.Extended
{
    public class LineNote : Basic.LineNote
    {
        public string? ContentCode { get; private set; }

        public string? SubjectCode { get; private set; }

        public LineNote(string? content, string? contentCode, string? subjectCode)
            : base(content)
        {
            ContentCode = contentCode;
            SubjectCode = subjectCode;
        }
    }
}
