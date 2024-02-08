using System.Xml.Serialization;

namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class SupplyChainConsignment
    {
        [XmlElement]
        public LogisticsTransportMovement[] SpecifiedLogisticsTransportMovement { get; set; }
    }
}
