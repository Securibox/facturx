using Securibox.FacturX.Schematron.Xslt;
using Securibox.FacturX.Utils;
using System.Globalization;
using System.Text;
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
                return String.Join('\n', result.Select(x => x.Trim()));
            }
            else
            {
                return string.Empty;
            }
        }

        public virtual EvaluationResult Evaluate(Schema schema, XPathNavigator navigator, XsltContext context)
        {
            var result = false;
            var assert = this.Test.Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ').Trim();
            assert = System.Text.RegularExpressions.Regex.Replace(assert, @"\s+", " ");
            result = EvaluateLogicalExpression(context, navigator, assert);

            return new EvaluationResult
            {
                Assertion = this,
                IsError = !result,
                AssertInnerText = this.EvaluateDescriptionFragment(context)
            };
        }

        public bool EvaluateLogicalExpression(XsltContext context, XPathNavigator navigator, string expr)
        {
            expr = TrimOuterParentheses(expr);
            var orParts = SplitTopLevel(expr, " or ");
            if (orParts.Length > 1)
            {
                return orParts.Any(part => EvaluateLogicalExpression(context, navigator, part));
            }

            var andParts = SplitTopLevel(expr, " and ");
            if (andParts.Length > 1)
            {
                return andParts.All(part => EvaluateLogicalExpression(context, navigator, part));
            }

            var pattern0 = "document\\(.*\\)(\\/.*)";
            var pattern1 = "document\\(.*,.*\\)";
            if (new Regex(pattern0).IsMatch(expr) || new Regex(pattern1).IsMatch(expr))
            {
                return EvaluateDocumentFunction(navigator, context, expr);
            }
            else
            {
                return GetAndEvaluateResult(context, navigator, expr);
            }
        }

        private bool EvaluateDocumentFunction(XPathNavigator navigator, XsltContext context, string exp)
        {
            exp = exp.Replace(")", ",\'") + "\')";
            var compiledExpr = navigator.Compile(exp);
            compiledExpr.SetContext(context);

            var result = navigator.Evaluate(compiledExpr);
            var returnType = compiledExpr.ReturnType;

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

            return ok;
        }

        private bool GetAndEvaluateResult(XsltContext context, XPathNavigator navigator, string expr, Dictionary<string, Object>? variablesDictionary = null)
        {
            #region Every condition
            var everyMatch = Regex.Match(expr, @"every\s+(.*)\s+satisfies\s+(.+)", RegexOptions.Singleline);
            if (everyMatch.Success && (expr.Contains(" for ") || expr.Contains(" let ")))
            {
                var parts = everyMatch.Groups[1].ToString().Split(new[] { " in " }, StringSplitOptions.None);
                var varName = parts[0].TrimStart('$').Trim();
                var exprPart = parts[1].Trim();
                var resultExprPart = navigator.XPath2Evaluate(exprPart, context);
                var varValues = new List<object>();
                if (resultExprPart is XPath2NodeIterator iterator)
                {
                    while (iterator.MoveNext())
                    {
                        varValues.Add(iterator.Current.TypedValue ?? iterator.Current.Value);
                    }
                }
                else if (resultExprPart is IEnumerable<object> sequence)
                {
                    foreach (var item in sequence)
                    {
                        varValues.Add(item);
                    }
                }
                else
                {
                    varValues.Add(resultExprPart);
                }

                string condition = everyMatch.Groups[2].ToString().Trim();
                if (condition.StartsWith('(') && condition.EndsWith(')'))
                    condition = condition.Substring(1, condition.Length - 2);

                if (variablesDictionary is null)
                {
                    variablesDictionary = new Dictionary<string, object>();
                }

                foreach (var value in varValues)
                {
                    variablesDictionary[varName] = value;
                    if (!GetAndEvaluateResult(context, navigator, condition, variablesDictionary))
                    {
                        return false;
                    }
                }

                return true;
            }
            #endregion

            #region Some condition
            var someMatch = Regex.Match(expr, @"some\s+(.*)\s+satisfies\s+(.+)", RegexOptions.Singleline);
            if (someMatch.Success && (expr.Contains(" for ") || expr.Contains(" let ")))
            {
                var parts = someMatch.Groups[1].ToString().Split(new[] { " in " }, StringSplitOptions.None);
                var varName = parts[0].TrimStart('$').Trim();
                var exprPart = Regex.Replace(parts[1].Trim(), @"xs:decimal\s*\(([^)]+)\)", "$1");
                var nodes = navigator.Select(exprPart, context);
                var varValues = new List<object>();
                while (nodes.MoveNext())
                {
                    varValues.Add(nodes.Current.UnderlyingObject ?? nodes.Current.Value);
                }

                if (variablesDictionary is null)
                {
                    variablesDictionary = new Dictionary<string, object>();
                }

                string condition = someMatch.Groups[2].ToString().Trim();
                if (condition.StartsWith('(') && condition.EndsWith(')'))
                    condition = condition.Substring(1, condition.Length - 2);

                foreach (var value in varValues)
                {
                    variablesDictionary[varName] = value;
                    if (GetAndEvaluateResult(context, navigator, condition, variablesDictionary))
                    {
                        return true;
                    }
                }

                return false;
            }
            #endregion

            var match = Regex.Match(expr, @"(?:for|let)\s+(.*?)\s+return\s+(.+)", RegexOptions.Singleline);
            if (match.Success)
            {
                expr = EvaluateForOrLetStatement(context, navigator, expr, match, variablesDictionary);
            }

            var schematronContext = context as SchematronContext;
            if (schematronContext == null)
            {
                throw new Exception("Schematron context is null");
            }

            var variables = new Dictionary<XmlQualifiedName, object>();
            foreach (var let in schematronContext.Lets)
            {
                if (let.Value == ".")
                {
                    variables[new XmlQualifiedName(let.Name)] = navigator.Value;
                }
                else
                {
                    variables[new XmlQualifiedName(let.Name)] = let.Value;
                }
            }

            var compiledExpr = XPath2Expression.Compile(expr, context);
            var returnType = compiledExpr.GetResultType(variables);
            object result;
            if (variables.Count != 0)
            {
                result = navigator.XPath2Evaluate(expr, context, DynamicXPathVariables.BuildDynamicProps(variables));
            }
            else
            {
                result = navigator.XPath2Evaluate(expr, context);
            }

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

            return ok;
        }

        private string EvaluateForOrLetStatement(XsltContext context, XPathNavigator navigator, string expr, Match forMatch, Dictionary<string, Object>? variables = null)
        {
            var bindings = forMatch.Groups[1].Value.Trim();
            var returnExpr = forMatch.Groups[2].Value.Trim();

            var bindingParts = bindings.Split(',')
                .Select(b => b.Trim())
                .Where(b => b.StartsWith('$'))
                .ToArray();

            if (variables is null)
            {
                variables = new Dictionary<string, object>();
            }

            foreach (var binding in bindingParts)
            {
                if (binding.Contains(" in "))
                {
                    var parts = binding.Split(new[] { " in " }, StringSplitOptions.None);
                    if (parts.Length != 2)
                        throw new ArgumentException($"Invalid binding: {binding}");

                    var varName = parts[0].TrimStart('$').Trim();
                    var exprPart = parts[1].Trim();
                    foreach (var kvp in variables)
                    {
                        exprPart = exprPart.Replace("$" + kvp.Key, ConversionUtils.ToInvariantNumericString(kvp.Value));
                    }

                    var value = navigator.XPath2Evaluate(exprPart, context);
                    if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)) || value.ToString() == "()")
                    {
                        value = 0;
                    }

                    variables[varName] = value;
                }
                else if (binding.Contains(":="))
                {
                    var parts = binding.Split(new[] { ":=" }, StringSplitOptions.None);
                    if (parts.Length != 2)
                        throw new ArgumentException($"Invalid 'let' binding: {binding}");

                    var varName = parts[0].TrimStart('$').Trim();
                    var exprPart = parts[1].Trim();

                    foreach (var kvp in variables)
                    {
                        exprPart = exprPart.Replace("$" + kvp.Key, ConversionUtils.ToInvariantNumericString(kvp.Value));
                    }
                        
                    var value = navigator.XPath2Evaluate(exprPart, context);
                    if (value == null || (value is string s && string.IsNullOrWhiteSpace(s)) || value.ToString() == "()")
                    {
                        value = 0;
                    }

                    variables[varName] = value;
                }
                else
                {
                    throw new ArgumentException($"Invalid binding: {binding}");
                }
            }

            foreach (var kvp in variables)
            {
                returnExpr = returnExpr.Replace("$" + kvp.Key, ConversionUtils.ToInvariantNumericString(kvp.Value));
            }

            return returnExpr;
        }

        private string[] SplitTopLevel(string expr, string op)
        {
            var parts = new List<string>();
            int parenDepth = 0;
            int bracketsDepth = 0;
            int last = 0;
            int i = 0;

            while (i <= expr.Length - op.Length)
            {
                if (char.IsLetter(expr[i]) && (expr.Substring(i).StartsWith("for", StringComparison.OrdinalIgnoreCase) || expr.Substring(i).StartsWith("let", StringComparison.OrdinalIgnoreCase)))
                {
                    var match = Regex.Match(expr, @"(?:for|let)\s+(.*?)\s+return\s+(.+)", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        int forBlockLength = match.Length;
                        i += forBlockLength;
                        continue;
                    }
                }

                if (expr[i] == '(')
                {
                    parenDepth++;
                }
                else if (expr[i] == ')')
                {
                    parenDepth--;
                }
                else if (expr[i] == '[')
                {
                    bracketsDepth++;
                }
                else if (expr[i] == ']')
                {
                    bracketsDepth--;
                }

                if (parenDepth == 0 && bracketsDepth == 0 && expr.Substring(i, op.Length) == op)
                {
                    parts.Add(expr.Substring(last, i - last).Trim());
                    last = i + op.Length;
                    i += op.Length - 1;
                }

                i++;
            }

            if (last < expr.Length)
            {
                parts.Add(expr.Substring(last).Trim());
            }

            return parts.ToArray();
        }

        private static string TrimOuterParentheses(string expr)
        {
            expr = expr.Trim();
            while (expr.StartsWith('(') && expr.EndsWith(')'))
            {
                int depth = 0;
                bool matched = false;

                for (int i = 0; i < expr.Length; i++)
                {
                    if (expr[i] == '(')
                    {
                        depth++;

                    }
                    else if (expr[i] == ')')
                    {
                        depth--;

                    }
                    if (depth == 0 && i < expr.Length - 1)
                    {
                        matched = false;
                        break;
                    }

                    matched = (depth == 0 && i == expr.Length - 1);
                }

                if (matched)
                {
                    expr = expr.Substring(1, expr.Length - 2).Trim();
                }
                else
                {
                    break;
                }
            }

            return expr;
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