using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Securibox.FacturX.Schematron.Xslt
{
    public class DocumentFunction : IXsltContextFunction
    {
        public XPathResultType[] ArgTypes { get { return null; } }

        public int Maxargs { get { return 4; } }

        public int Minargs { get { return 4; } }

        public XPathResultType ReturnType { get { return XPathResultType.NodeSet; } }

        public DocumentFunction()
        {
        }

        public object Invoke(XsltContext xsltContext, object[] args, XPathNavigator navigator)
        {
            return ResolveDocumentFunction(xsltContext, (string)args[0], (string)args[1], navigator);
        }

        private static XPathNodeIterator? ResolveDocumentFunction(XsltContext xsltContext, string externalFilePath, string path, XPathNavigator navigator)
        {
            externalFilePath = externalFilePath.Replace(".xml", string.Empty);
            var codedbFile = Resources.ResourceManager.GetString(externalFilePath);

            if (codedbFile != null)
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(codedbFile);
                using (var codedbStream = new MemoryStream(byteArray))
                {
                    var codeDbNav = new XPathDocument(codedbStream);
                    var codeDBNavigator = codeDbNav.CreateNavigator();
                    codeDBNavigator.MoveToFirstChild();

                    var regex = new Regex(@"\$([a-zA-Z]+[0-9]*)");
                    var dolarVariableMatch = regex.Match(path);

                    if (dolarVariableMatch != null && dolarVariableMatch.Groups != null && dolarVariableMatch.Groups.Count > 0)
                    {
                        var dolarVariable = xsltContext.ResolveVariable(string.Empty, dolarVariableMatch.Groups[1].Value);
                        var variable = dolarVariable.Evaluate(xsltContext);

                        if (variable != null)
                        {
                            var attributeRegex = new Regex(@"\@([a-zA-Z]+[0-9]*)"); //check if it is an xml attribute
                            var attributeMatch = attributeRegex.Match(variable.ToString());
                            if (attributeMatch != null && attributeMatch.Groups != null && attributeMatch.Groups.Count > 0)
                            {
                                var attribute = attributeMatch.Groups[1].Value.Replace("@", string.Empty);
                                navigator.MoveToAttribute(attribute, string.Empty);

                                var attributeValue = navigator.Value;
                                if (attributeValue is string)
                                {
                                    path = path.Replace(dolarVariableMatch.Value, string.Format("\"{0}\"", attributeValue));
                                }
                                else
                                {
                                    path = path.Replace(dolarVariableMatch.Value, attributeValue);
                                }

                                var compiledExpression = codeDBNavigator.Compile(path);
                                compiledExpression.SetContext(xsltContext);

                                var result = (XPathNodeIterator)codeDBNavigator.Evaluate(compiledExpression);
                                return result;
                            }
                        }
                    }
                }

            }
            return null;
        }
    }
}
