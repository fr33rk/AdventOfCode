using System.Text.RegularExpressions;
using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle11 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var monkeys = input.Chunk(7).Select(Monkey.FromStrings).ToArray();
        
        monkeys.ForEach(monkey => monkey.AttachMonkeys(monkeys));

        for (var round = 0; round < 20; round++)
        {
            monkeys.ForEach(monkey => monkey.Process());
        }

        return monkeys
            .Select(monkey => monkey.ItemsInspected)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((total, itemsInspected) => total * itemsInspected)
            .ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var monkeys = input.Chunk(7).Select(Monkey.FromStrings).ToArray();
        
        monkeys.ForEach(monkey => monkey.AttachMonkeys(monkeys));

        var superModulo = monkeys.Select(m => m.Divisor).Aggregate((t, d) => t * d);
        
        for (var round = 0; round < 10000; round++)
        {
            monkeys.ForEach(monkey => monkey.Process2(superModulo));
        }

        return monkeys
            .Select(monkey => monkey.ItemsInspected)
            .OrderByDescending(x => x)
            .Take(2)
            .Aggregate((total, itemsInspected) => total * itemsInspected)
            .ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "Monkey 0:",
            "  Starting items: 79, 98",
            "  Operation: new = old * 19",
            "  Test: divisible by 23",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 3",
            "",
            "Monkey 1:",
            "  Starting items: 54, 65, 75, 74",
            "  Operation: new = old + 6",
            "  Test: divisible by 19",
            "    If true: throw to monkey 2",
            "    If false: throw to monkey 0",
            "",
            "Monkey 2:",
            "  Starting items: 79, 60, 97",
            "  Operation: new = old * old",
            "  Test: divisible by 13",
            "    If true: throw to monkey 1",
            "    If false: throw to monkey 3",
            "",
            "Monkey 3:",
            "  Starting items: 74",
            "  Operation: new = old + 3",
            "  Test: divisible by 17",
            "    If true: throw to monkey 0",
            "    If false: throw to monkey 1"
        };
    }

    private class Monkey
    {
        private readonly IList<long> _items;
        private readonly Operation _operation;
        private readonly long _decider;
        private readonly int _deciderMonkeyTrueIndex;
        private readonly int _deciderMonkeyFalseIndex;
        private Monkey? _deciderTrueMonkey;
        private Monkey? _deciderFalseMonkey;

        public long ItemsInspected { get; private set; }

        public long Divisor => _decider;
        
        private Monkey(IList<long> items, Operation operation, int decider, int deciderMonkeyTrueIndex, int deciderMonkeyFalseIndex)
        {
            _items = items;
            _operation = operation;
            _decider = decider;
            _deciderMonkeyTrueIndex = deciderMonkeyTrueIndex;
            _deciderMonkeyFalseIndex = deciderMonkeyFalseIndex;
        }

        public static Monkey FromStrings(IEnumerable<string> strings)
        {
            var rawInput = strings.ToList();
            var numbers = rawInput[1]
                .Replace("Starting items: ", "")
                .Replace(" ", "")
                .Split(',')
                .Select(s => Convert.ToInt64(s))
                .ToList();

           return new Monkey(
                numbers,
                Operation.FromString(rawInput[2]),
                Convert.ToInt32(rawInput[3].Replace("Test: divisible by ", "").Trim()),
                Convert.ToInt32(rawInput[4].Replace("If true: throw to monkey ", "").Trim()),
                Convert.ToInt32(rawInput[5].Replace("If false: throw to monkey ", "").Trim())
            );
        }
        
        public void Process()
        {
            foreach (var item in _items)
            {
                var newWorryLevel = _operation.DoOperation(item);
                newWorryLevel = newWorryLevel / 3;

                if (newWorryLevel % _decider == 0)
                    _deciderTrueMonkey?.AddItem(newWorryLevel);
                else
                    _deciderFalseMonkey?.AddItem(newWorryLevel);
                ItemsInspected++;
            }
            
            _items.Clear();
        }

        public void Process2(long modulo)
        {
            foreach (var item in _items)
            {
                var newWorryLevel = _operation.DoOperation(item);

                newWorryLevel %= modulo;
                
                if (newWorryLevel % _decider == 0)
                    _deciderTrueMonkey?.AddItem(newWorryLevel);
                else
                    _deciderFalseMonkey?.AddItem(newWorryLevel);
                ItemsInspected++;
            }
            
            _items.Clear();
        }
        
        public void AttachMonkeys(ICollection<Monkey> allMonkeys)
        {
            _deciderTrueMonkey = allMonkeys.ElementAt(_deciderMonkeyTrueIndex);
            _deciderFalseMonkey = allMonkeys.ElementAt(_deciderMonkeyFalseIndex);
        }

        private void AddItem(long item)
        {
            _items.Add(item);
        }
    }

    private class Operation
    {
        private readonly long? _left;
        private readonly long? _right;
        private readonly char _operator;

        private Operation(int? left, int? right, char @operator)
        {
            _left = left;
            _right = right;
            _operator = @operator;
        }

        public static Operation FromString(string raw)
        {
            var match = Regex.Match(raw, @"Operation: new = (\d+|old) (\*|\+) (\d+|old)");
            if (!match.Success)
                throw new ArgumentException($"Cannot turn {raw} into an expression");

            return new Operation(
                match.Groups[1].Value != "old" ? Convert.ToInt32(match.Groups[1].Value) : null,
                match.Groups[3].Value != "old" ? Convert.ToInt32(match.Groups[3].Value) : null,
                match.Groups[2].Value[0]
            );
        }

        public long DoOperation(long oldValue)
        {
            var actualLeft = _left ?? oldValue;
            var actualRight = _right ?? oldValue;

            return _operator switch
            {
                '+' => actualLeft + actualRight,
                '*' => actualLeft * actualRight,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}