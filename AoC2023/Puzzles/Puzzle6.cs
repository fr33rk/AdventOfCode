using System.Globalization;
using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle6 : BasePuzzle
{
    record Race(long Time, long Distance);

    private double GetOptions(Race race)
    {
        // abc rule
        var d = Math.Pow(race.Time, 2) - 4 * race.Distance;
        var min = (race.Time - Math.Sqrt(d)) / 2;
        var max = (race.Time + Math.Sqrt(d)) / 2;

        Console.Write($"{min} - {max} = ");

        if (min % 1 == 0)
        {
            min += 1;
        }

        if (max % 1 == 0)
        {
            max -= 1;
        }

        var options = Math.Floor(max) - Math.Ceiling(min) + 1;
        Console.WriteLine(options);
        return options;
    }

    private double GetTotalOptions(IEnumerable<Race> races)
    {
        var answer = 1.0;
        foreach (var race in races)
        {
            answer *= GetOptions(race);
        }

        return answer;
    }

    protected override string SolvePart1(IEnumerable<string> input)
    {
        var lines = input.ToList();
        var valuesRegex = new Regex(@"\d+");
        var times = valuesRegex.Matches(lines.First()).Select(m => Convert.ToInt32(m.Value));
        var distances = valuesRegex.Matches(lines.Last()).Select(m => Convert.ToInt32(m.Value));
        var races = times.Zip(distances).Select(x => new Race(x.First, x.Second)).ToList();
        return GetTotalOptions(races).ToString(CultureInfo.InvariantCulture);
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var lines = input.Select(x => x.Split(':')[1].Replace(" ", "").Trim()).ToList();
        var race = new Race(Convert.ToInt64(lines[0]), Convert.ToInt64(lines[1]));
        return GetOptions(race).ToString(CultureInfo.InvariantCulture);
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "Time:      7  15   30",
            "Distance:  9  40  200"
        ];
    }
}