namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day11 : BaseDay
    {
        public override string Part1(string input)
        {
            List<Direction> directions = base.ReadFileToEnd(input).Split(',').Select(GetDirection).ToList();

            return GetScore(directions).ToString();
        }

        public override string Part2(string input)
        {
            List<Direction> directions = base.ReadFileToEnd(input).Split(',').Select(GetDirection).ToList();

            return Enumerable.Range(1, directions.Count)
                .Select(i => GetScore(directions.Take(i).ToList()))
                .Max()
                .ToString();
        }

        private static Direction GetDirection(string input)
        {
            switch (input)
            {
                case "n": return Direction.North;
                case "ne": return Direction.NorthEast;
                case "se": return Direction.SouthEast;
                case "s": return Direction.South;
                case "sw": return Direction.SouthWest;
                case "nw": return Direction.NorthWest;
                default: return default(Direction);
            }
        }

        private static int GetScore(IReadOnlyCollection<Direction> directions)
        {
            int axis1 = directions.Count(d => d == Direction.North) - directions.Count(d => d == Direction.South);
            int axis2 = directions.Count(d => d == Direction.NorthEast) - directions.Count(d => d == Direction.SouthWest);
            int axis3 = directions.Count(d => d == Direction.NorthWest) - directions.Count(d => d == Direction.SouthEast);

            if (axis2 > 0 && axis3 > 0)
            {
                int min = Math.Min(axis3, axis2);
                axis1 += min;
                axis2 -= min;
                axis3 -= min;
            }

            if (axis1 > 0 && axis3 < 0)
            {
                int min = Math.Min(axis1, -axis3);
                axis1 -= min;
                axis2 += min;
                axis3 += min;
            }

            if (axis1 < 0 && axis2 > 0)
            {
                int min = Math.Min(-axis1, axis2);
                axis1 += min;
                axis2 -= min;
                axis3 -= min;
            }

            if (axis2 < 0 && axis3 < 0)
            {
                int min = Math.Min(-axis3, -axis2);
                axis1 -= min;
                axis2 += min;
                axis3 += min;
            }

            if (axis1 < 0 && axis3 > 0)
            {
                int min = Math.Min(-axis1, axis3);
                axis1 += min;
                axis2 -= min;
                axis3 -= min;
            }

            if (axis1 > 0 && axis2 < 0)
            {
                int min = Math.Min(axis1, -axis2);
                axis1 -= min;
                axis2 += min;
                axis3 += min;
            }

            return Math.Abs(axis1) + Math.Abs(axis2) + Math.Abs(axis3);
        }

        private enum Direction
        {
            North,
            NorthEast,
            SouthEast,
            South,
            SouthWest,
            NorthWest
        }
    }
}
