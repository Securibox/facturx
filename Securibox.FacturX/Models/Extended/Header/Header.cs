namespace Securibox.FacturX.Models.Extended
{
    public class Header : BasicWL.Header
    {
        public string? InvoiceName { get; private set; }
        
        public bool? CopyIndicator { get; private set; }

        public IEnumerable<string>? LanguageList { get; private set; }

        public DateTime? EffectiveSpecifiedPeriod { get; private set; }

        public new IEnumerable<Extended.Note>? NoteList { get; private set; }

        public Header(string invoiceNumber, string invoiceType, DateTime emissionDate)
           : base(invoiceNumber, invoiceType, emissionDate) { }

        public void AddInvoiceName(string? invoiceName) => InvoiceName = invoiceName;
        public void AddCopyIndicator(bool copyIndicator) => CopyIndicator = copyIndicator;
        public void AddLanguageList(IEnumerable<string>? languageList) => LanguageList = languageList;
        public void AddEffectiveSpecifiedPeriod(DateTime? effectiveSpecifiedPeriod) => EffectiveSpecifiedPeriod = effectiveSpecifiedPeriod;
        public void AddExtendedNoteList(IEnumerable<Note>? noteList) => NoteList = noteList;
    }
}
