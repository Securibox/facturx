# .NET Factur-X library

## Context
Factur-X is a new French and German standard for electronic invoicing, expanding the German ZUGFeRD standard. 
It represents the first implementation of the European Commission’s European Semantic Standard EN 16931, introduced in 2017 by the [FNFE-MPE](http://fnfe-mpe.org/factur-x/). 
Factur-X belongs to a class of e-invoices known as mixed or hybrid invoices, that combine PDFs for users and XML data for automated processing.

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
For sample implementations, see the tests in the [Securibox.FacturX.Tests](https://github.com/Securibox/facturx/tree/main/Securibox.FacturX.Tests) project.

## Dependencies
Securibox.FacturX library utilises the following MIT licensed projects:
- [PDFSharpCore](https://github.com/ststeiger/PdfSharpCore)
- [XmpCore](https://github.com/drewnoakes/xmp-core-dotnet)
- [XPath2](https://github.com/StefH/XPath2.Net)

## Quick start
### Read information from Factur-X PDF invoice
In order to read a Factur-X PDF invoice you only need to :

```csharp
var importer = new FacturxImporter("C:\\Path\\To\\Facture.pdf"));
var crossIndustryInvoice = importer.ImportDataWithDeserialization();
var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Profile.CrossIndustryInvoice;
```

In the following examples we are reading a Factur-X PDF invoice with a Minimum and Basic profiles.
```csharp
var importer = new FacturxImporter("C:\\Path\\To\\Facture_UE_MINIMUM.pdf"));
var crossIndustryInvoice = importer.ImportDataWithDeserialization();
var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Minimum.CrossIndustryInvoice;
```
In the following example we are reading a Factur-X PDF invoice with a Basic profile.
```csharp
var importer = new FacturxImporter("C:\\Path\\To\\Facture_UE_BASIC.pdf"));
var crossIndustryInvoice = importer.ImportDataWithDeserialization();
var invoice = crossIndustryInvoice as FacturX.SpecificationModels.Basic.CrossIndustryInvoice;
```

### Validate Factur-X

To validate the factur-x you just need to call **IsFacturXValid()** method. 

This method returns either an object containing a boolean indicating the success of the validation and a list with information about the tests that have been made or an exception. 
In case of success, the boolean will assume the value true and the list will be empty, otherwise, the boolean will be false and an Exception will be thrown.This exception will have the Exception.Data property fullfilled with a list of information about the tests that have failed. 

This method validates the factur-x invoice against it's xsd schema and schematron.

A FacturX pdf is valid if:
    - it is a valid PDF/A-3;
    - it has a valid XMP;
    - the embebed xml is valid against the profile xsd;
    - the embebed xml is valid against the profile schematron.
    
Below an example is shown :
```csharp
var importer = new FacturxImporter(@"C:\Path\To\Facture.pdf"));
importer.IsFacturXValid();
```

### Generate a Factur-X PDF invoice

In order to generate a pdf invoice from a valid xml you will need to create a new CrossIndustryInvoice, matching the profile you want, this object will be used to generate the xml for your invoice. 
After that, you just need to create a new FacturXExporter and call the method CreateFacturXStream.

```csharp
CreateFacturXStream(@".\path\InvoiceToExtract.pdf", crossIndustryInvoice , invoiceName, invoiceDescription);
```

 - @".\path\InvoiceToExtract.pdf" => Path to the pdf file that you want to embeb the xml file;
 - crossIndustryInvoice => The CrossIndustryInvoice object that you created and populated and will be used to generate the xml;
 - invoiceName and invoiceDescription => Values that will be used in the pdf metadata.

The arguments invoiceName and invoiceDescription are optional.

Below you will find an extensive example of a facturx creation. 
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
         "Hotel payment"))
     {
         await stream.CopyToAsync(outputStream);
     }    
}
```


Happy coding! 



