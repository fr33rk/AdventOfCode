using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle07 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        return string.Empty;
        
        // Commands:
        // change directory  : \$ cd (\w+)
        // change to root    : \$ cd /
        // change to previous: \$ cd \.\.
        // list              : \$ ls
        
        // Listings:
        // folder            : dir (\w+)
        // file              : (\d+) (\w+\.?\w*)
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        return string.Empty;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "$ cd /",
            "$ ls",
            "dir a",
            "14848514 b.txt",
            "8504156 c.dat",
            "dir d",
            "$ cd a",
            "$ ls",
            "dir e",
            "29116 f",
            "2557 g",
            "62596 h.lst",
            "$ cd e",
            "$ ls",
            "584 i",
            "$ cd ..",
            "$ cd ..",
            "$ cd d",
            "$ ls",
            "4060174 j",
            "8033020 d.log",
            "5626152 d.ext",
            "7214296 k"
        };
    }

    private abstract class Node
    {
        public string Name { get; }

        public Node(string name)
        {
            Name = name;
        }

        public abstract void ToConsole(int indent);

        public abstract long GetSize();
    }

    private class Folder : Node
    {
        private IList<Node> _ChildNodes = new List<Node>();
        public IEnumerable<Node> ChildNodes => _ChildNodes;

        public Folder(string name) : base(name)
        {
        }

        public void AddNode(Node childNode)
        {
            _ChildNodes.Add(childNode);
        }
    }

    private class File : Node
    {
        public long Size { get; }

        public File(string name, long size) : base(name)
        {
            Size = size;
        }
    }
}