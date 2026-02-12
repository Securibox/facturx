using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Securibox.FacturX.Schematron.Types
{
    /*
     * 5.4.13 schema element
     * The top-level element of a Schematron schema.
     * The optional schemaVersion attribute gives the version of the schema. Its allowed values are not
     * defined by this part of ISO/IEC 19757 and its use is implementation-dependent.
     * The optional queryBinding attribute provides the short name of the query language binding in use
     * The defaultPhase attribute may be used to indicate the phase to use in the absence of explicit usersupplied
     * information.
     * The title and p elements allow rich documentation.
     * The icon, see and fpi attributes allow rich interfaces and documentation.
     * schema = element schema {
     *   attribute id { xsd:ID }?,
     *   rich,
     *   attribute schemaVersion { non-empty-string }?,
     *   attribute defaultPhase { xsd:IDREF }?,
     *   attribute queryBinding { non-empty-string }?,
     *   (foreign
     *   & inclusion*
     *   & (title?, ns*, p*, let*, phase*, pattern+, p*, diagnostics?, properties))
     * }
     */
    [Serializable]
    [XmlRoot(Namespace = "http://purl.oclc.org/dsdl/schematron", ElementName = "schema")]
    public class Schema
    {
        public const string SchematronNamespace = "http://purl.oclc.org/dsdl/schematron";
        public const string OldSchematronNamespace = "";

        #region Attributes

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "schemaVersion")]
        public string SchemaVersion { get; set; }

        [XmlAttribute(AttributeName = "queryBinding")]
        public string QueryBinding { get; set; }

        [XmlAttribute(AttributeName = "defaultPhase")]
        public string DefaultPhase { get; set; }

        #endregion

        #region Child Elements

        [XmlElement(ElementName = "title")]
        public Title Title { get; set; }

        [XmlElement(ElementName = "ns")]
        public Ns[] Namespaces { get; set; }

        [XmlElement(ElementName = "let")]
        public Let[] Lets { get; set; }

        [XmlElement(ElementName = "pattern")]
        public Pattern[] Patterns { get; set; }

        // let*
        // p*
        // phase*
        // pattern+
        // p* -> Why is this duplicated here?
        // diagnostics?
        // properties

        #endregion

        #region Rich Attributes

        [XmlAttribute(AttributeName = "icon")]
        public string Icon { get; set; }

        [XmlAttribute(AttributeName = "see")]
        public string See { get; set; }

        [XmlAttribute(AttributeName = "fpi")]
        public string Fpi { get; set; }

        #endregion

        [XmlElement(ElementName = "phase")]
        public Phase[] Phases { get; set; }

        [XmlIgnore]
        public Dictionary<string, Assert> AllAssertions { get; set; } = null;

        private void BuildAllAssertions()
        {
            AllAssertions = new Dictionary<string, Assert>();
            if (this.Patterns == null)
                return;

            for (int i = 0; i < this.Patterns.Length; i++)
            {
                if (this.Patterns[i].Rules != null)
                {
                    foreach (var rule in this.Patterns[i].Rules)
                    {
                        if (rule.Assertions != null)
                        {
                            for (int j = 0; j < rule.Assertions.Length; j++)
                            {
                                if (rule.Context != null)
                                {
                                    AllAssertions.Add(
                                        $"Pattern/{i}/" + rule.Context + "/Assertions/" + j,
                                        rule.Assertions[j]
                                    );
                                }
                            }

                            //foreach (var assert in rule.Assertions)
                            //{
                            //    if (rule.Context != null)
                            //        AllAssertions.Add(rule.Context, assert);
                            //}
                        }
                        if (rule.Reports != null)
                        {
                            for (int k = 0; k < rule.Reports.Length; k++)
                            {
                                if (rule.Context != null)
                                    AllAssertions.Add(
                                        $"Pattern/{i}/" + rule.Context + "/Reports/" + k,
                                        rule.Reports[k]
                                    );
                            }

                            //foreach (var report in rule.Reports)
                            //{
                            //    if (rule.Context != null)
                            //        AllAssertions.Add(rule.Context, report);
                            //}
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        public Dictionary<string, Rule> AllRules { get; set; } = null;

        private void BuildAllRules()
        {
            AllRules = new Dictionary<string, Rule>();
            if (this.Patterns == null)
                return;

            foreach (var pattern in this.Patterns)
            {
                if (pattern.Rules != null)
                {
                    for (int i = 0; i < pattern.Rules.Length; i++)
                    {
                        if (pattern.Rules[i].Context != null)
                        {
                            AllRules.Add(pattern.Rules[i].Context + "/Rule/" + i, pattern.Rules[i]);
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        public Dictionary<string, Pattern> AllPatterns { get; set; } = null;

        private void BuildAllPatterns()
        {
            AllPatterns = new Dictionary<string, Pattern>();
            if (this.Patterns != null)
            {
                if (this.Patterns.All(x => !string.IsNullOrWhiteSpace(x.Id)))
                {
                    foreach (var pattern in this.Patterns)
                    {
                        AllPatterns.Add(pattern.Id, pattern);
                    }
                }
                else
                {
                    for (int i = 0; i < this.Patterns.Length; i++)
                    {
                        AllPatterns.Add("Pattern/" + i, this.Patterns[i]);
                    }
                }
            }
        }

        public Dictionary<string, PhaseResult> EvaluatePhase(
            XPathNavigator navigator,
            string selectedPhase = null
        )
        {
            if (AllAssertions == null)
                BuildAllAssertions();
            if (AllRules == null)
                BuildAllRules();
            if (AllPatterns == null)
                BuildAllPatterns();

            List<Let> lets = new List<Let>();
            if (this.Lets != null)
                lets.AddRange(this.Lets);

            Dictionary<string, PhaseResult> result = new Dictionary<string, PhaseResult>();
            if (this.Phases != null)
            {
                foreach (var phase in this.Phases)
                {
                    if (
                        selectedPhase != null
                        && selectedPhase.Equals(phase.Id)
                        && selectedPhase != "#all"
                    )
                        continue;

                    //TODO
                    if (phase.Id.Equals("syntax_phase"))
                        continue;

                    List<PatternResult> results = new List<PatternResult>();
                    foreach (var pattern in phase.ActivePatterns)
                    {
                        if (AllPatterns.ContainsKey(pattern.Pattern))
                        {
                            var patternRef = AllPatterns[pattern.Pattern];
                            navigator.MoveToRoot();
                            var patternResult = patternRef.Evaluate(this, navigator, lets);
                            results.Add(patternResult);
                        }
                        else
                        {
                            throw new ArgumentException("Pattern Not Found");
                        }
                    }
                    result.Add(
                        phase.Id,
                        new PhaseResult() { Phase = phase, PatternResults = results.ToArray() }
                    );
                }
            }
            return result;
        }

        public List<PatternResult> Evaluate(XPathNavigator navigator)
        {
            if (AllAssertions == null)
                BuildAllAssertions();
            if (AllRules == null)
                BuildAllRules();
            if (AllPatterns == null)
                BuildAllPatterns();

            List<Let> lets = new List<Let>();
            if (this.Lets != null)
                lets.AddRange(this.Lets);

            List<PatternResult> result = new List<PatternResult>();
            foreach (var pattern in this.Patterns)
            {
                navigator.MoveToRoot();
                if (!pattern.Abstract)
                {
                    var patternResult = pattern.Evaluate(this, navigator, lets);
                    result.Add(patternResult);
                }
            }
            return result;
        }

        public static Schema FromFile(string path, XmlResolver resolver = null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Schema));
            using (var s = File.OpenRead(path))
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                if (resolver != null)
                {
                    settings.DtdProcessing = DtdProcessing.Parse;
                    settings.XmlResolver = resolver;
                }
                XmlReader reader = XmlReader.Create(s, settings);
                return (Schema)serializer.Deserialize(reader);
            }
        }
    }
}
