namespace Securibox.FacturX.Schematron.Types
{
    public class EvaluationResult
    {
        public Assert Assertion { get; set; } = null;

        public bool IsError { get; set; } = false;

        public bool IsWarning { get; set; } = false;

        public string AssertInnerText { get; set; } = null;

        public int ContextLine { get; set; } = -1;

        public int ContextPosition { get; set; } = -1;

        public string ContextElement { get; set; } = null;

        public static readonly EvaluationResult Empty = new EvaluationResult();
    }
}
