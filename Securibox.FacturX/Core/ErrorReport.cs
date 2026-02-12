namespace Securibox.FacturX.Core
{
    public class ErrorReport
    {
        public string ErrorClass { get; internal set; }

        public string ErrorDescription { get; internal set; }

        public string ErrorNodePath { get; internal set; }

        public ErrorLevel ErrorLevel { get; internal set; }

        public string? InnerXml { get; internal set; }

        public ErrorReport() { }
    }
}
