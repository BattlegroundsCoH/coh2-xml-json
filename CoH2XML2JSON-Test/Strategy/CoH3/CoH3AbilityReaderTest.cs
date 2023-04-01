using System.Diagnostics.CodeAnalysis;

using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;

using CoH2XML2JSON_Test.Xml;

namespace CoH2XML2JSON_Test.Strategy.CoH3;

public class CoH3AbilityReaderTest {

    [NotNull]
    private IBlueprintReader<AbilityBlueprint> abilityReader;

    [NotNull]
    private XmlResource gammonBombXml;

    [NotNull]
    private XmlResource grenadeParentXml;

    private Helpers abilityHelpers;

    [SetUp]
    public void SetUp() {
        abilityReader = new CoH3AbilityReader();
        gammonBombXml = new XmlResource("coh3/abilities/races/british_africa/gammon_bomb_guards_africa_uk.xml");
        grenadeParentXml = new XmlResource("coh3/abilities/races/common/infantry/grenade_parent.xml");
        abilityHelpers = new Helpers(new[] { new VariantSelector("default") });
    }

    [Test]
    public void CanLoadGammonBomb() {

        // Load it
        AbilityBlueprint? gammonBomb = abilityReader.FromXml(gammonBombXml.Document, string.Empty, "gammon_bomb_guards_africa_uk", abilityHelpers);
        Assert.That(gammonBomb, Is.Not.Null);

        // Make assertions
        Assert.Multiple(() => {
            
            Assert.That(gammonBomb.PBGID, Is.EqualTo(2071762));
            
            Assert.That(gammonBomb.Cost, Is.Not.Null);
            Assert.That(gammonBomb.Cost!.Munition, Is.EqualTo(35.0f));

            Assert.That(gammonBomb.Display, Is.Not.Null);
            Assert.That(gammonBomb.Display!.LocaleName, Is.EqualTo("11186431"));
            Assert.That(gammonBomb.Display!.LocaleDescriptionLong, Is.EqualTo("11249131"));
            Assert.That(gammonBomb.Display!.LocaleDescriptionShort, Is.EqualTo("11249504"));
            Assert.That(gammonBomb.Display!.Icon, Is.EqualTo("legacy\\abilities\\ability_british_gammon_bomb"));
            Assert.That(gammonBomb.Display!.Position, Is.EqualTo(0)); // Is inherited from grenade parent

        });

    }

    [Test]
    public void CanLoadGrenadeParent() {

        AbilityBlueprint? parent = abilityReader.FromXml(grenadeParentXml.Document, string.Empty, "grenade_parent", abilityHelpers);
        Assert.That(parent, Is.Not.Null);

        // Make load assertions
        Assert.Multiple(() => {

            Assert.That(parent.Display!.Position, Is.EqualTo(7));
            Assert.That(parent.Activation, Is.EqualTo("targeted"));

        });

    }

    [Test]
    public void CanHandleBlueprintRelation() {

        AbilityBlueprint? gammonBomb = abilityReader.FromXml(gammonBombXml.Document, string.Empty, "gammon_bomb_guards_africa_uk", abilityHelpers);
        Assert.That(gammonBomb, Is.Not.Null);

        AbilityBlueprint? parent = abilityReader.FromXml(grenadeParentXml.Document, string.Empty, "grenade_parent", abilityHelpers);
        Assert.That(parent, Is.Not.Null);

        // Assign
        gammonBomb!.Extends = parent;
        gammonBomb!.Cost!.Extends = parent.Cost!;
        gammonBomb!.Display!.Extends = parent.Display!;

        // Check values
        Assert.Multiple(() => {
            
            Assert.That(gammonBomb.GetParent(), Is.EqualTo(parent));

            Assert.That(gammonBomb.PBGID, Is.EqualTo(2071762));

            Assert.That(gammonBomb.Cost, Is.Not.Null);
            Assert.That(gammonBomb.Cost!.Munition, Is.EqualTo(35.0f));

            Assert.That(gammonBomb.Display, Is.Not.Null);
            Assert.That(gammonBomb.Display!.LocaleName, Is.EqualTo("11186431"));
            Assert.That(gammonBomb.Display!.LocaleDescriptionLong, Is.EqualTo("11249131"));
            Assert.That(gammonBomb.Display!.LocaleDescriptionShort, Is.EqualTo("11249504"));
            Assert.That(gammonBomb.Display!.Icon, Is.EqualTo("legacy\\abilities\\ability_british_gammon_bomb"));
            Assert.That(gammonBomb.Display!.Position, Is.EqualTo(7));

            Assert.That(gammonBomb.Activation, Is.EqualTo("targeted"));

        });

    }

}
