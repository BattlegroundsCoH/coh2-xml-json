using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;

using CoH2XML2JSON.Strategy;

using CoH2XML2JSON_Test.Xml;

namespace CoH2XML2JSON_Test.Strategy.CoH3;

public class CoH3EntityReaderTest {

    [NotNull]
    private IBlueprintReader<EntityBlueprint> entityReader;

    [NotNull]
    private XmlResource panzer3Xml;

    private Helpers entityHelpers;

    [SetUp]
    public void SetUp() {
        entityReader = new CoH3EntityReader();
        panzer3Xml = new XmlResource("coh3/ebps/races/afrika_korps/panzer_iii_ak.xml");
        entityHelpers = new Helpers(new[] { new VariantSelector("default") });
    }

    [Test]
    public void CanLoadPanzerIII() {

        // Load it
        EntityBlueprint? panzerIII = entityReader.FromXml(panzer3Xml.Document, string.Empty, "panzer_iii_ak", entityHelpers);
        Assert.That(panzerIII, Is.Not.Null);

        // Make assertions
        Assert.Multiple(() => {

            Assert.That(panzerIII.PBGID, Is.EqualTo(198274));

            Assert.That(panzerIII.Cost, Is.Not.Null);
            Assert.That(panzerIII.Cost!.Manpower, Is.EqualTo(340.0f));
            Assert.That(panzerIII.Cost!.Fuel, Is.EqualTo(80.0f));
            Assert.That(panzerIII.Cost!.FieldTime, Is.EqualTo(45.0f));

            Assert.That(panzerIII.Types!.Contains("panzer_iii"), Is.True);
            Assert.That(panzerIII.Types!.Contains("rideable_tank"), Is.True);
            Assert.That(panzerIII.Types!.Contains("vehicle_treaded"), Is.True);
            Assert.That(panzerIII.Types!.Contains("side_armor_tank"), Is.True);
            Assert.That(panzerIII.Types!.Contains("allow_track_damage_when_immobilized"), Is.True);
            Assert.That(panzerIII.Types!.Contains("allow_brew_up"), Is.True);
            Assert.That(panzerIII.Types!.Contains("allow_turret_off"), Is.True);

            Assert.That(panzerIII.Health, Is.EqualTo(600.0f));

        });

    }

}
