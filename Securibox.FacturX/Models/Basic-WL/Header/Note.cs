using Securibox.FacturX.Models.BasicWL.Enum;

namespace Securibox.FacturX.Models.BasicWL
{
    public class Note
    {
        public string Content { get; internal set; }
        public string? SubjectCode { get; internal set; }

        public Note(string content, string? subjectCode)
        {
            Content = content;
            SubjectCode = subjectCode;
        }
    }
}
