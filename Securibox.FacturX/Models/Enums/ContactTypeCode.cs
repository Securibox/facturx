using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.Enums
{
    public class ContactTypeCode : Enumeration
    {
        public static ContactTypeCode Undefined = new ContactTypeCode(-1, "Undefined");

        private ContactTypeCode(int id, string name)
            : base(id, name) { }
    }
}
