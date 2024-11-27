using System.Globalization;
using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle4 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        double totalScore = 0;
        foreach (var line in input)
        {
            var cardNumberParts = line.Split(':')[1].Split("|");
            var winners = Regex.Matches(cardNumberParts[0], @"\d+").Select(m => int.Parse(m.Value)).ToList();
            var cardNumbers = Regex.Matches(cardNumberParts[1], @"\d+").Select(m => int.Parse(m.Value)).ToList();
            var winningCardNumbers = winners.Intersect(cardNumbers).Count();
            var score = winningCardNumbers == 0 ? 0 : Math.Pow(2,  winningCardNumbers - 1);
            totalScore += score;
        }

        return totalScore.ToString(CultureInfo.InvariantCulture);
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        int answer = 0;
        var cards = (
                from line in input
                select line.Split(':')[1].Split("|")
                into cardNumberParts
                let winners = Regex.Matches(cardNumberParts[0], @"\d+")
                    .Select(m => int.Parse(m.Value)).ToList()
                let cardNumbers = Regex.Matches(cardNumberParts[1], @"\d+")
                    .Select(m => int.Parse(m.Value)).ToList()
                select winners.Intersect(cardNumbers).Count())
            .ToArray();

        for (var cardIndex = 0; cardIndex < cards.Length; cardIndex++)
        {
            answer += GetCardCount(cards, cardIndex);
        }

        return answer.ToString(CultureInfo.InvariantCulture);

        int GetCardCount(int[] nextCards, int startIndex)
        {
            if (startIndex >= nextCards.Length)
            {
                return 0;
            }

            var result = 1;
            for (var x = 1; x <= nextCards[startIndex]; x++)
            {
                result += GetCardCount(nextCards, startIndex + x);
            }

            return result;
        }
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return
        [
            "Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53",
            "Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19",
            "Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1",
            "Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83",
            "Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36",
            "Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11"
        ];
    }
}