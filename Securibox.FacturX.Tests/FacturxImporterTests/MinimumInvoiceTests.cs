using NUnit.Framework;
using System.Linq;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    public class MinimumInvoiceTests
    {
        private readonly string _mainDir = $"{System.IO.Directory.GetCurrentDirectory()?.Split("\\bin")?.ElementAtOrDefault(0)}\\Invoices\\Minimum\\";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ExtractData_Facture_FR_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_FR_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0010", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171113", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.IsNull(invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("Au bon moulin", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("99999999800010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("Ma jolie boutique", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("78787878400035", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO445", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(470.15, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(671.15, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(624.90, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(46.25m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_UE_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_UE_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0008", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171103", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.IsNull(invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("Au bon moulin", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("99999999800010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("Me gusta olive", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization);

            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("COMPRA0832", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(1453.76, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(2076.76, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(2076.76, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.0m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220023_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220023", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(104.90, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(104.90, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(4.90m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220024_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220024", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(108.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(108.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(8.00m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220025_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220025", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(120.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(20.00m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220026_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220026", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(90.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220027_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220027", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(110.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220028_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220028", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("381", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(110.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);
            
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220029_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220029", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(-90.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(-100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(-100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220030_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220030", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(90.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(100.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.0, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_F20220031_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("F20220031", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.AreEqual("A1", invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("LE FOURNISSEUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR11123456782", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("LE CLIENT", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("SERVEXEC", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(107.82, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(107.82, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(100.80, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(7.02m, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Avoir_FR_type380_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Avoir_FR_type380_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("AV-2017-0005", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171116", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.IsNull(invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("Au bon moulin", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("99999999800010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR11999999998", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("Ma jolie boutique", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("78787878400035", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO445", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(-233.47, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(-233.47, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(-218.48, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(-14.99, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Avoir_FR_type381_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Avoir_FR_type381_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("AV-2017-0005", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171116", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("381", invoice.ExchangedDocument.TypeCode);

            Assert.IsNull(invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("Au bon moulin", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("99999999800010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR11999999998", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);

            Assert.AreEqual("Ma jolie boutique", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("78787878400035", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO445", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(233.47, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(233.47, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(218.48, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(14.99, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }

        [Test]
        public void ExtractData_Facture_DOM_MINIMUM_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_DOM_MINIMUM.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0009", invoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171105", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice.ExchangedDocument.TypeCode);

            Assert.IsNull(invoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:factur-x.eu:1p0:minimum", invoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.IsTrue(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty is FacturX.SpecificationModels.Minimum.TradeParty);
            Assert.AreEqual("Au bon moulin", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);

            Assert.AreEqual("99999999800010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR11999999998", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("FR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("Hôtel Saint Denis", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("34343434600010", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("BC543", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(383.75, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);

            Assert.AreEqual(530.75, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);

            Assert.AreEqual(530.75, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00, invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Value);
            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.CurrencyID);

            Assert.AreEqual("EUR", invoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);
        }
    }
}