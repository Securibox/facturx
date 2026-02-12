namespace Securibox.FacturX.Models.Basic
{
    public class LineNote
    {
        public string? Content { get; private set; }

        public LineNote(string? content) => Content = content;
    }
}
