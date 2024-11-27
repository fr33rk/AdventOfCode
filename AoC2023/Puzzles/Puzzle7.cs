using System.Diagnostics;
using PuzzleSolver.Core;

namespace AoC2023.Puzzles;

public class Puzzle7 : BasePuzzle
{
    record Hand(int Value, int[] CardValues, string rawHand, int Bid);

    protected override string SolvePart1(IEnumerable<string> input)
    {
        int CardToValue(char card)
        {
            // 23456789TJQHA
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 11,
                'T' => 10,
                _ => card - '0'
            };
        }

        int[] ConvertHand(string hand)
        {
            return hand.Select(CardToValue).ToArray();
        }

        var hands = new List<Hand>();

        int GetHandValue(int firstGroupCount, int secondGroupCount)
        {
            return firstGroupCount switch
            {
                5 =>
                    // Five of a kind
                    600,
                4 =>
                    // Four of a kind
                    500,
                3 when secondGroupCount == 2 =>
                    // Full house
                    400,
                3 =>
                    // Three of a kind
                    300,
                2 when secondGroupCount == 2 =>
                    // Two pairs
                    200,
                2 =>
                    // One pair
                    100,
                _ => 0
            };
        }

        foreach (var line in input)
        {
            var splitLine = line.Split(' ');
            var convertedHand = ConvertHand(splitLine[0]);
            var sortedHand = convertedHand
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var handValue = GetHandValue(sortedHand[0].Count, sortedHand.Count > 1 ? sortedHand[1].Count : 0);
            handValue += convertedHand[0];
            hands.Add(new Hand(handValue, convertedHand, line, int.Parse(splitLine[1])));
        }

        var answer = hands
            .OrderBy(x => x, new HandComparer())
            .Select((hand, i) => (i + 1) * hand.Bid)
            .Sum();

        //253910319
        return answer.ToString();
    }

    private class HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? x, Hand? y)
        {
            if(x == null || y == null)
            {
                throw new InvalidOperationException();
            }

            if (x.Value > y.Value)
            {
                return 1;
            }
            if (x.Value < y.Value)
            {
                return -1;
            }

            for (var i = 1; i < x.CardValues.Length; i++)
            {
                if (x.CardValues[i] > y.CardValues[i])
                {
                    return 1;
                }
                if (x.CardValues[i] < y.CardValues[i])
                {
                    return -1;
                }
            }

            return 0;
        }
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        int CardToValue(char card)
        {
            // 23456789TJQHA
            return card switch
            {
                'A' => 14,
                'K' => 13,
                'Q' => 12,
                'J' => 1,
                'T' => 10,
                _ => card - '0'
            };
        }

        int[] ConvertHand(string hand)
        {
            return hand.Select(CardToValue).ToArray();
        }

        var hands = new List<Hand>();

        int GetHandValue(int firstGroupCount, int secondGroupCount, int jokerCount)
        {
            if (jokerCount == 5)
            {
                firstGroupCount = 5;
            }
            else
            {
                firstGroupCount += jokerCount;
            }

            return firstGroupCount switch
            {
                5 =>
                    // Five of a kind
                    600,
                4 =>
                    // Four of a kind
                    500,
                3 when secondGroupCount == 2 =>
                    // Full house
                    400,
                3 =>
                    // Three of a kind
                    300,
                2 when secondGroupCount == 2 =>
                    // Two pairs
                    200,
                2 =>
                    // One pair
                    100,
                _ => 0
            };
        }

        foreach (var line in input)
        {
            var splitLine = line.Split(' ');
            var convertedHand = ConvertHand(splitLine[0]);
            var sortedHand = convertedHand
                .GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .ToList();

            var handValue = GetHandValue(
                sortedHand.FirstOrDefault(x => x.Value > 1)?.Count ?? 0,
                sortedHand.Where(x => x.Value > 1).Skip(1).FirstOrDefault()?.Count ?? 0,
                sortedHand.FirstOrDefault(x => x.Value == 1)?.Count ?? 0);

            handValue += convertedHand[0];
            hands.Add(new Hand(handValue, convertedHand, line, int.Parse(splitLine[1])));
        }

        var test = hands
            .OrderBy(x => x, new HandComparer());

        foreach (var t in test)
        {
            Console.WriteLine(t.rawHand);
        }

        var answer = hands
            .OrderBy(x => x, new HandComparer())
            .Select((hand, i) => (i + 1) * hand.Bid)
            .Sum();

        return answer.ToString();
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return [
            "32T3K 765",
            "T55J5 684",
            "KK677 28",
            "KTJJT 220",
            "QQQJA 483"
        ];
    }
}