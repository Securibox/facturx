using System.Xml.XPath;
using System.Xml.Xsl;

namespace Securibox.FacturX.Schematron.Types
{
    [Serializable]
    public class Report : Assert
    {
        public override EvaluationResult Evaluate(Schema schema, XPathNavigator navigator, XsltContext context, string? ruleContext = null)
        {
            var res = base.Evaluate(schema, navigator, context);
            res.IsError = !res.IsError;
            return res;
        }
    }
}
