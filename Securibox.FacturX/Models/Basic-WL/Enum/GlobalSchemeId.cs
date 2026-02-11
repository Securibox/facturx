using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.BasicWL.Enum
{
    public class GlobalSchemeId : Enumeration
    {
        public static GlobalSchemeId Undefined = new GlobalSchemeId(-1, "Undefined");
        public static GlobalSchemeId Swift = new GlobalSchemeId(21, "0021");
        public static GlobalSchemeId Duns = new GlobalSchemeId(60, "0060");
        public static GlobalSchemeId Gln = new GlobalSchemeId(88, "0088");
        public static GlobalSchemeId Odette = new GlobalSchemeId(177, "0177");
        public static GlobalSchemeId SiretCode = new GlobalSchemeId(9, "0009");
        public static GlobalSchemeId ElectronicMail = new GlobalSchemeId(1000, "EM");
        public static GlobalSchemeId SimpleMailTransferProtocol = new GlobalSchemeId(1001, "SMTP");
        public static GlobalSchemeId SirenOrSiretCode = new GlobalSchemeId(2, "0002");

        // ISO/IEC 6523 list
        public static GlobalSchemeId GTIN = new GlobalSchemeId(160, "0160");

        private GlobalSchemeId(int id, string name)
            : base(id, name) { }
    }
}
