using System.Text.RegularExpressions;
using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle05 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var answer = "";
        Split(input, out var stacks, out var instructions);

        foreach (var instruction in instructions)
            for (var crateCount = 0; crateCount < instruction.Amount; crateCount++)
                stacks[instruction.To - 1].Push(stacks[instruction.From - 1].Pop());

        stacks.ForEach(stack => answer += stack.Peek());
        return answer;
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var answer = "";
        Split(input, out var stacks, out var instructions);
        var craneStack = new Stack<char>();

        foreach (var instruction in instructions)
        {
            for (var crateCount = 0; crateCount < instruction.Amount; crateCount++)
                craneStack.Push(stacks[instruction.From - 1].Pop());

            for (var crateCount = 0; crateCount < instruction.Amount; crateCount++)
                stacks[instruction.To - 1].Push(craneStack.Pop());
        }

        stacks.ForEach(stack => answer += stack.Peek());
        return answer;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "    [D]    ",
            "[N] [C]    ",
            "[Z] [M] [P]",
            " 1   2   3 ",
            "",
            "move 1 from 2 to 1",
            "move 3 from 1 to 3",
            "move 2 from 2 to 1",
            "move 1 from 1 to 2"
        };
    }

    private static void Split(IEnumerable<string> input, out IList<Stack<char>> stacks,
        out IList<Instruction> instructions)
    {
        instructions = new List<Instruction>();
        stacks = new List<Stack<char>>();
        var stackLists = new List<List<char>>();

        foreach (var line in input)
        {
            var crateMatches = Regex.Matches(line, @"(\s{3}|\[\w\])(?=\s|$)", RegexOptions.Compiled);
            if (crateMatches.Any())
                SplitCreates(stackLists, crateMatches);
            else
                SplitInstructions(instructions, line);
        }

        CreateCrateStacks(stacks, stackLists);
    }

    private static void SplitCreates(IList<List<char>> stackLists, MatchCollection crateMatches)
    {
        if (!stackLists.Any())
            for (var i = 0; i < crateMatches.Count; i++)
                stackLists.Add(new List<char>());

        for (var stack = 0; stack < crateMatches.Count; stack++)
        {
            var match = Regex.Match(crateMatches[stack].Value, @"\w");
            if (match.Success) stackLists[stack].Add(match.Value[0]);
        }
    }

    private static void SplitInstructions(ICollection<Instruction> instructions, string line)
    {
        var match = Regex.Match(line, @"move (\d+) from (\d+) to (\d+)");
        if (match.Success)
            instructions.Add(new Instruction(
                Convert.ToInt32(match.Groups[1].Value),
                Convert.ToInt32(match.Groups[2].Value),
                Convert.ToInt32(match.Groups[3].Value)));
    }

    private static void CreateCrateStacks(ICollection<Stack<char>> stacks, List<List<char>> stackLists)
    {
        foreach (var stackList in stackLists)
        {
            stacks.Add(new Stack<char>());
            stackList.Reverse();
            foreach (var crate in stackList) stacks.Last().Push(crate);
        }
    }

    private record Instruction(int Amount, int From, int To);
}