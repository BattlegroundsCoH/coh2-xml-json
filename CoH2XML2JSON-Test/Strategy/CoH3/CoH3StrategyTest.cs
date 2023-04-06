using CoH2XML2JSON;
using CoH2XML2JSON.Blueprints;
using CoH2XML2JSON.Strategy;

namespace CoH2XML2JSON_Test.Strategy.CoH3;

public class CoH3StrategyTest {

    const string OUT_PATH = "~test_output";

    [NotNull] private CoH3Strategy strategy;

    [SetUp]
    public void SetUp() {
        strategy = new CoH3Strategy();
        if (Directory.Exists(OUT_PATH)) {
            Directory.Delete(OUT_PATH, true);
        }
        Directory.CreateDirectory(OUT_PATH);
    }

    [TearDown] 
    public void TearDown() {
        if (Directory.Exists(OUT_PATH)) {
            Directory.Delete(OUT_PATH, true);
        }
    }

    private class CoH3TestListener : IStrategyListener {
        private readonly Dictionary<Type, Action<IList<IBlueprint>>> assertions = new();
        public void Notify<T>(IList<T> blueprints) where T : IBlueprint {
            if (assertions.TryGetValue(typeof(T), out var handler) && handler is not null) {
                handler(blueprints.Cast<IBlueprint>().ToList());
            }
        }
        public CoH3TestListener AssertBlueprints<T>(Action<IList<T>> action) where T : IBlueprint {
            assertions[typeof(T)] = (IList<IBlueprint> x) => action(x.Cast<T>().ToList()); return this;
        }
    }

    [Test]
    public void TestCoH3Loader() {

        // Create goal
        Goal goal = new Goal() {
            OutPath = OUT_PATH,
            InstancePath = "Xml/coh3",
            ModName = "vcoh"
        };

        // Create listener
        Assert.Multiple(() => {
            CoH3TestListener listener = new CoH3TestListener()
                .AssertBlueprints<AbilityBlueprint>(abps => {
                    Assert.That(abps, Has.Count.EqualTo(2), "Expected 2 abilities");
                })
                .AssertBlueprints<EntityBlueprint>(ebps => {
                    Assert.That(ebps, Has.Count.EqualTo(4), "Expected 4 entities");
                })
                .AssertBlueprints<SquadBlueprint>(sbps => {
                    Assert.That(sbps, Has.Count.EqualTo(4), "Expected 4 squads");
                })
                .AssertBlueprints<UpgradeBlueprint>(ubps => {
                    Assert.That(ubps, Has.Count.EqualTo(3), "Expected 3 upgrades");
                })
                .AssertBlueprints<WeaponBlueprint>(wbps => {
                    Assert.That(wbps, Has.Count.EqualTo(3), "Expected 3 weapons");
                });

            // Execute
            strategy.Execute(goal, listener);
        });

    }

}
