using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Securibox.FacturX.Models.EN16931.Enum;

namespace Securibox.FacturX.Tests.FacturxImporterTests
{
    public class EN16931InvoiceTests
    {
        private readonly string _mainDir = Path.Combine(
            System.IO.Directory.GetCurrentDirectory(),
            "Invoices",
            "EN16931"
        );

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [Test]
        public void ExtractData_Avoir_FR_type380_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(_mainDir, "Avoir_FR_type380_EN16931.pdf")
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo(
                    "Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé"
                )
            );

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1.SpecifiedTradeProduct is not null);
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370400049"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("Nougat de l'Abbaye 250g"));

            Assert.That(line1.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.55)
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    is not null
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ChargeIndicator
                    .Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ActualAmount
                    .Value,
                Is.EqualTo(-0.45)
            );

            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.10)
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(-5.000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(-20.48)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2.SpecifiedTradeProduct is not null);
            Assert.That(line2!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("HOLANCL"));
            Assert.That(
                line2!.SpecifiedTradeProduct.Name,
                Is.EqualTo("Huile d'olive à l'ancienne")
            );

            Assert.That(line2.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value,
                Is.EqualTo(-10.000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("LTR")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(5.50)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(-198.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Tony Dubois")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 56")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("tony.dubois@aubonmoulin.fr")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("84340")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("1242 chemin de l'olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("Malaucène")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Alexandre Payet")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 67")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("alexandre.payet@majolieboutique.net")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("69001")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue de la République")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Lyon")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR19787878784")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("MSPE2017")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("AV-2017-0005")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(
                taxDistributionList?.ElementAt(0).CalculatedAmount.Value,
                Is.EqualTo(-4.10)
            );
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(-20.48));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(
                taxDistributionList?.ElementAt(1).CalculatedAmount.Value,
                Is.EqualTo(-10.89)
            );
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(-198.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(5.50));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .Description,
                Is.EqualTo("Paiement immédiat")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20171116")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(-218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(-233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(-218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(-14.99)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(-0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(-233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("FA-2017-0010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171113")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
        }

        [Test]
        public void ExtractData_Avoir_FR_type381_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(_mainDir, "Avoir_FR_type381_EN16931.pdf")
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo(
                    "Avoir suite à bidon 10L d'huile d'olive percé et carton de nougat renversé"
                )
            );

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1.SpecifiedTradeProduct is not null);
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370400049"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("Nougat de l'Abbaye 250g"));

            Assert.That(line1.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.55)
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    is not null
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ChargeIndicator
                    .Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ActualAmount
                    .Value,
                Is.EqualTo(0.45)
            );

            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.10)
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(5.000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(20.48)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2.SpecifiedTradeProduct is not null);
            Assert.That(line2!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("HOLANCL"));
            Assert.That(
                line2!.SpecifiedTradeProduct.Name,
                Is.EqualTo("Huile d'olive à l'ancienne")
            );

            Assert.That(line2.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(10.000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("LTR")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(5.50)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(198.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Tony Dubois")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 56")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("tony.dubois@aubonmoulin.fr")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("84340")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("1242 chemin de l'olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("Malaucène")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Alexandre Payet")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 67")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("alexandre.payet@majolieboutique.net")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("69001")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue de la République")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Lyon")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR19787878784")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("MSPE2017")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("69001")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue de la République")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Lyon")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("AV-2017-0005")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(4.10));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(20.48));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(
                taxDistributionList?.ElementAt(1).CalculatedAmount.Value,
                Is.EqualTo(10.89)
            );
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(198.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(5.50));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .Description,
                Is.EqualTo("Paiement immédiat")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20171116")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(218.48)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(14.99)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(233.47)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("FA-2017-0010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20171113")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );
        }

        [Test]
        public void ExtractData_FACTURE_FR_type380_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_FR_EN16931.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("Franco de port (commande > 300 € HT)")
            );

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370400049"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line1!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("NOUG250"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("Nougat de l'Abbaye 250g"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.55)
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ChargeIndicator
                    .Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ActualAmount
                    .Value,
                Is.EqualTo(0.45)
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.10)
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(20.000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(81.90)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370200090"));
            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("Biscuits aux raisins 300g"));
            Assert.That(line2!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("BRAIS300"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(3.20)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(15.000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(5.50)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(48.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));
            Assert.That(
                line3!.SpecifiedTradeProduct.Name,
                Is.EqualTo("Huile d'olive à l'ancienne")
            );
            Assert.That(line3!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("HOLANCL"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );
            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(25.000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("LTR")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(5.50)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(495.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("84340")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("1242 chemin de l'olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("Malaucène")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Tony Dubois")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 56")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("tony.dubois@aubonmoulin.fr")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("Ma jolie boutique")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("78787878400035")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("69001")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue de la République")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Lyon")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Alexandre Payet")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 67")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("alexandre.payet@majolieboutique.net")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR19787878784")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO445")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("MSPE2017")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("69001")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue de la République")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Lyon")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("FA-2017-0010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .Information,
                Is.EqualTo("Virement sur compte Banque Fiducial")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeeSpecifiedCreditorFinancialInstitution.BICID.Value,
                Is.EqualTo("FIDCFR21XXX")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR2012421242124212421242124")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(
                taxDistributionList?.ElementAt(0).CalculatedAmount.Value,
                Is.EqualTo(16.38)
            );
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(81.90));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("5"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode is null);

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .Description,
                Is.EqualTo("30% d'acompte, solde à 30 j")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20171213")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(624.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(624.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(46.25)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(671.15)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(201.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(470.15)
            );
        }

        [Test]
        public void ExtractData_FACTURE_EU_type380_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_UE_EN16931.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("Free shipping (amount > 300 €)")
            );

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370400049"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("Nougat de l'Abbaye 250g"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.55)
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ChargeIndicator
                    .Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ActualAmount
                    .Value,
                Is.EqualTo(0.45)
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.10)
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(8.000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(32.76)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370200090"));
            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("Biscuits aux raisins 300g"));
            Assert.That(line2!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("BRAIS300"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(3.20)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(20.000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(64.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));
            Assert.That(
                line3!.SpecifiedTradeProduct.Name,
                Is.EqualTo("Huile d'olive à l'ancienne")
            );
            Assert.That(line3!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("HOLANCL"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );
            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value,
                Is.EqualTo(100.000)
            );
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("LTR")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(1980.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("84340")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("1242 chemin de l'olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("Malaucène")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Tony Dubois")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 56")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("tony.dubois@aubonmoulin.fr")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("Me gusta olive")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("41700")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("87 camino de la calor")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Dos Hermanas")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("ES")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("ESA12345674")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Pedro Sanchez")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+34 978 23 41 23")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("pedro@megustaolive.es")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("COMPRA0832")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("FROLIVE2017")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("Me gusta olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("41700")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("87 camino de la calor")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Dos Hermanas")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("ES")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20170311")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("FA-2017-0008")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .Information,
                Is.EqualTo("Credit transfer Banque Fiducial")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeeSpecifiedCreditorFinancialInstitution.BICID.Value,
                Is.EqualTo("FIDCFR21XXX")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR2012421242124212421242124")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(2076.76));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReason,
                Is.EqualTo(
                    "French VAT exemption according to articles 262 ter I (for products) and/or 283-2 (for services) of \"CGI\""
                )
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .Description,
                Is.EqualTo("30% advance payment, balance at 30 days")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20171203")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(2076.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(2076.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(2076.76)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(623.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(1453.76)
            );
        }

        [Test]
        public void ExtractData_Facture_DOM_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(Path.Combine(_mainDir, "Facture_DOM_EN16931.pdf"));

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("Franco de port (Commande > 300 € HT)")
            );

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1.SpecifiedTradeProduct is not null);
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370400049"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line1!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("NOUG250"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("Nougat de l'Abbaye 250g"));

            Assert.That(line1.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.55)
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    is not null
            );
            Assert.That(
                line1
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ChargeIndicator
                    .Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeAgreement
                    .GrossPriceProductTradePrice
                    .AppliedTradeAllowanceCharge
                    .ActualAmount
                    .Value,
                Is.EqualTo(0.45)
            );

            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(4.10)
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(50.000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("G")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(204.75)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2.SpecifiedTradeProduct is not null);
            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("3518370200090"));
            Assert.That(line2!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0160"));
            Assert.That(line2!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("BRAIS300"));
            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("Biscuits aux raisins 300g"));

            Assert.That(line2.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(3.20)
            );

            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(3.20)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(40.000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("G")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(128.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);
            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3.SpecifiedTradeProduct is not null);
            Assert.That(line3!.SpecifiedTradeProduct.SellerAssignedID, Is.EqualTo("HOLANCL"));
            Assert.That(
                line3!.SpecifiedTradeProduct.Name,
                Is.EqualTo("Huile d'olive à l'ancienne")
            );

            Assert.That(line3.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice is not null);
            Assert.That(
                line3!.SpecifiedLineTradeAgreement.GrossPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line3.SpecifiedLineTradeAgreement.NetPriceProductTradePrice is not null);
            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(19.80)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(10.000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("LTR")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("G")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(198.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("Au bon moulin")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("99999999800010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Tony Dubois")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 4 72 07 08 56")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("tony.dubois@aubonmoulin.fr")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("84340")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("1242 chemin de l'olive")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("Malaucène")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11999999998")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("Hôtel Saint Denis")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("34343434600010")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Stéphanie Hoarau")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("+33 2 62 94 26 01")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("achats@hotelsaintdenis.re")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("SMTP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("RE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("97400")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("42 rue du stade")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Saint Denis")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR90343434346")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerReference
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("BC543")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("WELCOME_PACK_2017")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .ID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("97400")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("42 rue du stade")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("Saint Denis de la réunion")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("RE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("FA-2017-0009")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .Information,
                Is.EqualTo("Virement sur compte Banque Fiducial")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeeSpecifiedCreditorFinancialInstitution.BICID.Value,
                Is.EqualTo("FIDCFR21XXX")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans?.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR2012421242124212421242124")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReason,
                Is.EqualTo("Exonération de TVA selon article 262 I du Code général des impôts")
            );
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(530.75));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("G"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .Description,
                Is.EqualTo("30% d'acompte, solde à 30 j")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20171205")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(530.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(530.75)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(530.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(147.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(383.75)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceReferencedDocument
                    is null
            );
        }

        [Test]
        public void ExtractData_Facture_F20220023_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220023-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("E")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(30.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(3.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(2.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(28.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("2")
            );

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(7.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(7.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID.FirstOrDefault(),
                Is.EqualTo("123")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(4)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .Value,
                Is.EqualTo("12345678200077")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .SchemeID,
                Is.EqualTo("0009")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .Value,
                Is.EqualTo("DUNS1235487")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .SchemeID,
                Is.EqualTo("0060")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .Value,
                Is.EqualTo("ODETTE254879")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .SchemeID,
                Is.EqualTo("0177")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("M. CONTACT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("DEP SELLER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 02 03 54 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("seller@seller.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("MON ADRESSE LIGNE 1")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Buyer line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("MA VILLE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Buyer contact name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("Buyer dep")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 01 25 45 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("buyer@buyer.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .Name,
                Is.EqualTo("SELLER TAX REP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .ReferenceTypeCode,
                Is.EqualTo("IT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID.FirstOrDefault(),
                Is.EqualTo("PRIVATE_ID_DEL")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL ADRESSE LIGNE 1")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID,
                Is.EqualTo("CREDID")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .Name,
                Is.EqualTo("PAYEE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("587451236586")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID.Value,
                Is.EqualTo("LOC BANK ACCOUNT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount.IBANID.Value,
                Is.EqualTo("FRDEBIT")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
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
            Assert.That(
                taxDistributionList?.ElementAt(1).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-79-C")
            );

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
            Assert.That(
                taxDistributionList?.ElementAt(3).ExemptionReason,
                Is.EqualTo("LIVRAISON INTRACOMMUNAUTAIRE")
            );
            Assert.That(
                taxDistributionList?.ElementAt(3).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-IC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CalculationPercent,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ReasonCode,
                Is.EqualTo("95")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CalculationPercent,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ReasonCode,
                Is.EqualTo("100")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CalculationPercent,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .ReasonCode,
                Is.EqualTo("100")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CalculationPercent,
                Is.EqualTo(2.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ActualAmount.Value,
                Is.EqualTo(2.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ReasonCode
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .CalculationPercent,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .ReasonCode,
                Is.EqualTo("FC")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        4
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .CalculationPercent,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .ReasonCode,
                Is.EqualTo("ADR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        5
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .CalculationPercent,
                Is.EqualTo(2.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .ActualAmount.Value,
                Is.EqualTo(2.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .ReasonCode,
                Is.EqualTo("FC")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        6
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .CalculationPercent,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .ReasonCode,
                Is.EqualTo("FC")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        7
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    .Value,
                Is.EqualTo("MANDATE PT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(14.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(9.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(4.90)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(104.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(104.90)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220024_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220024-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("E")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod is null);

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(30.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(3.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod is null);

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(2.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(28.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("2")
            );

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(7.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(7.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID.FirstOrDefault(),
                Is.EqualTo("123")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("M. CONTACT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("DEP SELLER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 02 03 54 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("seller@seller.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Buyer contact name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("Buyer dep")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 01 25 45 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("buyer@buyer.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Buyer line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Buyer contact name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("Buyer dep")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 01 25 45 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("buyer@buyer.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .Name,
                Is.EqualTo("SELLER TAX REP")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTaxRepresentativeTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.AdditionalReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID,
                Is.EqualTo("CREDID")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.ID.FirstOrDefault(),
                Is.EqualTo("123")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .GlobalID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .Name,
                Is.EqualTo("PAYEE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID.Value,
                Is.EqualTo("LOC BANK ACCOUNT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount.IBANID.Value,
                Is.EqualTo("FRDEBIT")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
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
            Assert.That(
                taxDistributionList?.ElementAt(1).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-D")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CalculationPercent,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ReasonCode,
                Is.EqualTo("95")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CalculationPercent,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ReasonCode,
                Is.EqualTo("FC")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    .Value,
                Is.EqualTo("MANDATE PT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(8.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(108.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(108.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220025_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220025-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod is null);

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod is null);

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);
            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("2")
            );

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.ID
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.AdditionalReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL ADRESSE LIGNE 1")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .ID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .GlobalID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .Name,
                Is.EqualTo("PAYEE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(
                taxDistributionList?.ElementAt(0).CalculatedAmount.Value,
                Is.EqualTo(20.00)
            );
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(20.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(120.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(20.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220026_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220026-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime is null
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line3.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument is null);

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime is null
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.ID
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count(),
                Is.EqualTo(3)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL 58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("BERLIN")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReason,
                Is.EqualTo("LIVRAISON INTRACOMMUNAUTAIRE")
            );
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-IC")
            );
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220027_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220027-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime is null
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line3.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument is null);

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime is null
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.ID
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count(),
                Is.EqualTo(3)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL 58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(
                taxDistributionList?.ElementAt(0).CalculatedAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(110.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220028_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220028-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime is null
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line3.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument is null);

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime is null
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.ID
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count(),
                Is.EqualTo(3)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL 58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(
                taxDistributionList?.ElementAt(0).CalculatedAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(110.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220029_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220029-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value,
                Is.EqualTo(-1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(-60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value,
                Is.EqualTo(-3.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime is null
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(-30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line3.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument is null);

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value,
                Is.EqualTo(-1.0000)
            );
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime is null
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(-5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.ID
                    is null
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(1)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR 05 987 654 321")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.Count(),
                Is.EqualTo(3)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL 58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("BERLIN")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(-100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("K"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-IC")
            );
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(-5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(-10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("K")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(-95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(-10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(-5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(-100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(-100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(-10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(-90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220030_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220030-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(line1!.SpecifiedTradeProduct.Name, Is.EqualTo("PRESTATION SUPPORT"));
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("O")
            );
            Assert.That(
                line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent is null
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(10.0000)
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("O")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent is null
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.EndDateTime is null
            );

            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);
            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge is null);

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(30.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(line3.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument is null);

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(5.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("O")
            );
            Assert.That(
                line3.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent is null
            );

            Assert.That(
                line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod.StartDateTime is null
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID.FirstOrDefault(),
                Is.EqualTo("123")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(4)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .Value,
                Is.EqualTo("12345678200077")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .SchemeID,
                Is.EqualTo("0009")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .Value,
                Is.EqualTo("DUNS1235487")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .SchemeID,
                Is.EqualTo("0060")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedTaxRegistration
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("M. CONTACT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("DEP SELLER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 02 03 54 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("seller@seller.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedTaxRegistration
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTaxRepresentativeTradeParty
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.SchemeID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .ID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL 58 rue de la mer")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("BERLIN")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("DE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .Name,
                Is.EqualTo("PAYEE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("587451236586")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount
                    is null
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReason,
                Is.EqualTo("HORS SCOPE TVA")
            );
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(100.00));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("O"));
            Assert.That(
                taxDistributionList?.ElementAt(0).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-O")
            );
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent is null);

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CalculationPercent,
                Is.EqualTo(0.0)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .BasisAmount
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ReasonCode
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("O")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CalculationPercent,
                Is.EqualTo(0.0)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .BasisAmount
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ReasonCode
                    is null
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("O")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(0.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(95.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(5.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.0)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(90.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.FirstOrDefault()
                    ?.IssuerAssignedID.Value,
                Is.EqualTo("F20220003")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.InvoiceReferencedDocument.ElementAt(
                        0
                    )
                    .FormattedIssueDateTime.DateTimeString.Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }

        [Test]
        public void ExtractData_Facture_F20220031_EN16931_SUCCESS()
        {
            var importer = new FacturxImporter(
                Path.Combine(
                    _mainDir,
                    "Facture_F20220031-LE_FOURNISSEUR-POUR-LE_CLIENT_EN_16931.pdf"
                )
            );

            var crossIndustryInvoice = importer.ImportDataWithDeserialization();
            Assert.That(crossIndustryInvoice is not null);

            var invoice =
                crossIndustryInvoice as FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

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

            var noteList = invoice?.ExchangedDocument.IncludedNote;
            Assert.That(noteList is not null);
            Assert.That(
                noteList?.ElementAt(0).Content,
                Is.EqualTo("FOURNISSEUR F SARL au capital de 50 000 EUR")
            );
            Assert.That(noteList?.ElementAt(0).SubjectCode, Is.EqualTo("REG"));
            Assert.That(noteList?.ElementAt(1).Content, Is.EqualTo("RCS MAVILLE 123 456 782"));
            Assert.That(noteList?.ElementAt(1).SubjectCode, Is.EqualTo("ABL"));
            Assert.That(
                noteList?.ElementAt(2).Content,
                Is.EqualTo(
                    "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789"
                )
            );
            Assert.That(noteList?.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));
            Assert.That(
                noteList?.ElementAt(3).Content,
                Is.EqualTo(
                    "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal. "
                )
            );
            Assert.That(noteList?.ElementAt(3).SubjectCode, Is.EqualTo("PMD"));
            Assert.That(
                noteList?.ElementAt(4).Content,
                Is.EqualTo(
                    "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €."
                )
            );
            Assert.That(noteList?.ElementAt(4).SubjectCode, Is.EqualTo("PMT"));
            Assert.That(
                noteList?.ElementAt(5).Content,
                Is.EqualTo(
                    "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte."
                )
            );
            Assert.That(noteList?.ElementAt(5).SubjectCode, Is.EqualTo("AAB"));

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
                Is.EqualTo("urn:cen.eu:en16931:2017")
            );

            var line1 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(0);

            Assert.That(line1!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));

            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.Value, Is.EqualTo("598785412598745"));
            Assert.That(line1!.SpecifiedTradeProduct.GlobalID.SchemeID, Is.EqualTo("0088"));
            Assert.That(
                line1!.SpecifiedTradeProduct.Name,
                Is.EqualTo("REMBOURSEMENT AFFRANCHISSEMENT")
            );
            Assert.That(line1!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("1")
            );

            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(60.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(1.0000)
            );
            Assert.That(
                line1!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line1!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line1!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("E")
            );
            Assert.That(
                line1!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(0.00)
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .BasisAmount.Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeAllowanceCharge.ElementAt(0).CalculationPercent,
                Is.EqualTo(1.00m));

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(7)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line1!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(60.00)
            );

            var line2 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(1);

            Assert.That(line2!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));

            Assert.That(line2!.SpecifiedTradeProduct.Name, Is.EqualTo("FOURNITURES DIVERSES"));
            Assert.That(line2!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("3")
            );

            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(30.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value,
                Is.EqualTo(3.0000)
            );
            Assert.That(
                line2!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(line2!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(3.0000));
            Assert.That(
                line2!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line2!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220131")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ActualAmount.Value,
                Is.EqualTo(2.00)
            );
            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .BasisAmount
                    is null
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .ReasonCode,
                Is.EqualTo("71")
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(2)
                    .Reason,
                Is.EqualTo("REMISE VOLUME")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(3)
                    .ReasonCode,
                Is.EqualTo("100")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(4)
                    .ReasonCode,
                Is.EqualTo("ADL")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(5)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line2
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .ActualAmount.Value,
                Is.EqualTo(1.00)
            );
            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(6)
                    .Reason,
                Is.EqualTo("FRAIS PALETTE")
            );

            Assert.That(
                line2!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(28.00)
            );

            var line3 =
                invoice?.SupplyChainTradeTransaction?.IncludedSupplyChainTradeLineItem.ElementAt(2);

            Assert.That(line3!.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("3"));

            Assert.That(line3!.SpecifiedTradeProduct.Name, Is.EqualTo("APPEL"));
            Assert.That(line3!.SpecifiedTradeProduct.Description, Is.EqualTo("Description"));

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.BuyerOrderReferencedDocument.LineID.Value,
                Is.EqualTo("2")
            );

            Assert.That(
                line3!.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value,
                Is.EqualTo(12.0000)
            );

            Assert.That(line3!.SpecifiedLineTradeDelivery.BilledQuantity.Value, Is.EqualTo(1.0000));
            Assert.That(
                line3!.SpecifiedLineTradeDelivery.BilledQuantity.UnitCode,
                Is.EqualTo("C62")
            );

            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                line3!.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(line3.SpecifiedLineTradeSettlement.BillingSpecifiedPeriod is null);

            Assert.That(
                line3!
                    .SpecifiedLineTradeSettlement
                    .SpecifiedTradeSettlementLineMonetarySummation
                    .LineTotalAmount
                    .Value,
                Is.EqualTo(12.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerReference,
                Is.EqualTo("SERVEXEC")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.Name,
                Is.EqualTo("LE FOURNISSEUR")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.ID.FirstOrDefault(),
                Is.EqualTo("123")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.Count(),
                Is.EqualTo(4)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        0
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .Value,
                Is.EqualTo("12345678200077")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        1
                    )
                    .SchemeID,
                Is.EqualTo("0009")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .Value,
                Is.EqualTo("DUNS1235487")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        2
                    )
                    .SchemeID,
                Is.EqualTo("0060")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .Value,
                Is.EqualTo("587451236587")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.GlobalID.ElementAt(
                        3
                    )
                    .SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.SpecifiedLegalOrganization
                    .TradingBusinessName,
                Is.EqualTo("SELLER TRADE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("75018")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineOne,
                Is.EqualTo("35 rue d'ici")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Seller line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Seller line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.PostalTradeAddress
                    .CityName,
                Is.EqualTo("PARIS")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("moi@seller.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .PersonName,
                Is.EqualTo("M. CONTACT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("DEP SELLER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 02 03 54 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerTradeParty
                    ?.DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("seller@seller.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("3654789851")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.BuyerTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .Name,
                Is.EqualTo("LE CLIENT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("987654321")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .PersonName,
                Is.EqualTo("Buyer contact name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .DepartmentName,
                Is.EqualTo("Buyer dep")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .TelephoneUniversalCommunication
                    .CompleteNumber,
                Is.EqualTo("01 01 25 45 87")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .DefinedTradeContact
                    .EmailURIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("buyer@buyer.com")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("MON ADRESSE LIGNE 1")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("Buyer line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .LineThree,
                Is.EqualTo("Buyer line 3")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("MA VILLE")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .Value,
                Is.EqualTo("me@buyer.com")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerTradeParty
                    .URIUniversalCommunication
                    .URIID
                    .SchemeID,
                Is.EqualTo("EM")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.Value,
                Is.EqualTo("FR11123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.SellerTradeParty?.SpecifiedTaxRegistration?.FirstOrDefault()
                    ?.ID.SchemeID,
                Is.EqualTo("VA")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.BuyerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("PO201925478")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.ContractReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("CT2018120802")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SellerOrderReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("SALES REF 2547")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("SUPPort doc")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .URIID.Value,
                Is.EqualTo("url:gffter")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .Name,
                Is.EqualTo("support descript")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("916")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("TENDER-002")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        1
                    )
                    .TypeCode,
                Is.EqualTo("50")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .IssuerAssignedID.Value,
                Is.EqualTo("REFCLI0215")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .ReferenceTypeCode,
                Is.EqualTo("IT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeAgreement?.AdditionalReferencedDocument.ElementAt(
                        2
                    )
                    .TypeCode,
                Is.EqualTo("130")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .ID
                    .Value,
                Is.EqualTo("PROJET2547")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeAgreement
                    ?.SpecifiedProcuringProject
                    .Name,
                Is.EqualTo("Project reference")
            );

            Assert.That(
                invoice?.SupplyChainTradeTransaction?.ApplicableHeaderTradeDelivery.ShipToTradeParty.ID.FirstOrDefault(),
                Is.EqualTo("PRIVATE_ID_DEL")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .Name,
                Is.EqualTo("DEL Name")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .PostcodeCode,
                Is.EqualTo("06000")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineOne,
                Is.EqualTo("DEL ADRESSE LIGNE 1")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .LineTwo,
                Is.EqualTo("DEL line 2")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CityName,
                Is.EqualTo("NICE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ShipToTradeParty
                    .PostalTradeAddress
                    .CountryID,
                Is.EqualTo("FR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ActualDeliverySupplyChainEvent
                    .OccurrenceDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220128")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .DespatchAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("DESPADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeDelivery
                    .ReceivingAdviceReferencedDocument
                    .IssuerAssignedID
                    .Value,
                Is.EqualTo("RECEIV-ADV002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220101")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .StartDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20221231")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.BillingSpecifiedPeriod
                    .EndDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.CreditorReferenceID,
                Is.EqualTo("CREDID")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PaymentReference,
                Is.EqualTo("F20180023BUYER")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceCurrencyCode,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .Name,
                Is.EqualTo("PAYEE NAME")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.Value,
                Is.EqualTo("587451236586")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.PayeeTradeParty.GlobalID.FirstOrDefault()
                    ?.SchemeID,
                Is.EqualTo("0088")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .Value,
                Is.EqualTo("123456782")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.PayeeTradeParty
                    .SpecifiedLegalOrganization
                    .ID
                    .SchemeID,
                Is.EqualTo("0002")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .TypeCode,
                Is.EqualTo("30")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.IBANID.Value,
                Is.EqualTo("FR76 1254 2547 2569 8542 5874 698")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayeePartyCreditorFinancialAccount?.ProprietaryID.Value,
                Is.EqualTo("LOC BANK ACCOUNT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementPaymentMeans.ElementAt(
                        0
                    )
                    .PayerPartyDebtorFinancialAccount.IBANID.Value,
                Is.EqualTo("FRDEBIT")
            );

            var taxDistributionList = invoice
                ?.SupplyChainTradeTransaction
                ?.ApplicableHeaderTradeSettlement
                ?.ApplicableTradeTax;
            Assert.That(taxDistributionList is not null);

            Assert.That(taxDistributionList?.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(5.88));
            Assert.That(taxDistributionList?.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(0).BasisAmount.Value, Is.EqualTo(29.40));
            Assert.That(taxDistributionList?.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(0).ExemptionReasonCode is null);
            Assert.That(taxDistributionList?.ElementAt(0).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(0).RateApplicablePercent, Is.EqualTo(20.00));

            Assert.That(taxDistributionList?.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(0.00));
            Assert.That(taxDistributionList?.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(1).ExemptionReason, Is.EqualTo("DEBOURS"));
            Assert.That(taxDistributionList?.ElementAt(1).BasisAmount.Value, Is.EqualTo(60.00));
            Assert.That(taxDistributionList?.ElementAt(1).CategoryCode, Is.EqualTo("E"));
            Assert.That(
                taxDistributionList?.ElementAt(1).ExemptionReasonCode,
                Is.EqualTo("VATEX-EU-79-C")
            );
            Assert.That(taxDistributionList?.ElementAt(1).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(1).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(taxDistributionList?.ElementAt(2).CalculatedAmount.Value, Is.EqualTo(1.14));
            Assert.That(taxDistributionList?.ElementAt(2).TypeCode, Is.EqualTo("VAT"));
            Assert.That(taxDistributionList?.ElementAt(2).ExemptionReason is null);
            Assert.That(taxDistributionList?.ElementAt(2).BasisAmount.Value, Is.EqualTo(11.40));
            Assert.That(taxDistributionList?.ElementAt(2).CategoryCode, Is.EqualTo("S"));
            Assert.That(taxDistributionList?.ElementAt(2).ExemptionReasonCode is null);
            Assert.That(taxDistributionList?.ElementAt(2).DueDateTypeCode, Is.EqualTo("72"));
            Assert.That(taxDistributionList?.ElementAt(2).RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CalculationPercent,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .BasisAmount.Value,
                Is.EqualTo(28.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ActualAmount.Value,
                Is.EqualTo(1.40)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .ReasonCode,
                Is.EqualTo("100")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        0
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ChargeIndicator.Indicator
                    is false
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CalculationPercent,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .BasisAmount.Value,
                Is.EqualTo(12.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ActualAmount.Value,
                Is.EqualTo(1.20)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .ReasonCode,
                Is.EqualTo("100")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .Reason,
                Is.EqualTo("REMISE COMMERCIALE")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        1
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CalculationPercent,
                Is.EqualTo(10.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .BasisAmount.Value,
                Is.EqualTo(28.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .ActualAmount.Value,
                Is.EqualTo(2.80)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .ReasonCode,
                Is.EqualTo("FC")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        2
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(20.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ChargeIndicator.Indicator
                    is true
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CalculationPercent,
                Is.EqualTo(5.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .BasisAmount.Value,
                Is.EqualTo(12.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ActualAmount.Value,
                Is.EqualTo(0.60)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .ReasonCode,
                Is.EqualTo("ADR")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .Reason,
                Is.EqualTo("FRAIS DEPLACEMENT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.TypeCode,
                Is.EqualTo("VAT")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.CategoryCode,
                Is.EqualTo("S")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeAllowanceCharge.ElementAt(
                        3
                    )
                    .CategoryTradeTax.RateApplicablePercent,
                Is.EqualTo(10.00)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Value,
                Is.EqualTo("20220302")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DueDateDateTime
                    .DateTimeString
                    .Format,
                Is.EqualTo("102")
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradePaymentTerms
                    .DirectDebitMandateID
                    .Value,
                Is.EqualTo("MANDATE PT")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.LineTotalAmount
                    .Value,
                Is.EqualTo(100.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.ChargeTotalAmount
                    .Value,
                Is.EqualTo(3.40)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.AllowanceTotalAmount
                    .Value,
                Is.EqualTo(2.60)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TaxBasisTotalAmount
                    .Value,
                Is.EqualTo(100.80)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.Value,
                Is.EqualTo(7.02)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction?.ApplicableHeaderTradeSettlement?.SpecifiedTradeSettlementHeaderMonetarySummation?.TaxTotalAmount?.FirstOrDefault()
                    ?.CurrencyID,
                Is.EqualTo("EUR")
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.GrandTotalAmount
                    .Value,
                Is.EqualTo(107.82)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.TotalPrepaidAmount
                    .Value,
                Is.EqualTo(0.00)
            );
            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.SpecifiedTradeSettlementHeaderMonetarySummation
                    ?.DuePayableAmount
                    .Value,
                Is.EqualTo(107.82)
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.InvoiceReferencedDocument
                    is null
            );

            Assert.That(
                invoice
                    ?.SupplyChainTradeTransaction
                    ?.ApplicableHeaderTradeSettlement
                    ?.ReceivableSpecifiedTradeAccountingAccount
                    .ID
                    .Value,
                Is.EqualTo("BUYER ACCOUNT REF")
            );
        }
    }
}
