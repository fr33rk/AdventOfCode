using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle14 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        // Get the line definitions (puzzle input)
        var lines = input
            .Select(line => line.Replace(" ", "").Split("->"))
            .Select(line => line
                .Select(point => point.Split(","))
                .Select(point => new Point2D(
                    Convert.ToInt32(point[0]),
                    Convert.ToInt32(point[1]))))
            .ToList();

        var linesFlat = lines.SelectMany(x => x).ToList();
        
        // Find out how large the cave is
        var minX = linesFlat.Min(point => point.X);
        var maxX = linesFlat.Max(point => point.X);
        var maxY = linesFlat.Max(point => point.Y);
        var offsetX = minX - 1;
        
        var width = maxX - minX + 3;
        
        // Create the cave
        var cave = new Material[maxY +2, width];
        
        // Add the void
        DefineLine(cave, new Point2D(0, 0), new Point2D(0, maxY), Material.TheVoid);
        DefineLine(cave, new Point2D(width-1, 0), new Point2D(width-1, maxY), Material.TheVoid);
        DefineLine(cave, new Point2D(0, maxY+1), new Point2D(width-1, maxY+1), Material.TheVoid);
        
        // Add the rock lines
        var lineParts =
            lines.Select(line =>
                {
                    var point2Ds = line.ToList();
                    return point2Ds.Skip(1).Zip(point2Ds, (second, first) => new[] { first, second });
                })
                .SelectMany(x => x)
                .ToList();

        foreach (var linePart in lineParts)
        {
            var start = new Point2D(linePart.First().X - offsetX, linePart.First().Y);
            var end = new Point2D(linePart.Last().X - offsetX, linePart.Last().Y);
            
            DefineLine(cave, start, end, Material.Rock);    
        }

        var unitsOfSand = 0;
        while (ProcessUnitOfSand(cave, new Point2D(500 - offsetX, 0)) == Result.Settled)
        {
            unitsOfSand++;
        }

        ToConsole(cave);
        
        return unitsOfSand.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        // Get the line definitions (puzzle input)
        var lines = input
            .Select(line => line.Replace(" ", "").Split("->"))
            .Select(line => line
                .Select(point => point.Split(","))
                .Select(point => new Point2D(
                    Convert.ToInt32(point[0]),
                    Convert.ToInt32(point[1]))))
            .ToList();
        
        var linesFlat = lines.SelectMany(x => x).ToList();
        
        // Find out how large the cave is
        var maxY = linesFlat.Max(point => point.Y);
        var offsetX = 500 - maxY - 3;

        var width = (maxY + 4) * 2; 
        
        // Create the cave
        var cave = new Material[maxY + 3, width];
        
        // Add the bottom
        DefineLine(cave, new Point2D(0, maxY + 2), new Point2D(width-1, maxY+2), Material.Rock);
        
        // Add the rock lines
        var lineParts =
            lines.Select(line =>
                {
                    var point2Ds = line.ToList();
                    return point2Ds.Skip(1).Zip(point2Ds, (second, first) => new[] { first, second });
                })
                .SelectMany(x => x)
                .ToList();

        foreach (var linePart in lineParts)
        {
            var start = new Point2D(linePart.First().X - offsetX, linePart.First().Y);
            var end = new Point2D(linePart.Last().X - offsetX, linePart.Last().Y);
            
            DefineLine(cave, start, end, Material.Rock);    
        }
        
        var unitsOfSand = 0;
        while (ProcessUnitOfSand(cave, new Point2D(500 - offsetX, 0)) == Result.Settled)
        {
            unitsOfSand++;
            if (unitsOfSand == 48)
                ToConsole(cave);
        }

        
        ToConsole(cave);

        return unitsOfSand.ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "498,4 -> 498,6 -> 496,6",
            "503,4 -> 502,4 -> 502,9 -> 494,9"
        };
    }

    private static void ToConsole(Material[,] cave)
    {
        for (var y = 0; y < cave.GetLength(0); y++)
        {
            for (var x = 0; x < cave.GetLength(1); x++)
            {
                Console.Write(cave[y,x] switch
                {
                    Material.Air => ".",
                    Material.Rock => "#",
                    Material.Sand => "o",
                    Material.TheVoid => "~",
                    _ => throw new ArgumentOutOfRangeException()
                });
            }

            Console.WriteLine();
        }
    }

    private static void DefineLine(Material[,] cave, Point2D start, Point2D end, Material material)
    {
        if (start.X == end.X)
        {
            var startY = Math.Min(start.Y, end.Y);
            var endY = Math.Max(start.Y, end.Y);
            for (var y = startY; y <= endY; y++)
            {
                cave[y, start.X] = material;
            }
            return;
        }
        
        var startX = Math.Min(start.X, end.X);
        var endX = Math.Max(start.X, end.X);
        for (var x = startX; x <= endX; x++)
        {
            cave[start.Y, x] = material;
        } 
    }

    private static Result ProcessUnitOfSand(Material[,] cave, Point2D currentPos)
    {
        if (cave[currentPos.Y, currentPos.X] == Material.Sand)
            return Result.ToTheCeiling;
        
        switch (cave[currentPos.Y + 1, currentPos.X])
        {
            case Material.Air:
                return ProcessUnitOfSand(cave, new Point2D(currentPos.X, currentPos.Y + 1));
            case Material.TheVoid:
                return Result.InTheVoid;
            case Material.Rock:
            case Material.Sand:
                if (cave[currentPos.Y + 1, currentPos.X - 1] is Material.Air or Material.TheVoid)
                    return ProcessUnitOfSand(cave, new Point2D(currentPos.X - 1, currentPos.Y + 1));
                if (cave[currentPos.Y + 1, currentPos.X + 1] is Material.Air or Material.TheVoid)
                    return ProcessUnitOfSand(cave, new Point2D(currentPos.X + 1, currentPos.Y + 1));
                cave[currentPos.Y, currentPos.X] = Material.Sand;
                return Result.Settled;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private record Point2D(int X, int Y);
    
    private enum Material
    {
        Air,
        Rock,
        Sand,
        TheVoid
    }

    private enum Result
    {
        Settled,
        InTheVoid,
        ToTheCeiling
    }
}