using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    public class MinimumInvoiceTests
    {
        private readonly string _mainDir = Path.Combine(
            System.IO.Directory.GetCurrentDirectory(),
            "Invoices",
            "Minimum"
        );

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [Test]
        public void ExtractData_Facture_FR_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_FR_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0010"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171113")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter
                    is null
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(470.15)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(671.15)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(624.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(46.25m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_UE_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_UE_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0008"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171103")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter
                    is null
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("Me gusta olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("COMPRA0832")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(1453.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(2076.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(2076.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(0.0m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220023_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220023"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(104.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(104.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(4.90m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220024_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220024"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(108.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(108.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(8.00m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220025_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220025"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(120.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(20.00m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220026_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220026"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(0.00m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220027_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220027"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(110.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220028_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220028"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("381"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(110.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220029_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220029"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(-90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(-100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(-100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(0.00m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220030_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220030"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(0.00m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220031_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220031"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .BusinessProcessSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("A1")
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(107.82)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(107.82)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.80)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(7.02m)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Avoir_FR_type380_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(_mainDir, "Avoir_FR_type380_MINIMUM.pdf")
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("AV-2017-0005"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171116")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter
                    is null
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11999999998")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(-233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(-233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(-218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(-14.99)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Avoir_FR_type381_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(_mainDir, "Avoir_FR_type381_MINIMUM.pdf")
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("AV-2017-0005"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171116")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("381"));

            Assert.That(
                invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter
                    is null
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11999999998")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(14.99)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }

        [Test]
        public void ExtractData_Facture_DOM_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_DOM_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0009"));
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171105")
            );
            Assert.That(
                invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(
                invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter
                    is null
            );

            Assert.That(
                invoice
                    ?.ExchangedDocumentContext
                    .GuidelineSpecifiedDocumentContextParameter
                    .ID
                    .Value,
                Is.EqualTo("urn:factur-x.eu:1p0:minimum")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty,
                Is.InstanceOf<FacturX.SpecificationModels.Minimum.TradeParty>()
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .Value,
                Is.EqualTo("FR11999999998")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .SpecifiedTaxRegistration
                    .ID
                    .SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .SellerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .Name,
                Is.EqualTo("Hôtel Saint Denis")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("34343434600010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeAgreement
                    .BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("BC543")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .DuePayableAmount
                    .Value,
                Is.EqualTo(383.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .GrandTotalAmount
                    .Value,
                Is.EqualTo(530.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(530.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .SpecifiedTradeSettlementHeaderMonetarySummation
                    .TaxTotalAmount
                    .CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    .ApplicableHeaderTradeSettlement
                    .InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );
        }
    }
}
