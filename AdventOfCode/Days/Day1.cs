namespace AdventOfCode.Days
{
    using System;
    using System.Linq;

    public class Day1 : BaseDay
    {
        public override string Part1(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);
            return Sum(input, (c, i) => c == input[(i + 1) % input.Length]).ToString();
        }

        public override string Part2(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);
            return Sum(input, (c, i) => c == input[(i + input.Length / 2) % input.Length]).ToString();
        }

        private static int Sum(string input, Func<char, int, bool> filter)
        {
            return input.Where(filter).Sum(c => int.Parse(c.ToString()));
        }
    }
}