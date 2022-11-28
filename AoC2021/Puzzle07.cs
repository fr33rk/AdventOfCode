using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle07 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        Solve(input, (x, y) => Math.Abs(x - y));
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        Solve(input, (x, y) =>
        {
            var distance = Math.Abs(x - y);
            return distance * (distance + 1) / 2;
        });
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[] { "16,1,2,0,4,2,7,1,2,14" };
    }

    private void Solve(IEnumerable<string> input, Func<int, int, int> fuelCalculation)
    {
        var crabPositions = input
            .FirstOrDefault()?
            .Split(',')
            .Select(x => Convert.ToInt32(x))
            .ToList();

        if(crabPositions == null)
            return;
        
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
            // Console.WriteLine($"Position: {testPosition}, using {totalFuel} fuel");
        }

        Console.WriteLine($"Answer: position {bestPosition}, using {minFuelUsed} fuel");
    }
}