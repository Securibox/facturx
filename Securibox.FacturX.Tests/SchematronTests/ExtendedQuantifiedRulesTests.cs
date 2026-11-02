using System.Globalization;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Securibox.FacturX.Models.Enums;

namespace Securibox.FacturX.Tests.SchematronTests
{
    /// <summary>
    /// Pass/fail checks for Extended rules that rely on XPath 2.0 constructs
    /// (every/some quantifiers, nested and top-level 'for'), which go through
    /// custom handling in Schematron.Types.Assert. The XML is a minimal invoice,
    /// so only the targeted rule is asserted, not the overall validation result.
    /// </summary>
    internal class ExtendedQuantifiedRulesTests
    {
        private class Line(string id, decimal amount, string? status = null, string? parentId = null)
        {
            public string Id { get; } = id;
            public decimal Amount { get; } = amount;
            public string? Status { get; } = status;
            public string? ParentId { get; } = parentId;
        }

        private static string BuildInvoice(
            Line[] lines,
            decimal? headerLineTotal = null,
            decimal? taxBasisTotal = null,
            decimal taxTotal = 0m,
            decimal? grandTotal = null
        )
        {
            var detailSum = lines.Where(l => l.Status is null or "DETAIL").Sum(l => l.Amount);
            var lineTotal = headerLineTotal ?? detailSum;
            var basis = taxBasisTotal ?? lineTotal;
            var grand = grandTotal ?? basis + taxTotal;

            var sb = new StringBuilder();
            sb.Append(
                """
                <rsm:CrossIndustryInvoice xmlns:rsm="urn:un:unece:uncefact:data:standard:CrossIndustryInvoice:100" xmlns:ram="urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:100" xmlns:udt="urn:un:unece:uncefact:data:standard:UnqualifiedDataType:100">
                  <rsm:ExchangedDocumentContext>
                    <ram:GuidelineSpecifiedDocumentContextParameter>
                      <ram:ID>urn:cen.eu:en16931:2017#conformant#urn:factur-x.eu:1p0:extended</ram:ID>
                    </ram:GuidelineSpecifiedDocumentContextParameter>
                  </rsm:ExchangedDocumentContext>
                  <rsm:ExchangedDocument>
                    <ram:ID>TEST-001</ram:ID>
                    <ram:TypeCode>380</ram:TypeCode>
                  </rsm:ExchangedDocument>
                  <rsm:SupplyChainTradeTransaction>
                """
            );

            foreach (var line in lines)
            {
                sb.Append("<ram:IncludedSupplyChainTradeLineItem><ram:AssociatedDocumentLineDocument>");
                sb.Append($"<ram:LineID>{line.Id}</ram:LineID>");
                if (line.ParentId != null)
                    sb.Append($"<ram:ParentLineID>{line.ParentId}</ram:ParentLineID>");
                if (line.Status != null)
                    sb.Append($"<ram:LineStatusReasonCode>{line.Status}</ram:LineStatusReasonCode>");
                sb.Append("</ram:AssociatedDocumentLineDocument>");
                sb.Append($"<ram:SpecifiedTradeProduct><ram:Name>Item {line.Id}</ram:Name></ram:SpecifiedTradeProduct>");
                sb.Append("<ram:SpecifiedLineTradeSettlement><ram:SpecifiedTradeSettlementLineMonetarySummation>");
                sb.Append($"<ram:LineTotalAmount>{Format(line.Amount)}</ram:LineTotalAmount>");
                sb.Append("</ram:SpecifiedTradeSettlementLineMonetarySummation></ram:SpecifiedLineTradeSettlement>");
                sb.Append("</ram:IncludedSupplyChainTradeLineItem>");
            }

            sb.Append(
                $"""
                    <ram:ApplicableHeaderTradeSettlement>
                      <ram:InvoiceCurrencyCode>EUR</ram:InvoiceCurrencyCode>
                      <ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                        <ram:LineTotalAmount>{Format(lineTotal)}</ram:LineTotalAmount>
                        <ram:TaxBasisTotalAmount>{Format(basis)}</ram:TaxBasisTotalAmount>
                        <ram:TaxTotalAmount currencyID="EUR">{Format(taxTotal)}</ram:TaxTotalAmount>
                        <ram:GrandTotalAmount>{Format(grand)}</ram:GrandTotalAmount>
                        <ram:DuePayableAmount>{Format(grand)}</ram:DuePayableAmount>
                      </ram:SpecifiedTradeSettlementHeaderMonetarySummation>
                    </ram:ApplicableHeaderTradeSettlement>
                  </rsm:SupplyChainTradeTransaction>
                </rsm:CrossIndustryInvoice>
                """
            );

            return sb.ToString();
        }

