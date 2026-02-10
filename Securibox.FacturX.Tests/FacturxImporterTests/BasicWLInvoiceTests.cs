using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    public class BasicWLInvoiceTests
    {
        private readonly string _mainDir = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Invoices", "BasicWL");

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [Test]
        public void ExtractData_Facture_FR_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_FR_BASICWL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0010"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20171113"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);

            Assert.That(invoice?.ExchangedDocument.IncludedNote.First().Content, Is.EqualTo("Franco de port (commande > 300 € HT)"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter is null);

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Au bon moulin"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("99999999800010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("84340"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("1242 chemin de l'olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Malaucène"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11999999998"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Ma jolie boutique"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("78787878400035"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("69001"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue de la République"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Lyon"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO445"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("MSPE2017"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("FA-2017-0010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR2012421242124212421242124"));

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(16.38));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(81.90));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(29.87));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(543.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(5.50));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20171213"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(624.90));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(624.90));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.Value, Is.EqualTo(46.25));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(671.15));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(201.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(470.15));
        }

        [Test]
        public void ExtractData_Facture_UE_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_UE_BASICWL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0008"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20171103"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("Free shipping (amount > 300 €)"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter is null);

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Au bon moulin"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("99999999800010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("84340"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("1242 chemin de l'olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Malaucène"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11999999998"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Me gusta olive"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("41700"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("87 camino de la calor"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Dos Hermanas"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("ES"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("ESA12345674"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("COMPRA0832"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("FROLIVE2017"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("Me gusta olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("41700"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("87 camino de la calor"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Dos Hermanas"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("ES"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20170311"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("FA-2017-0008"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR2012421242124212421242124"));

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(2076.76));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason, Is.EqualTo("French VAT exemption according to articles 262 ter I (for products) and/or 283-2 (for services) of \"CGI\""));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20171203"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(2076.76));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(2076.76));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAt(0).Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAt(0).CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(2076.76));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(623.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(1453.76));
        }

        [Test]
        public void ExtractData_Facture_F20220023_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220023"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID.ElementAtOrDefault(0)?.Value, Is.EqualTo("123"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(4));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).Value, Is.EqualTo("12345678200077"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).SchemeID, Is.EqualTo("0009"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).Value, Is.EqualTo("DUNS1235487"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).SchemeID, Is.EqualTo("0060"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).Value, Is.EqualTo("ODETTE254879"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).SchemeID, Is.EqualTo("0177"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("MON ADRESSE LIGNE 1"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Buyer line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("MA VILLE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.Name, Is.EqualTo("SELLER TAX REP"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID.ElementAt(0).Value, Is.EqualTo("PRIVATE_ID_DEL"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL ADRESSE LIGNE 1"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value, Is.EqualTo("CREDID"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("587451236586"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID.Value, Is.EqualTo("LOC BANK ACCOUNT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount.IBANID.Value, Is.EqualTo("FRDEBIT"));


            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(2.20));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(11.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(60.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("E"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReason, Is.EqualTo("DEBOURS"));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReasonCode, Is.EqualTo("VATEX-EU-79-C"));

            Assert.That(taxDistributionList?.ElementAt(2).CalculatedAmount.Value, Is.EqualTo(2.70));
            Assert.That(taxDistributionList?.ElementAt(2).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(2).BasisAmount.Value, Is.EqualTo(27.00));
            Assert.That(taxDistributionList?.ElementAt(2).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(2).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(2).RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(taxDistributionList?.ElementAt(3).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(3).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(3).BasisAmount.Value, Is.EqualTo(2.00));
            Assert.That(taxDistributionList?.ElementAt(3).CategoryCode, Is.EqualTo("K"));
            Assert.That(taxDistributionList?.ElementAt(3).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(3).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(3).ExemptionReason, Is.EqualTo("LIVRAISON INTRACOMMUNAUTAIRE"));
            Assert.That(taxDistributionList?.ElementAt(3).ExemptionReasonCode, Is.EqualTo("VATEX-EU-IC"));


            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent, Is.EqualTo(5.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode, Is.EqualTo("95"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator is false);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CalculationPercent, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode, Is.EqualTo("100"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator is false);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CalculationPercent, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode, Is.EqualTo("100"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator is false);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CalculationPercent, Is.EqualTo(2.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value, Is.EqualTo(2.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator is true);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CalculationPercent, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode, Is.EqualTo("FC"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator is true);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CalculationPercent, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ReasonCode, Is.EqualTo("ADR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator is true);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CalculationPercent, Is.EqualTo(2.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value, Is.EqualTo(2.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ReasonCode, Is.EqualTo("FC"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.CategoryCode, Is.EqualTo("K"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ChargeIndicator.Indicator is true);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CalculationPercent, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ActualAmount.Value, Is.EqualTo(1.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ReasonCode, Is.EqualTo("FC"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value, Is.EqualTo("MANDATE PT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(14.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(9.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(4.90m));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(104.90));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(104.90));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAtOrDefault(0)?.IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAtOrDefault(0)?.FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAtOrDefault(0)?.FormattedIssueDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220024_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220024"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID.FirstOrDefault()?.Value, Is.EqualTo("123"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Buyer line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.Name, Is.EqualTo("SELLER TAX REP"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value, Is.EqualTo("CREDID"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID.ElementAtOrDefault(0)?.Value, Is.EqualTo("123"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID.Value, Is.EqualTo("LOC BANK ACCOUNT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount.IBANID.Value, Is.EqualTo("FRDEBIT"));

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(8.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(40.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(60.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("E"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReason, Is.EqualTo("VAT EXEMP"));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReasonCode, Is.EqualTo("VATEX-EU-D"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent, Is.EqualTo(5.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode, Is.EqualTo("95"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CalculationPercent, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode, Is.EqualTo("FC"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value, Is.EqualTo("MANDATE PT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(8.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(108.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(108.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220025_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220025"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineThree is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineThree is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL ADRESSE LIGNE 1"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(20.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).CalculationPercent, Is.EqualTo(0.0));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(20.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(120.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(20.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220026_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220026"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL 58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("BERLIN"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason, Is.EqualTo("LIVRAISON INTRACOMMUNAUTAIRE"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode, Is.EqualTo("VATEX-EU-IC"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("K"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("K"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAt(0).Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAt(0).CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(90.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220027_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220027"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL 58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(10.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(10.00));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(110.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220028_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220028"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("381"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL 58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(10.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(10.00));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(110.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220029_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220029"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(1));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL 58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("BERLIN"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(-100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode, Is.EqualTo("VATEX-EU-IC"));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(-5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("K"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(-10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("K"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(-95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(-10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(-5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(-100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(-100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(-10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(-90.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format, Is.EqualTo("102"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220030_BASICWL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220030"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID.FirstOrDefault()?.Value, Is.EqualTo("123"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(4));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).Value, Is.EqualTo("12345678200077"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).SchemeID, Is.EqualTo("0009"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).Value, Is.EqualTo("DUNS1235487"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).SchemeID, Is.EqualTo("0060"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL 58 rue de la mer"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("BERLIN"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("DE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("587451236586"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("O"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason, Is.EqualTo("HORS SCOPE TVA"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode, Is.EqualTo("VATEX-EU-O"));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(taxDistributionList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("O"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.0));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("O"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(0.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(95.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(5.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.0));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(100.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(10.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(90.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("F20220003"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20220101"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Facture_F20220031_BASIC_WL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_BASIC_WL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("F20220031"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20220131"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR"));
            Assert.That(noteList!.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList!.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList!.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(noteList!.ElementAt(2).Content, Is.EqualTo("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"));
            Assert.That(noteList!.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(noteList!.ElementAt(3).Content, Is.EqualTo("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "));
            Assert.That(noteList!.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(noteList!.ElementAt(4).Content, Is.EqualTo("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."));
            Assert.That(noteList!.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(noteList!.ElementAt(5).Content, Is.EqualTo("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."));
            Assert.That(noteList!.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference, Is.EqualTo("SERVEXEC"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("LE FOURNISSEUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.ID.FirstOrDefault()?.Value, Is.EqualTo("123"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.Count(), Is.EqualTo(4));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).Value, Is.EqualTo("12345678200077"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(1).SchemeID, Is.EqualTo("0009"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).Value, Is.EqualTo("DUNS1235487"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(2).SchemeID, Is.EqualTo("0060"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).Value, Is.EqualTo("587451236587"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(3).SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName, Is.EqualTo("SELLER TRADE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75018"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue d'ici"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Seller line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Seller line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("moi@seller.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.FirstOrDefault()?.Value, Is.EqualTo("3654789851"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.GlobalID.FirstOrDefault()?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("LE CLIENT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("987654321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("MON ADRESSE LIGNE 1"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("Buyer line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineThree, Is.EqualTo("Buyer line 3"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("MA VILLE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("me@buyer.com"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR 05 987 654 321"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTaxRepresentativeTradeParty is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO201925478"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("CT2018120802"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID.FirstOrDefault()?.Value, Is.EqualTo("PRIVATE_ID_DEL"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name, Is.EqualTo("DEL Name"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("06000"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("DEL ADRESSE LIGNE 1"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo, Is.EqualTo("DEL line 2"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName, Is.EqualTo("NICE"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value, Is.EqualTo("20220128"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("DESPADV002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value, Is.EqualTo("20220101"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value, Is.EqualTo("20221231"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value, Is.EqualTo("CREDID"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("F20180023BUYER"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.Value, Is.EqualTo("587451236586"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.ElementAtOrDefault(0)?.SchemeID, Is.EqualTo("0088"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name, Is.EqualTo("PAYEE NAME"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("123456782"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR76 1254 2547 2569 8542 5874 698"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.ProprietaryID.Value, Is.EqualTo("LOC BANK ACCOUNT"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayerPartyDebtorFinancialAccount.IBANID.Value, Is.EqualTo("FRDEBIT"));

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(5.88));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(29.40));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReason, Is.EqualTo("DEBOURS"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(60.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("E"));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReasonCode, Is.EqualTo("VATEX-EU-79-C"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(taxDistributionList?.ElementAt(2).CalculatedAmount.Value, Is.EqualTo(1.14));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(2).BasisAmount.Value, Is.EqualTo(11.40));
            Assert.That(taxDistributionList?.ElementAt(2).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(2).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(2).RateApplicablePercent, Is.EqualTo(10.00));

            var allowanceChargeList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge;
            Assert.That(allowanceChargeList is not null);

            Assert.That(allowanceChargeList?.ElementAt(0).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(0).CalculationPercent, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(28.00));
            Assert.That(allowanceChargeList?.ElementAt(0).ActualAmount.Value, Is.EqualTo(1.40));
            Assert.That(allowanceChargeList?.ElementAt(0).ReasonCode, Is.EqualTo("100"));
            Assert.That(allowanceChargeList?.ElementAt(0).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(0).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(allowanceChargeList?.ElementAt(1).ChargeIndicator.Indicator is false);
            Assert.That(allowanceChargeList?.ElementAt(1).CalculationPercent, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(12.00));
            Assert.That(allowanceChargeList?.ElementAt(1).ActualAmount.Value, Is.EqualTo(1.20));
            Assert.That(allowanceChargeList?.ElementAt(1).ReasonCode, Is.EqualTo("100"));
            Assert.That(allowanceChargeList?.ElementAt(1).Reason, Is.EqualTo("REMISE COMMERCIALE"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(1).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(allowanceChargeList?.ElementAt(2).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(2).CalculationPercent, Is.EqualTo(10.00));
            Assert.That(allowanceChargeList?.ElementAt(2).BasisAmount.Value, Is.EqualTo(28.00));
            Assert.That(allowanceChargeList?.ElementAt(2).ActualAmount.Value, Is.EqualTo(2.80));
            Assert.That(allowanceChargeList?.ElementAt(2).ReasonCode, Is.EqualTo("FC"));
            Assert.That(allowanceChargeList?.ElementAt(2).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(2).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(2).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(2).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(allowanceChargeList?.ElementAt(3).ChargeIndicator.Indicator is true);
            Assert.That(allowanceChargeList?.ElementAt(3).CalculationPercent, Is.EqualTo(5.00));
            Assert.That(allowanceChargeList?.ElementAt(3).BasisAmount.Value, Is.EqualTo(12.00));
            Assert.That(allowanceChargeList?.ElementAt(3).ActualAmount.Value, Is.EqualTo(0.60));
            Assert.That(allowanceChargeList?.ElementAt(3).ReasonCode, Is.EqualTo("ADR"));
            Assert.That(allowanceChargeList?.ElementAt(3).Reason, Is.EqualTo("FRAIS DEPLACEMENT"));
            Assert.That(allowanceChargeList?.ElementAt(3).CategoryTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(allowanceChargeList?.ElementAt(3).CategoryTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(allowanceChargeList?.ElementAt(3).CategoryTradeTax.RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20220302"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value, Is.EqualTo("MANDATE PT"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(100.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value, Is.EqualTo(3.40));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value, Is.EqualTo(2.60));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(100.80));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(7.02));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(107.82));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(107.82));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value, Is.EqualTo("BUYER ACCOUNT REF"));
        }

        [Test]
        public void ExtractData_Avoir_FR_type380_BASIC_WL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Avoir_FR_type380_BASICWL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("AV-2017-0005"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20171116"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter is null);

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Au bon moulin"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("99999999800010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("84340"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("1242 chemin de l'olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Malaucène"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11999999998"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Ma jolie boutique"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("78787878400035"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("69001"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue de la République"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Lyon"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR19787878784"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO445"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("MSPE2017"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery is not null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("AV-2017-0005"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(-4.10));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(-20.48));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(-10.89));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(-198.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(5.50));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20171116"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(-218.48));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(-218.48));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.Value, Is.EqualTo(-14.99));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.ElementAtOrDefault(0)?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(-233.47));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(-0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(-233.47));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("FA-2017-0010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20171113"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format, Is.EqualTo("102"));


        }

        [Test]
        public void ExtractData_Avoir_FR_type381_BASIC_WL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Avoir_FR_type381_BASICWL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("AV-2017-0005"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20171116"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("381"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter is null);

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Au bon moulin"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("99999999800010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("84340"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("1242 chemin de l'olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Malaucène"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11999999998"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Ma jolie boutique"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("78787878400035"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("69001"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("35 rue de la République"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Lyon"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR19787878784"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("PO445"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("MSPE2017"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery is not null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("AV-2017-0005"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans is null);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(4.10));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(20.48));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(10.89));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(198.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(5.50));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20171116"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(218.48));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(218.48));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.Value, Is.EqualTo(14.99));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(233.47));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(233.47));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value, Is.EqualTo("FA-2017-0010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value, Is.EqualTo("20171113"));
        }

        [Test]
        public void ExtractData_Facture_DOM_BASIC_WL_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine( _mainDir, "Facture_DOM_BASICWL.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;
            Assert.That(invoice is not null);

            Assert.That(invoice?.ExchangedDocument.ID.Value, Is.EqualTo("FA-2017-0009"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20171105"));
            Assert.That(invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(invoice?.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(noteList!.ElementAt(0).Content, Is.EqualTo("Franco de port (Commande > 300 € HT)"));

            Assert.That(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter is null);

            Assert.That(invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:factur-x.eu:1p0:basicwl"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Au bon moulin"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("99999999800010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("84340"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("1242 chemin de l'olive"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Malaucène"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR11999999998"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Hôtel Saint Denis"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("34343434600010"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("97400"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("42 rue du stade"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Saint Denis"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("RE"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ID.Value, Is.EqualTo("FR90343434346"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerReference is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("BC543"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value, Is.EqualTo("WELCOME_PACK_2017"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery is not null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty is null);
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument is null);

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference, Is.EqualTo("FA-2017-0009"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).TypeCode, Is.EqualTo("30"));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(0).PayeePartyCreditorFinancialAccount?.IBANID.Value, Is.EqualTo("FR2012421242124212421242124"));

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(530.75));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("G"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason, Is.EqualTo("Exonération de TVA selon article 262 I du Code général des impôts"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20171205"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value, Is.EqualTo(530.75));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value, Is.EqualTo(530.75));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.Value, Is.EqualTo(0.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount.FirstOrDefault()?.CurrencyID, Is.EqualTo("EUR"));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value, Is.EqualTo(530.75));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value, Is.EqualTo(147.00));
            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value, Is.EqualTo(383.75));

            Assert.That(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument is null);
        }
    }
}