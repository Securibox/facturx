namespace Securibox.FacturX.SpecificationModels.EN16931
{
    public class TradeContact
    {
        public string PersonName { get; set; }
        public string DepartmentName { get; set; }
        public TelephoneUniversalCommunication TelephoneUniversalCommunication { get; set; }
        public BasicWL.UniversalCommunication EmailURIUniversalCommunication { get; set; }
    }
}