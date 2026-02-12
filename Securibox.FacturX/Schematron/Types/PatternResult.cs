namespace Securibox.FacturX.Schematron.Types
{
    public class PatternResult
    {
        public Pattern Pattern { get; set; } = null;

        public bool PatternFired { get; set; } = false;

        public RuleResult[] RuleResults { get; set; } = null;

        public static readonly PatternResult Empty = new PatternResult();
    }
}
