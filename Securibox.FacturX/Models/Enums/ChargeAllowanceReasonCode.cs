using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.Enums
{
    public class ChargeAllowanceReasonCode : Enumeration
    {
        public static ChargeAllowanceReasonCode Undefined = new ChargeAllowanceReasonCode(-1, "Undefined");
        public static ChargeAllowanceReasonCode AdvertisingDiscount = new ChargeAllowanceReasonCode(1, "AA");
        public static ChargeAllowanceReasonCode PackingSupplement = new ChargeAllowanceReasonCode(2, "ABL");
        public static ChargeAllowanceReasonCode OtherServices = new ChargeAllowanceReasonCode(3, "ADR");
        public static ChargeAllowanceReasonCode Removal = new ChargeAllowanceReasonCode(4, "ADT");
        public static ChargeAllowanceReasonCode TransportationCosts = new ChargeAllowanceReasonCode(5, "FC");
        public static ChargeAllowanceReasonCode FinancialWxpenses = new ChargeAllowanceReasonCode(6, "FI");
        public static ChargeAllowanceReasonCode Labeling = new ChargeAllowanceReasonCode(7, "LA");
        public static ChargeAllowanceReasonCode BonusWorksAheadOfSchedule = new ChargeAllowanceReasonCode(41, "Bonus for works ahead of schedule");
        public static ChargeAllowanceReasonCode OtherBonus = new ChargeAllowanceReasonCode(42, "Other bonus");
        public static ChargeAllowanceReasonCode ManufacturersConsumerDiscount = new ChargeAllowanceReasonCode(60, "Manufacturer’s consumer discount");
        public static ChargeAllowanceReasonCode DueToMilitaryStatus = new ChargeAllowanceReasonCode(62, "Due to military status");
        public static ChargeAllowanceReasonCode DueToWorkAccident = new ChargeAllowanceReasonCode(63, "Due to work accidentt");
        public static ChargeAllowanceReasonCode SpecialAgreement = new ChargeAllowanceReasonCode(64, "Special agreement");
        public static ChargeAllowanceReasonCode ProductionErrorDiscount = new ChargeAllowanceReasonCode(65, "Production error discount");
        public static ChargeAllowanceReasonCode NewOutletDiscount = new ChargeAllowanceReasonCode(66, "New outlet discount");
        public static ChargeAllowanceReasonCode SampleDiscount = new ChargeAllowanceReasonCode(67, "Sample discount");
        public static ChargeAllowanceReasonCode EndOfRangeDiscount = new ChargeAllowanceReasonCode(68, "End-of-range discount");
        public static ChargeAllowanceReasonCode IncotermDiscount = new ChargeAllowanceReasonCode(70, "Incoterm discount");
        public static ChargeAllowanceReasonCode PointOfSalesThresholdAllowance = new ChargeAllowanceReasonCode(71, "Point of sales threshold allowance");
        public static ChargeAllowanceReasonCode MaterialSurchargeOrDeduction = new ChargeAllowanceReasonCode(88, "Material surcharge/deduction");
        public static ChargeAllowanceReasonCode Discount = new ChargeAllowanceReasonCode(95, "Discount");
        public static ChargeAllowanceReasonCode SpecialRebate = new ChargeAllowanceReasonCode(100, "Special rebate");
        public static ChargeAllowanceReasonCode FixedLongTerm = new ChargeAllowanceReasonCode(102, "Fixed long term");
        public static ChargeAllowanceReasonCode Temporary = new ChargeAllowanceReasonCode(103, "Temporary");
        public static ChargeAllowanceReasonCode Standard = new ChargeAllowanceReasonCode(104, "Standard");
        public static ChargeAllowanceReasonCode YearlyTurnover = new ChargeAllowanceReasonCode(105, "Yearly turnover");

        public ChargeAllowanceReasonCode(int id, string name) : base(id, name) { }
    }
}
