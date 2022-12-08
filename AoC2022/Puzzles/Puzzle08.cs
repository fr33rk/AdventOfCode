using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle08 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var forrest = input.Select(line => line.Select(c => new Tree((short)char.GetNumericValue(c))).ToList()).ToList();
        
        
        foreach (var treeLine in forrest)
        {
            // West
            CheckVisibility(treeLine);
            
            // East
            CheckVisibility(treeLine.AsEnumerable().Reverse().ToList());
        }

        for (var treeIndex = 0; treeIndex < forrest[0].Count; treeIndex++)
        {
            var verticalLine = forrest.Select(treeline => treeline[treeIndex]);
            
            // North
            CheckVisibility(verticalLine.ToList());
            // South
            CheckVisibility(verticalLine.Reverse().ToList());
        }
        
        ForrestToConsole(forrest);

        return forrest.Sum(line => line.Count(tree => tree.Visible)).ToString();
    }

    private static void CheckVisibility(List<Tree> treeLine)
    {
        treeLine[0].MarkVisible();
        for (var treeIndex = 0; treeIndex < treeLine.Count - 1; treeIndex++)
        {
            var treeToTest = treeLine[treeIndex + 1];
            var treeToTestWith = treeLine[treeIndex];
            
            if (treeToTest.Height > treeToTestWith.Height)
                treeToTest.MarkVisible();
            else if (treeToTest.Height < treeToTestWith.Height)
                break;
        }
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        return string.Empty;
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
        public short Height { get; }
        public bool Visible { get; private set; }

        public Tree(short height)
        {
            Height = height;
        }

        public void MarkVisible()
        {
            Visible = true;
        }
    }
    
}