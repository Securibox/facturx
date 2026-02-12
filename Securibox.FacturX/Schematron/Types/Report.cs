using System.Xml.XPath;
using System.Xml.Xsl;
using Securibox.FacturX.Schematron.Types;

namespace Securibox.FacturX.Schematron.Types
{
    [Serializable]
    public class Report : Assert
    {
        public override EvaluationResult Evaluate(
            Schema schema,
            XPathNavigator navigator,
            XsltContext context
        )
        {
            var res = base.Evaluate(schema, navigator, context);
            return new EvaluationResult
            {
                Assertion = this,
                IsWarning = !res.IsError,
                AssertInnerText = this.EvaluateDescriptionFragment(context),
            };
        }
    }
}
