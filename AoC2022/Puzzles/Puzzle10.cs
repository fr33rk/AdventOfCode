using System.ComponentModel;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle10 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var valuesOfX = RunProgram(input);

        var answer = valuesOfX.Skip(18)
            .Where((x, i) => i % 40 == 0)
            .Select((x, i) => x * (20 + i * 40))
            .Sum();
        
        return answer.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var valuesOfX = RunProgram(input);
        var pixel = 1;
        var crt = new List<char>();
        Console.Write("#");
        
        foreach (var regX in valuesOfX)
        {
            var pixelInLine = pixel % 40;

            if (pixelInLine == 0)
                Console.WriteLine();
            
            if (pixelInLine >= regX - 1 && pixelInLine <= regX + 1)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(" ");
            }
            
            pixel++;
        }
        Console.WriteLine();
        
        return string.Empty;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {   
            "addx 15",
            "addx -11",
            "addx 6",
            "addx -3",
            "addx 5",
            "addx -1",
            "addx -8",
            "addx 13",
            "addx 4",
            "noop",
            "addx -1",
            "addx 5",
            "addx -1",
            "addx 5",
            "addx -1",
            "addx 5",
            "addx -1",
            "addx 5",
            "addx -1",
            "addx -35",
            "addx 1",
            "addx 24",
            "addx -19",
            "addx 1",
            "addx 16",
            "addx -11",
            "noop",
            "noop",
            "addx 21",
            "addx -15",
            "noop",
            "noop",
            "addx -3",
            "addx 9",
            "addx 1",
            "addx -3",
            "addx 8",
            "addx 1",
            "addx 5",
            "noop",
            "noop",
            "noop",
            "noop",
            "noop",
            "addx -36",
            "noop",
            "addx 1",
            "addx 7",
            "noop",
            "noop",
            "noop",
            "addx 2",
            "addx 6",
            "noop",
            "noop",
            "noop",
            "noop",
            "noop",
            "addx 1",
            "noop",
            "noop",
            "addx 7",
            "addx 1",
            "noop",
            "addx -13",
            "addx 13",
            "addx 7",
            "noop",
            "addx 1",
            "addx -33",
            "noop",
            "noop",
            "noop",
            "addx 2",
            "noop",
            "noop",
            "noop",
            "addx 8",
            "noop",
            "addx -1",
            "addx 2",
            "addx 1",
            "noop",
            "addx 17",
            "addx -9",
            "addx 1",
            "addx 1",
            "addx -3",
            "addx 11",
            "noop",
            "noop",
            "addx 1",
            "noop",
            "addx 1",
            "noop",
            "noop",
            "addx -13",
            "addx -19",
            "addx 1",
            "addx 3",
            "addx 26",
            "addx -30",
            "addx 12",
            "addx -1",
            "addx 3",
            "addx 1",
            "noop",
            "noop",
            "noop",
            "addx -9",
            "addx 18",
            "addx 1",
            "addx 2",
            "noop",
            "noop",
            "addx 9",
            "noop",
            "noop",
            "noop",
            "addx -1",
            "addx 2",
            "addx -37",
            "addx 1",
            "addx 3",
            "noop",
            "addx 15",
            "addx -21",
            "addx 22",
            "addx -6",
            "addx 1",
            "noop",
            "addx 2",
            "addx 1",
            "noop",
            "addx -10",
            "noop",
            "noop",
            "addx 20",
            "addx 1",
            "addx 2",
            "addx 2",
            "addx -6",
            "addx -11",
            "noop",
            "noop",
            "noop"
        };  
    }       
    
    private static IEnumerable<long> RunProgram(IEnumerable<string> input)
    {
        var regX = 1L;
        var valuesOfX = new List<long>();

        var instructions = input
            .Select(line => line.Split(' '))
            .Select(rawInstruction => new
            {
                Instruction = rawInstruction[0],
                Value = rawInstruction.Length == 2
                    ? Convert.ToInt32(rawInstruction[1])
                    : 0
            });

        foreach (var instruction in instructions)
        {
            switch (instruction.Instruction)
            {
                case "noop":
                    valuesOfX.Add(regX);
                    break;
                case "addx":
                    valuesOfX.Add(regX);
                    regX += instruction.Value;
                    valuesOfX.Add(regX);
                    break;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }

        return valuesOfX;
    }
}           