using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle06 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var test = input.First();
        var done = false;
        var answer = 0;
        for (var index = 0; index < test.Length - 3 && !done; index++)
        {
            done = test.Skip(index).Take(4).Distinct().Count() == 4;
            if (done)
                answer = index + 4;
        }

        return answer.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var test = input.First();
        var done = false;
        var answer = 0;
        for (var index = 0; index < test.Length - 13 && !done; index++)
        {
            done = test.Skip(index).Take(14).Distinct().Count() == 14;
            if (done)
                answer = index + 14;
        }

        return answer.ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "mjqjpqmgbljsphdztnvjfqwrcgsmlb",
            "bvwbjplbgvbhsrlpgdmjqwftvncz",
            "nppdvjthqldpwncqszvftbrmjlhg",
            "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg",
            "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw",
        };
    }
}