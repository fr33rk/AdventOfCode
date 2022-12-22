using MoreLinq;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public class Puzzle12 : BasePuzzle
{
  
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var map = input
            .Select((line, y) => line
                .Select((height, x) => ToPosition(x, y, height))
                .ToArray())
            .ToArray();

        return AStar(map).ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var map = input
            .Select((line, y) => line
                .Select((height, x) => ToPosition(x, y, height))
                .ToArray())
            .ToArray();
        
        // No need for smart... took long enough, use force
        var possibleStarts = map.SelectMany(x => x).Where(pos => pos.Height == 'a');

        var shortestPath = int.MaxValue;
        foreach (var possibleStart in possibleStarts)
        {
            map.SelectMany(x => x).ForEach(pos => pos.Reset());
            possibleStart.IsStart = true;
            var foundLength = AStar(map);
            if (foundLength > -1 && foundLength < shortestPath)
                shortestPath = foundLength;
        }
        
        return shortestPath.ToString();
    }
    
    private static void PrintMap(Position[][] map)
    {
        Console.WriteLine();
        for (var y = 0; y < map.Length; y++)
        {
            for (var x = 0; x < map[y].Length; x++)
            {
                map[y][x].ToConsole();
            }
            Console.WriteLine();
        }
    }
    
    private Position ToPosition(int x, int y, char c)
    {
        return c switch
        {
            'S' => Position.CreateStart(x,y),
            'E' => Position.CreateFinish(x,y),
            _ => Position.Create(x, y, c)
        };
    }

    private enum EvaluationState
    {
        Open,
        Closed,
        Undiscovered,
        PartOfRoute
    }
    
    private class Position
    {
        private char _AsChar;
        
        public int Height { get; }
        
        public bool IsStart { get; set; }
        
        public bool IsFinish { get; }
        
        public EvaluationState EvaluationState { get; set; }
        
        public int X { get; }
        public int Y { get; }
        
        public int GScore { get; set; }

        public int FScore { get; set; }
        
        public Position? CameFrom { get; set; }
        
        private Position(int x, int y, int height, bool isStart, bool isFinish)
        {
            Height = height;
            IsStart = isStart;
            IsFinish = isFinish;
            X = x;
            Y = y;
            EvaluationState = EvaluationState.Undiscovered;
            GScore = int.MaxValue;
            
            if (isFinish)
                _AsChar = 'E';
            else if (isStart)
                _AsChar = 'S';
            else
                _AsChar = (char)height;
        }

        public static Position CreateStart(int x, int y)
        {
            return new Position(x, y,'a', true, false);
        }
        
        public static Position CreateFinish(int x, int y)
        {
            return new Position(x, y, 'z', false, true);
        }
        
        public static Position Create(int x, int y, int height)
        {
            return new Position(x, y, height, false, false);
        }

        public void Reset()
        {
            IsStart = false;  
            EvaluationState = EvaluationState.Undiscovered;
            GScore = int.MaxValue;
            CameFrom = null;
        }
        
        public void MarkAsRoute()
        {
            EvaluationState = EvaluationState.PartOfRoute;
            CameFrom?.MarkAsRoute();
        }
        
        public void ToConsole()
        {
            var savedColor = Console.BackgroundColor;
            Console.BackgroundColor = EvaluationState switch
            {
                EvaluationState.Closed => ConsoleColor.Red,
                EvaluationState.Open => ConsoleColor.Cyan,
                EvaluationState.PartOfRoute => ConsoleColor.Green,
                _ => savedColor
            };
            
            Console.Write(_AsChar);

            Console.BackgroundColor = savedColor;
        }
    }
    
    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "Sabqponm",
            "abcryxxl",
            "accszExk",
            "acctuvwj",
            "abdefghi"
        };
    }

    private int AStar(Position[][] map)
    {
        var flattenedMap = map.SelectMany(x => x).ToList();

        var goal = flattenedMap.Single(pos => pos.IsFinish);
        
        // Add the start position to the open list.
        var start = flattenedMap.Single(pos => pos.IsStart);
        start.EvaluationState = EvaluationState.Open;
        start.GScore = 0;
        start.FScore = CalculateHeuristicScore(start, goal);
        
        var openList = flattenedMap.Where(pos => pos.EvaluationState == EvaluationState.Open).ToList();
        
        while (openList.Any())
        {
            var current = openList.OrderBy(pos => pos.FScore).First();
            if (current == goal)
            {
                //current.MarkAsRoute();
                //PrintMap(map);
                return current.GScore;
            }

            current.EvaluationState = EvaluationState.Closed;

            foreach (var neighbour in GetNeighbours(map, current))
            {
                var tentativeGScore = current.GScore + 1;
                if (tentativeGScore < neighbour.GScore)
                {
                    // This path to neighbor is better than any previous one. Record it!
                    neighbour.CameFrom = current;
                    neighbour.GScore = tentativeGScore;
                    neighbour.FScore = neighbour.GScore + CalculateHeuristicScore(neighbour, goal);
                    neighbour.EvaluationState = EvaluationState.Open;
                }
            }
            
            openList = flattenedMap.Where(pos => pos.EvaluationState == EvaluationState.Open).ToList();
        }
        
        // Open set is empty but goal was never reached
        return -1;
    }

    private IEnumerable<Position> GetNeighbours(Position[][] map, Position position)
    {
        // North
        if (position.Y > 0 // Not on the edge
            && map[position.Y - 1][position.X].Height - position.Height <= 1)
            yield return map[position.Y - 1][position.X];
        
        // West
        if (position.X < map[0].Length - 1 // Not on the edge
            && map[position.Y][position.X + 1].Height - position.Height <= 1)
            yield return map[position.Y][position.X+1];
        
        // South
        if (position.Y < map.Length - 1 // Not on the edge
            && map[position.Y + 1][position.X].Height - position.Height <= 1)
            yield return map[position.Y + 1][position.X];
        
        // East
        if (position.X > 0 // Not on the edge
            && map[position.Y][position.X - 1].Height - position.Height <= 1)
            yield return map[position.Y][position.X - 1];
    }

    private int CalculateHeuristicScore(Position position, Position endPosition)
    {
        return Math.Abs(position.X - endPosition.X)
               + Math.Abs(position.Y - endPosition.Y);
    }
}