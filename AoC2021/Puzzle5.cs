using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle5 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var lineRegex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);
        var oceanFloor = new OceanFloor();
        
        var lines = input
            .Select(i =>
            {
                var match = lineRegex.Match(i);
                return new
                {
                    Start = new Point(Convert.ToInt32(match.Groups[1].Value), Convert.ToInt32(match.Groups[2].Value)),
                    End = new Point(Convert.ToInt32(match.Groups[3].Value), Convert.ToInt32(match.Groups[4].Value)),
                };
            })
            .Where(line => line.Start.X == line.End.X || line.Start.Y == line.End.Y);
        
        oceanFloor.ToConsole();


    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        
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

    private record Point(int X, int Y);

    private class OceanFloor
    {
        private short[,] _diagram;

        public OceanFloor()
        {
            _diagram = new short[9,9];
        }

        public void ToConsole()
        {
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

        public void AddLine(Point start, Point end)
        {
            _diagram[start.X, start.Y]++;
            
            
        }
    }
}