using MoreLinq.Extensions;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle11 : BasePuzzle
{
    protected override long SolvePart1(IEnumerable<string> input)
    {
        var totalFlashes = 0;
        
        var grid = input
            .Select(line => line
                .ToArray()
                .Select(c => (short)char.GetNumericValue(c))
                .ToArray())
            .ToArray();

        PrintGrid(grid);
        
        for (var step = 1; step <= 2; step++)
        {
            for (var y = 0; y < grid.Length; y++)
            {
                for (var x = 0; x < grid[y].Length; x++)
                {
                    grid[y][x]++;
                }
            }

            var newFlashes = HandleFlashes(grid);
            totalFlashes += newFlashes;
            
            Console.WriteLine($"Step: {step}");
            PrintGrid(grid);
        }

        return totalFlashes;
    }
    
    protected override long SolvePart2(IEnumerable<string> input)
    {
        return default(long);
    }

    private static void PrintGrid(short[][] grid)
    {
        var savedColor = Console.BackgroundColor;
        
        foreach (var line in grid)
        {
            foreach (var octopus in line)
            {
                if (octopus == 0)
                    Console.BackgroundColor = ConsoleColor.Yellow;
                
                Console.Write(octopus);

                if (octopus == 0)
                    Console.BackgroundColor = savedColor;
            }
            Console.WriteLine();
        }
        
        Console.WriteLine();
    }

    private static int HandleFlashes(short[][] grid)
    {
        var flashes = 0;
        
        for (var y = 0; y < grid.Length; y++)
        {
            for (var x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] > 9)
                {
                    flashes++;
                    grid[y][x] = 0;
                    
                    
                    
                }
            }
        }

        return flashes;
    }
    
    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "5483143223",
            "2745854711",
            "5264556173",
            "6141336146",
            "6357385478",
            "4167524645",
            "2176841721",
            "6882881134",
            "4846848554",
            "5283751526"
        };
    }
}