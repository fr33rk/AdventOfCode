using MoreLinq;
using MoreLinq.Extensions;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle09:BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var head = new Coordinate(0, 0);
        var tail = new Coordinate(0, 0);
        var tailPositions = new List<Coordinate>();

        var commands = input
            .Select(line => line.Split(' '))
            .Select(rawCommand => new { Direction = rawCommand[0], Steps = Convert.ToInt32(rawCommand[1]) });

        foreach (var command in commands)
        {
            for(var step = 0; step < command.Steps; step++)
            {
                head = command.Direction switch
                {
                    "R" => new Coordinate(head.X + 1, head.Y),
                    "L" => new Coordinate(head.X - 1, head.Y),
                    "U" => new Coordinate(head.X, head.Y + 1),
                    "D" => new Coordinate(head.X, head.Y - 1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                tail = DetermineTail(head, tail);
                tailPositions.Add(tail);
            }
        }
        
        return tailPositions.Distinct().Count().ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
       
        var tailPositions = new List<Coordinate>();
        var rope = Enumerable.Repeat(new Coordinate(0, 0), 10).ToArray();
        var commands = input
            .Select(line => line.Split(' '))
            .Select(rawCommand => new { Direction = rawCommand[0], Steps = Convert.ToInt32(rawCommand[1]) });

        foreach (var command in commands)
        {
            for (var step = 0; step < command.Steps; step++)
            {
                var head = rope.First();
                head = command.Direction switch
                {
                    "R" => new Coordinate(head.X + 1, head.Y),
                    "L" => new Coordinate(head.X - 1, head.Y),
                    "U" => new Coordinate(head.X, head.Y + 1),
                    "D" => new Coordinate(head.X, head.Y - 1),
                    _ => throw new ArgumentOutOfRangeException()
                };

                rope[0] = head;
                for (var x = 1; x < rope.Length; x++)
                {

                    rope[x] = DetermineTail(rope[x - 1], rope[x]);
                }

                tailPositions.Add(rope.Last());
            }
        }

        return tailPositions.Distinct().Count().ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            // First example:
            // "R 4",
            // "U 4",
            // "L 3",
            // "D 1",
            // "R 4",
            // "D 1",
            // "L 5",
            // "R 2"
            
            // Larger example
            "R 5",
            "U 8",
            "L 8",
            "D 3",
            "R 17",
            "D 10",
            "L 25",
            "U 20"
        };
    }
    
    private static Coordinate DetermineTail(Coordinate head, Coordinate tail)
    {
        // H H H H H 
        // H . . . H
        // H . T . H
        // H . . . H
        // H H H H H
        var deltaX = head.X - tail.X;
        var deltaY = head.Y - tail.Y;

        return deltaY switch
        {
            2 => deltaX switch
            {
                2 => new Coordinate(head.X - 1, head.Y - 1),
                -2 => new Coordinate(head.X + 1, head.Y - 1),
                _ => new Coordinate(head.X, head.Y - 1)
            },
            -2 => deltaX switch
            {
                2 => new Coordinate(head.X - 1, head.Y + 1),
                -2 => new Coordinate(head.X + 1, head.Y + 1),
                _ => new Coordinate(head.X, head.Y + 1)
            },
            _ => deltaX switch
            {
                2 => new Coordinate(head.X - 1, head.Y),
                -2 => new Coordinate(head.X + 1, head.Y),
                _ => tail
            }
        };
    }

    private record Coordinate(int X, int Y);
}