        private static string Format(decimal value) =>
            value.ToString("0.00", CultureInfo.InvariantCulture);

        private static string[] FailedRules(string xml)
        {
            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
            var result = FacturxSchematronValidator.ValidateXml(
                stream,
                FacturXConformanceLevelType.Extended
            );

            return result
                ._results.Where(r => r.IsError || r.IsWarning)
                .Select(r => r.BusinessRuleCode!)
                .Distinct()
                .ToArray();
        }

        private static readonly Line[] GroupedLines =
        [
            new("1", 30m, "GROUP"),
            new("1.1", 10m, "DETAIL", "1"),
            new("1.2", 20m, "DETAIL", "1"),
            new("2", 5m, "DETAIL"),
        ];

        #region BR-FXEXT-08 (every ... satisfies ... sum(for ...))

        [Test]
        public void BR_FXEXT_08_GroupTotalMatchesChildren_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-08"));
        }

        [Test]
        public void BR_FXEXT_08_NestedGroups_SUCCESS()
        {
            Line[] lines =
            [
                new("1", 35m, "GROUP"),
                new("1.1", 25m, "GROUP", "1"),
                new("1.1.1", 25m, "DETAIL", "1.1"),
                new("1.2", 10m, "DETAIL", "1"),
            ];

            var failed = FailedRules(BuildInvoice(lines));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-08"));
        }

        [Test]
        public void BR_FXEXT_08_GroupTotalDiffersFromChildren_FAILS()
        {
            Line[] lines = [new("1", 99.99m, "GROUP"), .. GroupedLines.Skip(1)];

            var failed = FailedRules(BuildInvoice(lines));

            Assert.That(failed, Does.Contain("BR-FXEXT-08"));
        }

        [Test]
        public void BR_FXEXT_08_NoGroupLines_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice([new("1", 10m), new("2", 20m)]));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-08"));
        }

        #endregion

        #region BR-FXEXT-11 (some ... satisfies)

        [Test]
        public void BR_FXEXT_11_ParentLineExists_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-11"));
        }

        [Test]
        public void BR_FXEXT_11_ParentLineMissing_FAILS()
        {
            Line[] lines = [new("1", 10m, "DETAIL"), new("2", 20m, "DETAIL", "42")];

            var failed = FailedRules(BuildInvoice(lines));

            Assert.That(failed, Does.Contain("BR-FXEXT-11"));
        }

        #endregion

        #region BR-FXEXT-CO-10 / CO-13 / CO-15 (top-level for ... return)

        [Test]
        public void BR_FXEXT_CO_10_HeaderLineTotalMatchesDetailLines_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-CO-10"));
        }

        [Test]
        public void BR_FXEXT_CO_10_HeaderLineTotalDiffers_FAILS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines, headerLineTotal: 100m));

            Assert.That(failed, Does.Contain("BR-FXEXT-CO-10"));
        }

        [Test]
        public void BR_FXEXT_CO_13_TaxBasisMatchesDetailLines_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-CO-13"));
        }

        [Test]
        public void BR_FXEXT_CO_13_TaxBasisDiffers_FAILS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines, taxBasisTotal: 100m));

            Assert.That(failed, Does.Contain("BR-FXEXT-CO-13"));
        }

        [Test]
        public void BR_FXEXT_CO_15_GrandTotalIsBasisPlusTax_SUCCESS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines, taxTotal: 7m));

            Assert.That(failed, Does.Not.Contain("BR-FXEXT-CO-15"));
        }

        [Test]
        public void BR_FXEXT_CO_15_GrandTotalDiffers_FAILS()
        {
            var failed = FailedRules(BuildInvoice(GroupedLines, taxTotal: 7m, grandTotal: 100m));

            Assert.That(failed, Does.Contain("BR-FXEXT-CO-15"));
        }

        #endregion
    }
}
