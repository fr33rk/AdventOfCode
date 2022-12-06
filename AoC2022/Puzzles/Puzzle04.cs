using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle04 : BasePuzzle
{
    protected override long SolvePart1(IEnumerable<string> input)
    {
        return GetElfPairsFromInput(input)
            .Count(Overlaps);
    }

    protected override long SolvePart2(IEnumerable<string> input)
    {
       return GetElfPairsFromInput(input)
            .Count(PartlyOverlaps);
    }
    
    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "2-4,6-8",
            "2-3,4-5",
            "5-7,7-9",
            "2-8,3-7",
            "6-6,4-6",
            "2-6,4-8"
        };
    }

    private static IEnumerable<ElfPair> GetElfPairsFromInput(IEnumerable<string> input)
    {
        return input
            .Select(raw => raw.Split(','))
            .Select(rawAssignments =>
                new ElfPair(
                    FirstElf: ConvertAssignment(rawAssignments[0]),
                    SecondElf: ConvertAssignment(rawAssignments[1])
                ));
    }

    private static Assignment ConvertAssignment(string rawAssignment)
    {
        var values = rawAssignment.Split('-');
        return new Assignment(
            StartSector: Convert.ToInt32(values[0]),
            EndSector: Convert.ToInt32(values[1]));
    }

    private static bool PartlyOverlaps(ElfPair elfPair)
    {
        return (elfPair.FirstElf.StartSector <= elfPair.SecondElf.EndSector
                && elfPair.FirstElf.EndSector >= elfPair.SecondElf.StartSector);
    }

    private static bool Overlaps(ElfPair elfPair)
    {
        return (elfPair.FirstElf.StartSector <= elfPair.SecondElf.StartSector && elfPair.FirstElf.EndSector >= elfPair.SecondElf.EndSector)
            || (elfPair.FirstElf.StartSector >= elfPair.SecondElf.StartSector && elfPair.FirstElf.EndSector <= elfPair.SecondElf.EndSector);
    }
    
    private record ElfPair(Assignment FirstElf, Assignment SecondElf);
    
    private record Assignment(int StartSector, int EndSector);
}