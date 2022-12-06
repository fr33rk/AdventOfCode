using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using MoreLinq;
using MoreLinq.Extensions;
using PuzzleSolver.Core;

namespace AoC2021;

public class Puzzle04 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        input = input.ToList();
        
        ProcessInput(input, out var bingoNumbers, out var bingoNotes);

        BingoNote? winner = null;

        using var bingoNumber = bingoNumbers.GetEnumerator();

        while (winner == null && bingoNumber.MoveNext())
        {
            Console.WriteLine($"Draweed: {bingoNumber.Current}");
            winner = bingoNotes.FirstOrDefault(note => note.GetsBingo(bingoNumber.Current));
            //bingoNotes.ForEach(note => note.ToConsole());
        }

        if (winner == null)
            throw new InvalidOperationException("No winner found");
        
        Console.WriteLine("Winner:");
        winner.ToConsole();
        
        var sumOfAllUnticked = winner.SumOfAllUnTicked();
        Console.WriteLine($"Sum of unticked: {sumOfAllUnticked}");
        return (sumOfAllUnticked * bingoNumber.Current).ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        input = input.ToList();
        
        ProcessInput(input, out var bingoNumbers, out var bingoNotes);

        BingoNote? lastWinner = null;

        using var bingoNumber = bingoNumbers.GetEnumerator();

        while (lastWinner == null && bingoNumber.MoveNext())
        {
            Console.WriteLine($"Drawed: {bingoNumber.Current}");
            MoreEnumerable.ForEach(bingoNotes, note =>
            {
                note.GetsBingo(bingoNumber.Current);
                //note.ToConsole();
            });

            if (bingoNotes.Count == 1)
            {
                if (bingoNotes.First().HasBingo)
                    lastWinner = bingoNotes.First();
            }
            else
            {
                bingoNotes = bingoNotes.Where(note => !note.HasBingo).ToList();
            }
        }

        if (lastWinner == null)
            throw new InvalidOperationException("No winner found");
        
        Console.WriteLine("Last winner:");
        lastWinner.ToConsole();
        
        var sumOfAllUnticked = lastWinner.SumOfAllUnTicked();
        Console.WriteLine($"Sum of unticked: {sumOfAllUnticked}");
        Console.WriteLine($"Answer: {sumOfAllUnticked * bingoNumber.Current}");

        return (sumOfAllUnticked * bingoNumber.Current).ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new List<string>
        {
            "7,4,9,5,11,17,23,2,0,14,21,24,10,16,13,6,15,25,12,22,18,20,8,19,3,26,1",
            "",
            "22 13 17 11  0",
            " 8  2 23  4 24",
            "21  9 14 16  7",
            " 6 10  3 18  5",
            " 1 12 20 15 19",
            "",
            " 3 15  0  2 22",
            " 9 18 13 17  5",
            "19  8  7 25 23",
            "20 11 10 24  4",
            "14 21 16 12  6",
            "",
            "14 21 17 24  4",
            "10 16 15  9 19",
            "18  8 23 26 20",
            "22 11 13  6  5",
            " 2  0 12  3  7"
        };
    }

    private static void ProcessInput(IEnumerable<string> input, out IEnumerable<int> bingoNumbers, out IList<BingoNote> bingoNotes)
    {
        var idGenerator = new IdGenerator();

        bingoNumbers = input.First().Split(',').Select(s => Convert.ToInt32(s));
        
        input = input.Skip(2);
        bingoNotes = GetBingoNotes(idGenerator, input).ToList();
    }

    private static IEnumerable<BingoNote> GetBingoNotes(IdGenerator idGenerator, IEnumerable<string> input)
    {
        while (input.Any())
        {
            yield return BingoNote.FromStrings(idGenerator.GetNext(), input.Take(5));
            input = input.Skip(6);
        }
    }

    private class IdGenerator
    {
        private int _current = 0;

        public int GetNext()
        {
            return _current++;
        }
    }

    private class BingoNote
    {
        private class BingoNumber
        {
            public int Number { get; }
            public bool Ticked { get; private set; }

            public BingoNumber(int number)
            {
                Number = number;
                Ticked = false;
            }

            public void Tick()
            {
                Ticked = true;
            }

            public void ToConsole()
            {
                var savedBackgroundColor = Console.BackgroundColor;
                var savedForegroundColor = Console.ForegroundColor;
               
                if (Ticked)
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White; 
                }

                Console.Write($"{Number,3}");
                
                if (Ticked)
                {
                    Console.BackgroundColor = savedBackgroundColor;
                    Console.ForegroundColor = savedForegroundColor;
                }
            }
        }

        private readonly List<List<BingoNumber>> _horizontalLines;
        private readonly List<List<BingoNumber>> _verticalLines;
        private readonly int _Id;
        
        public bool HasBingo { get; private set; }
        
        private BingoNote(int id, IEnumerable<IEnumerable<BingoNumber>> bingoNumbers)
        {
            _Id = id;
            _horizontalLines = bingoNumbers.Select(x => x.ToList()).ToList();
            _verticalLines = new List<List<BingoNumber>>();
            
            var columns = _horizontalLines.First().Count;
            for (var column = 0; column < columns; column++)
            {
                _verticalLines.Add(_horizontalLines.Select(line => line.ElementAt(column)).ToList());
            }
        }
        

        public static BingoNote FromStrings(int id, IEnumerable<string> input)
        {
            var regex = new Regex(@"\d+", RegexOptions.Compiled);

            var numbers = input
                .Select(line =>
                    regex.Matches(line).Select(m => new BingoNumber(Convert.ToInt32(m.Groups.Values.First().Value))));
            
            return new BingoNote(id, numbers);
        }

        public bool GetsBingo(int nextNumber)
        {
            var horizontalLine = _horizontalLines.FirstOrDefault(line => line.Any(n => n.Number == nextNumber));

            if (horizontalLine == null) 
                return false;
            
            horizontalLine.First(n => n.Number == nextNumber).Tick();
            var verticalLine = _verticalLines.First(line => line.Any(n => n.Number == nextNumber));

            HasBingo = horizontalLine.All(n => n.Ticked)
                      || verticalLine.All(n => n.Ticked);

            return HasBingo;
        }

        public void ToConsole()
        {
            Console.WriteLine($"Note ID: {_Id}");
            _horizontalLines.ForEach(line =>
            {
                line.ForEach(number => number.ToConsole());
                Console.WriteLine();
            });
            Console.WriteLine();
        }

        public int SumOfAllUnTicked()
        {
            return _horizontalLines
                .SelectMany(n => n)
                .Where(n => !n.Ticked)
                .Sum(n => n.Number);
        }
    }
}