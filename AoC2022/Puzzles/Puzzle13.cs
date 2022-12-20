using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public partial class Puzzle13 : BasePuzzle
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

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var packets = input
            .Where(s => s.Length > 0)
            .Append("[[2]]")
            .Append("[[6]]")
            .Select(Parse)
            .OrderBy(p => p, new PacketComparer())
            .ToList();

        var two = packets.Single(element => element is ListElement { Elements: [ListElement { Elements: [ValueElement { Value: 2 }] } inner] } outer);
        var six = packets.Single(element => element is ListElement { Elements: [ListElement { Elements: [ValueElement { Value: 6 }] } inner] } outer);

        return ((packets.IndexOf(two) + 1) * (packets.IndexOf(six) + 1)).ToString();
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

    public enum CompareResult
    {
        Equal,
        Ordered,
        NotOrdered
    }
    
    public static CompareResult IsOrdered(Element left, Element right)
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

    [GeneratedRegex("\\[(.*)\\]$", RegexOptions.Compiled)]
    private static partial Regex ListRegex();

    [GeneratedRegex(@"^(?<value>\d+),?(?<rest>.*)", RegexOptions.Compiled)]
    private static partial Regex ValueRegex();
}

public abstract class Element
{
}

public class ValueElement : Element
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

public class ListElement : Element
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

public class PacketComparer : IComparer<Element>
{
    public int Compare(Element? x, Element? y)
    {
        if (x == null || y == null) 
            throw new InvalidOperationException("Comparing using null");

        var result = Puzzle13.IsOrdered(x, y);
        return result switch
        {
            Puzzle13.CompareResult.Ordered => -1,
            Puzzle13.CompareResult.Equal => 0,
            Puzzle13.CompareResult.NotOrdered => 1,
            _ => throw new ArgumentOutOfRangeException()
        };

    }
}