using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Xsl;
using System.Xml;

namespace Securibox.FacturX.Schematron.Xslt
{
    public class SchematronContext : XsltContext
    {
        private IEnumerable<Types.Let> Lets { get; set; }

        public SchematronContext(IEnumerable<Types.Let> lets)
        {
            this.Lets = lets;
        }

        public SchematronContext(NameTable nt, IEnumerable<Types.Let> lets)
            : base(nt)
        {
            this.Lets = lets;
        }

        public override bool Whitespace => true;

        public override int CompareDocument(string baseUri, string nextbaseUri)
        {
            return 0;
        }

        public override bool PreserveWhitespace(XPathNavigator node)
        {
            return false;
        }

        public override IXsltContextFunction ResolveFunction(string prefix, string name, XPathResultType[] ArgTypes)
        {
            return null;
        }

        public override IXsltContextVariable ResolveVariable(string prefix, string name)
        {
            var let = this.Lets.FirstOrDefault(x => x.Name.Equals(name));
            if (let != null)
            {
                return new SchematronVariable(prefix, name, let);
            }
            return null;
        }

    }
}
