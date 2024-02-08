namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class DocumentLineDocument
    {
        public Minimum.ID LineID { get; set; }
        public Minimum.ID ParentLineID { get; set; }
        public string LineStatusCode { get; set; }
        public string LineStatusReasonCode { get; set; }
        public NoteExtended IncludedNote { get; set; }  
    }
}