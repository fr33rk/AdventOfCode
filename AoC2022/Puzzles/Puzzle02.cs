using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle02 : BasePuzzle
{
    private enum Shape
    {
        Scissor,
        Paper,
        Rock
    }

    private enum Outcome
    {
        Win, 
        Lose,
        Draw
    }
    
    protected override long SolvePart1(IEnumerable<string> input)
    {
        var answer = input.Select(x =>
            {
                var round = x.Split(' ');
                return new
                {
                    Oppenent = ToShape(round[0]),
                    Me = ToShape(round[1])
                };
            })
            .Select(round => Score(round.Oppenent, round.Me))
            .Sum();

        return answer;
    }

    protected override long SolvePart2(IEnumerable<string> input)
    {
        var answer = input.Select(x =>
            {
                var round = x.Split(' ');
                return new
                {
                    Oppenent = ToShape(round[0]),
                    Me = FromOutCome(ToShape(round[0]), ToOutcome(round[1]))
                };
            })
            .Select(round => Score(round.Oppenent, round.Me))
            .Sum();

        return answer;
    }

    private static Shape ToShape(string s)
    {
        return s switch
        {
            "A" or "X" => Shape.Rock,
            "B" or "Y" => Shape.Paper,
            "C" or "Z" => Shape.Scissor,
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }

    private static Outcome ToOutcome(string s)
    {
        return s switch
        {
            "X" => Outcome.Lose,
            "Y" => Outcome.Draw,
            "Z" => Outcome.Win,
            _ => throw new ArgumentOutOfRangeException(nameof(s), s, null)
        };
    }
    
    private static Shape FromOutCome(Shape opponent, Outcome outcome)
    {
        return opponent switch
        {
            Shape.Paper => outcome switch
            {
                Outcome.Win => Shape.Scissor,
                Outcome.Lose => Shape.Rock,
                Outcome.Draw => Shape.Paper,
                _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
            },
            Shape.Rock => outcome switch
            {
                Outcome.Win => Shape.Paper,
                Outcome.Lose => Shape.Scissor,
                Outcome.Draw => Shape.Rock,
                _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
            },
            Shape.Scissor => outcome switch
            {
                Outcome.Win => Shape.Rock,
                Outcome.Lose => Shape.Paper,
                Outcome.Draw => Shape.Scissor,
                _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(opponent), opponent, null)
        };
    }
    
    private static int Score(Shape opponent, Shape me)
    {
        var score = me switch
        {
            Shape.Scissor => 3,
            Shape.Paper => 2,
            Shape.Rock => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(me), me, null)
        };

        if (opponent == me)
        {
            score += 3;
        }
        else if ((opponent == Shape.Paper && me == Shape.Scissor)
                 || (opponent == Shape.Rock && me == Shape.Paper)
                 || (opponent == Shape.Scissor && me == Shape.Rock))
        {
            score += 6;
        }

        return score;
    }
    
    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "A Y",
            "B X",
            "C Z"
        };
    }
}