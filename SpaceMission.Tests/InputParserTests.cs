using System;
using System.IO;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class InputParserTests
{
    [Fact]
    public void ReadGridFromConsole_ValidInput_CreatesCorrectGrid()
    {
        string simulated = "2\n3\nS1 O F\nO D S2\n";
        var originalIn = Console.In;
        try
        {
            Console.SetIn(new StringReader(simulated));
            var grid = InputParser.ReadGridFromConsole();

            Assert.Equal(2, grid.Rows);
            Assert.Equal(3, grid.Cols);
            Assert.Equal("S1", grid[0, 0].Symbol);
            Assert.Equal("F", grid[0, 2].Symbol);
            Assert.Equal("S2", grid[1, 2].Symbol);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }
}
