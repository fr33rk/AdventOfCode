using System.Diagnostics;

namespace PuzzleSolver.Core;

public abstract class BasePuzzle
{
    private readonly Stopwatch _stopwatch = Stopwatch.StartNew();
    
    public void Solve()
    {
        var realInput = GetInput().ToList();
        var testInput = GetTestInput().ToList();

        ExecuteSolve("Part 1 - Test input", () => SolvePart1(testInput));
        ExecuteSolve("Part 1 - Real input", () => SolvePart1(realInput));
        ExecuteSolve("Part 2 - Test input", () => SolvePart2(testInput));
        ExecuteSolve("Part 2 - Real input", () => SolvePart2(realInput));
    }

    private void ExecuteSolve(string description, Action solveAction)
    {
        _stopwatch.Restart();
        Console.WriteLine($"{this.GetType().Name} - {description}");
        solveAction.Invoke();
        Console.WriteLine($"(Solved in: {_stopwatch.ElapsedMilliseconds} ms)");
        Console.WriteLine();
    }

    protected abstract void SolvePart1(IEnumerable<string> input);

    protected abstract void SolvePart2(IEnumerable<string> input);

    protected abstract IEnumerable<string> GetTestInput();

    private IEnumerable<string> GetInput()
    {
        var fileName = @$".\Inputs\{GetType().Name}.txt";
        try
        {
            return File.ReadLines(fileName);
        }
        catch (Exception e) when (e is DirectoryNotFoundException
                                      or FileNotFoundException
                                      or IOException)
        {
            Console.WriteLine($"Unable to open {fileName}");
            return new List<string>();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}