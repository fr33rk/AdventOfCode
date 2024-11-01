using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle2 : BasePuzzle
{
    /// <inheritdoc />
    protected override string SolvePart1(IEnumerable<string> input)
    {
        const int MaxRed = 12;
        const int MaxGreen = 13;
        const int MaxBlue = 14;

        var regex = new Regex(@"^Game (?<id>\d+)|(?<redCount>\d+) red|(?<greenCount>\d+) green|(?<blueCount>\d+) blue", RegexOptions.Compiled);

        var result = 0;

        foreach (var line in input)
        {
            var matches = regex.Matches(line);
            var id = matches.Select(m => m.Groups["id"]).Where(g => g.Success).Select(g => Convert.ToInt32(g.Value)).First();
            var green = matches.Select(m => m.Groups["greenCount"]).Where(g => g.Success).Select(g => Convert.ToInt32(g.Value)).Sum();
            var red = matches.Select(m => m.Groups["redCount"]).Where(g => g.Success).Select(g => Convert.ToInt32(g.Value)).Sum();
            var blue = matches.Select(m => m.Groups["blueCount"]).Where(g => g.Success).Select(g => Convert.ToInt32(g.Value)).Sum();
            var pass = green <= MaxGreen && red <= MaxRed && blue <= MaxBlue;

            Console.WriteLine($@"{id}: ({red}, {green}, {blue}) - {(pass ? "passed" : "failed")}");

            if (pass)
            {
                result += id;
            }
        }

        return result.ToString();
    }

    /// <inheritdoc />
    protected override string SolvePart2(IEnumerable<string> input)
    {
        return "?";
    }

    /// <inheritdoc />
    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
            "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
            "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
            "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
            "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green"
        ];
    }
}