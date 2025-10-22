using Securibox.FacturX.Schematron.Types;

namespace Securibox.FacturX.Schematron.Helpers
{
    public class ValidationReportMapper
    {
        private List<PatternResult> _patternResults;

        public ValidationReportMapper(IEnumerable<PatternResult> patternResults)
        {
            _patternResults = patternResults.ToList();
        }

        public ValidationReportMapper(Dictionary<string, PhaseResult> phaseResults)
        {
            _patternResults = new List<PatternResult>();
            foreach (var phaseResult in phaseResults.Values)
            {
                _patternResults.AddRange(phaseResult.PatternResults);
            }
        }

        private bool IsSuccessfullValidation()
        {
            foreach (var patternResult in _patternResults)
            {
                var ruleResults = patternResult.RuleResults.ToList();
                foreach (var ruleResult in ruleResults)
                {
                    if (ruleResult.ExecutedAssertions.Any(x => x.IsError == true || x.IsWarning == true))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// if the rule wasnt fired and no assertion has been made, we are not adding it to the list
        /// </summary>
        internal ValidationResult MapReport()
        {
            var validationStatus = IsSuccessfullValidation();

            var results = new List<ValidationReport>();
            foreach (var patternResult in _patternResults)
            {

                foreach (var ruleResult in patternResult.RuleResults)
                {

                    foreach (var executedAssertion in ruleResult.ExecutedAssertions)
                    {
                        var validationReport = new ValidationReport();
                        validationReport.Path = ruleResult.Rule.Context;
                        validationReport.ContextElement = executedAssertion.ContextElement;
                        validationReport.ContextLine = executedAssertion.ContextLine;
                        validationReport.ContextPosition = executedAssertion.ContextPosition;
                        validationReport.Description = executedAssertion.AssertInnerText;
                        validationReport.IsError = executedAssertion.IsError;
                        validationReport.IsWarning = executedAssertion.IsWarning;
                        validationReport.BusinessRuleCode = executedAssertion.Assertion.Id;
                        validationReport.Test = executedAssertion.Assertion.Test;

                        results.Add(validationReport);
                    }
                }
            }

            return new ValidationResult(validationStatus, results);
        }
    }
}
