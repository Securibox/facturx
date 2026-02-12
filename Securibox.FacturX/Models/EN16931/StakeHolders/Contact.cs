using Securibox.FacturX.Models.BasicWL;

namespace Securibox.FacturX.Models.EN16931
{
    public class Contact
    {
        public string? PersonName { get; internal set; }

        public string? DepartmentName { get; internal set; }

        public string? PhoneNumber { get; internal set; }

        public ElectronicAddress? ElectronicAddress { get; internal set; }
    }
}
