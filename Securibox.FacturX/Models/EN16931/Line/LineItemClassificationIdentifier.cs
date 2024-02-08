namespace Securibox.FacturX.Models.EN16931
{
    public class LineItemClassificationIdentifier
    {
        public string Id { get; private set; }
        public string SchemeId { get; private set; }
        public string? SchemeVersionId { get; private set; }

        internal LineItemClassificationIdentifier(string id, string schemeId, string? schemeVersionId)
        {
            Id = id;
            SchemeId = schemeId;
            SchemeVersionId = schemeVersionId;
        }
    }
}
