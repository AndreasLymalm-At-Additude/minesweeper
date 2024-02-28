using Minesweeper;

namespace MinesweeperTest;

[TestClass]
public class Tests
{
    [TestMethod]
    public void Initialize_BasicWidth_HasCorrectWidth()
    {
        var minefield = new Minefield(1, 5, 1);
        
        Assert.AreEqual(minefield.Width, 5);
    }

    [TestMethod]
    public void Initialize_NegativeWidth_HasPositiveWidth()
    {
        var minefield = new Minefield(1, -1, 1);

        Assert.IsTrue(minefield.Width > 0);
    }

    [TestMethod]
    public void Initialize_BasicHeight_HasCorrectHeight()
    {
        var minefield = new Minefield(5, 1, 1);

        Assert.AreEqual(minefield.Height, 5);
    }

    [TestMethod]
    public void Initialize_NegativeHeight_HasPositiveHeight()
    {
        var minefield = new Minefield(-1, 1, 1);

        Assert.IsTrue(minefield.Height > 0);
    }

    [TestMethod]
    public void Initialize_MineCountIsLessThanArea()
    {
        var minefield = new Minefield(5, 10, 100);

        Assert.IsTrue(minefield.MineCount < 50);
    }

    [TestMethod]
    public void Visit_ClearsCoordinate()
    {
        var minefield = new Minefield(5, 5, 1);
        int x = 3, y = 4;

        minefield.Visit(x, y);

        Assert.IsFalse(minefield.IsCovered(x, y));
    }

    [TestMethod]
    public void Reset_CoversCoordinate()
    {
        var minefield = new Minefield(5, 5, 1);
        int x = 3, y = 4;

        minefield.Visit(x, y);
        minefield.Reset();

        Assert.IsTrue(minefield.IsCovered(x, y));
    }
}
