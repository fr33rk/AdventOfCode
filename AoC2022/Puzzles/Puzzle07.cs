using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle07 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var toRootRegex = new Regex(@"\$ cd /", RegexOptions.Compiled);
        var changeToFolderRegex = new Regex(@"\$ cd (\w+)", RegexOptions.Compiled);
        var changeToPreviousRegex = new Regex(@"\$ cd \.\.", RegexOptions.Compiled);
        var isFolderRegex = new Regex(@"dir (\w+)", RegexOptions.Compiled);

        var folderStack = new Stack<FolderNode>();
        var rootNode = new FolderNode(@"\");
        
        foreach (var line in input)
        {
            if (toRootRegex.IsMatch(line))
            {
                folderStack.Clear();
                folderStack.Push(rootNode);
            }
            else if (isFolderRegex.IsMatch(line))
            {
                var folderName = isFolderRegex.Match(line).Groups[1].Value;
                folderStack.Peek().AddNode(new FolderNode(folderName));
            }
            else if (changeToFolderRegex.IsMatch(line))
            {
                var folderName = changeToFolderRegex.Match(line).Groups[1].Value;
                var folder = folderStack.Peek().GetFolder(folderName);
                folderStack.Push(folder);
            }
            else if (changeToPreviousRegex.IsMatch(line)) 
            {
                folderStack.Pop();
            }
        }

        rootNode.ToConsole(0);
        
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

        protected Node(string name)
        {
            Name = name;
        }

        public abstract void ToConsole(int indent);

        public abstract long GetSize();
    }

    private class FolderNode : Node
    {
        private readonly IList<Node> _childNodes = new List<Node>();

        public FolderNode(string name) : base(name)
        {
        }

        public void AddNode(Node childNode)
        {
            if (_childNodes.SingleOrDefault(node => node.GetType() == childNode.GetType() && node.Name == childNode.Name) != null)
            {
                return;
            }
            
            _childNodes.Add(childNode);
        }

        public override void ToConsole(int indent)
        {
            Console.WriteLine(new string('\t', indent) + $"- {Name} (dir)");
            _childNodes.ForEach(node => node.ToConsole(indent + 1));
        }

        public override long GetSize()
        {
            return _childNodes.Select(node => GetSize()).Sum();
        }

        public FolderNode GetFolder(string name)
        {
            return _childNodes.OfType<FolderNode>().Single(node => node.Name == name);
        }
    }

    private class File : Node
    {
        private readonly long _size;

        public File(string name, long size) : base(name)
        {
            _size = size;
        }

        public override void ToConsole(int indent)
        {
            Console.WriteLine(new string('\t', indent) + $"- {Name} (file, size={_size})");
        }

        public override long GetSize()
        {
            return _size;
        }
    }
}