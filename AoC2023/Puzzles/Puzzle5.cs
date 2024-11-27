using System.Diagnostics;
using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle5 : BasePuzzle
{
    private record MapCode(long Input, long Offset, long Count);

    private class Node
    {
        private readonly Dictionary<long, long> _lookupTable = new();
        private Node? _nextNode;

        public Node(IEnumerable<MapCode> rawLookupTable)
        {
            var sorted = rawLookupTable.OrderBy(x => x.Input).ToList();
            if (sorted.First().Input != 0)
            {
                _lookupTable[0] = 0;
            }

            foreach (var lookupRecord in sorted)
            {
                _lookupTable[lookupRecord.Input] = lookupRecord.Offset - lookupRecord.Input;
                _lookupTable[lookupRecord.Input + lookupRecord.Count] = 0;
            }
        }

        public void Attach(Node nextNode)
        {
            if (_nextNode == null)
            {
                _nextNode = nextNode;
            }
            else
            {
                _nextNode.Attach(nextNode);
            }
        }

        public long Process(long input)
        {
            var key = _lookupTable.Keys.Where(k => k <= input).Max();
            var output = input + _lookupTable[key];
            return _nextNode?.Process(output) ?? output;
        }
    }

    private Node Parse(List<string> lines)
    {
        Node? firstNode = null;
        List<MapCode> foundMapCodes = new();
        foreach (var line in lines.Skip(2))
        {
            if (line.Length == 0)
            {
                if (firstNode == null)
                {
                    firstNode = new Node(foundMapCodes);
                }
                else
                {
                    firstNode.Attach(new Node(foundMapCodes));
                }
                foundMapCodes.Clear();
                continue;
            }

            if (char.IsDigit(line[0]))
            {
                var rawMapCode = line.Split(" ").Select(long.Parse).ToArray();
                foundMapCodes.Add(new MapCode(rawMapCode[1], rawMapCode[0], rawMapCode[2]));
            }
        }

        return firstNode!;
    }

    protected override string SolvePart1(IEnumerable<string> input)
    {
        var lines = input.ToList();
        lines.Add("");
        var seeds = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();

        var firstNode = Parse(lines);

        var answer = seeds.Select(firstNode!.Process).Min();

        return answer.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var lines = input.ToList();
        lines.Add("");
        var seeds = lines[0].Split(":")[1].Trim().Split(" ").Select(long.Parse).ToList();

        var firstNode = Parse(lines);
        var min = long.MaxValue;
        for (var x = 0; x < 71155519; x++)
        {
            min = Math.Min(min, firstNode.Process(630335678 + x));
        }


        var answer = seeds.Select(firstNode!.Process).Min();

        return answer.ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return [
            "seeds: 79 14 55 13",
            "",
            "seed-to-soil map:",
            "50 98 2",
            "52 50 48",
            "",
            "soil-to-fertilizer map:",
            "0 15 37",
            "37 52 2",
            "39 0 15",
            "",
            "fertilizer-to-water map:",
            "49 53 8",
            "0 11 42",
            "42 0 7",
            "57 7 4",
            "",
            "water-to-light map:",
            "88 18 7",
            "18 25 70",
            "",
            "light-to-temperature map:",
            "45 77 23",
            "81 45 19",
            "68 64 13",
            "",
            "temperature-to-humidity map:",
            "0 69 1",
            "1 0 69",
            "",
            "humidity-to-location map:",
            "60 56 37",
            "56 93 4"
        ];
    }
}