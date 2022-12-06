using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle07 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        return Solve(input, (x, y) => Math.Abs(x - y)).ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        return Solve(input, (x, y) =>
        {
            var distance = Math.Abs(x - y);
            return distance * (distance + 1) / 2;
        }).ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[] { "16,1,2,0,4,2,7,1,2,14" };
    }

    private long Solve(IEnumerable<string> input, Func<int, int, int> fuelCalculation)
    {
        var crabPositions = input
            .FirstOrDefault()?
            .Split(',')
            .Select(x => Convert.ToInt32(x))
            .ToList();

        var minPosition = crabPositions.Min();
        var maxPosition = crabPositions.Max();
        var bestPosition = 0;
        var minFuelUsed = int.MaxValue;

        for (var testPosition = minPosition; testPosition <= maxPosition; testPosition++)
        {
            var totalFuel = crabPositions.Select(x => fuelCalculation(x, testPosition)).Sum();
            if (totalFuel < minFuelUsed)
            {
                bestPosition = testPosition;
                minFuelUsed = totalFuel;
            }
        }

        Console.WriteLine($"Answer: position {bestPosition}, using {minFuelUsed} fuel");
        return minFuelUsed;
    }
}