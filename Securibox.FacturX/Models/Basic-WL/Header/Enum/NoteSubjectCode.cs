using Securibox.FacturX.Core;

namespace Securibox.FacturX.Models.BasicWL.Enum
{
    public class NoteSubjectCode : Enumeration
    {
        public static NoteSubjectCode Undefined = new NoteSubjectCode(-1, "Undefined");
        public static NoteSubjectCode GeneralInformation = new NoteSubjectCode(0, "AAI");
        public static NoteSubjectCode SupplierNotes = new NoteSubjectCode(1, "SUR");
        public static NoteSubjectCode RegulatoryInformation = new NoteSubjectCode(2, "REG");
        public static NoteSubjectCode LegalInformation = new NoteSubjectCode(3, "ABL");
        public static NoteSubjectCode TaxInformation = new NoteSubjectCode(4, "TXD");
        public static NoteSubjectCode CustomsInformation = new NoteSubjectCode(4, "CUS");
        //public static NoteSubjectCode GoodsItemDescription = new NoteSubjectCode(0, "AAA");
        //public static NoteSubjectCode PaymentTerm = new NoteSubjectCode(1, "AAB");
        //public static NoteSubjectCode DangerousGoodsAdditionalInformation = new NoteSubjectCode(2, "AAC");
        //public static NoteSubjectCode DangerousGoodsTechnicalName = new NoteSubjectCode(3, "AAD");
        //public static NoteSubjectCode AcknowledgementDescription = new NoteSubjectCode(4, "AAE");
        //public static NoteSubjectCode RateAdditionalInformation = new NoteSubjectCode(5, "AAF");
        //public static NoteSubjectCode PartyInstructions = new NoteSubjectCode(6, "AAG");
        //public static NoteSubjectCode GeneralInformation = new NoteSubjectCode(7, "AAI");
        //public static NoteSubjectCode AdditionalConditionsOfSaleOrPurchase = new NoteSubjectCode(8, "AAJ");
        //public static NoteSubjectCode PriceConditions = new NoteSubjectCode(9, "AAK");
        //public static NoteSubjectCode GoodsDimensionsInCharacters = new NoteSubjectCode(10, "AAL");
        //public static NoteSubjectCode EquipmentReusageRestrictions = new NoteSubjectCode(11, "AAM");
        //public static NoteSubjectCode HandlingRestriction = new NoteSubjectCode(12, "AAN");
        //public static NoteSubjectCode ErrorDescription = new NoteSubjectCode(13, "AAO");
        //public static NoteSubjectCode Response = new NoteSubjectCode(14, "AAP");
        //public static NoteSubjectCode PackageContentsDescription = new NoteSubjectCode(15, "AAQ");

        //public static NoteSubjectCode TermsOfDelivery = new NoteSubjectCode(16, "AAR");
        //public static NoteSubjectCode BillOfladingRemarks = new NoteSubjectCode(17, "AAS");
        //public static NoteSubjectCode ModeOfSettlementInformation = new NoteSubjectCode(18, "AAT");
        //public static NoteSubjectCode ConsignmentInvoiceInformation = new NoteSubjectCode(19, "AAU");
        //public static NoteSubjectCode ClearanceInvoiceInformation = new NoteSubjectCode(20, "AAV");
        //public static NoteSubjectCode LetterOfCreditInformation = new NoteSubjectCode(21, "AAW");
        //public static NoteSubjectCode LicenseInformation = new NoteSubjectCode(22, "AAX");
        //public static NoteSubjectCode CertificationStatements = new NoteSubjectCode(23, "AAY");
        //public static NoteSubjectCode AdditionalExportInformation = new NoteSubjectCode(24, "AAZ");
        //public static NoteSubjectCode TariffStatements = new NoteSubjectCode(25, "ABA");

