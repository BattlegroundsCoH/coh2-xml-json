using CoH2XML2JSON.Strategy.Handlers;

namespace CoH2XML2JSON_Test.Strategy.Handlers;

public class PathHandlerTest {

    [Test]
    public void CanGetUniqueNameFromPathHandler() {
        var uniquePath = new UniquePathHandler();
        Assert.Multiple(() => {
            Assert.That(uniquePath.GetNameFromPath("ebps\\races\\british_africa\\black_prince.xml"), Is.EqualTo("black_prince"));
            Assert.That(uniquePath.GetNameFromPath("ebps\\races\\british_africa\\black_prince"), Is.EqualTo("black_prince"));
            Assert.That(uniquePath.GetNameFromPath("sbps\\races\\british_africa\\black_prince_squad.xml"), Is.EqualTo("black_prince_squad"));
        });
    }

    [Test]
    public void CanGetScarPathFromScarPathHandler() {
        var scarPath = new ScarPathHandler();
        Assert.Multiple(() => {
            Assert.That(scarPath.GetNameFromPath("ebps\\races\\british_africa\\black_prince.xml"), Is.EqualTo("EBP.BRITISH_AFRICA.BLACK_PRINCE"));
            Assert.That(scarPath.GetNameFromPath("ebps\\races\\british_africa\\black_prince"), Is.EqualTo("EBP.BRITISH_AFRICA.BLACK_PRINCE"));
            Assert.That(scarPath.GetNameFromPath("sbps\\races\\british_africa\\black_prince_squad.xml"), Is.EqualTo("SBP.BRITISH_AFRICA.BLACK_PRINCE_SQUAD"));
        });
    }

}
