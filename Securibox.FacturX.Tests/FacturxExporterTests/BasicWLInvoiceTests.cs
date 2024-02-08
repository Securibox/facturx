using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Securibox.FacturX.Tests.FacturxExporterTests
{
    internal class BasicWLInvoiceTests
    {
        private readonly string _mainDir = $"{System.IO.Directory.GetCurrentDirectory()?.Split("\\bin")?.ElementAtOrDefault(0)}\\Invoices\\Custom\\";

        [SetUp]
        public void Setup()
        {
        }

        public static SpecificationModels.BasicWL.CrossIndustryInvoice GetInvoice()
        {
            Securibox.FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice invoice = new Securibox.FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice()
            {
                ExchangedDocument = new SpecificationModels.BasicWL.ExchangedDocument()
                {
                    ID = new SpecificationModels.Minimum.ID { Value = "F20220025" },
                    IssueDateTime = new SpecificationModels.Minimum.IssueDateTime() { DateTimeString = new SpecificationModels.Minimum.DateTimeString() { Value = "20220131", Format = "102" } },
                    TypeCode = "380",
                    IncludedNote = new SpecificationModels.BasicWL.Note[]
                    {
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "FOURNISSEUR F SARL au capital de 50 000 EUR",
                            SubjectCode = "REG",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "RCS MAVILLE 123 456 782",
                            SubjectCode = "ABL",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789",
                            SubjectCode = "AAI",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal.",
                            SubjectCode = "PMD",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.",
                            SubjectCode = "PMT",

                        },
                        new SpecificationModels.BasicWL.Note()
                        {
                            Content = "Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.",
                            SubjectCode = "AAB",
                        },
                    },
                },
                ExchangedDocumentContext = new SpecificationModels.Minimum.ExchangedDocumentContext()
                {
                    BusinessProcessSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() {  Value = "A1" },
                    },
                    GuidelineSpecifiedDocumentContextParameter = new SpecificationModels.Minimum.DocumentContextParameter()
                    {
                        ID = new SpecificationModels.Minimum.ID() { Value = "urn:factur-x.eu:1p0:basicwl" }
                    },
                },
                SupplyChainTradeTransaction = new SpecificationModels.BasicWL.SupplyChainTradeTransaction()
                {
                    ApplicableHeaderTradeAgreement = new SpecificationModels.BasicWL.HeaderTradeAgreement()
                    {
                        BuyerReference = "SERVEXEC",
                        SellerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            GlobalID = new SpecificationModels.Minimum.ID[]
                            {
                                new SpecificationModels.Minimum.ID()
                                {
                                    Value = "587451236587",
                                    SchemeID = "0088"
                                },
                            },
                            Name = "LE FOURNISSEUR",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002"
                                },
                                TradingBusinessName = "SELLER TRADE NAME",
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "75018",
                                LineOne = "35 rue d'ici",
                                LineTwo = "Seller line 2",
                                CityName = "PARIS",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "moi@seller.com",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR11123456782",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            GlobalID = new SpecificationModels.Minimum.ID[]
                            {
                                new SpecificationModels.Minimum.ID()
                                {
                                    Value = "3654789851",
                                    SchemeID = "0088"
                                },
                            },
                            Name = "LE CLIENT",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID() { Value = "987654321", SchemeID = "0002" },
                            },
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "06000",
                                LineOne = "58 rue de la mer",
                                LineTwo = "Buyer line 2",
                                CityName = "NICE",
                            },
                            URIUniversalCommunication = new SpecificationModels.BasicWL.UniversalCommunication()
                            {
                                URIID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "moi@buyer.com",
                                    SchemeID = "EM"
                                }
                            },
                            SpecifiedTaxRegistration = new SpecificationModels.Minimum.TaxRegistration()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "FR 05 987 654 321",
                                    SchemeID = "VA"
                                }
                            }
                        },
                        BuyerOrderReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID() { Value = "PO201925478" }
                        },
                        ContractReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID()
                            {
                                Value = "CT2018120802"
                            }
                        },
                    },
                    ApplicableHeaderTradeDelivery = new SpecificationModels.BasicWL.HeaderTradeDelivery() 
                    {
                        ShipToTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "DEL Name",
                            PostalTradeAddress = new SpecificationModels.BasicWL.TradeAddress()
                            {
                                CountryID = "FR",
                                PostcodeCode = "06000",
                                LineOne = "DEL ADRESSE LIGNE 1",
                                LineTwo = "DEL line 2",
                                CityName = "NICE",
                            },
                        },
                        ActualDeliverySupplyChainEvent = new SpecificationModels.BasicWL.SupplyChainEvent()
                        {
                            OccurrenceDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20220128",
                                    Format = "102",
                                }
                            }
                        },
                        DespatchAdviceReferencedDocument = new SpecificationModels.BasicWL.ReferencedDocument()
                        {
                            IssuerAssignedID = new SpecificationModels.Minimum.ID { Value = "DESPADV002" },
                        },
                    },    
                    ApplicableHeaderTradeSettlement = new SpecificationModels.BasicWL.HeaderTradeSettlement() 
                    {
                        PaymentReference = "F20180023BUYER",
                        InvoiceCurrencyCode = "EUR",
                        PayeeTradeParty = new SpecificationModels.BasicWL.TradeParty()
                        {
                            Name = "PAYEE NAME",
                            SpecifiedLegalOrganization = new SpecificationModels.BasicWL.LegalOrganization()
                            {
                                ID = new SpecificationModels.Minimum.ID()
                                {
                                    Value = "123456782",
                                    SchemeID = "0002",
                                }
                            }
                        },
                        SpecifiedTradeSettlementPaymentMeans = new SpecificationModels.BasicWL.TradeSettlementPaymentMeans()
                        {
                            TypeCode = "30",
                            PayeePartyCreditorFinancialAccount = new SpecificationModels.BasicWL.CreditorFinancialAccount[]
                            {
                                new SpecificationModels.BasicWL.CreditorFinancialAccount()
                                {
                                    IBANID = new SpecificationModels.Minimum.ID { Value = "FR76 1254 2547 2569 8542 5874 698" },
                                }
                            },
                        },
                        ApplicableTradeTax = new SpecificationModels.BasicWL.TradeTax[]
                        {
                            new SpecificationModels.BasicWL.TradeTax()
                            {
                                CalculatedAmount = new SpecificationModels.Minimum.Amount { Value = 20.00m },
                                TypeCode = "VAT",
                                BasisAmount = new SpecificationModels.Minimum.Amount { Value = 100.00m },
                                CategoryCode = "S",
                                DueDateTypeCode = "72",
                                RateApplicablePercent = 20.00m,
                            }
                        },
                        SpecifiedTradeAllowanceCharge = new SpecificationModels.BasicWL.TradeAllowanceCharge[]
                        {
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = false,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 5.00m },
                                Reason = "REMISE COMMERCIALE",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                            new SpecificationModels.BasicWL.TradeAllowanceCharge()
                            {
                                ChargeIndicator = new SpecificationModels.BasicWL.IndicatorType()
                                {
                                    Indicator = true,
                                },
                                ActualAmount = new SpecificationModels.Minimum.Amount { Value = 10.00m },
                                Reason = "FRAIS DEPLACEMENT",
                                CategoryTradeTax = new SpecificationModels.BasicWL.TradeTax()
                                {
                                    TypeCode = "VAT",
                                    CategoryCode = "S",
                                    RateApplicablePercent = 20.00m,
                                }
                            },
                        },
                        SpecifiedTradePaymentTerms = new SpecificationModels.BasicWL.TradePaymentTerms()
                        {
                            DueDateDateTime = new SpecificationModels.Minimum.IssueDateTime()
                            {
                                DateTimeString = new SpecificationModels.Minimum.DateTimeString()
                                {
                                    Value = "20220302",
                                    Format = "102"
                                }
                            }
                        },
                        SpecifiedTradeSettlementHeaderMonetarySummation = new SpecificationModels.BasicWL.TradeSettlementHeaderMonetarySummation()
                        {
                            LineTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 95.00m, },
                            ChargeTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 10.00m },
                            AllowanceTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 5.00m },
                            TaxBasisTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 100.80m },
                            TaxTotalAmount = new SpecificationModels.Minimum.Amount[] { new SpecificationModels.Minimum.Amount() { Value = 20.00m, CurrencyID = "EUR" } },
                            GrandTotalAmount = new SpecificationModels.Minimum.Amount() { Value = 120.00m },
                            TotalPrepaidAmount = new SpecificationModels.Minimum.Amount() { Value = 20.00m },
                            DuePayableAmount = new SpecificationModels.Minimum.Amount() { Value = 100.00m },
                        },
                        ReceivableSpecifiedTradeAccountingAccount = new SpecificationModels.BasicWL.TradeAccountingAccount()
                        {
                            ID = new SpecificationModels.Minimum.ID() { Value = "BUYER ACCOUNT REF" },
                        }
                    },
                }
            };

            return invoice;

        }

        [Test]
        public async Task WriteData_BasicWL_SUCCESS()
        {
            Securibox.FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice invoice = GetInvoice();
            FacturxExporter exporter = new FacturxExporter();

            using (var stream = exporter.CreateFacturXStream(
                Path.Combine(_mainDir, "2023-6013_facture.pdf"),
                invoice,
                $"SEPEM: Invoice ",
                $"Invoice "))
            {
                await stream.CopyToAsync(new FileStream(Path.Combine(_mainDir, "2023-6013_facture_facturx_basicWL.pdf"), FileMode.Create));
            }
        }

        [Test]
        public async Task AssertWrittenData_BasicWL_SUCCESS()
        {
            var invoicePath = string.Format("{0}\\{1}", _mainDir, "2023-6013 - BasicWL - FacturX.pdf");

            var importer = new FacturxImporter(invoicePath);
            var basicWLInvoice = importer.ImportDataWithDeserialization() as Securibox.FacturX.SpecificationModels.BasicWL.CrossIndustryInvoice;

            Assert.NotNull(basicWLInvoice);

            Assert.AreEqual("F20220025", basicWLInvoice.ExchangedDocument.ID.Value);
            Assert.AreEqual("20220131", basicWLInvoice.ExchangedDocument.IssueDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicWLInvoice.ExchangedDocument.IssueDateTime.DateTimeString.Format);
            Assert.AreEqual("380", basicWLInvoice.ExchangedDocument.TypeCode);

            Assert.AreEqual(6, basicWLInvoice.ExchangedDocument.IncludedNote.Count());

            Assert.AreEqual("FOURNISSEUR F SARL au capital de 50 000 EUR", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(0).Content);
            Assert.AreEqual("REG", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(0).SubjectCode);

            Assert.AreEqual("RCS MAVILLE 123 456 782", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(1).Content);
            Assert.AreEqual("ABL", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(1).SubjectCode);

            Assert.AreEqual("35 ma rue a moi, code postal Ville Pays – contact@masociete.fr - www.masociete.fr  – N° TVA : FR32 123 456 789", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(2).Content);
            Assert.AreEqual("AAI", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(2).SubjectCode);

            Assert.AreEqual("Tout retard de paiement engendre une pénalité exigible à compter de la date d'échéance, calculée sur la base de trois fois le taux d'intérêt légal.", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(3).Content);
            Assert.AreEqual("PMD", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(3).SubjectCode);

            Assert.AreEqual("Indemnité forfaitaire pour frais de recouvrement en cas de retard de paiement : 40 €.", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(4).Content);
            Assert.AreEqual("PMT", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(4).SubjectCode);

            Assert.AreEqual("Les réglements reçus avant la date d'échéance ne donneront pas lieu à escompte.", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(5).Content);
            Assert.AreEqual("AAB", basicWLInvoice.ExchangedDocument.IncludedNote.ElementAt(5).SubjectCode);

            Assert.AreEqual("A1", basicWLInvoice.ExchangedDocumentContext.BusinessProcessSpecifiedDocumentContextParameter.ID.Value);
            Assert.AreEqual("urn:factur-x.eu:1p0:basicwl", basicWLInvoice.ExchangedDocumentContext.GuidelineSpecifiedDocumentContextParameter.ID.Value);

            Assert.AreEqual("SERVEXEC", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerReference);

            Assert.AreEqual("PO201925478", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerOrderReferencedDocument.IssuerAssignedID.Value);
            Assert.AreEqual("CT2018120802", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.ContractReferencedDocument.IssuerAssignedID.Value);

            Assert.AreEqual("LE CLIENT", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.Name);
            Assert.AreEqual("987654321", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.BuyerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("LE FOURNISSEUR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.Name);
            Assert.AreEqual("587451236587", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).Value);
            Assert.AreEqual("0088", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.GlobalID.ElementAt(0).SchemeID);
            Assert.AreEqual("123456782", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.ID.SchemeID);
            Assert.AreEqual("SELLER TRADE NAME", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedLegalOrganization.TradingBusinessName);
            Assert.AreEqual("FR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("75018", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("35 rue d'ici", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("Seller line 2", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("FR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("PARIS", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.PostalTradeAddress.CityName);
            Assert.AreEqual("moi@seller.com", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.URIUniversalCommunication.URIID.Value);
            Assert.AreEqual("FR11123456782", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.Value);
            Assert.AreEqual("VA", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeAgreement.SellerTradeParty.SpecifiedTaxRegistration.ID.SchemeID);

            Assert.AreEqual("DEL Name", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.Name);
            Assert.AreEqual("FR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CountryID);
            Assert.AreEqual("06000", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.PostcodeCode);
            Assert.AreEqual("DEL ADRESSE LIGNE 1", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineOne);
            Assert.AreEqual("DEL line 2", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.LineTwo);
            Assert.AreEqual("NICE", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ShipToTradeParty.PostalTradeAddress.CityName);

            Assert.AreEqual("20220128", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.ActualDeliverySupplyChainEvent.OccurrenceDateTime.DateTimeString.Format);

            Assert.AreEqual("DESPADV002", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeDelivery.DespatchAdviceReferencedDocument.IssuerAssignedID.Value);
            
            Assert.AreEqual("F20180023BUYER", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PaymentReference);
            Assert.AreEqual("EUR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.InvoiceCurrencyCode);

            Assert.AreEqual("PAYEE NAME", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.Name);
            Assert.AreEqual("123456782", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.SpecifiedLegalOrganization.ID.Value);
            Assert.AreEqual("0002", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.PayeeTradeParty.SpecifiedLegalOrganization.ID.SchemeID);

            Assert.AreEqual("30", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.TypeCode);

            Assert.AreEqual(1, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.Count());
            Assert.AreEqual("FR76 1254 2547 2569 8542 5874 698", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementPaymentMeans.PayeePartyCreditorFinancialAccount.ElementAt(0).IBANID.Value);

            Assert.AreEqual(1, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.Count());
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CalculatedAmount.Value);
            Assert.AreEqual("VAT", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).TypeCode);
            Assert.AreEqual(100.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).BasisAmount.Value);
            Assert.AreEqual("S", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).CategoryCode);
            Assert.AreEqual("72", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).DueDateTypeCode);
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ApplicableTradeTax.ElementAt(0).RateApplicablePercent);

            Assert.AreEqual(2, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.Count());

            Assert.AreEqual(false, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ChargeIndicator.Indicator);
            Assert.AreEqual(5.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).ActualAmount.Value);
            Assert.AreEqual("REMISE COMMERCIALE", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).Reason);
            Assert.AreEqual("VAT", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(0).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual(true, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ChargeIndicator.Indicator);
            Assert.AreEqual(10.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).ActualAmount.Value);
            Assert.AreEqual("FRAIS DEPLACEMENT", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).Reason);
            Assert.AreEqual("VAT", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.TypeCode);
            Assert.AreEqual("S", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.CategoryCode);
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeAllowanceCharge.ElementAt(1).CategoryTradeTax.RateApplicablePercent);

            Assert.AreEqual("20220302", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Value);
            Assert.AreEqual("102", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradePaymentTerms.DueDateDateTime.DateTimeString.Format);

            Assert.AreEqual(95.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.LineTotalAmount.Value);
            Assert.AreEqual(10.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.ChargeTotalAmount.Value);
            Assert.AreEqual(5.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.AllowanceTotalAmount.Value);
            Assert.AreEqual(100.80, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxBasisTotalAmount.Value);

            Assert.AreEqual(1, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.Count());
            Assert.AreEqual("EUR", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).CurrencyID);
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TaxTotalAmount.ElementAt(0).Value);

            Assert.AreEqual(120.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.GrandTotalAmount.Value);
            Assert.AreEqual(100.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.DuePayableAmount.Value);
            Assert.AreEqual(20.00, basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.SpecifiedTradeSettlementHeaderMonetarySummation.TotalPrepaidAmount.Value);

            Assert.AreEqual("BUYER ACCOUNT REF", basicWLInvoice.SupplyChainTradeTransaction.ApplicableHeaderTradeSettlement.ReceivableSpecifiedTradeAccountingAccount.ID.Value);
        }
    }
}
