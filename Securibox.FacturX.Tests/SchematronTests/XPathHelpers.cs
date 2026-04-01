using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using Securibox.FacturX.Schematron.Helpers;

namespace Securibox.FacturX.Tests.SchematronTests
{
    internal class XPathHelpersTests
    {
        private XmlTextReader SimpleDoc()
        {
            var content = $$"""
            <a>
                <b>
                    <c></c>
                </b>
                <b>
                    <c></c>
                    <c></c>
                    <c></c>
                </b>
            </a>
""";

            return new XmlTextReader(new StringReader(content));
        }

        [Test]
        public void GetPath_NotIndexed()
        {
            var path = "/a/b/c";

            var xmlDoc = SimpleDoc();
            var xPathDoc = new XPathDocument(xmlDoc);

            var navigator = xPathDoc.CreateNavigator();

            var nodes = navigator.Select(path);
            nodes.MoveNext();

            var node = nodes.Current!;

            Assert.That(node.GetPath(), Is.EqualTo("/a/b/c"));

            var result = navigator.Select(node.GetPath());
            result.MoveNext();

            Assert.That(node, Is.EqualTo(result.Current!).Using(XPathNavigator.NavigatorComparer));
        }

        [Test]
        public void GetPath_IndexedFirst()
        {
            var path = "/a[1]/b[1]/c[1]";

            var xmlDoc = SimpleDoc();
            var xPathDoc = new XPathDocument(xmlDoc);

            var navigator = xPathDoc.CreateNavigator();

            var nodes = navigator.Select(path);
            nodes.MoveNext();

            var node = nodes.Current!;

            Assert.That(node.GetPath(), Is.EqualTo("/a/b/c"));

            var result = navigator.Select(node.GetPath());
            result.MoveNext();

            Assert.That(node, Is.EqualTo(result.Current!).Using(XPathNavigator.NavigatorComparer));
        }

        [Test]
        public void GetPath_IndexedNotFirst()
        {
            var path = "/a[1]/b[2]/c[3]";

            var xmlDoc = SimpleDoc();
            var xPathDoc = new XPathDocument(xmlDoc);

            var navigator = xPathDoc.CreateNavigator();

            var nodes = navigator.Select(path);
            nodes.MoveNext();

            var node = nodes.Current!;

            Assert.That(node.GetPath(), Is.EqualTo("/a/b[2]/c[3]"));

            var result = navigator.Select(node.GetPath());
            result.MoveNext();

            Assert.That(node, Is.EqualTo(result.Current!).Using(XPathNavigator.NavigatorComparer));
        }

        [Test]
        public void GetPath_Root()
        {
            var path = "/a";

            var xmlDoc = SimpleDoc();
            var xPathDoc = new XPathDocument(xmlDoc);

            var navigator = xPathDoc.CreateNavigator();

            var nodes = navigator.Select(path);
            nodes.MoveNext();

            var node = nodes.Current!;

            Assert.That(node.GetPath(), Is.EqualTo("/a"));

            var result = navigator.Select(node.GetPath());
            result.MoveNext();

            Assert.That(node, Is.EqualTo(result.Current!).Using(XPathNavigator.NavigatorComparer));
        }
    }
}
