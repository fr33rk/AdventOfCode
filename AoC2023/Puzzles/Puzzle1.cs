using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle1 : BasePuzzle
{
    /// <inheritdoc />
    protected override string SolvePart1(IEnumerable<string> input)
    {
        int result = input
            .Select(line => Regex.Matches(line, @"\d"))
            .Select(digits => Convert.ToInt32(string.Concat(digits.First().Value, digits.Last().Value))).Sum();
        return result.ToString();
    }

    /// <inheritdoc />
    protected override string SolvePart2(IEnumerable<string> input)
    {
        var digitRegexFirst = new Regex( @"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.Compiled);
        var digitRegexLast = new Regex( @"(\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft | RegexOptions.Compiled);

        var digitMap = new Dictionary<string, int>()
        {
            {"one", 1},
            {"two", 2},
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9}
        };

        int ConvertDigit(string digitAsString)
        {
            try
            {
                return Convert.ToInt32(digitAsString);
            }
            catch (FormatException)
            {
                return digitMap[digitAsString];
            }
        }

        int GetTwoDigit(string line)
        {
            var first = digitRegexFirst.Match(line);
            var last = digitRegexLast.Match(line);
            var result = ConvertDigit(first.Value) * 10 + ConvertDigit(last.Value);
            return result;
        }

        int result = input
            .Select(GetTwoDigit)
            .Sum();

        return result.ToString();
    }

    /// <inheritdoc />
    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "1abc2",
            "pqr3stu8vwx",
            "a1b2c3d4e5f",
            "treb7uchetnine"
        ];
    }

    /// <inheritdoc />
    protected override IEnumerable<string> GetTestInputPart2()
    {
        return
        [
            "two1nine",
            "eightwothree",
            "abcone2threexyz",
            "xtwone3four",
            "4nineeightseven2",
            "zoneight234",
            "7pqrstsixteen",
        ];
    }
}