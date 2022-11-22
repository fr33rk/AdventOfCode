using BenchmarkDotNet.Running;
using PuzzleSolver.Core;

var puzzleTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsSubclassOf(typeof(BasePuzzle)))
    .ToList();


var puzzle = (BasePuzzle)Activator.CreateInstance(puzzleTypes.Last())!;

#if BENCHMARK
    BenchmarkRunner.Run(puzzle.GetType());
#else
    // Running last puzzle
    puzzle.Solve();

    // Running all Puzzles
    // foreach (var puzzleType in puzzleTypes)
    // {
    //     var puzzle = (BasePuzzle)Activator.CreateInstance(puzzleType)!;
    //     puzzle.Solve();
    // }
#endif
