using System.Text.RegularExpressions;
using MoreLinq.Extensions;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle05 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var lines = GetLines(input)
            .Where(line => line.Start.X == line.End.X || line.Start.Y == line.End.Y)
            .ToList();

        var oceanFloor = CreateFittingOceanFloor(lines);
        
        lines.ForEach(line => oceanFloor.AddLine(line));
        oceanFloor.ToConsole();

        Console.WriteLine($"Answer: {oceanFloor.CountIntersections()}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var lines = GetLines(input)
            .ToList();

        var oceanFloor = CreateFittingOceanFloor(lines);
        
        lines.ForEach(line => oceanFloor.AddLine(line));
        oceanFloor.ToConsole();

        Console.WriteLine($"Answer: {oceanFloor.CountIntersections()}");
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new List<string>
        {
            "0,9 -> 5,9",
            "8,0 -> 0,8",
            "9,4 -> 3,4",
            "2,2 -> 2,1",
            "7,0 -> 7,4",
            "6,4 -> 2,0",
            "0,9 -> 2,9",
            "3,4 -> 1,4",
            "0,0 -> 8,8",
            "5,5 -> 8,2"
        };
    }

    private static IEnumerable<Line> GetLines(IEnumerable<string> input)
    {
        var lineRegex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);

        return input
            .Select(i =>
            {
                var match = lineRegex.Match(i);
                return new Line(
                    Start: new Point(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value)),
                    End: new Point(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value))
                );
            });
    }

    private static OceanFloor CreateFittingOceanFloor(IEnumerable<Line> lines)
    {
        var maxSize = lines
            .SelectMany(line => new List<int> { line.Start.X, line.Start.Y, line.End.X, line.End.Y })
            .Max();
            
        return new OceanFloor(maxSize + 1);
    }

    private record Point(int X, int Y);

    private record Line(Point Start, Point End);
    
    private class OceanFloor
    {
        private readonly short[,] _diagram;

        public OceanFloor(int size)
        {
            _diagram = new short[size,size];
        }

        public void ToConsole()
        {
            if (_diagram.GetLength(0) > 10)
                return;
            
            for (var x = 0; x < _diagram.GetLength(0); x++)
            {
                for (var y = 0; y < _diagram.GetLength(1); y++)
                {
                    var value = _diagram[x,y];
                    Console.Write(value == 0 ? "." : value.ToString());
                }
                Console.WriteLine();
            }
        }

        public void AddLine(Line line)
        {
            if (line.Start.Y == line.End.Y) 
            {
                // Vertical line
                for (var i = Math.Min(line.Start.X, line.End.X); i <= Math.Max(line.Start.X, line.End.X); i++)
                {
                    _diagram[line.Start.Y, i]++;
                }
            }
            else if (line.Start.X == line.End.X)
            {
                // Horizontal line
                for (var i = Math.Min(line.Start.Y, line.End.Y); i <= Math.Max(line.Start.Y, line.End.Y); i++)
                {
                    _diagram[i, line.Start.X]++;
                }
            }
            else
            {
                var rc = (line.Start.X - line.End.X) / (line.Start.Y - line.End.Y);
                var length = Math.Abs(line.Start.X - line.End.X);
                var origen = line.Start.X < line.End.X ? line.Start : line.End; 
                
                for (var i = 0; i <= length; i++)
                {
                    var x = origen.X + i;
                    var y = origen.Y + i * rc;
                    _diagram[y, x]++;
                }
            }
        }

        public int CountIntersections()
        {
            return _diagram.Cast<short>().Count(x => x > 1);
        }
    }
}