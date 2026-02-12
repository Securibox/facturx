namespace Securibox.FacturX.Schematron.Helpers
{
    public class ValidationResult
    {
        public bool _isSuccessfullValidation;
        public IEnumerable<ValidationReport> _results;

        public ValidationResult(bool isSuccessfullValidation, IEnumerable<ValidationReport> results)
        {
            _isSuccessfullValidation = isSuccessfullValidation;
            _results = results;
        }
    }
}
