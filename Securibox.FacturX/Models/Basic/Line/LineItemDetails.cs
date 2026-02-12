using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.Basic
{
    public class LineItemDetails
    {
        public GlobalIdentification? StandardIdentification { get; private set; }

        public string Name { get; private set; }

        public LineItemDetails(string name, GlobalIdentification? standardIdentification)
        {
            Name = name;
            StandardIdentification = standardIdentification;
        }
    }
}
