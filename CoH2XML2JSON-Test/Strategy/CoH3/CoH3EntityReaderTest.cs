using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;

using CoH2XML2JSON.Strategy;

using CoH2XML2JSON_Test.Xml;

namespace CoH2XML2JSON_Test.Strategy.CoH3;

public class CoH3EntityReaderTest {

    [NotNull]
    private IBlueprintReader<EntityBlueprint> entityReader;

    private Helpers entityHelpers;

    [SetUp]
    public void SetUp() {
        entityReader = new CoH3EntityReader();
        entityHelpers = new Helpers(new IBlueprintHelperHandler[] { new VariantSelector("default"), new ScarPathHandler() });
    }

    [Test]
    public void CanLoadPanzerIII() {

        // Load panzer 3 XML
        XmlResource panzer3Xml = new XmlResource("coh3/ebps/races/afrika_korps/panzer_iii_ak.xml");

        // Load it as blueprint
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

            Assert.That(panzerIII.IsInventoryItem, Is.False);

        });

    }

    [Test]
    public void CanLoadEntityItem() {


        // Load british Lee-enfield XML
        XmlResource leenfieldXml = new XmlResource("coh3/ebps/races/british_africa/weapons/small_arms/single_fire/rifle/w_lee_enfield_tommy_africa_uk.xml");

        // Load it as blueprint
        EntityBlueprint? enfield = entityReader.FromXml(leenfieldXml.Document, string.Empty, "w_lee_enfield_tommy_africa_uk", entityHelpers);
        Assert.That(enfield, Is.Not.Null);

        // Check all
        Assert.Multiple(() => {

            Assert.That(enfield.IsInventoryItem, Is.True);
            Assert.That(enfield.InventoryRequiredCapacity, Is.EqualTo(0));
            Assert.That(enfield.InventoryDropOnDeathChance, Is.EqualTo(0.0f));

        });

    }

}
