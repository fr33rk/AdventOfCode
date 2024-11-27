using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle8 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var lines = input.ToList();

        var instructions = input.First();
        var nodes = input.Skip(1)
            .Where(x => x.Length > 1)
            .Select(line => line.Split(new[] { " = " }, StringSplitOptions.None))
            .ToDictionary(parts => parts[0], parts => parts[1].Trim('(', ')').Split(", "));

        string currentNode = "AAA";
        int steps = 0;
        int instructionIndex = 0;

        while (currentNode != "ZZZ")
        {
            char direction = instructions[instructionIndex];
            currentNode = direction == 'L' ? nodes[currentNode][0] : nodes[currentNode][1];
            steps++;
            instructionIndex = (instructionIndex + 1) % instructions.Length;
        }

        return steps.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var lines = input.ToList();

        var instructions = lines.First();
        var nodes = lines.Skip(1)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split([" = "], StringSplitOptions.None))
            .Select(parts =>
            {
                var nextNodes = parts[1].Trim('(', ')').Split(", ");
                return new Node(parts[0], nextNodes[0], nextNodes[1]);
            })
            .ToList();

        // Link the nodes
        foreach (var node in nodes)
        {
            node.Left = nodes.Single(n => n.Name == node.RawLeft);
            node.Right = nodes.Single(n => n.Name == node.RawRight);
        }

        var paths = nodes.Where(n => n.IsStart).ToArray();
        var stepsNeeded = new List<ulong>();
        foreach (var path in paths)
        {
            ulong steps = 0;
            int instructionIndex = 0;
            var currentPath = path;
            while (!currentPath.IsFinal)
            {
                char direction = instructions[instructionIndex];
                currentPath = direction == 'L' ? currentPath.Left! : currentPath.Right!;
                instructionIndex = (instructionIndex + 1) % instructions.Length;
                steps++;
            }
            stepsNeeded.Add(steps);
            Console.WriteLine(steps);
        }

        // 14265111103729
        ulong lcm = stepsNeeded.Aggregate(LCM);
        return lcm.ToString();
    }

    private static ulong GCD(ulong a, ulong b)
    {
        while (b != 0)
        {
            ulong temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    private static ulong LCM(ulong a, ulong b)
    {
        return (a / GCD(a, b)) * b;
    }

    private class Node
    {
        public Node(string name, string left, string right)
        {
            IsStart = name.EndsWith('A');
            IsFinal = name.EndsWith('Z');
            Name = name;
            RawLeft = left;
            RawRight = right;
        }

        public bool IsStart { get; }
        public bool IsFinal { get; }

        public string Name { get; }
        public string RawLeft { get; }

        public string RawRight { get; }

        public Node? Left { get; set; }

        public Node? Right { get; set; }
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "LLR",
            "AAA = (BBB, BBB)",
            "BBB = (AAA, ZZZ)",
            "ZZZ = (ZZZ, ZZZ)"
        ];
    }

    protected override IEnumerable<string> GetTestInputPart2()
    {
        return
        [
            "LR",
            "11A = (11B, XXX)",
            "11B = (XXX, 11Z)",
            "11Z = (11B, XXX)",
            "22A = (22B, XXX)",
            "22B = (22C, 22C)",
            "22C = (22Z, 22Z)",
            "22Z = (22B, 22B)",
            "XXX = (XXX, XXX)"
        ];
    }
}