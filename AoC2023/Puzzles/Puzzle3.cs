using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle3 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var answer = 0;
        var rows = input.ToList();
        rows.Insert(0, new string('.', rows[0].Length));
        rows.Add(new string('.', rows[0].Length));

        var partNumber = 0;
        var processingPartNumber = false;
        var processStart = 0;
        var symbolRegex = new Regex(@"[^\d|\.]", RegexOptions.Compiled);

        for (var rowCount = 1; rowCount < rows.Count - 1; rowCount++)
        {
            var previousRow = rows[rowCount - 1];
            var currentRow = rows[rowCount];
            var nextRow = rows[rowCount + 1];

            for (var column = 0; column < currentRow.Length; column++)
            {
                var element = currentRow[column];
                var processingDone = false;
                if (char.IsDigit(element))
                {
                    if (!processingPartNumber)
                    {
                        processStart = column - 1 < 0 ? 0 : column - 1;
                        processingPartNumber = true;
                    }

                    partNumber = partNumber * 10 + (element - '0');
                    processingDone = column + 1 == currentRow.Length;
                }
                else
                {
                    processingDone = processingPartNumber;
                }

                if (!processingDone)
                    continue;

                var processEnd = column + 1 >= currentRow.Length ? column : column + 1;
                if (symbolRegex.IsMatch(previousRow.AsSpan(processStart, processEnd - processStart))
                    || symbolRegex.IsMatch(currentRow.AsSpan(processStart, processEnd - processStart))
                    || symbolRegex.IsMatch(nextRow.AsSpan(processStart, processEnd - processStart)))
                {
                    answer += partNumber;
                    Console.WriteLine($"Part {partNumber} is valid");
                }
                else
                {
                    Console.WriteLine($"Part {partNumber} is invalid");
                }

                partNumber = 0;
                processingPartNumber = false;
            }
        }

        return answer.ToString();

    }

    private record Gear(int Row, int Column, IList<int> PartNumbers);

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var rows = input.ToList();
        rows.Insert(0, new string('.', rows[0].Length));
        rows.Add(new string('.', rows[0].Length));

        var partNumber = 0;
        var processingPartNumber = false;
        var processStart = 0;
        var gearPositions = new List<Gear>();

        for (var rowCount = 1; rowCount < rows.Count - 1; rowCount++)
        {
            var previousRow = rows[rowCount - 1];
            var currentRow = rows[rowCount];
            var nextRow = rows[rowCount + 1];

            for (var column = 0; column < currentRow.Length; column++)
            {
                var element = currentRow[column];
                var processingDone = false;
                if (char.IsDigit(element))
                {
                    if (!processingPartNumber)
                    {
                        processStart = column - 1 < 0 ? 0 : column - 1;
                        processingPartNumber = true;
                    }

                    partNumber = partNumber * 10 + (element - '0');
                    processingDone = column + 1 == currentRow.Length;
                }
                else
                {
                    processingDone = processingPartNumber;
                }

                if (!processingDone)
                    continue;

                var processEnd = column + 1 >= currentRow.Length ? column : column + 1;
                // Ok we got the part number, we now need to check if it is adjacent to a gear.
                FindAndAddToGearPosition(previousRow.AsSpan(processStart, processEnd-processStart), rowCount - 1, processStart, partNumber);
                FindAndAddToGearPosition(currentRow.AsSpan(processStart, processEnd-processStart), rowCount, processStart, partNumber);
                FindAndAddToGearPosition(nextRow.AsSpan(processStart, processEnd-processStart),  rowCount + 1, processStart, partNumber);

                partNumber = 0;
                processingPartNumber = false;
            }
        }

        return gearPositions
            .Where(gp => gp.PartNumbers.Count == 2)
            .Select(gp => gp.PartNumbers
                .Aggregate((p1, p2) => p1 * p2 ))
            .Sum()
            .ToString();

        void FindAndAddToGearPosition(ReadOnlySpan<char> part, int rowIndex, int columnOffset, int foundPartNumber)
        {
            for (var columnIndex = 0; columnIndex < part.Length; columnIndex++)
            {
                if (part[columnIndex] != '*')
                    continue;

                var gearPosition = gearPositions.SingleOrDefault(gp => gp.Row == rowIndex && gp.Column == columnIndex + columnOffset);
                if (gearPosition == null)
                {
                    gearPosition = new Gear(rowIndex, columnIndex + columnOffset, new List<int>());
                    gearPositions.Add(gearPosition);
                }
                gearPosition.PartNumbers.Add(foundPartNumber);
            }
        }
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "467..114..",
            "...*......",
            "..35..633.",
            "......#...",
            "617*......",
            ".....+.58.",
            "..592.....",
            "......755.",
            "...$.*....",
            ".664.598.."
        ];
    }
}