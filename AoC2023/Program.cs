using PuzzleSolver.Core;

var puzzleTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsSubclassOf(typeof(BasePuzzle)))
    .ToList();

var puzzle = (BasePuzzle)Activator.CreateInstance(puzzleTypes.Last())!;

puzzle.Solve();

Console.Write("Press any key to close");
Console.ReadKey();