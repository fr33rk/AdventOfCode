using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle10 : BasePuzzle
{
    protected override void SolvePart1(IEnumerable<string> input)
    {
        var chunkProcessor = new ChunkProcessor();
        var answer = 0;
        foreach (var line in input)
        {
            foreach (var character in line.ToArray())
            {
                if (!chunkProcessor.TryProcess(character, out var score))
                {
                    Console.WriteLine(line);
                    answer += score;
                    break;
                }
            }
            chunkProcessor.Reset();
        }
        
        Console.WriteLine($"Answer: {answer}");
    }

    protected override void SolvePart2(IEnumerable<string> input)
    {
        var chunkProcessor = new ChunkProcessor();
        var answer = 0;
        var totalScores = new List<int>();
        
        foreach (var line in input)
        {
            var invalid = false;
            chunkProcessor.Reset();
            foreach (var character in line.ToArray())
            {
                if (chunkProcessor.TryProcess(character, out _)) 
                    continue;
                
                invalid = true;
                break;
            }

            if (invalid) 
                continue;
            
            chunkProcessor.Complete(out var missing, out var points);

            if (points <= 0) 
                continue;
            
            //Console.WriteLine($"{line} missing {missing}, points: {points}");
            //Console.WriteLine($"{line} missing {missing}, points: {points}");
            totalScores.Add(points);
        }

        totalScores.OrderBy(x => x).ForEach(x => Console.WriteLine(x));
        
        answer = totalScores.OrderBy(x => x).Skip(totalScores.Count / 2).First();
        
        Console.WriteLine($"Answer: {answer}");
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "[({(<(())[]>[[{[]{<()<>>",
            "[(()[<>])]({[<{<<[]>>(",
            "{([(<{}[<>[]}>{[]{[(<()>",
            "(((({<>}<{<{<>}{[]{[]{}",
            "[[<[([]))<([[{}[[()]]]",
            "[{[{({}]{}}([{[{{{}}([]",
            "{<[[]]>}<{[{[{[]{()[[[]",
            "[<(<(<(<{}))><([]([]()",
            "<{([([[(<>()){}]>(<<{{",
            "<{([{{}}[<[[[<>{}]]]>[]]"
        };
    }

    private class ChunkProcessor
    {
        private readonly List<Pair> _pairs = new List<Pair>
        {
            new Pair('(',')'),
            new Pair('{','}'),
            new Pair('[',']'),
            new Pair('<','>')
        };

        private readonly Dictionary<char, Pair> _opening;
        private readonly Stack<Pair> _pairStack = new();
        
        public ChunkProcessor()
        {
            _opening = _pairs.ToDictionary(x => x.Opening, x => x);
        }
        
        public bool TryProcess(char character, out int score)
        {
            score = 0;
            if (_opening.ContainsKey(character))
            {
                _pairStack.Push(_opening[character]);
                return true;
            }

            if (_pairStack.Peek().Closing == character)
            {
                _pairStack.Pop();
                return true;
            }

            score = character switch
            {
                ')' => 3,
                ']' => 57,
                '}' => 1197,
                '>' => 25137
            };
            
            return false;
        }

        public void Reset()
        {
            _pairStack.Clear();
        }

        public void Complete(out string line, out int score)
        {
            line = string.Empty;
            score = 0;
            
            while (_pairStack.TryPop(out var pair))
            {
                line += pair.Closing;
                score *= 5;
                score += pair.Closing switch
                {
                    ')' => 1,
                    ']' => 2,
                    '}' => 3,
                    '>' => 4,
                    _ => 0
                };
            }
        }
        
        private record Pair(char Opening, char Closing);
    }
}