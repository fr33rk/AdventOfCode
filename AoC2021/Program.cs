using PuzzleSolver.Core;

var puzzleTypes = AppDomain.CurrentDomain.GetAssemblies()
    .SelectMany(a => a.GetTypes())
    .Where(t => t.IsSubclassOf(typeof(BasePuzzle)))
    .ToList();

foreach (var puzzleType in puzzleTypes)
{
    var puzzle = (BasePuzzle)Activator.CreateInstance(puzzleType)!;
    puzzle.Solve();
}