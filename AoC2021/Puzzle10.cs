using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle10 : BasePuzzle
{
    protected override long SolvePart1(IEnumerable<string> input)
    {
        var chunkProcessor = new ChunkProcessor();
        var answer = 0;
        var invalid = 0;
        foreach (var line in input)
        {
            foreach (var character in line.ToArray())
            {
                if (!chunkProcessor.TryProcess(character, out var score))
                {
                    Console.WriteLine(line);
                    answer += score;
                    invalid++;
                    break;
                }
            }
            chunkProcessor.Reset();
        }
        Console.WriteLine($"Invalid: {invalid}");

        return answer;
    }

    protected override long SolvePart2(IEnumerable<string> input)
    {
        var chunkProcessor = new ChunkProcessor();
        var lineIndex = 0;
        var scores = new List<long>();

        foreach (var line in input)
        {
            lineIndex++;
            chunkProcessor.Reset();
            var invalid = false;
            
            foreach (var character in line.ToArray())
            {
                if (!chunkProcessor.TryProcess(character, out _))
                {
                    Console.WriteLine($"{lineIndex}: INVALID: {line}");
                    invalid = true;
                    break;
                }
            }

            if (invalid)
                continue;
            
            chunkProcessor.Complete(out var remaining, out var score);
            if (score == 0)
            {
                Console.WriteLine($"{lineIndex}: VALID: {line}");
                continue;
            }
            
            Console.WriteLine($"{lineIndex}: INCOMPLETE: {line} - {remaining} ({score})");
            scores.Add(score);
        }

        return scores.OrderBy(x => x).Skip(scores.Count / 2).First();
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
                '>' => 25137,
                _ => 0
            };
            
            return false;
        }

        public void Reset()
        {
            _pairStack.Clear();
        }

        public void Complete(out string line, out long score)
        {
            line = string.Empty;
            score = 0;

            if (_pairStack.Count == 0)
                return;
            
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