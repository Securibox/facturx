using NUnit.Framework;
using Securibox.FacturX.SpecificationModels.Minimum;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class EN16931InvoiceTests
    {
        private readonly string _mainDir = Path.Combine(System.IO.Directory.GetCurrentDirectory()?.Split("bin").First()!, "Invoices", "Custom");
        private readonly string _invoiceName = "2023-6013_facture_facturx_en16931.pdf";

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine(_mainDir);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            var outputPath = Path.Combine(_mainDir, _invoiceName);
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }
        }

        public static SpecificationModels.EN16931.CrossIndustryInvoice GetInvoice_SpecificationModels()
        {
            Securibox.FacturX.SpecificationModels.EN16931.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.EN16931.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.BasicWL.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "2023-6013" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20230920", Format = "102" } },
                    TypeCode = "380",
                    IncludedNote = new SpecificationModels.BasicWL.Note[]
                     {
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "SASU au capital de 200000€",
                            SubjectCode = "REG",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "R.C.S Paris 123 456 789",
                            SubjectCode = "ABL",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "APE 5510Z – TVA FR40123456824",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement.",
                            SubjectCode = "PMT",

                        },
                     },
                },
                ExchangedDocumentContext = new SpecificationModels.Minimum.ExchangedDocumentContext()
                {
                    BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "A1" },
                    },
                    GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "urn:cen.eu:en16931:2017" }
                    },
                },
                SupplyChainTradeTransaction = new SpecificationModels.EN16931.SupplyChainTradeTransaction()
                {
                    IncludedSupplyChainTradeLineItem = new SpecificationModels.EN16931.SupplyChainTradeLineItem[]
                     {
                        new SpecificationModels.EN16931.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = new SpecificationModels.Minimum.ID { Value = "1" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.EN16931.TradeProduct()
                            {
                                Name = "Chambre du 09/08/2023 au 15/08/2023",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.EN16931.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value =  205.9000m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.EN16931.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.EN16931.TradeTaxEN16931()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 10.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                }
                            }

                        },
                        new SpecificationModels.EN16931.SupplyChainTradeLineItem()
                        {
                            AssociatedDocumentLineDocument = new SpecificationModels.Basic.DocumentLineDocument()
                            {
                                LineID = new ID { Value = "2" },
                            },
                            SpecifiedTradeProduct = new SpecificationModels.EN16931.TradeProduct()
                            {
                                Name = "Forfait taxe de séjour",
                            },
                            SpecifiedLineTradeAgreement = new SpecificationModels.EN16931.LineTradeAgreement()
                            {
                                NetPriceProductTradePrice = new SpecificationModels.Basic.TradePrice()
                                {
                                    ChargeAmount = new SpecificationModels.Minimum.Amount { Value = 1.65m },
                                    BasisQuantity = new SpecificationModels.Basic.Quantity()
                                    {
                                        UnitCode = "C62",
                                        Value = 6.0000m,
                                    }
                                }
                            },
                            SpecifiedLineTradeSettlement = new SpecificationModels.EN16931.LineTradeSettlement()
                            {
                                ApplicableTradeTax = new SpecificationModels.EN16931.TradeTaxEN16931()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "Z",
                                    RateApplicablePercent = 0.00m,
                                },
                                SpecifiedTradeSettlementLineMonetarySummation = new SpecificationModels.Basic.TradeSettlementLineMonetarySummation()
                                {
                                    LineTotalAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                }
                            },
                            SpecifiedLineTradeDelivery = new SpecificationModels.Basic.LineTradeDelivery()
                            {
                                BilledQuantity = new SpecificationModels.Basic.Quantity()
                                {
                                    UnitCode = "C62",
                                    Value = 6.0000m,
                                }
                            }
                        },
                     },
                    ApplicableHeaderTradeAgreement = new SpecificationModels.EN16931.HeaderTradeAgreement()
                    {
                        SellerTradeParty = new SpecificationModels.EN16931.TradePartyEN16931()
                        {
                            Name = "Société Hôtelière du Pacano",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "12345682400016",
                                    SchemeID = "0002"
                                },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75000",
                                LineOne = "2, rue de la Paix",
                                CityName = "PARIS",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "info@hotel-du-pacano.fr",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                            {
                                new SpecificationModels.Minimum.TaxRegistration()
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR40123456824",
                                        SchemeID = "VA"
                                    }
                                },
                            }
                        },
                        BuyerTradeParty = new SpecificationModels.EN16931.TradePartyEN16931()
                        {
                            Name = "Securibox SARL",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "50000371000034", SchemeID = "0002" },
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration[]
                            {
                                new SpecificationModels.Minimum.TaxRegistration
                                {
                                    ID = new SpecificationModels.Minimum.ID()
                                    {
                                        Value = "FR38500003710",
                                        SchemeID = "VA"
                                    }
                                },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75008",
                                LineOne = "27, Rue de Bassano",
                                CityName = "Paris",
                            },
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.EN16931.HeaderTradeDelivery()
                    {
                    },
                    ApplicableHeaderTradeSettlement = new SpecificationModels.EN16931.HeaderTradeSettlement()
                    {
                        InvoiceCurrencyCode = "EUR",
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.EN16931.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.EN16931.CreditorFinancialAccount[]
                             {
                                new SpecificationModels.EN16931.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID { Value = "FR7430003000402964223654P78" },
                                }
                             },
                        },
                        ApplicableTradeTax = new SpecificationModels.EN16931.TradeTaxEN16931[]
                         {
                            new SpecificationModels.EN16931.TradeTaxEN16931()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 123.54m },
                                TypeCode = "VAT",
                                CategoryCode = "S",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 1235.40m },
                                RateApplicablePercent = 10.00m,
                            },
                            new SpecificationModels.EN16931.TradeTaxEN16931()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 0.00m },
                                TypeCode = "VAT",
                                CategoryCode = "Z",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 9.90m },
                                RateApplicablePercent = 0.00m,
                            }
                         },
                        SpecifiedTradePaymentTerms = new SpecificationModels.BasicWL.TradePaymentTerms()
                        {
                            DueDateDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20231019",
                                    Format = "102"
                                }
                            },
                            Description = "30 jours net",
                        },
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.EN16931.TradeSettlementHeaderMonetarySummation()
                        {
                            LineTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1245.30m, },
                            TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1245.30m },
                            TaxTotalAmount = new SpecificationModels.Minimum.Amount[] { new SpecificationModels.Minimum.Amount() { Value = 123.54m, CurrencyID = "EUR" } },
                            GrandTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 1368.84m },
                            TotalPrepaidAmount = new SpecificationModels.Minimum.Amount() { Value = 452.98m },
                            DuePayableAmount = new SpecificationModels.Minimum.Amount() { Value = 915.86m },
                        },
                    },
                }
            };

            return invoice;
        }

        [Test]
        [Order(1)]
        public async Task WriteData_EN16931_SUCCESS()
        {
            var outputPath = Path.Combine(_mainDir, "2023-6013_facture_facturx_en16931.pdf");

            Securibox.FacturX.SpecificationModels.EN16931.CrossIndustryInvoice invoice = GetInvoice_SpecificationModels();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
                $"SEPEM: Invoice ",
                $"Invoice "))
            {
                using (var fileStream = new FileStream(outputPath, FileMode.Create))
                {
                    await stream.CopyToAsync(fileStream);
                }
            }
        }

        [Test]
        [Order(2)]
        public void AssertWrittenData_EN16931_SUCCESS()
        {
            var invoicePath = Path.Combine(_mainDir, "2023-6013_facture_facturx_en16931.pdf");

            var importer = new FacturxImporter(invoicePath);
            var en16931Invoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.EN16931.CrossIndustryInvoice;

            Assert.That(en16931Invoice is not null);

            Assert.That(en16931Invoice!.ExchangedDocument.ID.Value, Is.EqualTo("2023-6013"));
            Assert.That(en16931Invoice!.ExchangedDocument.IssueDateTime.DateTimeString.Value, Is.EqualTo("20230920"));
            Assert.That(en16931Invoice!.ExchangedDocument.IssueDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(en16931Invoice!.ExchangedDocument.TypeCode, Is.EqualTo("380"));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.Count(), Is.EqualTo(6));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(0).Content, Is.EqualTo("SASU au capital de 200000€"));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(0).SubjectCode, Is.EqualTo("REG"));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(1).Content, Is.EqualTo("R.C.S Paris 123 456 789"));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(1).SubjectCode, Is.EqualTo("ABL")  );

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(2).Content, Is.EqualTo("2, rue de la Paix – 75000 Paris – Tel: +33 1 01 12 34 56"));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(2).SubjectCode, Is.EqualTo("AAI"));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(3).Content, Is.EqualTo("APE 5510Z – TVA FR40123456824"));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(3).SubjectCode, Is.EqualTo("AAI"));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(4).Content, Is.EqualTo("La loi n°92/1442 du 31 décembre 1992 nous fait l’obligation de vous indiquer que le non-respect des conditions de paiement entraine des intérêts de retard suivant modalités et taux défini par la loi."));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(4).SubjectCode, Is.EqualTo("PMD"));

            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(5).Content, Is.EqualTo("Une indemnité forfaitaire de 40€ sera due pour frais de recouvrement en cas de retard de paiement."));
            Assert.That(en16931Invoice!.ExchangedDocument.IncludedNote.ElementAt(5).SubjectCode, Is.EqualTo("PMT"));

            Assert.That(en16931Invoice!.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("A1"));
            Assert.That(en16931Invoice!.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value, Is.EqualTo("urn:cen.eu:en16931:2017"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.Count(), Is.EqualTo(2));

            var line1 = en16931Invoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(0);
            Assert.That(line1.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("1"));
            Assert.That(line1.SpecifiedTradeProduct.Name, Is.EqualTo("Chambre du 09/08/2023 au 15/08/2023"));
            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value, Is.EqualTo(205.9000m));
            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value, Is.EqualTo(6.0000m));
            Assert.That(line1.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode, Is.EqualTo("C62"));
            Assert.That(line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode, Is.EqualTo("S"));
            Assert.That(line1.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent, Is.EqualTo(10.00m));
            Assert.That(line1.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value, Is.EqualTo(1235.40m));

            var line2 = en16931Invoice!.SupplyChainTradeTransaction.IncludedSupplyChainTradeLineItem.ElementAt(1);
            Assert.That(line2.AssociatedDocumentLineDocument.LineID.Value, Is.EqualTo("2"));
            Assert.That(line2.SpecifiedTradeProduct.Name, Is.EqualTo("Forfait taxe de séjour"));
            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.ChargeAmount.Value, Is.EqualTo(1.6500m));
            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.Value, Is.EqualTo(6.0000m));
            Assert.That(line2.SpecifiedLineTradeAgreement.NetPriceProductTradePrice.BasisQuantity.UnitCode, Is.EqualTo("C62"));
            Assert.That(line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.TypeCode, Is.EqualTo("VAT"));
            Assert.That(line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.CategoryCode, Is.EqualTo("Z"));
            Assert.That(line2.SpecifiedLineTradeSettlement.ApplicableTradeTax.RateApplicablePercent, Is.EqualTo(0.00m));
            Assert.That(line2.SpecifiedLineTradeSettlement.SpecifiedTradeSettlementLineMonetarySummation.LineTotalAmount.Value, Is.EqualTo(9.90m));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name, Is.EqualTo("Securibox SARL"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("50000371000034"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.Value, Is.EqualTo("FR38500003710"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.SchemeID, Is.EqualTo("VA"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75008"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("27, Rue de Bassano"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("Paris"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name, Is.EqualTo("Société Hôtelière du Pacano"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value, Is.EqualTo("12345682400016"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID, Is.EqualTo("0002"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID, Is.EqualTo("FR"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode, Is.EqualTo("75000"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne, Is.EqualTo("2, rue de la Paix"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName, Is.EqualTo("PARIS"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value, Is.EqualTo("info@hotel-du-pacano.fr"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.SchemeID, Is.EqualTo("EM"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.Value, Is.EqualTo("FR40123456824"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ElementAt(0).ID.SchemeID, Is.EqualTo("VA"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode, Is.EqualTo("EUR"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.TypeCode, Is.EqualTo("30"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.Count(), Is.EqualTo(1));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.ElementAt(0).IBANID.Value, Is.EqualTo("FR7430003000402964223654P78"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.Count(), Is.EqualTo(2));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value, Is.EqualTo(123.54m));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode, Is.EqualTo("VAT"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value, Is.EqualTo(1235.40m));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode, Is.EqualTo("S"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent, Is.EqualTo(10.00));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CalculatedAmount.Value, Is.EqualTo(0.00)    );
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).TypeCode, Is.EqualTo("VAT"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).BasisAmount.Value, Is.EqualTo(9.90));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).CategoryCode, Is.EqualTo("Z"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(1).RateApplicablePercent, Is.EqualTo(0.00));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value, Is.EqualTo("20231019"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format, Is.EqualTo("102"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.Description, Is.EqualTo("30 jours net"));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value, Is.EqualTo(1245.30m));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value, Is.EqualTo(1245.30m));

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count(), Is.EqualTo(1));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID, Is.EqualTo("EUR"));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value, Is.EqualTo(123.54m)  );

            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value, Is.EqualTo(1368.84m));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value, Is.EqualTo(915.86m));
            Assert.That(en16931Invoice!.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value, Is.EqualTo(452.98m));
        }
    }
}