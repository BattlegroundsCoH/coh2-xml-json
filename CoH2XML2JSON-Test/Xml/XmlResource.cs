using System.Xml;

namespace CoH2XML2JSON_Test.Xml;

public class XmlResource {

    private XmlDocument xmlDocument;

    public XmlDocument Document => xmlDocument;

    public XmlResource(string resourcename) {
        xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(File.ReadAllText("Xml/" + resourcename));
    }

}
