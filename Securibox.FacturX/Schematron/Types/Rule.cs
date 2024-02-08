using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Wmhelp.XPath2;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * rule = element rule {
     *   attribute flag { flagValue }?,
     *   rich,
     *   linkable,
     *   (foreign
     *   & inclusion*
     *   & ((attribute abstract { “true” },
     *   attribute id { xsd:ID },
     *   let*,
     *   (assert | report | extends | p)+)
     *   | (attribute context { pathValue },
     *   attribute id { xsd:ID }?,
     *   attribute abstract { “false” }?,
     *   let*,
     *   (assert | report | extends | p)+)))
     * }
     */
    public class Rule
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        // linkable
        #region Linkable

        [XmlAttribute(AttributeName = "role")]
        public string Role { get; set; }

        [XmlAttribute(AttributeName = "subject")]
        public string Subject { get; set; }

        #endregion

        [XmlElement(ElementName = "include")]
        public Inclusion[] Inclusions { get; set; }

        [XmlElement(ElementName = "let")]
        public Let[] Lets { get; set; }

        [XmlAttribute(AttributeName = "abstract")]
        public bool Abstract { get; set; }

        [XmlElement(ElementName = "p")]
        public P[] Description { get; set; }

        [XmlAttribute(AttributeName = "context")]
        public string Context { get; set; }
        private XPath2Expression compiledContext;

        #region Rich Attributes


        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }

        [XmlAttribute(AttributeName = "see")]
        public string See { get; set; }

        [XmlAttribute(AttributeName = "fpi")]
        public string Fpi { get; set; }

        #endregion

        [XmlElement(ElementName = "assert")]
        public Assert[] Assertions { get; set; }

        [XmlElement(ElementName = "report")]
        public Report[] Reports { get; set; }

        [XmlElement(ElementName = "extends")]
        public Extends[] Extensions { get; set; }

        private EvaluationResult[] EvaluateAssertions(Schema schema, XPathNavigator navigator, Xslt.SchematronContext context, string? ruleContext = null)
        {
            List<EvaluationResult> results = new List<EvaluationResult>();
            if (this.Assertions != null)
            {
                foreach (var assert in this.Assertions)
                {
                    var result = assert.Evaluate(schema, navigator, context, ruleContext);
                    results.Add(result);
                }
            }
            if (this.Reports != null)
            {
                foreach (var report in this.Reports)
                {
                    var result = report.Evaluate(schema, navigator, context);
                    results.Add(result);
                }
            }
            return results.ToArray();
        }

        public RuleResult Evaluate(Schema schema, XPathNavigator navigator, IEnumerable<Let> lets, bool evalAbstract = false)
        {
            if (this.Abstract && !evalAbstract) return RuleResult.Empty;

            List<Let> combined = new List<Let>(lets);
            if (this.Lets != null) combined.AddRange(this.Lets);

            var context = new Xslt.SchematronContext(new NameTable(), combined);
            if (schema.Namespaces != null)
            {
                foreach (var ns in schema.Namespaces)
                {
                    context.AddNamespace(ns.Prefix, ns.Uri);
                }
            }

            var results = new List<EvaluationResult>();

            if (compiledContext == null)
            {
                compiledContext = XPath2Expression.Compile(this.Context, context);
            }

            var returnType = compiledContext.GetResultType(new Dictionary<XmlQualifiedName, object>());
            //compiledContext.SetContext(context);

            var executionContext = compiledContext;
            if (returnType != XPath2ResultType.NodeSet)
            {
                var newContext = (string)navigator.Evaluate(this.Context, context);
                executionContext = XPath2Expression.Compile("//" + newContext);
                //executionContext.SetContext(context);
            }
            else if (!this.Context.StartsWith("/"))
            {
                compiledContext = XPath2Expression.Compile("//" + this.Context, context);
                //compiledContext.SetContext(context);
                executionContext = compiledContext;
            }

            navigator.MoveToRoot();
            //navigator.MoveToFirstChild();
            XPath2NodeIterator nodes = navigator.XPath2Select(this.Context, context);

            foreach (XPathNavigator nav in nodes)
            {
                int contextLine = -1;
                int contextPosition = -1;
                if (nav is IXmlLineInfo info)
                {
                    contextLine = info.LineNumber;
                    contextPosition = info.LinePosition;
                }
                string contextName = nav.Name;

                if (this.Extensions != null)
                {
                    foreach (var extension in this.Extensions)
                    {
                        if (schema.AllRules.ContainsKey(extension.Rule))
                        {
                            var rule = schema.AllRules[extension.Rule];
                            var result = rule.EvaluateAssertions(schema, nav, context);
                            results.AddRange(result);
                        }
                    }
                }
                var assertResults = this.EvaluateAssertions(schema, nav, context);
                foreach (var result in assertResults)
                {
                    result.ContextLine = contextLine;
                    result.ContextPosition = contextPosition;
                    result.ContextElement = contextName;
                }
                results.AddRange(assertResults);
            }

            return new RuleResult() {
                Rule = this,
                RuleFired = nodes.Count > 0,
                ExecutedAssertions = results.ToArray(),
            };
        }
    }
}
