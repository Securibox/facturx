using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * pattern = element pattern {
     *   attribute documents { pathValue }?,
     *   rich,
     *   (foreign
     *   & inclusion*
     *   & ((attribute abstract { “true” },
     *   attribute id { xsd:ID },
     *   title?,
     *   (p*, let*, rule*))
     *   | (attribute abstract { “false” }?,
     *   attribute id { xsd:ID }?,
     *   title?,
     *   (p*, let*, rule*))
     *   | (attribute abstract { “false” }?,
     *   attribute is-a { xsd:IDREF },
     *   attribute id { xsd:ID }?,
     *   title?,
     *   (p*, param*))))
     * }
     */
    [Serializable]
    public class Pattern
    {
        public string Documents { get; set; }

        [XmlElement(ElementName = "title")]
        public Title Title { get; set; }

        [XmlElement(ElementName = "let")]
        public Let[] Lets { get; set; }

        [XmlElement(ElementName = "include")]
        public Inclusion[] Inclusions { get; set; }

        #region Rich Attributes

        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }

        [XmlAttribute(AttributeName = "see")]
        public string See { get; set; }

        [XmlAttribute(AttributeName = "fpi")]
        public string Fpi { get; set; }

        #endregion

        [XmlAttribute(AttributeName = "abstract")]
        public bool Abstract { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlElement(ElementName = "p")]
        public P[] Description { get; set; }

        [XmlElement(ElementName = "rule")]
        public Rule[] Rules { get; set; }

        [XmlElement(ElementName = "param")]
        public Param[] Parameters { get; set; }

        [XmlAttribute(AttributeName = "is-a")]
        public string IsA { get; set; }

        public PatternResult Evaluate(
            Schema schema,
            XPathNavigator navigator,
            IEnumerable<Let> lets,
            bool evalAbstract = false
        )
        {
            if (this.Abstract && !evalAbstract)
                return PatternResult.Empty;

            List<Let> combined = new List<Let>(lets);
            if (this.Lets != null)
                combined.AddRange(this.Lets);
            if (this.Parameters != null)
                combined.AddRange(this.Parameters);

            var context = new Xslt.SchematronContext(new NameTable(), combined);
            var results = new List<RuleResult>();

            if (IsA != null)
            {
                if (schema.AllPatterns.ContainsKey(this.IsA))
                {
                    var pattern = schema.AllPatterns[this.IsA];
                    var result = pattern.Evaluate(schema, navigator, combined, true);
                    results.AddRange(result.RuleResults);
                }
            }
            if (this.Rules != null)
            {
                foreach (var rule in this.Rules)
                {
                    if (!rule.Abstract)
                    {
                        var result = rule.Evaluate(schema, navigator, combined);
                        results.Add(result);
                    }
                }
            }

            return new PatternResult()
            {
                Pattern = this,
                PatternFired = results.Any(x => x.RuleFired),
                RuleResults = results.ToArray(),
            };
        }
    }
}
