# .NET Factur-X library

## Context
Factur-X is a new French and German standard for electronic invoicing, expanding on the German ZUGFeRD standard. It represents the first implementation of the European Commission’s European Semantic Standard EN 16931, introduced in 2017 by the [FNFE-MPE](http://fnfe-mpe.org/factur-x/). Factur-X belong to a class of e-invoices known as mixed or hybrid invoices, combining PDFs for users and XML data for automated processing.

Several standard data profiles are available with more or less information:
- Minimum: Does not contain all of the invoicing information necessary for use in Germany
- Basic WL: Does not contain all of the invoicing information necessary for use in Germany
- Basic: Suitable for simple invoices
- EN16931: Adds data for fully automated invoicing, covering EN 16931
- Extended: Adds additional data for sending invoices across industries

## Library
This library, developed in .NET 8.0, allows you to easily perform the following tasks:
- **Generate a Factur-X PDF invoice** in any of the available profiles
- **Extract the information** contained in a Factur-X PDF invoice
- **Validate a Factur-X PDF invoice** against the official Factur-X XML Schema Definition and Schematrons

## Installation
To use this library in your C# project, you can either download the Securibox.FacturX library directly from our Github repository or if you have the NuGet package manager installed or Visual Studio, you can grab it automatically:
```sh
dotnet add package Securibox.FacturX
```
Once you have the Securibox FacturX library installed, you can include calls to it in your code.
For sample implementations, see the tests in the Securibox.FacturX.Tests project.

## Dependencies
Securibox.FacturX library utilises the following MIT licensed projects:
- [PDFSharpCore](https://github.com/ststeiger/PdfSharpCore)
- [XmpCore](https://github.com/drewnoakes/xmp-core-dotnet)
- [XPath2](https://github.com/StefH/XPath2.Net)

## Quick start
### Read information from Factur-X PDF invoice
The following is the minimum needed code to read a Factur-X PDF invoice in Minimum profile:
```csharp
var importer = new FacturxImporter("C:\\Path\\To\\Facture_UE_MINIMUM.pdf"));
var crossIndustryInvoice = importer.ImportDataWithDeserialization();
var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;
Assert.AreEqual("FA-2017-0008", invoice.ExchangedDocument.ID.Value);
Assert.AreEqual("20171103", invoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
```
The following is the minimum needed code to read a Factur-X PDF invoice in Basic profile:
```csharp
var importer = new FacturxImporter("C:\\Path\\To\\Facture_UE_MINIMUM.pdf"));
var crossIndustryInvoice = importer.ImportDataWithDeserialization();
var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Basic.CrossIndustryInvoice;
```

### Validate Factur-X
```csharp
var importer = new FacturxImporter(@"C:\Path\To\Facture_UE_MINIMUM.pdf"));
importer.IsFacturXValid();
```

### Generate a Factur-X PDF invoice
```csharp
Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice invoiceToExport = new Securibox.FacturX.SpecificationModels.Minimum.CrossIndustryInvoice()
{
    ExchangedDocument = new SpecificationModels.Minimum.ExchangedDocument()
    {
        ID = new SpecificationModels.Minimum.ID() { Value = "2023-6026" },
        IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20230928", Format = "102" } },
        TypeCode = "380"
    },
    ExchangedDocumentContext = new SpecificationModels.Minimum.ExchangedDocumentContext()
    {
        BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
        {
            ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
        },
        GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
        {
            ID = new SpecificationModels.Minimum.ID() { Value = "urn:factur-x.eu:1p0:minimum" }
        },
    },
    SupplyChainTradeTransaction = new SpecificationModels.Minimum.SupplyChainTradeTransaction()
    {
        ApplicableHeaderTradeAgreement = new SpecificationModels.Minimum.HeaderTradeAgreement()
        {
            BuyerTradeParty = new SpecificationModels.Minimum.TradeParty()
            {
                Name = "Securibox SARL",
                SpecifiedLegalOrganization = new SpecificationModels.Minimum.LegalOrganization()
                {
                    ID = new SpecificationModels.Minimum.ID() { Value = "50000371000034", SchemeID = "0002" },
                }
            },
            SellerTradeParty = new SpecificationModels.Minimum.TradeParty()
            {
                Name = "Société Hôtelière du Pacano",
                SpecifiedLegalOrganization = new SpecificationModels.Minimum.LegalOrganization()
                {
                    ID = new SpecificationModels.Minimum.ID()
                    {
                        Value = "12345682400016",
                        SchemeID = "0002"
                    },
                },
                PostalTradeAddress = new SpecificationModels.Minimum.TradeAddress()
                {
                    CountryID = "FR"
                }
            },
        },
        ApplicableHeaderTradeDelivery = new SpecificationModels.Minimum.HeaderTradeDelivery() { },
        ApplicableHeaderTradeSettlement = new SpecificationModels.Minimum.HeaderTradeSettlement()
        {
            InvoiceCurrencyCode = "EUR",
            SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.Minimum.TradeSettlementHeaderMonetarySummation()
            {
                TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 207.55m },
                TaxTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 20.59m, CurrencyID = "EUR" },
                GrandTotalAmount = new SpecificationModels.Minimum.Amount()
                {
                    Value = 228.14m,
                },
                DuePayableAmount = new SpecificationModels.Minimum.Amount()
                {
                    Value = 228.14m,
                },
            },
        },
    }
};

var inputNonFacturxPdfPath = @"c:\path\to\notAFacturX.pdf";
var outputPath = @"c:\path\to\facturx.pdf";
FacturxExporter exporter = new FacturxExporter();
using (var outputStream = new new FileStream(outputPath, FileMode.Create))
{
     using (var stream = exporter.CreateFacturXStream(
         inputNonFacturxPdfPath,
         invoiceToExport,
         "Invoice 2023-6026",
         $"Hotel payment"))
     {
         await stream.CopyToAsync(outputStream);
     }    
}
```



