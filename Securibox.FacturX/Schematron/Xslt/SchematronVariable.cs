using System.Xml.XPath;
using System.Xml.Xsl;

namespace Securibox.FacturX.Schematron.Xslt
{
    // The interface used to resolve references to user-defined variables
    // in XPath query expressions at run time. An instance of this class 
    // is returned by the overridden ResolveVariable function of the 
    // custom XsltContext class.
    public class SchematronVariable : IXsltContextVariable
    {
        // Namespace of user-defined variable.
        private string prefix;
        // The name of the user-defined variable.
        private string varName;
        private Types.Let Let { get; set; }

        // Constructor used in the overridden ResolveVariable function of custom XsltContext.
        public SchematronVariable(string prefix, string varName, Types.Let let)
        {
            this.prefix = prefix;
            this.varName = varName;
            this.Let = let;
        }

        // Function to return the value of the specified user-defined variable.
        // The GetParam method of the XsltArgumentList property of the active
        // XsltContext object returns value assigned to the specified variable.
        public object Evaluate(System.Xml.Xsl.XsltContext xsltContext)
        {
            return this.Let.Value;
        }

        // Determines whether this variable is a local XSLT variable.
        // Needed only when using a style sheet.
        public bool IsLocal
        {
            get
            {
                return false;
            }
        }

        // Determines whether this parameter is an XSLT parameter.
        // Needed only when using a style sheet.
        public bool IsParam
        {
            get
            {
                return false;
            }
        }

        public System.Xml.XPath.XPathResultType VariableType
        {
            get
            {
                return XPathResultType.String;
            }
        }
    }
}
