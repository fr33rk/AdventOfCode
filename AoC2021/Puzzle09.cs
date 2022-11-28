using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle09 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var map = ToMap(input);

        IdLowPoints(map);

        PrintMap(map);

        var answer = map
            .SelectMany(sample => sample)
            .Where(sample => sample.HasId)
            .Select(sample => sample.Depth + 1)
            .Sum();
        
        Console.WriteLine($"Answer: {answer}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var map = ToMap(input);
        IdLowPoints(map);

        var done = false;

        while (!done)
        {
            done = true;
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x].BasinId != null)
                        continue;

                    if (x - 1 >= 0 && map[y][x - 1].HasId)
                    {
                        map[y][x].BasinId = map[y][x - 1].BasinId;
                    }
                    else if (x + 1 < map[y].Length && map[y][x + 1].HasId)
                    {
                        map[y][x].BasinId = map[y][x + 1].BasinId;
                    }
                    else if (y - 1 >= 0 && map[y - 1][x].HasId)
                    {
                        map[y][x].BasinId = map[y - 1][x].BasinId;
                    }
                    else if (y + 1 < map.Length && map[y + 1][x].HasId)
                    {
                        map[y][x].BasinId = map[y + 1][x].BasinId;
                    }
                    else
                    {
                        done = false;
                    }
                }
            }
        }
        PrintMap(map);

        var basins = map
            .SelectMany(sample => sample)
            .Where(sample => sample.HasId)
            .GroupBy(sample => sample.BasinId)
            .Select(basin => new { Id = basin.Key, Size = basin.Count() });
            
        var answer = basins
            .OrderByDescending(basin => basin.Size)
            .Take(3)
            .Aggregate(1, (acc, field) => acc * field.Size);

        Console.WriteLine($"Answer: {answer}");
    }

    private static Sample[][] ToMap(IEnumerable<string> input)
    {
        return input
            .Select(line => line
                .ToArray()
                .Select(depth => new Sample((int)char.GetNumericValue(depth), depth == '9' ? int.MaxValue : null))
                .ToArray())
            .ToArray();
    }

    private static void IdLowPoints(Sample[][] map)
    {
        var id = 0;
        
        for (var x = 0; x < map.Length; x++)
        {
            for (var y = 0; y < map[x].Length; y++)
            {
                var depthToCheck = map[x][y].Depth;
                // Left
                if (x - 1 >= 0 && map[x-1][y].Depth <= depthToCheck)
                    continue;
                // Right
                if (x + 1 < map.Length && map[x+1][y].Depth <= depthToCheck)
                    continue;
                // Top
                if (y - 1 >= 0 && map[x][y - 1].Depth <= depthToCheck) 
                    continue;
                if (y + 1 < map[x].Length && map[x][y+1].Depth <= depthToCheck)
                    continue;

                map[x][y].BasinId = id++;
            }
        }
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "2199943210",
            "3987894921",
            "9856789892",
            "8767896789",
            "9899965678"
        };
    }

    private static void PrintMap(IEnumerable<Sample[]> map)
    {
        var savedBackgroundColor = Console.BackgroundColor;
        var savedForegroundColor = Console.ForegroundColor;

        foreach (var line in map)
        {
            foreach (var sample in line)
            {
                if (sample.HasId)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.Write(sample.Depth);

                if (sample.HasId)
                {
                    Console.BackgroundColor = savedBackgroundColor;
                    Console.ForegroundColor = savedForegroundColor;
                }
            }

            Console.WriteLine();
        }
    }
    
    private struct Sample
    {
        public Sample(int depth, int? basinId)
        {
            Depth = depth;
            BasinId = basinId;
        }
        
        public bool HasId => BasinId != null && BasinId != int.MaxValue;
        public readonly int Depth;
        public int? BasinId;
    }
}