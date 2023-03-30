using System.Diagnostics.CodeAnalysis;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy;
using CoH2XML2JSON.Strategy.CoH2;

using CoH2XML2JSON_Test.Xml;

namespace CoH2XML2JSON_Test.Strategy.CoH2;

public class CoH2SlotItemReaderTest {

    [NotNull]
    private XmlResource slotItemXmlData;

    [NotNull]
    private CoH2SlotItemReader slotItemReader;

    [SetUp]
    public void Setup() {
        slotItemReader = new CoH2SlotItemReader();
        slotItemXmlData = new XmlResource("SlotItemDummy.xml");
    }

    [Test]
    public void CanParseCoH2SlotItem() {

        // Load
        SlotItemBlueprint sib = slotItemReader.FromXml(slotItemXmlData.Document, string.Empty, "SlotItemDummy.xml", new Helpers());
        Assert.That(sib, Is.Not.Null);
        Assert.That(sib.SlotSize, Is.EqualTo(6));

    }

}
