using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy.CoH3;
using CoH2XML2JSON.Strategy.Handlers;
using CoH2XML2JSON.Strategy;
using CoH2XML2JSON_Test.Xml;
using CoH2XML2JSON.Blueprints.DataEntry;

namespace CoH2XML2JSON_Test.Strategy.CoH3;

public class CoH3SquadReaderTest {

    [NotNull] private IBlueprintReader<SquadBlueprint> squadReader;

    [NotNull] private ScarPathHandler scarPathHandler;

    [NotNull] private XmlResource tigerXml;
    [NotNull] private EntityBlueprint tigerBlueprint;

    [NotNull] private XmlResource hmgXml;
    [NotNull] private EntityBlueprint hmgBlueprint;
    [NotNull] private EntityBlueprint crewBlueprint;

    [NotNull] private TestProducer registryProducer;
    [NotNull] private RegistryConsumer<EntityBlueprint> registryConsumer;

    private class TestProducer : RegistryProducer<EntityBlueprint> {
        public EntityBlueprint Create(string filepath, Cost? cost = null) {
            EntityBlueprint ebp = new EntityBlueprint() {
                Name = filepath,
                Cost = cost ?? new Cost(150, 0, 0, 5)
            };
            return base.Handle(ebp, filepath);
        }
    }

    private Helpers helpers;

    [SetUp]
    public void SetUp() {
        squadReader = new CoH3SquadReader();
        scarPathHandler = new ScarPathHandler();

        registryProducer = new TestProducer();

        tigerXml = new XmlResource("coh3/sbps/races/german/tiger_ger.xml");
        tigerBlueprint = registryProducer.Create(scarPathHandler.GetNameFromPath("coh3/ebps/races/german/tiger_ger.xml"));

        hmgXml = new XmlResource("coh3/sbps/races/german/hmg_mg42_ger.xml");
        crewBlueprint = registryProducer.Create(scarPathHandler.GetNameFromPath("coh3/ebps/races/german/infantry/crew_hmg_ger.xml"));
        hmgBlueprint = registryProducer.Create(scarPathHandler.GetNameFromPath("coh3/ebps/races/german/weapons/small_arms/machine_gun/heavy_machine_gun/w_mg42_hmg_ger.xml"));

        registryConsumer = registryProducer.CreateConsumer();

        helpers = new Helpers(new IBlueprintHelperHandler[] { new VariantSelector("default"), registryConsumer, scarPathHandler });

    }

    [Test]
    public void HasEntities() {
        Assert.That(registryConsumer.Registry, Has.Count.EqualTo(3));
        Assert.That(registryConsumer.Registry, Has.Member(tigerBlueprint));
        Assert.That(registryConsumer.Registry, Has.Member(crewBlueprint));
        Assert.That(registryConsumer.Registry, Has.Member(hmgBlueprint));
    }

    [Test]
    public void CanLoadTiger() {

        SquadBlueprint? tiger = squadReader.FromXml(tigerXml.Document, string.Empty, "tiger_ger", helpers);
        Assert.That(tiger, Is.Not.Null);

        // Assert on contents
        Assert.Multiple(() => {

            Assert.That(tiger.PBGID, Is.EqualTo(198543));

            Assert.That(tiger.Display, Is.Not.Null);
            Assert.That(tiger.Display!.LocaleName, Is.EqualTo("11183176"));

            Assert.That(tiger.Veterancy, Is.Not.Null);
            Assert.That(tiger.Veterancy!.Length, Is.EqualTo(3));
            Assert.That(tiger.Veterancy![0].Experience, Is.EqualTo(2200.0f));
            Assert.That(tiger.Veterancy![1].Experience, Is.EqualTo(6600.0f));
            Assert.That(tiger.Veterancy![2].Experience, Is.EqualTo(13200.0f));

            Assert.That(tiger.Upgrades, Is.Not.Null);
            Assert.That(tiger.Upgrades!.Length, Is.EqualTo(1));
            Assert.That(tiger.Upgrades![0], Is.EqualTo("upgrade\\german\\vehicle\\mg42_tiger_ger"));

            Assert.That(tiger.Types, Is.Not.Null);
            Assert.That(tiger.Types.Length, Is.EqualTo(3));
            Assert.That(tiger.Types, Has.Member("vehicle_turret"));
            Assert.That(tiger.Types, Has.Member("heavy_armor"));
            Assert.That(tiger.Types, Has.Member("apex_armor"));

            Assert.That(tiger.Entities, Is.Not.Null);
            Assert.That(tiger.Entities![0], Is.EqualTo(new SquadBlueprint.Loadout("EBP.GERMANS.TIGER_GER", 1)));

        });

    }

    [Test]
    public void CanLoadHmg() {

        SquadBlueprint? hmg = squadReader.FromXml(hmgXml.Document, string.Empty, "hmg_42", helpers);
        Assert.That(hmg, Is.Not.Null);

        Assert.Multiple(() => {

            Assert.That(hmg.PBGID, Is.EqualTo(137121));

            Assert.That(hmg.IsSyncWeapon, Is.True);

        });

    }

}
