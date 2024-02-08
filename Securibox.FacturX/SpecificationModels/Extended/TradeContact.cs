namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradeContact
    {
        public string PersonName { get; set; }
        public string DepartmentName { get; set; }
        public string TypeCode { get; set; }
        public EN16931.TelephoneUniversalCommunication TelephoneUniversalCommunication { get; set; }
        public BasicWL.UniversalCommunication FaxUniversalCommunication { get; set; }
        public BasicWL.UniversalCommunication EmailURIUniversalCommunication { get; set; }
    }
}