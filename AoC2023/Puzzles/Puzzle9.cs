using System.Globalization;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle9 : BasePuzzle
{
    private enum Ask
    {
        Previous,
        Next,
    }

    // Function to calculate divided differences
    private double[,] DividedDifferenceTable(double[] x, double[] y, int n)
    {
        double[,] table = new double[n, n];
        for (int i = 0; i < n; i++)
            table[i, 0] = y[i];

        for (int j = 1; j < n; j++)
        {
            for (int i = 0; i < n - j; i++)
            {
                table[i, j] = (table[i + 1, j - 1] - table[i, j - 1]) / (x[i + j] - x[i]);
            }
        }
        return table;
    }

    // Function to apply Newton's interpolation
    private double NewtonInterpolationValue(double[] x, double[,] table, double value, int n)
    {
        double result = table[0, 0];
        for (int i = 1; i < n; i++)
        {
            double term = table[0, i];
            for (int j = 0; j < i; j++)
            {
                term *= (value - x[j]);
            }
            result += term;
        }
        return result;
    }

    private double Solve(IEnumerable<string> input, Ask ask)
    {
        var answer = 0;

        var sequences = input
            .Select(line => line.Split(' ')
                .Select(Convert.ToDouble).ToArray())
            .ToList();

        foreach (var sequence in sequences)
        {
            var xs = Enumerable.Range(1, sequence.Length).Select(Convert.ToDouble).ToArray();
            var table = DividedDifferenceTable(xs, sequence, sequence.Length);
            var result = NewtonInterpolationValue(xs, table, ask == Ask.Previous ? 0 : sequence.Length + 1, sequence.Length);
            answer += Convert.ToInt32(Math.Round(result));
        }

        return answer;
    }

    /// <inheritdoc />
    protected override string SolvePart1(IEnumerable<string> input)
    {
        return Solve(input, Ask.Next).ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    protected override string SolvePart2(IEnumerable<string> input)
    {
        return Solve(input, Ask.Previous).ToString(CultureInfo.InvariantCulture);
    }

    /// <inheritdoc />
    protected override IEnumerable<string> GetTestInput()
    {
        return [
            "0 3 6 9 12 15",
            "1 3 6 10 15 21",
            "10 13 16 21 30 45",
        ];
    }
}