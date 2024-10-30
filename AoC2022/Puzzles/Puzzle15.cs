using System.Text.RegularExpressions;
using PuzzleSolver.Core;

namespace AoC2022.Puzzles;

public partial class Puzzle15 : BasePuzzle
{
    protected override string SolvePart1(IEnumerable<string> input)
    {
        var points = ProcessInput(input);

        // Test and real input have a different line of interest.
        var lineOfInterest = points.Count == 14 ? 10 : 2000000;

        var answer = points
            .Select(point => GetCoveredPositions(point, lineOfInterest))
            .SelectMany(x => x)
            .GroupBy(position => position.X)
            .Count(group => !group.Any(position => position.HasBeacon));

        return answer.ToString();
    }

    protected override string SolvePart2(IEnumerable<string> input)
    {
        var points = ProcessInput(input);


        return "Fail";
    }

    protected override IEnumerable<string> GetTestInput()
    {
        return new[]
        {
            "Sensor at x=2, y=18: closest beacon is at x=-2, y=15",
            "Sensor at x=9, y=16: closest beacon is at x=10, y=16",
            "Sensor at x=13, y=2: closest beacon is at x=15, y=3",
            "Sensor at x=12, y=14: closest beacon is at x=10, y=16",
            "Sensor at x=10, y=20: closest beacon is at x=10, y=16",
            "Sensor at x=14, y=17: closest beacon is at x=10, y=16",
            "Sensor at x=8, y=7: closest beacon is at x=2, y=10",
            "Sensor at x=2, y=0: closest beacon is at x=2, y=10",
            "Sensor at x=0, y=11: closest beacon is at x=2, y=10",
            "Sensor at x=20, y=14: closest beacon is at x=25, y=17",
            "Sensor at x=17, y=20: closest beacon is at x=21, y=22",
            "Sensor at x=16, y=7: closest beacon is at x=15, y=3",
            "Sensor at x=14, y=3: closest beacon is at x=15, y=3",
            "Sensor at x=20, y=1: closest beacon is at x=15, y=3"
        };
    }

    private static List<SensorBeaconPair> ProcessInput(IEnumerable<string> input)
    {
        var pointsRegex = PointsRegex();

        return input
            .Select(line => pointsRegex.Match(line).Groups)
            .Select(groups => new SensorBeaconPair(
                    new Coordinate(
                        x: Convert.ToInt32(groups["sx"].Value),
                        y: Convert.ToInt32(groups["sy"].Value)),
                    new Coordinate(
                        x: Convert.ToInt32(groups["bx"].Value),
                        y: Convert.ToInt32(groups["by"].Value))
                )
            )
            .ToList();
    }

    private IEnumerable<CoveredPosition> GetCoveredPositions(SensorBeaconPair sensorBeaconPair, int lineOfInterest)
    {
        var radius = Math.Abs(sensorBeaconPair.Beacon.X - sensorBeaconPair.Sensor.X)
                   + Math.Abs(sensorBeaconPair.Beacon.Y - sensorBeaconPair.Sensor.Y);

        var distanceToLineOfInterest = 0;

        if (lineOfInterest < sensorBeaconPair.Sensor.Y)
        {
            if (lineOfInterest < sensorBeaconPair.Sensor.Y - radius)
                yield break;

            distanceToLineOfInterest = sensorBeaconPair.Sensor.Y - lineOfInterest;
        }
        else if (lineOfInterest > sensorBeaconPair.Sensor.Y)
        {
            if (lineOfInterest > sensorBeaconPair.Sensor.Y + radius)
                yield break;

            distanceToLineOfInterest = lineOfInterest - sensorBeaconPair.Sensor.Y;
        }

        var radiusAtLineOfInterest = radius - distanceToLineOfInterest;
        var offSet = sensorBeaconPair.Sensor.X - radiusAtLineOfInterest;
        var width = radiusAtLineOfInterest * 2 + 1;

        for (var x = 0; x < width; x++)
        {
            yield return new CoveredPosition(x + offSet, sensorBeaconPair.Beacon.Y == lineOfInterest && sensorBeaconPair.Beacon.X == x + offSet);
        }

    }

    [GeneratedRegex(@".*?x=(?<sx>\-?\d+), y=(?<sy>\-?\d+).*?x=(?<bx>\-?\d+), y=(?<by>\-?\d+)", RegexOptions.Compiled)]
    private static partial Regex PointsRegex();

    private record SensorBeaconPair(Coordinate Sensor, Coordinate Beacon);

    private record CoveredPosition(int X, bool HasBeacon);
}