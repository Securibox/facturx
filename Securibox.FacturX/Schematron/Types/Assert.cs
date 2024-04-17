using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Xml.Xsl;
using Wmhelp.XPath2;


namespace Securibox.FacturX.Schematron.Types
{
    /*
     * assert = element assert {
     *   attribute test { exprValue },
     *   attribute flag { flagValue }?,
     *   attribute id { xsd:ID }?,
     *   attribute diagnostics { xsd:IDREFS }?,
     *   attribute properties { xsd:IDREFS }?,
     *   rich,
     *   linkable,
     *   (foreign & (text | name | value-of | emph | dir | span)*)
     *   }
     */
    [Serializable]
    public class Assert : IXmlSerializable
    {
        private XPath2Expression? compiledExpressionXPath2;
        private XPathExpression? compiledExpression;

        public string Id { get; set; }
        public string Test { get; set; }
        public string Flag { get; set; }
        public string Diagnostics { get; set; }
        public string Properties { get; set; }

        #region Linkable
        public string Role { get; set; }
        public string Subject { get; set; }
        #endregion

        #region Rich Attributes
        public string Icon { get; set; }
        public string See { get; set; }
        public string Fpi { get; set; }
        #endregion

        public string DescriptionFragment { get; set; }


        public string EvaluateDescriptionFragment(XsltContext context)
        {
            var fragment = new XmlDocument();
            var nav = fragment.CreateNavigator();
            if (nav != null)
            {
                fragment.LoadXml($"<root xmlns:sch=\"http://purl.oclc.org/dsdl/schematron\">{this.DescriptionFragment}</root>");
                var nsMgr = new XmlNamespaceManager(fragment.NameTable);
                nsMgr.AddNamespace("sch", "http://purl.oclc.org/dsdl/schematron");
                var nodes = fragment.SelectNodes("//sch:value-of", nsMgr);
                if (nodes != null && nodes.Count > 0)
                {
                    foreach (XmlNode node in nodes)
                    {
                        if (node.Attributes != null && node.Attributes["select"].Value != null)
                        {
                            var evalExpr = XPathExpression.Compile(node.Attributes["select"].Value);
                            evalExpr.SetContext(context);
                            var text = nav.Evaluate(evalExpr);
                            var replNode = fragment.CreateTextNode((string)text);
                            if (node.ParentNode != null)
                            {
                                node.ParentNode.ReplaceChild(replNode, node);
                            }
                        }
                    }
                }
            }

            var result = fragment.DocumentElement?.InnerXml.Trim().Split('\n');
            if (result != null && result.Length > 0)
            {
                return String.Join("\n", result.Select(x => x.Trim()));
            }
            else
            {
                return string.Empty;
            }
        }

        private EvaluationResult EvaluateDocumentFunction(Schema schema, XPathNavigator navigator, XsltContext context)
        {
            compiledExpression ??= navigator.Compile(this.Test);
            compiledExpression.SetContext(context);

            var result = navigator.Evaluate(compiledExpression);
            var returnType = compiledExpression.ReturnType;

            bool ok = false;
            switch (returnType)
            {
                case XPathResultType.Boolean:
                    ok = (bool)result;
                    break;
                case XPathResultType.Error:
                    ok = false;
                    break;
                case XPathResultType.NodeSet:
                    ok = ((XPathNodeIterator)result).Count != 0;
                    break;
                case XPathResultType.Number:
                    ok = int.TryParse(result.ToString(), out int t);
                    //ok = (int)result != 0;
                    break;
                case XPathResultType.String:
                    ok = !String.IsNullOrEmpty((string)result);
                    break;
                default:
                    ok = false;
                    break;
            }

            return new EvaluationResult()
            {
                Assertion = this,
                IsError = !ok,
                AssertInnerText = this.EvaluateDescriptionFragment(context)
            };
        }

        public virtual EvaluationResult Evaluate(Schema schema, XPathNavigator navigator, XsltContext context)
        {
            var pattern0 = "document\\(.*\\)(\\/\\/.*)";
            var pattern1 = "document\\(.*,.*\\)";

            if (new Regex(pattern0).IsMatch(this.Test) || new Regex(pattern1).IsMatch(this.Test))
            {
                this.Test = this.Test.Replace(")", ",\'") + "\')";
                return EvaluateDocumentFunction(schema, navigator, context);
            }
            else
            {
                compiledExpressionXPath2 ??= XPath2Expression.Compile(this.Test, context);

                var returnType = compiledExpressionXPath2.GetResultType(new Dictionary<XmlQualifiedName, object>());
                var result = navigator.XPath2Evaluate(this.Test, context);

                bool ok = false;
                switch (returnType)
                {
                    case XPath2ResultType.Boolean:
                        ok = (bool)result;
                        break;
                    case XPath2ResultType.Error:
                        ok = false;
                        break;
                    case XPath2ResultType.NodeSet:
                        ok = ((XPath2NodeIterator)result).Count != 0;
                        break;
                    case XPath2ResultType.Number:
                        ok = int.TryParse(result.ToString(), out int t);
                        //ok = (int)result != 0;
                        break;
                    case XPath2ResultType.String:
                        ok = !String.IsNullOrEmpty((string)result);
                        break;
                    default:
                        ok = false;
                        break;
                }

                return new EvaluationResult()
                {
                    Assertion = this,
                    IsError = !ok,
                    AssertInnerText = this.EvaluateDescriptionFragment(context)
                };
            }
        }

        public XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(XmlReader reader)
        {
            this.Id = reader.GetAttribute("id");
            this.Test = reader.GetAttribute("test");
            this.Flag = reader.GetAttribute("flag");
            this.Role = reader.GetAttribute("role");
            this.Subject = reader.GetAttribute("subject");
            this.Icon = reader.GetAttribute("icon");
            this.See = reader.GetAttribute("see");
            this.Fpi = reader.GetAttribute("fpi");
            this.DescriptionFragment = reader.ReadInnerXml();
            reader.Read();
        }

        public void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
