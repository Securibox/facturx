namespace Securibox.FacturX.Schematron.Types
{
    public class RuleResult
    {
        public Rule Rule { get; set; } = null;

        public bool RuleFired { get; set; } = false;

        public EvaluationResult[] ExecutedAssertions { get; set; } = null;

        public static readonly RuleResult Empty = new RuleResult();
    }
}
