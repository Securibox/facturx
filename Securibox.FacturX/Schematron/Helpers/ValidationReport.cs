namespace Securibox.FacturX.Schematron.Helpers
{
    public class ValidationReport
    {
        public string? Path { get; set; } = null;
        public string? Test { get; set; } = null;
        public string? Description { get; set; } = null;
        public string? BusinessRuleCode { get; set; } = null;
        public int ContextLine { get; set; } = -1;
        public int ContextPosition { get; set; } = -1;
        public string? ContextElement { get; set; } = null;
        public bool IsError { get; internal set; } = false;
    }
}