        //public static NoteSubjectCode MedicalHistory = new NoteSubjectCode(26, "ABB");
        //public static NoteSubjectCode ConditionsOfSaleOrPurchase = new NoteSubjectCode(27, "ABC");
        //public static NoteSubjectCode ContractDocumentType = new NoteSubjectCode(28, "ABD");
        //public static NoteSubjectCode AdditionalTermsAndOrConditions = new NoteSubjectCode(29, "ABE");
        //public static NoteSubjectCode InstructionsOrInformationAboutStandbyDocumentary = new NoteSubjectCode(30, "ABF");
        //public static NoteSubjectCode InstructionsOrInformationAboutPartialShipment = new NoteSubjectCode(31, "ABG");
        //public static NoteSubjectCode InstructionsOrInformationAboutTranshipment = new NoteSubjectCode(32, "ABH");
        //public static NoteSubjectCode AdditionalHandlingInstructionsDocumentaryCredit = new NoteSubjectCode(33, "ABI");
        //public static NoteSubjectCode DomesticRoutingInformation = new NoteSubjectCode(34, "ABJ");
        //public static NoteSubjectCode ChargeableCategoryOfEquipment = new NoteSubjectCode(35, "ABK");
        //public static NoteSubjectCode GovernmentInformation = new NoteSubjectCode(36, "ABL");
        //public static NoteSubjectCode OnwardRoutingInformation = new NoteSubjectCode(37, "ABM");
        //public static NoteSubjectCode AccountingInformation = new NoteSubjectCode(38, "ABN");
        //public static NoteSubjectCode DiscrepancyInformation = new NoteSubjectCode(39, "ABO");
        //public static NoteSubjectCode ConfirmationInstructions = new NoteSubjectCode(40, "ABP");
        //public static NoteSubjectCode MethodOfIssuance = new NoteSubjectCode(41, "ABQ");
        //public static NoteSubjectCode DocumentsDeliveryInstructions = new NoteSubjectCode(42, "ABR");
        //public static NoteSubjectCode AdditionalConditions = new NoteSubjectCode(44, "ABS");
        //public static NoteSubjectCode InformationInstructionsAboutAdditionalAmountsCovered = new NoteSubjectCode(45, "ABT");
        //public static NoteSubjectCode DeferredPaymentTermedAdditional = new NoteSubjectCode(46, "ABU");
        //public static NoteSubjectCode AcceptanceTermsAdditional = new NoteSubjectCode(47, "ABV");
        //public static NoteSubjectCode NegotiationTermsAdditional = new NoteSubjectCode(48, "ABW");
        //public static NoteSubjectCode DocumentNameAndDocumentaryRequirements = new NoteSubjectCode(49, "ABX");
        //public static NoteSubjectCode InstructionsOrInformationAboutRevolvingDocumentaryCredit = new NoteSubjectCode(50, "ABZ");
        //public static NoteSubjectCode DocumentaryRequirements = new NoteSubjectCode(51, "ACA");
        //public static NoteSubjectCode AdditionalInformation = new NoteSubjectCode(52, "ACB");
        //public static NoteSubjectCode FactorAssignmentClause = new NoteSubjectCode(53, "ACC");
        //public static NoteSubjectCode Reason = new NoteSubjectCode(54, "ACD");
        //public static NoteSubjectCode Dispute = new NoteSubjectCode(55, "ACE");
        //public static NoteSubjectCode AdditionalAttributeInformation = new NoteSubjectCode(56, "ACF");
        //public static NoteSubjectCode AbsenceDeclarations = new NoteSubjectCode(57, "ACG");
        //public static NoteSubjectCode AggregationStatement = new NoteSubjectCode(58, "ACH");
        //public static NoteSubjectCode CompilationStatement = new NoteSubjectCode(59, "ACI");
        //public static NoteSubjectCode DefinitionalException = new NoteSubjectCode(60, "ACJ");
        //public static NoteSubjectCode PrivacyStatement = new NoteSubjectCode(61, "ACK");
        //public static NoteSubjectCode QualityStatement = new NoteSubjectCode(62, "ACL");
        //public static NoteSubjectCode StatisticalDescription = new NoteSubjectCode(63, "ACM");
        //public static NoteSubjectCode StatisticalDefinition = new NoteSubjectCode(64, "ACN");
        //public static NoteSubjectCode StatisticalName = new NoteSubjectCode(65, "ACO");
        //public static NoteSubjectCode StatisticalTitle = new NoteSubjectCode(66, "ACP");
        //public static NoteSubjectCode OffDimensionInformation = new NoteSubjectCode(67, "ACQ");
        //public static NoteSubjectCode UnexpectedStopsInformation = new NoteSubjectCode(68, "ACR");
        //public static NoteSubjectCode Principles = new NoteSubjectCode(69, "ACS");
        //public static NoteSubjectCode TermsAndDefinition = new NoteSubjectCode(70, "ACT");
        //public static NoteSubjectCode SegmentName = new NoteSubjectCode(71, "ACU");
        //public static NoteSubjectCode SimpleDataElementName = new NoteSubjectCode(72, "ACV");
        //public static NoteSubjectCode Scope = new NoteSubjectCode(73, "ACW");
        //public static NoteSubjectCode MessageTypeName = new NoteSubjectCode(74, "ACX");
        //public static NoteSubjectCode Introduction = new NoteSubjectCode(75, "ACY");
        //public static NoteSubjectCode Glossary = new NoteSubjectCode(76, "ACZ");
        //public static NoteSubjectCode FunctionalDefinition = new NoteSubjectCode(77, "ADA");
        //public static NoteSubjectCode Examples = new NoteSubjectCode(78, "ADB");
        //public static NoteSubjectCode CoverPage = new NoteSubjectCode(79, "ADC");
        //public static NoteSubjectCode DependencySyntaxNotes = new NoteSubjectCode(80, "ADD");
        //public static NoteSubjectCode CodeValueName = new NoteSubjectCode(81, "ADE");
        //public static NoteSubjectCode CodeListName = new NoteSubjectCode(82, "ADF");
        //public static NoteSubjectCode ClarificationOfUsage = new NoteSubjectCode(83, "ADG");
        //public static NoteSubjectCode CompositeDataElementName = new NoteSubjectCode(84, "ADH");
        //public static NoteSubjectCode FieldOfApplication = new NoteSubjectCode(85, "ADI");
        //public static NoteSubjectCode TypeOfAssetsAndLiabilities = new NoteSubjectCode(86, "ADJ");
        //public static NoteSubjectCode PromotionInformation = new NoteSubjectCode(87, "ADK");
        //public static NoteSubjectCode MeterCondition = new NoteSubjectCode(88, "ADL");
        //public static NoteSubjectCode MeterReadingInformation = new NoteSubjectCode(89, "ADM");
        //public static NoteSubjectCode TypeOfTransactionReason = new NoteSubjectCode(90, "ADN");
        //public static NoteSubjectCode TypeOfSurveyQuestion = new NoteSubjectCode(91, "ADO");
        //public static NoteSubjectCode CarriersAgentCounterInformation = new NoteSubjectCode(92, "ADP");


        //public static NoteSubjectCode SupplierNotes = new NoteSubjectCode(1, "SUR");
        //public static NoteSubjectCode RegulatoryInformation = new NoteSubjectCode(2, "REG");
        //public static NoteSubjectCode LegalInformation = new NoteSubjectCode(3, "ABL");
        //public static NoteSubjectCode TaxInformation = new NoteSubjectCode(4, "TXD");
        //public static NoteSubjectCode CustomsInformation = new NoteSubjectCode(5, "CUS");
        //public static NoteSubjectCode PMD = new NoteSubjectCode(6, "PMD");
        //public static NoteSubjectCode PMT = new NoteSubjectCode(7, "PMT");
        //public static NoteSubjectCode AAB = new NoteSubjectCode(8, "AAB");

        public NoteSubjectCode(int id, string name) : base(id, name) { }
    }
}
