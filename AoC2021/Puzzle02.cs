using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle02 : BasePuzzle
{
    private enum Command
    {
        Forward,
        Down,
        Up
    }
    
    protected override long SolvePart1(IEnumerable<string> input)
    {
        var commands = input.Select(c =>
            {
                var split = c.Split(' ');
                return new
                {
                    Command = Enum.Parse<Command>(char.ToUpper(split[0][0]) + split[0][1..]),
                    Distance = Convert.ToInt32(split[1])
                };
            })
            .GroupBy(c => c.Command)
            .Select(c => new { Command = c.Key, Total = c.Sum(c => c.Distance) })
            .ToList();

        var depth = commands.Single(c => c.Command == Command.Down).Total -
                    commands.Single(c => c.Command == Command.Up).Total;
        var position = commands.Single(c => c.Command == Command.Forward).Total;

        Console.WriteLine($"Depth: {depth}");
        Console.WriteLine($"Distance: {position}");
        Console.WriteLine($"Product: {depth * position}");

        return depth * position;
    }

    protected override long SolvePart2(IEnumerable<string> input)
    {
        var depth = 0;
        var position = 0;
        var aim = 0;
        
        var commands = input.Select(c =>
        {
            var split = c.Split(' ');
            return new
            {
                Command = Enum.Parse<Command>(char.ToUpper(split[0][0]) + split[0][1..]), 
                Distance = Convert.ToInt32(split[1])
            };
        });
        
        foreach (var command in commands)
        {
            switch (command.Command)
            {
                case Command.Down:
                    aim += command.Distance;
                    break;
                case Command.Forward:
                    position += command.Distance;
                    depth += command.Distance * aim;
                    break;
                case Command.Up:
                    aim -= command.Distance;
                    break;
            }
        }
        
        Console.WriteLine($"Depth: {depth}");
        Console.WriteLine($"Distance: {position}");
        Console.WriteLine($"Product: {depth * position}");

        return depth * position;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new List<string>
        {
            "forward 5",
            "down 5",
            "forward 8",
            "up 3",
            "down 8",
            "forward 2"
        };
    }
}