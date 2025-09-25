namespace Securibox.FacturX.Models.BasicWL
{
    public class Header : Minimum.Header
    {
        public IEnumerable<Note>? NoteList { get; private set; }

        public Header(string invoiceNumber, string invoiceType, DateTime emissionDate) 
            : base(invoiceNumber, invoiceType, emissionDate) { }
        
        public void AddBasicWLNoteList(IEnumerable<Note>? noteList) => NoteList = noteList;
    }
}