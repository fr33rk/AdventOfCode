using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public partial class Puzzle12 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var answer = input
            .Where(s => s.Length > 0)
            .Chunk(2)
            .Select((rawPairs, i) => new
            {
                index = i + 1,
                left = Parse(rawPairs[0]),
                right = Parse(rawPairs[1])
            })
            .Where(pair => IsOrdered(pair.left, pair.right) != CompareResult.NotOrdered)
            .Sum(pair => pair.index);
        
        return answer.ToString();
    }

    private record Sequence(string Name, int ID);
    
    protected override string SolvePart2(IEnumerable<string> input)
    {
        return string.Empty;
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "[1,1,3,1,1]",
            "[1,1,5,1,1]",
            "",
            "[[1],[2,3,4]]",
            "[[1],4]",
            "",
            "[9]",
            "[[8,7,6]]",
            "",
            "[[4,4],4,4]",
            "[[4,4],4,4,4]",
            "",
            "[7,7,7,7]",
            "[7,7,7]",
            "",
            "[]",
            "[3]",
            "",
            "[[[]]]",
            "[[]]",
            "",
            "[1,[2,[3,[4,[5,6,7]]]],8,9]",
            "[1,[2,[3,[4,[5,6,0]]]],8,9]"
        };
    }

    private enum CompareResult
    {
        Equal,
        Ordered,
        NotOrdered
    }
    
    private static CompareResult IsOrdered(Element left, Element right)
    {
        if (left is ValueElement leftValue)
        {
            if (right is ValueElement rightValue)
            {
                if (leftValue.Value < rightValue.Value)
                    return CompareResult.Ordered;
                if (leftValue.Value > rightValue.Value)
                    return CompareResult.NotOrdered;

                return CompareResult.Equal;
            }

            return IsOrdered(leftValue.ToListElement(), (ListElement)right);
        }
        
        else
        {
            var leftList = (ListElement)left;
            if (right is ListElement rightList)
            {
                var result = CompareResult.Equal;
                for (var index = 0; index < leftList.Elements.Count && result == CompareResult.Equal; index++)
                {
                    if (index >= rightList.Elements.Count)
                        return CompareResult.NotOrdered;

                    result = IsOrdered(leftList.Elements[index], rightList.Elements[index]);
                }

                if (result == CompareResult.Equal && leftList.Elements.Count < rightList.Elements.Count)
                    // Left side ran out of elements.
                    return CompareResult.Ordered;
                
                return result;
            }

            return IsOrdered(leftList, ((ValueElement)right).ToListElement());
        }
    }

    private Element Parse(string raw)
    {
        var parentElement = new ListElement();
        var listMatch = ListRegex().Match(raw);
        if (listMatch.Success)
        {
            Parse(parentElement, listMatch.Groups[1].Value);
        }

        return parentElement;
    }

    private void Parse(ListElement parent, string raw)
    {
        while (raw.Length > 0)
        {
            var valueMatch = ValueRegex().Match(raw);
            if (valueMatch.Success)
            {
                parent.AddElement(new ValueElement(Convert.ToInt32(valueMatch.Groups["value"].Value)));
                raw = valueMatch.Groups["rest"].Value;
                continue;
            }

            ParseList(raw, out var list, out var rest);
            var newListElement = new ListElement();
            Parse(newListElement, list);
            parent.AddElement(newListElement);
            raw = rest;
        }
    }

    private static void ParseList(string raw, out string list, out string rest)
    {
        var bracketCount = 0;
        var index = 0;
        for (index = 0; index < raw.Length; index++)
        {
            if (raw[index] == '[')
                bracketCount++;
            else if (raw[index] == ']')
            {
                bracketCount--;
                if (bracketCount == 0)
                {
                    break;                    
                }
            }
        }

        list = raw.Substring(1, index-1);
        rest = raw.Substring(index + 1, raw.Length - index - 1);
        if (rest.Length > 0 && rest[0] == ',')
        {
            rest = rest.Substring(1, rest.Length - 1);
        }
    }

    private abstract class Element
    {
    }

    private class ValueElement : Element
    {
        public int Value { get; }

        public ValueElement(int value)
        {
            Value = value;
        }

        public ListElement ToListElement()
        {
            return new ListElement(new Element[]{this});
        }
    }

    private class ListElement : Element
    {
        public IList<Element> Elements { get; }

        public ListElement()
        {
            Elements = new List<Element>();
        }

        public ListElement(IList<Element> elements)
        {
            Elements = elements;
        }

        public void AddElement(Element childElement)
        {
            Elements.Add(childElement);
        }
    }

    [GeneratedRegex("\\[(.*)\\]$", RegexOptions.Compiled)]
    private static partial Regex ListRegex();

    [GeneratedRegex(@"^(?<value>\d+),?(?<rest>.*)", RegexOptions.Compiled)]
    private static partial Regex ValueRegex();
}