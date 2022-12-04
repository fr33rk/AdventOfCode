using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle03 : BasePuzzle
{
    protected override long SolvePart1(IEnumerable<string> input)
    {
        var answer = input.Select(bag => new
            {
                comp1 = bag[..(bag.Length / 2)],
                comp2 = bag.Substring(bag.Length / 2, bag.Length / 2)
            })
            .Select(bag => bag.comp1.Intersect(bag.comp2).SingleOrDefault())
            .Select(ToValue)
            .Sum();

        return answer;
    }
    
    protected override long SolvePart2(IEnumerable<string> input)
    {
        var answer = input
            .Chunk(3)
            .Select(rucksacks => rucksacks[0]
                .Intersect(rucksacks[1]
                    .Intersect(rucksacks[2])
                ).Single()
            )
            .Select(ToValue)
            .Sum();

        return answer;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "vJrwpWtwJgWrhcsFMMfFFhFp",
            "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            "PmmdzqPrVvPwwTWBwg",
            "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            "ttgJtRGJQctTZtZT",
            "CrZsJsPPZsGzwwsLwLmpwMDw"
        };
    }

    private int ToValue(char c)
    {
        return c switch
        {
            >= 'a' and <= 'z' => c - 96,
            >= 'A' and <= 'Z' => c - 65 + 27,
            _ => throw new ArgumentOutOfRangeException(nameof(c), c, null)
        };
    }
}