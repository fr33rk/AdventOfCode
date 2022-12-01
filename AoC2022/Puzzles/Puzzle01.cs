using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle01 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var elves = input.Split(x => x.Length == 0)
            .Select(elf => elf.Select(cal => Convert.ToInt32(cal)).Sum());

        Console.WriteLine($"Answer: {elves.Max()}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var elves = input.Split(x => x.Length == 0)
            .Select(elf => elf.Select(cal => Convert.ToInt32(cal)).Sum());

        var topThree = elves.OrderByDescending(x => x)
            .Take(3);
        
        Console.WriteLine($"Answer: {topThree.Sum()}");
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "1000",
            "2000",
            "3000",
            "",
            "4000",
            "",
            "5000",
            "6000",
            "",
            "7000",
            "8000",
            "9000",
            "",
            "10000"
        };
    }
}