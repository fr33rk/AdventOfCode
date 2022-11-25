using System.Collections;
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
        if (rawInput == null)
            return;

        var adultFishes = rawInput
            .Split(',')
            .Select(x => Convert.ToInt32(x))
            .Concat(Enumerable.Range(0, 7))
            .GroupBy(x => x)
            .Select(group => new { Age = group.Key, Count = group.Count() - 1 })
            .OrderBy(group => group.Age)
            .Select(group => Convert.ToInt64(group.Count))
            .ToArray();
        
        var embryoFishes = 0L;
        var babyFishes = 0L;
        var childFishes = 0L;
        var zeroIndex = 0;
        var newAdultIndex = 6;
        
        for (var day = 1; day <= 256; day++)
        {
            zeroIndex = ++zeroIndex % 7;
            newAdultIndex = ++newAdultIndex % 7;
            adultFishes[newAdultIndex] += childFishes;
            childFishes = babyFishes;
            babyFishes = embryoFishes;
            embryoFishes = adultFishes[zeroIndex];
        }
        
        Console.WriteLine($"Answer: {adultFishes.Sum() + babyFishes + childFishes}");
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[] { "3,4,3,1,2" };
    }
}