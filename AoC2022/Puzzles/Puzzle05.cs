using System.Text.RegularExpressions;
using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle05 : BasePuzzle
{
    protected override long SolvePart1(IEnumerable<string> input)
    {
        Split(input, out var stacks, out var instructions);

        foreach (var instruction in instructions)
        {
            for (var crateCount = 0; crateCount < instruction.Amount; crateCount++)
            {
                stacks[instruction.To-1].Push(stacks[instruction.From-1].Pop());
            }
        }
        
        stacks.ForEach(stack => Console.Write(stack.Peek()));
        Console.WriteLine();
        return default(long);
    }

    protected override long SolvePart2(IEnumerable<string> input)
    {
        return default(long);
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
            "move 1 from 1 to 2",
        };
    }

    private void Split(IEnumerable<string> input, out IList<Stack<char>> stacks,
        out IList<Instruction> instructions)
    {
        instructions = new List<Instruction>();
        stacks = new List<Stack<char>>();
        var stackLists = new List<List<char>>();
        
        foreach (var line in input)
        {
            var creatMatches = Regex.Matches(line, @"(\s{3}|\[\w\])(?=\s|$)", RegexOptions.Compiled);
            if (creatMatches.Any())
            {
                if (!stackLists.Any())
                {
                    for (var i = 0; i < creatMatches.Count; i++)
                        stackLists.Add(new List<char>());
                }

                for (var stack = 0; stack < creatMatches.Count; stack++)
                {
                    var match = Regex.Match(creatMatches[stack].Value, @"\w");
                    if (match.Success)
                    {
                        stackLists[stack].Add(match.Value[0]);
                    }
                }
            }
            else
            {
                var instructionMatches = Regex.Matches(line, @"\d+");
                if (instructionMatches.Count == 3)
                {
                    instructions.Add(new Instruction(
                        Convert.ToInt32(instructionMatches[0].Value),
                        Convert.ToInt32(instructionMatches[1].Value),
                        Convert.ToInt32(instructionMatches[2].Value)));
                }
            }
        }

        foreach (var stackList in stackLists)
        {
            stacks.Add(new Stack<char>());
            stackList.Reverse();
            foreach (var crate in stackList)
            {
                stacks.Last().Push(crate);
            }
        }
    }
    
    private record Instruction(int Amount, int From, int To);
}