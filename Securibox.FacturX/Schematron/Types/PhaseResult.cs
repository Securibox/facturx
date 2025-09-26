namespace Securibox.FacturX.Schematron.Types
{
    public class PhaseResult
    {
        public Phase Phase { get; set; }

        public PatternResult[] PatternResults { get; set; }

        public static readonly PhaseResult Empty = new PhaseResult();
    }
}