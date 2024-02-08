using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Extended
{
    public class LineItemDetails : EN16931.LineItemDetails
    {
        public string? Id { get; private set; }

        public LineItemDetails(string name, GlobalIdentification? standardIdentification)
           : base(name, standardIdentification) { }

        internal void AddId(string? id) => Id = id;

    }
}
