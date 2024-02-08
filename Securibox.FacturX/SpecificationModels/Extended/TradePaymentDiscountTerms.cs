﻿namespace Securibox.FacturX.SpecificationModels.Extended
{
    public class TradePaymentDiscountTerms
    {
        public Minimum.IssueDateTime BasisDateTime { get; set; }
        public Measure BasisPeriodMeasure { get; set; }
        public Minimum.Amount BasisAmount { get; set; }
        public decimal CalculationPercent { get; set; }
        public Minimum.Amount ActualDiscountAmount { get; set; }
    }
}