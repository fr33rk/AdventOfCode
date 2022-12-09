using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle08 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var forrest = input.Select(line => line.Select(c => new Tree((short)char.GetNumericValue(c))).ToArray()).ToArray().ToMatrix();

        for (var direction = 0; direction < 4; direction++)
        {
            for (var treeLineIndex = 0; treeLineIndex < forrest.GetLength(1); treeLineIndex++)
            {
                CheckVisibility(forrest.GetRow(treeLineIndex));
            }

            forrest = forrest.RotateClockWise();
        }

        return forrest.AsEnumerable().Count(tree => tree.Visible).ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var forrest = input.Select(line => line.Select(c => new Tree((short)char.GetNumericValue(c))).ToArray()).ToArray().ToMatrix();

        for (var direction = 0; direction < 4; direction++)
        {
            for (var treeLineIndex = 0; treeLineIndex < forrest.GetLength(1); treeLineIndex++)
            {
                CalculateScores(forrest.GetRow(treeLineIndex).ToList());
            }

            forrest = forrest.RotateClockWise();
        }

        return forrest.AsEnumerable().Max(tree => tree.Score).ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "30373",
            "25512",
            "65332",
            "33549",
            "35390",
        };
    }

    private static void CheckVisibility(IEnumerable<Tree> treeLine)
    {
        using var treeEnumerator = treeLine.GetEnumerator();
        
        if (!treeEnumerator.MoveNext())
            return;
        
        treeEnumerator.Current.MarkVisible();
        var highestTree = treeEnumerator.Current.Height;

        while (treeEnumerator.MoveNext())
        {
            if (treeEnumerator.Current.Height <= highestTree) 
                continue;

            treeEnumerator.Current.MarkVisible();
            highestTree = treeEnumerator.Current.Height;
        }
    }

    private static void CalculateScores(ICollection<Tree> treeLine)
    {
        for (var treeIndex = 0; treeIndex < treeLine.Count; treeIndex++)
        {
            CalculateScore(treeLine.Skip(treeIndex).ToList());
        }
    }

    private static void CalculateScore(IList<Tree> treeLine)
    {
        var treeToTest = treeLine.First();
        var score = 0;
        for (var i = 1; i < treeLine.Count; i++)
        {
            score++;

            if (treeLine[i].Height >= treeToTest.Height)
            {
                break;
            }
        }
        treeToTest.AddScore(score);
    }

    private static void ForrestToConsole(IEnumerable<IEnumerable<Tree>> forrest)
    {
        var savedColor = Console.BackgroundColor;
        foreach (var line in forrest)
        {
            foreach (var tree in line)
            {
                if (tree.Visible)
                    Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(tree.Height);
                if (tree.Visible)
                    Console.BackgroundColor = savedColor;
            }

            Console.WriteLine();
        }
    }
    
    private class Tree
    {
        private bool _isFirstScore = true;
        
        public short Height { get; }
        public bool Visible { get; private set; }
        public int Score { get; private set; }

        public Tree(short height)
        {
            Height = height;
        }

        public void MarkVisible()
        {
            Visible = true;
        }

        public void AddScore(int score)
        {
            if (_isFirstScore)
            {
                _isFirstScore = false;
                Score = score;
            }
            else
                Score *= score;
        }
    }
}