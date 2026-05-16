using System.IO;
using System.Linq;
using System.Text;
using System;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class CellTests
{
    [Theory]
    [InlineData("O", 1, true, false)]
    [InlineData("X", int.MaxValue, false, false)]
    [InlineData("D", 2, true, false)]
    [InlineData("S1", 1, true, true)]
    public void CellProperties_AreCorrect(string symbol, int expectedCost, bool expectedPassable, bool expectedAstronaut)
    {
        var cell = new Cell(symbol, 0, 0);

        Assert.Equal(expectedCost, cell.MoveCost);
        Assert.Equal(expectedPassable, cell.IsPassable);
        Assert.Equal(expectedAstronaut, cell.IsAstronaut);
        Assert.Equal(symbol, cell.ToString());
    }
}
