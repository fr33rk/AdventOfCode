using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle6 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var rawInput = input.FirstOrDefault();
        if (rawInput == null)
            return;

        var lanternFishes = rawInput.Split(',').Select(x => Convert.ToInt32(x));

        for (var day = 1; day <= 80; day++)
        {
            lanternFishes = lanternFishes
                .Concat(Enumerable.Repeat(9, lanternFishes.Count(x => x == 0)))
                .Select(x => x == 0 ? 6 : --x);
        }

        Console.WriteLine($"Answer: {lanternFishes.Count()}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var rawInput = input.FirstOrDefault();
        if (rawInput == null || rawInput.Length > 10)
            return;

        var lanternFishes = rawInput.Split(',').Select(x => Convert.ToInt32(x)).ToList();
        
        for (var day = 1; day <= 100; day++)
        {
            Console.Write($"Day {day}: (+{lanternFishes.Count(x => x == 0)})");
            
            lanternFishes.AddRange(Enumerable.Repeat(9, lanternFishes.Count(x => x == 0)));
            
            for (var index = 0; index < lanternFishes.Count; index++)
            {
                if (lanternFishes[index] == 0)
                {
                    lanternFishes[index] = 6;
                }
                else
                {
                    lanternFishes[index]--;
                }
            }
            
            //Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($": {lanternFishes.Count}");
            //lanternFishes.ForEach(x => Console.Write($"{x}, "));
            //Console.WriteLine();
        }
        Console.WriteLine();
        Console.WriteLine($"Answer: {lanternFishes.Count()}");
        
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[] { "3,4,3,1,2" };
    }
}