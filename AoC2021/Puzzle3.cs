using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle3 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        input = input.ToList();
        var valueLength = input.First().Length;
        var splitInput = input.Select(c => c.ToCharArray()).ToList();
        
        var gamma = 0;
        var epsilon = 0;
        
        for (var index = 0; index < valueLength; index++)
        {
            gamma <<= 1;
            epsilon <<= 1;
            if (GetMostCommon(splitInput, index, '1') == '1')
                gamma |= 1;
            else
                epsilon |= 1;
        }
        
        Console.WriteLine(gamma);
        Console.WriteLine(epsilon);
        Console.WriteLine(gamma * epsilon);
    }

    
    
    protected override void SolvePart2(IEnumerable<string> input)
    {
        input = input.ToList();
        var oxygenInput = input.Select(c => c.ToCharArray()).ToList();
        var carbonOxideInput = input.Select(c => c.ToCharArray()).ToList();
        var valueLength = input.First().Length;

        // Oxygen
        for (var index = 0; index < valueLength && oxygenInput.Count > 1; index++)
        {
            var mostCommon = GetMostCommon(oxygenInput, index, '1' );
            oxygenInput = GetMatching(oxygenInput, mostCommon, index).ToList();
        }

        var oxygen = ToInt(oxygenInput.First());

        // Carbonoxide
        for (var index = 0; index < valueLength && carbonOxideInput.Count > 1; index++)
        {
            var mostCommon = GetLeastCommon(carbonOxideInput, index, '0' );
            carbonOxideInput = GetMatching(carbonOxideInput, mostCommon, index).ToList();
        }

        var carbonOxide = ToInt(carbonOxideInput.First());

        Console.WriteLine($"Oxygen: {oxygen}");
        Console.WriteLine($"Carbon Oxide: {carbonOxide}");
        Console.WriteLine($"Life support: {oxygen * carbonOxide}");
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new List<string>
        {
            "00100",
            "11110",
            "10110",
            "10111",
            "10101",
            "01111",
            "00111",
            "11100",
            "10000",
            "11001",
            "00010",
            "01010"
        };
    }
    
    private static char GetMostCommon(IEnumerable<char[]> input, int index, char preferred)
    {
        var sortedList = input
            .Select(c => c[index])
            .GroupBy(c => c)
            .Select(grp => new { Digit = grp.Key, Count = grp.Count() })
            .OrderByDescending(grp => grp.Count)
            .ToList();

        return sortedList.First().Count == sortedList.Last().Count 
            ? preferred 
            : sortedList.Select(grp => grp.Digit).First();
    }
    
    private static char GetLeastCommon(IEnumerable<char[]> input, int index, char preferred)
    {
        var sortedList = input
            .Select(c => c[index])
            .GroupBy(c => c)
            .Select(grp => new { Digit = grp.Key, Count = grp.Count() })
            .OrderBy(grp => grp.Count)
            .ToList();

        return sortedList.First().Count == sortedList.Last().Count 
            ? preferred 
            : sortedList.Select(grp => grp.Digit).First();
    }
    
    private static IEnumerable<char[]> GetMatching(IEnumerable<char[]> input, char match, int index)
    {
        return input.Where(s => s[index] == match);
    }
        
    private static int ToInt(IEnumerable<char> binaryCode)
    {
        var retValue = 0;
        foreach (var digit in binaryCode)
        {
            retValue <<= 1;
            if (digit == '1')
                retValue |= 1;
        }

        return retValue;
    }
}