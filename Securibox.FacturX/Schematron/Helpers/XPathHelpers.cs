using System.Runtime.CompilerServices;
using System.Xml.XPath;

[assembly: InternalsVisibleTo("Securibox.FacturX.Tests")]

namespace Securibox.FacturX.Schematron.Helpers
{
    internal static class XPathHelpers
    {
        internal static string GetPath(this XPathNavigator navigator)
        {
            var parts = new List<String>();

            // SelectAncestors always yields an unnamed virtual root node, that's why using prevNode works
            var ancestors = navigator.SelectAncestors(XPathNodeType.All, false);
            var prevNode = navigator;
            foreach (XPathNavigator node in ancestors)
            {
                var index = 0;
                foreach (XPathNavigator child in node.SelectChildren(XPathNodeType.All))
                {
                    if (child.Name == prevNode.Name)
                    {
                        index += 1;
                    }

                    if (XPathNavigator.NavigatorComparer.Equals(child, prevNode))
                    {
                        // index 1 is the default, there is no need to put it there
                        parts.Add(index == 1 ? prevNode.Name : $"{prevNode.Name}[{index}]");

                        break;
                    }
                }

                prevNode = node;
            }

            // add empty node so the path will start with a slash
            parts.Add("");
            parts.Reverse();

            return String.Join("/", parts);
        }
    }
}
