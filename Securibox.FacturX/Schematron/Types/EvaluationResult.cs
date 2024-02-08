namespace Securibox.FacturX.Schematron.Types
{
    public class EvaluationResult
    {
        public Assert Assertion { get; set; } = null;
        public bool IsError { get; set; } = false;
        public string AssertInnerText { get; set; } = null;
        public int ContextLine { get; set; } = -1;
        public int ContextPosition { get; set; } = -1;
        public string ContextElement { get; set; } = null;
        public static readonly EvaluationResult Empty = new EvaluationResult();
    }
    public class RuleResult
    {
        public Rule Rule { get; set; } = null;
        public bool RuleFired { get; set; } = false;
        public EvaluationResult[] ExecutedAssertions { get; set; } = null;
        public static readonly RuleResult Empty = new RuleResult();
    }
    public class PatternResult
    {
        public Pattern Pattern { get; set; } = null;
        public bool PatternFired { get; set; } = false;
        public RuleResult[] RuleResults { get; set; } = null;
        public static readonly PatternResult Empty = new PatternResult();
    }
    public class PhaseResult
    {
        public Phase Phase { get; set; }
        public PatternResult[] PatternResults { get; set; }
        public static readonly PhaseResult Empty = new PhaseResult();
    }
}
