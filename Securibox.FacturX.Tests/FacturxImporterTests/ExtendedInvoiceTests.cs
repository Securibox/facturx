using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    public class ExtendedInvoiceTests
    {
        private readonly string _mainDir = Path.Combine(System.IO.Directory.GetCurrentDirectory()?.Split("bin").First()!, "Invoices", "Extended");

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [Test]
        public void ExtractData_Avoir_FR_type380_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Avoir_FR_type380_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;
            Assert.AreEqual("AV-2017-0005", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171116", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.IsNotNull(noteList);
            Assert.AreEqual("Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé", noteList?.ElementAt(0).Content);

            Assert.IsNull(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line1?.SpecifiedTradeProduct);
            Assert.AreEqual("3518370400049", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("Nougat de l'Abbaye 250g", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("NOUG250", line1?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(4.55, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.IsNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.BasisAmount);
            Assert.IsFalse(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ChargeIndicator.Indicator);
            Assert.AreEqual(-0.45, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ActualAmount.Value);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(4.10, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(-5.000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(-20.48, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line2?.SpecifiedTradeProduct);
            Assert.AreEqual("HOLANCL", line2?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Huile d'olive à l'ancienne", line2?.SpecifiedTradeProduct.Name);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(19.80, line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(19.80, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(-10.000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("LTR", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(5.50, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(-198.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Au bon moulin", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("99999999800010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Tony Dubois", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 56", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("tony.dubois@aubonmoulin.fr", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("84340", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("1242 chemin de l'olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Malaucène", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Ma jolie boutique", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("78787878400035", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Alexandre Payet", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 67", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("alexandre.payet@majolieboutique.net", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR19787878784", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("PO445", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("MSPE2017", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("AV-2017-0005", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(-4.10, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(-20.48, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(-10.89, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual(-198.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(5.50, taxDistributionList!.ElementAt(1).RateApplicablePercent);

            Assert.AreEqual("Paiement immédiat", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.Description);
            Assert.AreEqual("20171116", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(-218.48, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);

            Assert.AreEqual(-233.47, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(-218.48, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(-14.99, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(-0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);

            Assert.AreEqual(-233.47, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("FA-2017-0010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20171113", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format);
        }

        [Test]
        public void ExtractData_Avoir_FR_type381_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Avoir_FR_type381_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("AV-2017-0005", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171116", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("381", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.IsNotNull(noteList);
            Assert.AreEqual("Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé", noteList!.ElementAt(0).Content);

            Assert.IsNull(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line1?.SpecifiedTradeProduct);
            Assert.AreEqual("3518370400049", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("Nougat de l'Abbaye 250g", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("NOUG250", line1?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(4.55, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge);
            Assert.IsFalse(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ChargeIndicator.Indicator);
            Assert.AreEqual(0.45, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ActualAmount.Value);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(4.10, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(5.000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(20.48, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line2?.SpecifiedTradeProduct);
            Assert.AreEqual("HOLANCL", line2?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Huile d'olive à l'ancienne", line2?.SpecifiedTradeProduct.Name);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(19.80, line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(19.80, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(10.000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("LTR", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(5.50, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(198.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Au bon moulin", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("99999999800010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Tony Dubois", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 56", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("tony.dubois@aubonmoulin.fr", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("84340", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("1242 chemin de l'olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Malaucène", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Ma jolie boutique", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("78787878400035", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Alexandre Payet", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 67", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("alexandre.payet@majolieboutique.net", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR19787878784", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("PO445", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("MSPE2017", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("AV-2017-0005", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(4.10, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(20.48, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(10.89, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual(198.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(5.50, taxDistributionList!.ElementAt(1).RateApplicablePercent);

            Assert.AreEqual("Paiement immédiat", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.Description);

            Assert.AreEqual("20171116", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(218.48, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);

            Assert.AreEqual(233.47, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(218.48, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(14.99, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);

            Assert.AreEqual(233.47, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("FA-2017-0010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20171113", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format);
        }

        [Test]
        public void ExtractData_FACTURE_FR_type380_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_FR_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0010", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171113", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("Franco de port (commande > 300 € HT)", noteList!.ElementAt(0).Content);

            Assert.IsNull(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("3518370400049", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("NOUG250", line1?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Nougat de l'Abbaye 250g", line1?.SpecifiedTradeProduct.Name);

            Assert.AreEqual(4.55, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.IsFalse(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ChargeIndicator.Indicator);
            Assert.AreEqual(0.45, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ActualAmount.Value);

            Assert.AreEqual(4.10, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(20.000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(81.90, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("3518370200090", line2?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line2?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("Biscuits aux raisins 300g", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("BRAIS300", line2?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(15.000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(5.50, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(48.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Huile d'olive à l'ancienne", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("HOLANCL", line3?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(25.000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("LTR", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(5.50, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(495.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Au bon moulin", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("99999999800010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("84340", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("1242 chemin de l'olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Malaucène", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Tony Dubois", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 56", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("tony.dubois@aubonmoulin.fr", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("Ma jolie boutique", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("78787878400035", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);

            Assert.AreEqual("Alexandre Payet", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 67", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("alexandre.payet@majolieboutique.net", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR19787878784", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("PO445", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("MSPE2017", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);
            Assert.AreEqual("69001", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue de la République", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Lyon", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent);

            Assert.AreEqual("FA-2017-0010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("Virement sur compte Banque Fiducial", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.Information);
            Assert.AreEqual("FIDCFR21XXX", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution.BICID.Value);
            Assert.AreEqual("FR2012421242124212421242124", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(16.38, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(81.90, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReasonCode);

            Assert.AreEqual(29.87, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual(543.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("5", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(5.50, taxDistributionList!.ElementAt(1).RateApplicablePercent);
            Assert.IsNull(taxDistributionList!.ElementAt(1).ExemptionReason);
            Assert.IsNull(taxDistributionList!.ElementAt(1).ExemptionReasonCode);

            Assert.AreEqual("30% d'acompte, solde à 30 j", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.Description);
            Assert.AreEqual("20171213", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);

            Assert.AreEqual(624.90, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);

            Assert.AreEqual(624.90, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(46.25, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(671.15, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(201.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(470.15, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);
        }

        [Test]
        public void ExtractData_FACTURE_EU_type380_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_UE_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0008", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171103", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("Free shipping (amount > 300 €)", noteList!.ElementAt(0).Content);

            Assert.IsNull(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("3518370400049", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("Nougat de l'Abbaye 250g", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("NOUG250", line1?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.AreEqual(4.55, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.IsFalse(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ChargeIndicator.Indicator);
            Assert.AreEqual(0.45, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ActualAmount.Value);

            Assert.AreEqual(4.10, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(8.000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(32.76, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("3518370200090", line2?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line2?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("Biscuits aux raisins 300g", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("BRAIS300", line2?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(20.000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(64.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);
            Assert.AreEqual("Huile d'olive à l'ancienne", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("HOLANCL", line3?.SpecifiedTradeProduct.SellerAssignedID);

            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(100.000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("LTR", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(1980.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Au bon moulin", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("99999999800010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("84340", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("1242 chemin de l'olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Malaucène", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Tony Dubois", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 56", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("tony.dubois@aubonmoulin.fr", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("Me gusta olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization);

            Assert.AreEqual("41700", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("87 camino de la calor", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Dos Hermanas", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);
            Assert.AreEqual("ES", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("ESA12345674", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Pedro Sanchez", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+34 978 23 41 23", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("pedro@megustaolive.es", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("COMPRA0832", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("FROLIVE2017", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("Me gusta olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);
            Assert.AreEqual("41700", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("87 camino de la calor", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Dos Hermanas", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("ES", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20170311", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("FA-2017-0008", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("Credit transfer Banque Fiducial", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.Information);
            Assert.AreEqual("FIDCFR21XXX", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution.BICID.Value);
            Assert.AreEqual("FR2012421242124212421242124", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(2076.76, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("K", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);
            Assert.AreEqual("French VAT exemption according to articles 262 ter I (for products) and/or 283-2 (for services) of \"CGI\"", taxDistributionList!.ElementAt(0).ExemptionReason);

            Assert.AreEqual("30% advance payment, balance at 30 days", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.Description);
            Assert.AreEqual("20171203", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(2076.76, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);

            Assert.AreEqual(2076.76, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(2076.76, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(1453.76, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

        }

        [Test]
        public void ExtractData_Facture_DOM_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_DOM_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("FA-2017-0009", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20171105", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.IsNotNull(noteList);
            Assert.AreEqual("Franco de port (Commande > 300 € HT)", noteList!.ElementAt(0).Content);

            Assert.IsNull(invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line1?.SpecifiedTradeProduct);
            Assert.AreEqual("3518370400049", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("NOUG250", line1?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Nougat de l'Abbaye 250g", line1?.SpecifiedTradeProduct.Name);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(4.55, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);
            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge);
            Assert.IsFalse(line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ChargeIndicator.Indicator);
            Assert.AreEqual(0.45, line1?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.AppliedTradeAllowanceCharge.ActualAmount.Value);

            Assert.NotNull(line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(4.10, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(50.000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("G", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(204.75, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line2?.SpecifiedTradeProduct);
            Assert.AreEqual("3518370200090", line2?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0160", line2?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("BRAIS300", line2?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Biscuits aux raisins 300g", line2?.SpecifiedTradeProduct.Name);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.NotNull(line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(3.20, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(40.000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("G", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(128.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);
            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.NotNull(line3?.SpecifiedTradeProduct);
            Assert.AreEqual("HOLANCL", line3?.SpecifiedTradeProduct.SellerAssignedID);
            Assert.AreEqual("Huile d'olive à l'ancienne", line3?.SpecifiedTradeProduct.Name);

            Assert.NotNull(line3?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice);
            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value);

            Assert.NotNull(line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice);
            Assert.AreEqual(19.80, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(10.000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("LTR", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("G", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(198.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("Au bon moulin", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("99999999800010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Tony Dubois", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 4 72 07 08 56", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("tony.dubois@aubonmoulin.fr", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("84340", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("1242 chemin de l'olive", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Malaucène", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11999999998", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Hôtel Saint Denis", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("34343434600010", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Stéphanie Hoarau", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("+33 2 62 94 26 01", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("achats@hotelsaintdenis.re", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);
            Assert.AreEqual("SMTP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("RE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("97400", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("42 rue du stade", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Saint Denis", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR90343434346", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("BC543", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("WELCOME_PACK_2017", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("97400", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("42 rue du stade", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineThree);
            Assert.AreEqual("Saint Denis de la réunion", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("RE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod);

            Assert.AreEqual("FA-2017-0009", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("Virement sur compte Banque Fiducial", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.Information);
            Assert.AreEqual("FIDCFR21XXX", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution.BICID.Value);
            Assert.AreEqual("FR2012421242124212421242124", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual("Exonération de TVA selon article 262 I du Code général des impôts", taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(530.75, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("G", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual("30% d'acompte, solde à 30 j", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.Description);
            Assert.AreEqual("20171205", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(530.75, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);

            Assert.AreEqual(530.75, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(530.75, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(147.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);

            Assert.AreEqual(383.75, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument);
        }

        [Test]
        public void ExtractData_Facture_F20220023_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220023", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("E", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual(100.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).Reason);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(30.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(2.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("ADL", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason);

            Assert.AreEqual(28.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("2", line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(7.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(7.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("123", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(4, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("12345678200077", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(1).Value);
            Assert.AreEqual("0009", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(1).SchemeID);

            Assert.AreEqual("DUNS1235487", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(2).Value);
            Assert.AreEqual("0060", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(2).SchemeID);

            Assert.AreEqual("ODETTE254879", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(3).Value);
            Assert.AreEqual("0177", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(3).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Seller line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("M. CONTACT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("DEP SELLER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 02 03 54 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("seller@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("MON ADRESSE LIGNE 1", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Buyer line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("MA VILLE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Buyer contact name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("Buyer dep", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 01 25 45 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("buyer@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("SELLER TAX REP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.Name);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Seller line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);
            Assert.AreEqual("IT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).ReferenceTypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("PRIVATE_ID_DEL", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID);
            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL ADRESSE LIGNE 1", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.AreEqual("CREDID", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("PAYEE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name);

            Assert.AreEqual("587451236586", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.AreEqual("LOC BANK ACCOUNT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID.Value);
            Assert.AreEqual("FRDEBIT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayerPartyDebtorFinancialAccount.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(2.20, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(11.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual(60.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("E", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).RateApplicablePercent);
            Assert.AreEqual("DEBOURS", taxDistributionList!.ElementAt(1).ExemptionReason);
            Assert.AreEqual("VATEX-EU-79-C", taxDistributionList!.ElementAt(1).ExemptionReasonCode);

            Assert.AreEqual(2.70, taxDistributionList!.ElementAt(2).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(2).TypeCode);
            Assert.AreEqual(27.00, taxDistributionList!.ElementAt(2).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(2).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(2).DueDateTypeCode);
            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(2).RateApplicablePercent);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(3).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(3).TypeCode);
            Assert.AreEqual(2.00, taxDistributionList!.ElementAt(3).BasisAmount.Value);
            Assert.AreEqual("K", taxDistributionList!.ElementAt(3).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(3).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(3).RateApplicablePercent);
            Assert.AreEqual("LIVRAISON INTRACOMMUNAUTAIRE", taxDistributionList!.ElementAt(3).ExemptionReason);
            Assert.AreEqual("VATEX-EU-IC", taxDistributionList!.ElementAt(3).ExemptionReasonCode);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("95", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("100", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).BasisAmount.Value);
            Assert.AreEqual("100", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(2.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).BasisAmount.Value);
            Assert.AreEqual(2.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).BasisAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("FC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(4).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).BasisAmount.Value);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("ADR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(5).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(2.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).BasisAmount.Value);
            Assert.AreEqual(2.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("FC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.TypeCode);
            Assert.AreEqual("K", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(6).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).BasisAmount.Value);
            Assert.AreEqual(1.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ActualAmount.Value);
            Assert.AreEqual("FC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(7).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("MANDATE PT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(14.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(9.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.0, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(4.90, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(104.90, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(104.90, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("F20220003", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220024_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220024", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("E", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("ADL", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("ADL", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ReasonCode);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ActualAmount.Value);
            Assert.AreEqual("ADL", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ReasonCode);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(30.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(2.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("ADL", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("ADL", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("ADL", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ReasonCode);

            Assert.AreEqual(28.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("2", line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(7.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(7.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("123", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Seller line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("M. CONTACT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("01 02 03 54 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("seller@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Buyer contact name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("Buyer dep", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 01 25 45 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("buyer@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Buyer line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("Buyer contact name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("Buyer dep", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 01 25 45 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("buyer@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("SELLER TAX REP", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.Name);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Seller line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod);

            Assert.AreEqual("CREDID", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("123", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID);

            Assert.AreEqual("PAYEE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.AreEqual("LOC BANK ACCOUNT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID.Value);
            Assert.AreEqual("FRDEBIT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayerPartyDebtorFinancialAccount.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(8.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(40.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual(60.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("E", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).RateApplicablePercent);
            Assert.AreEqual("VAT EXEMP", taxDistributionList!.ElementAt(1).ExemptionReason);
            Assert.AreEqual("VATEX-EU-D", taxDistributionList!.ElementAt(1).ExemptionReasonCode);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("95", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CalculationPercent);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual("MANDATE PT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.0, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(8.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(108.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(108.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220025_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220025", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);
            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(10.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(30.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("2", line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(5.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual(5.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject);

            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL ADRESSE LIGNE 1", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID);

            Assert.AreEqual("PAYEE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual(100.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.0, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(120.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220026_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220026", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(10.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(30.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.IsNull(line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument);

            Assert.AreEqual(5.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime);
            Assert.AreEqual("20220131", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.AreEqual(5.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("DE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(3, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count());

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL 58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("BERLIN", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("DE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.AreEqual("LIVRAISON INTRACOMMUNAUTAIRE", taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(100.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("K", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("VATEX-EU-IC", taxDistributionList!.ElementAt(0).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("K", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("K", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(90.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("F20220003", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220027_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220027", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(10.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(30.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.IsNull(line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument);

            Assert.AreEqual(5.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime);
            Assert.AreEqual("20220131", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.AreEqual(5.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(3, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count());

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL 58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(100.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(110.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("F20220003", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220028_BasicWL_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220028", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("381", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(10.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(30.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.IsNull(line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument);

            Assert.AreEqual(5.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime);
            Assert.AreEqual("20220131", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);

            Assert.AreEqual(5.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(3, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count());

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL 58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(100.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID);

            Assert.AreEqual(95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(110.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("F20220003", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Format);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220029_Extended_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220029", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("PRESTATION SUPPORT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(-1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsNull(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(-60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(10.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(-3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime);

            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge);

            Assert.AreEqual(-30.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.IsNull(line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument);

            Assert.AreEqual(5.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(-1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("K", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime);
            Assert.AreEqual("20220131", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.AreEqual(-5.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(1, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.AreEqual("DE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);
            Assert.AreEqual("FR 05 987 654 321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual(3, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count());

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL 58 rue de la mer", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("BERLIN", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("DE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeeSpecifiedCreditorFinancialInstitution);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(-100.00, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("K", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.AreEqual("VATEX-EU-IC", taxDistributionList!.ElementAt(0).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(-5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("K", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(-10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("K", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID);

            Assert.AreEqual(-95.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(-10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(-5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(-100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(-100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(-10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(-90.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.AreEqual("F20220003", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(0).FormattedIssueDateTime.DateTimeString.Value);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }

        [Test]
        public void ExtractData_Facture_F20220030_Extended_FAILURE_INVALID_AGAINST_SCHEMATRON()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));
            Assert.Throws<Exception>(() => importer.ImportDataWithDeserialization());
        }

        [Test]
        public void ExtractData_Facture_F20220031_BasicWL_SUCCESS()
        {
            var importer = new FacturxImporter(string.Format("{0}\\{1}", _mainDir, "Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EXTENDED.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.NotNull(crossIndustryInvoice);

            var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Extended.CrossIndustryInvoice;

            Assert.AreEqual("F20220031", invoice?.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", invoice?.ExchangedDocument.TypeCode);

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.NotNull(noteList);
            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", noteList!.ElementAt(0).Content);
            Assert.AreEqual("REG", noteList!.ElementAt(0).SubjectCode);
            Assert.AreEqual("RCS MAVILLE 123 456 782", noteList?.ElementAt(1).Content);
            Assert.AreEqual("ABL", noteList?.ElementAt(1).SubjectCode);
            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", noteList?.ElementAt(2).Content);
            Assert.AreEqual("AAI", noteList?.ElementAt(2).SubjectCode);
            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. ", noteList?.ElementAt(3).Content);
            Assert.AreEqual("PMD", noteList?.ElementAt(3).SubjectCode);
            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", noteList?.ElementAt(4).Content);
            Assert.AreEqual("PMT", noteList?.ElementAt(4).SubjectCode);
            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", noteList?.ElementAt(5).Content);
            Assert.AreEqual("AAB", noteList?.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", invoice?.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended", invoice?.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            var line1 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.AreEqual("1", line1?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("598785412598745", line1?.SpecifiedTradeProduct.GlobalID.Value);
            Assert.AreEqual("0088", line1?.SpecifiedTradeProduct.GlobalID.SchemeID);
            Assert.AreEqual("REMBOURSEMENT AFFRANCHISSEMENT", line1?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line1?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("1", line1?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(60.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(1.0000, line1?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line1?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("E", line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(0.00, line1?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220101", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line1?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual(100.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason);

            Assert.IsTrue(line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line1?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7).Reason);

            Assert.AreEqual(60.00, line1?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line2 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.AreEqual("2", line2?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("FOURNITURES DIVERSES", line2?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line2?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("3", line2?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(30.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);
            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode);

            Assert.AreEqual(3.0000, line2?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line2?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(20.00, line2?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220131", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20220131", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", line2?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(2.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.IsNull(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount);
            Assert.AreEqual(0.0, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("71", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("REMISE VOLUME", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);

            Assert.IsFalse(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("100", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ActualAmount.Value);
            Assert.AreEqual("ADL", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4).ReasonCode);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5).Reason);

            Assert.IsTrue(line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ChargeIndicator.Indicator);
            Assert.AreEqual(1.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).ActualAmount.Value);
            Assert.AreEqual("FRAIS PALETTE", line2?.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6).Reason);

            Assert.AreEqual(28.00, line2?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            var line3 = invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.AreEqual("3", line3?.AssociatedDocumentLineDocument.LineID.Value);

            Assert.AreEqual("APPEL", line3?.SpecifiedTradeProduct.Name);
            Assert.AreEqual("Description", line3?.SpecifiedTradeProduct.Description);

            Assert.AreEqual("2", line3?.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value);

            Assert.AreEqual(12.0000, line3?.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value);

            Assert.AreEqual(1.0000, line3?.SpecifiedLineTradeDelivery.BilledQuantity.Value);
            Assert.AreEqual("C62", line3?.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode);

            Assert.AreEqual("VAT", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode);
            Assert.AreEqual("S", line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode);
            Assert.AreEqual(10.00, line3?.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent);

            Assert.IsNull(line3?.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod);

            Assert.AreEqual(12.00, line3?.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value);

            Assert.AreEqual("SERVEXEC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference);

            Assert.AreEqual("LE FOURNISSEUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.Name);

            Assert.AreEqual("123", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID);

            Assert.AreEqual(4, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count());

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(0).SchemeID);

            Assert.AreEqual("12345678200077", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(1).Value);
            Assert.AreEqual("0009", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(1).SchemeID);

            Assert.AreEqual("DUNS1235487", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(2).Value);
            Assert.AreEqual("0060", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(2).SchemeID);

            Assert.AreEqual("587451236587", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(3).Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(3).SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedLegalOrganization.TradingBusinessName);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Seller line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("PARIS", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("moi@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);
            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);

            Assert.AreEqual("M. CONTACT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("DEP SELLER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 02 03 54 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("seller@seller.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("3654789851", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("LE CLIENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.Name);

            Assert.AreEqual("987654321", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("Buyer contact name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.PersonName);
            Assert.AreEqual("Buyer dep", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.DepartmentName);
            Assert.AreEqual("01 01 25 45 87", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.TelephoneUniversalCommunication.CompleteNumber);
            Assert.AreEqual("buyer@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.DefinedTradeContact.EmailURIUniversalCommunication.URIID.Value);

            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("MON ADRESSE LIGNE 1", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineOne);
            Assert.AreEqual("Buyer line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineTwo);
            Assert.AreEqual("Buyer line 3", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.LineThree);
            Assert.AreEqual("MA VILLE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.PostalTradeAddress.CityName);

            Assert.AreEqual("me@buyer.com", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("EM", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty?.URIUniversalCommunication.URIID.SchemeID);

            Assert.AreEqual("FR11123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.Value);
            Assert.AreEqual("VA", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()?.ID.SchemeID);

            Assert.AreEqual("PO201925478", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.ContractReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("SALES REF 2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerOrderReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("SUPPort doc", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).IssuerAssignedID.Value);
            Assert.AreEqual("url:gffter", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).URIID.Value);
            Assert.AreEqual("916", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).TypeCode);
            Assert.AreEqual("support descript", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(0).Name);

            Assert.AreEqual("TENDER-002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).IssuerAssignedID.Value);
            Assert.AreEqual("50", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(1).TypeCode);

            Assert.AreEqual("REFCLI0215", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).IssuerAssignedID.Value);
            Assert.AreEqual("130", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).TypeCode);
            Assert.AreEqual("IT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(2).ReferenceTypeCode);

            Assert.AreEqual("PROJET2547", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.ID.Value);
            Assert.AreEqual("Project reference", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SpecifiedProcuringProject.Name);

            Assert.AreEqual("PRIVATE_ID_DEL", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID);
            Assert.AreEqual("DEL Name", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);

            Assert.AreEqual("06000", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL ADRESSE LIGNE 1", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("FR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);

            Assert.AreEqual("20220128", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("RECEIV-ADV002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ReceivingAdviceReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("20220101", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.StartDateTime.DateTimeString.Format);
            Assert.AreEqual("20221231", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.BillingSpecifiedPeriod.EndDateTime.DateTimeString.Format);

            Assert.AreEqual("CREDID", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.CreditorReferenceID.Value);
            Assert.AreEqual("F20180023BUYER", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PaymentReference);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceCurrencyCode);

            Assert.AreEqual("PAYEE NAME", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.Name);

            Assert.AreEqual("587451236586", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID?.FirstOrDefault()?.Value);
            Assert.AreEqual("0088", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID?.FirstOrDefault()?.SchemeID);

            Assert.AreEqual("123456782", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.TypeCode);
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.IBANID.Value);
            Assert.AreEqual("LOC BANK ACCOUNT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayeePartyCreditorFinancialAccount?.FirstOrDefault()?.ProprietaryID.Value);
            Assert.AreEqual("FRDEBIT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.PayerPartyDebtorFinancialAccount.IBANID.Value);

            var taxDistributionList = invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ApplicableTradeTax;
            Assert.IsNotNull(taxDistributionList);

            Assert.AreEqual(5.88, taxDistributionList!.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(0).TypeCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReason);
            Assert.AreEqual(29.40, taxDistributionList!.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(0).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(0).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, taxDistributionList!.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(1).TypeCode);
            Assert.AreEqual("DEBOURS", taxDistributionList!.ElementAt(1).ExemptionReason);
            Assert.AreEqual(60.00, taxDistributionList!.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual("E", taxDistributionList!.ElementAt(1).CategoryCode);
            Assert.AreEqual("VATEX-EU-79-C", taxDistributionList!.ElementAt(1).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(1).DueDateTypeCode);
            Assert.AreEqual(0.00, taxDistributionList!.ElementAt(1).RateApplicablePercent);

            Assert.AreEqual(1.14, taxDistributionList!.ElementAt(2).CalculatedAmount.Value);
            Assert.AreEqual("VAT", taxDistributionList!.ElementAt(2).TypeCode);
            Assert.IsNull(taxDistributionList!.ElementAt(2).ExemptionReason);
            Assert.AreEqual(11.40, taxDistributionList!.ElementAt(2).BasisAmount.Value);
            Assert.AreEqual("S", taxDistributionList!.ElementAt(2).CategoryCode);
            Assert.IsNull(taxDistributionList!.ElementAt(2).ExemptionReasonCode);
            Assert.AreEqual("72", taxDistributionList!.ElementAt(2).DueDateTypeCode);
            Assert.AreEqual(10.00, taxDistributionList!.ElementAt(2).RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent);
            Assert.AreEqual(28.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual(1.40, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("100", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.IsFalse(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CalculationPercent);
            Assert.AreEqual(12.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).BasisAmount.Value);
            Assert.AreEqual(1.20, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("100", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).ReasonCode);
            Assert.AreEqual("REMISE COMMERCIALE", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CalculationPercent);
            Assert.AreEqual(28.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).BasisAmount.Value);
            Assert.AreEqual(2.80, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ActualAmount.Value);
            Assert.AreEqual("FC", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(2).CategoryTradeTax.RateApplicablePercent);

            Assert.IsTrue(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CalculationPercent);
            Assert.AreEqual(12.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).BasisAmount.Value);
            Assert.AreEqual(0.60, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ActualAmount.Value);
            Assert.AreEqual("ADR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).ReasonCode);
            Assert.AreEqual("FRAIS DEPLACEMENT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).Reason);
            Assert.AreEqual("VAT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(10.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(3).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);
            Assert.AreEqual("MANDATE PT", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradePaymentTerms.DirectDebitMandateID.Value);

            Assert.AreEqual(100.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.LineTotalAmount.Value);
            Assert.AreEqual(3.40, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.ChargeTotalAmount.Value);
            Assert.AreEqual(2.60, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.AllowanceTotalAmount.Value);

            Assert.AreEqual(100.80, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxBasisTotalAmount.Value);

            Assert.AreEqual(7.02, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.Value);
            Assert.AreEqual("EUR", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()?.CurrencyID);

            Assert.AreEqual(107.82, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.GrandTotalAmount.Value);

            Assert.AreEqual(0.00, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TotalPrepaidAmount.Value);
            Assert.AreEqual(107.82, invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.DuePayableAmount.Value);

            Assert.IsNull(invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument);

            Assert.AreEqual("BUYER ACCOUNT REF", invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }
    }
}