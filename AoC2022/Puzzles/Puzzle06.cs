using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle06 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        return InternalSolve(input.First(), 4);
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        return InternalSolve(input.First(), 14);
    }


    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
            "bvwbjplbgvbhsrlpgdmjqwftvncz",
            "nppdvjthqldpwncqszvftbrmjlhg",
            "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",
            "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw"
        };
    }

    private static string InternalSolve(string input, int distinctLength)
    {
        var done = false;
        var answer = 0;
        for (var index = 0; index <= input.Length - distinctLength && !done; index++)
        {
            done = input.Skip(index).Take(distinctLength).Distinct().Count() == distinctLength;
            if (done)
                answer = index + distinctLength;
        }

        return answer.ToString();
    }
}