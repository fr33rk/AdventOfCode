using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;

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

    [Benchmark]
    public void BenchmarkSolve()
    {
        var realInput = GetInput().ToList();
        ExecuteSolve("Part 1 - Real input", () => SolvePart1(realInput));
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
        var assembly = Assembly.GetEntryAssembly();

        if (assembly == null)
            return new List<string>();
        
        var resourceName = $"{assembly.GetName().Name}.Inputs.{GetType().Name}.txt";
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) 
            return new List<string>();
        
        using var reader = new StreamReader(stream);
        return ReadLines(reader).ToList();
    }

    private static IEnumerable<string> ReadLines(TextReader reader)
    {
        while (reader.ReadLine() is { } line)
        {
            yield return line;
        }
    }
}