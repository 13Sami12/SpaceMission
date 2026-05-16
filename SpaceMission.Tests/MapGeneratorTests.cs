using System;
using System.IO;
using System.Linq;
using SpaceMission.Core;
using Xunit;

namespace SpaceMission.Tests;

public class MapGeneratorTests
{
    [Fact]
    public void GenerateRandom_ProducesSolvableGrid()
    {
        string simulated = "2\n2\n0\nn\n1\n";
        var originalIn = Console.In;
        try
        {
            Console.SetIn(new StringReader(simulated));
            var grid = MapGenerator.GenerateRandom();

            Assert.NotNull(grid.FindStation());
            Assert.Single(grid.FindAstronauts());
            Assert.True(grid.FindAstronauts().First().IsAstronaut);
        }
        finally
        {
            Console.SetIn(originalIn);
        }
    }
}
