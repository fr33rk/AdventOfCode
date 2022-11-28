using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle08 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var wires = input.Select(x => x.Replace("| ", "").Split(" "));

        Console.WriteLine($"Answer: {wires.SelectMany(x => x.Skip(x.Length - 4)).Count(digit => digit.Length is 2 or 3 or 4 or 7)}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var wires = input.Select(x => x.Replace("| ", "").Split(" "));

        var answer = wires.Select(Decode).Sum();
        
        Console.WriteLine($"Answer: {answer}");
    }

    private static int Decode(string[] hints)
    {
        var foundDigits = new Dictionary<short, char[]?>();
        
        List<char[]?> sorted = hints.Select(x =>
        {
            var asChars = x.ToArray();
            Array.Sort(asChars);
            return asChars;
        }).ToList()!;

        if (TryFindDigit(2, sorted, out var wiresForDigit))
            foundDigits[1] = wiresForDigit;
        if (TryFindDigit(3, sorted, out wiresForDigit))
            foundDigits[7] = wiresForDigit;
        if (TryFindDigit(4, sorted, out wiresForDigit))
            foundDigits[4] = wiresForDigit;
        if (TryFindDigit(7, sorted, out wiresForDigit))
            foundDigits[8] = wiresForDigit;

        var sixes = sorted.Where(w => w?.Length == 6).ToList();

        foundDigits[6] = sixes.First(w => !Fits(w, foundDigits[1])
                                          && !Fits(w, foundDigits[4])
                                          && !Fits(w, foundDigits[7]));

        foundDigits[9] = sixes.FirstOrDefault(w => Fits(w, foundDigits[1])
                                                   && Fits(w, foundDigits[4])
                                                   && Fits(w, foundDigits[7]));
        
        foundDigits[0] = sixes.FirstOrDefault(w => Fits(w, foundDigits[1])
                                                   && !Fits(w, foundDigits[4])
                                                   && Fits(w, foundDigits[7]));
        
        var fives = sorted.Where(w => w?.Length == 5).ToList();

        foundDigits[3] = fives.FirstOrDefault(w => Fits(w, foundDigits[1]));

        foundDigits[2] = fives.FirstOrDefault(w => !Fits(w, foundDigits[1]) 
                                                   && !Fits(foundDigits[6], w));
        
        foundDigits[5] = fives.FirstOrDefault(w => !Fits(w, foundDigits[1]) 
                                          && Fits(foundDigits[6], w));

        var lastFour = sorted.Skip(hints.Length - 4).ToList();

        var retValue = 0;
        for (var index = 0; index < 4; index++)
        {
            retValue *= 10;
            retValue += GetDigit(foundDigits, lastFour[index]);
        }

        return retValue;
    }

    private static bool Fits(char[]? input, char[]? digitToFit)
    {
        if (digitToFit == null || input == null)
            return false;
        return input.Intersect(digitToFit).Count() == digitToFit.Length;
    }

    private static short GetDigit(Dictionary<short, char[]?> digitMap, char[]? digitToFit)
    {
        if (digitToFit == null)
            throw new ArgumentNullException($"digitToFit is not allowed to be null");
        
        return digitMap.First(map => 
            map.Value != null 
            && map.Value.Length == digitToFit.Length 
            && map.Value.Intersect(digitToFit).Count() == digitToFit.Length).Key;
    }
    
    private static bool TryFindDigit(int length, IEnumerable<char[]?> wires, out char[]? wiresForDigit)
    {
        var result = wires.FirstOrDefault(x => x?.Length == length);
        if (result != null)
        {
            wiresForDigit = result;
            return true;
        }

        wiresForDigit = Array.Empty<char>();
        return false;
    }
    
    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "be cfbegad cbdgef fgaecd cgeb fdcge agebfd fecdb fabcd edb | fdgacbe cefdb cefbgd gcbe",
            "edbfga begcd cbg gc gcadebf fbgde acbgfd abcde gfcbed gfec | fcgedb cgb dgebacf gc",
            "fgaebd cg bdaec gdafb agbcfd gdcbef bgcad gfac gcb cdgabef | cg cg fdcagb cbg",
            "fbegcd cbd adcefb dageb afcb bc aefdc ecdab fgdeca fcdbega | efabcd cedba gadfec cb",
            "aecbfdg fbg gf bafeg dbefa fcge gcbea fcaegb dgceab fcbdga | gecf egdcabf bgf bfgea",
            "fgeab ca afcebg bdacfeg cfaedg gcfdb baec bfadeg bafgc acf | gebdcfa ecba ca fadegcb",
            "dbcfg fgd bdegcaf fgec aegbdf ecdfab fbedc dacgb gdcebf gf | cefg dcbef fcge gbcadfe",
            "bdfegc cbegaf gecbf dfcage bdacg ed bedf ced adcbefg gebcd | ed bcgafe cdgba cbgef",
            "egadfb cdbfeg cegd fecab cgb gbdefca cg fgcdab egfdb bfceg | gbdfcae bgc cg cgb",
            "gcafb gcf dcaebfg ecagb gf abcdeg gaef cafbge fdbac fegbdc | fgae cfgab fg bagce",
        };
    }
}