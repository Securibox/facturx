using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.BasicWL.Enum
{
    public class ElectronicAddressSchemeId : Enumeration
    {
        // TODO
        public static ElectronicAddressSchemeId Undefined = new ElectronicAddressSchemeId(
            -1,
            "Undefined"
        );

        private ElectronicAddressSchemeId(int id, string name)
            : base(id, name) { }
    }
}
