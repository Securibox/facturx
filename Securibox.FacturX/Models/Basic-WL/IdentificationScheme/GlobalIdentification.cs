namespace Securibox.FacturX.Models.BasicWL
{
    public class GlobalIdentification
    {
        public string Id { get; internal set; }

        public string? Scheme { get; internal set; }

        public GlobalIdentification(string id, string? scheme)
        {
            Id = id;
            Scheme = scheme;
        }
    }
}
