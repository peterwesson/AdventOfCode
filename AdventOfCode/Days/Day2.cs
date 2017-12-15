namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day2 : BaseDay
    {
        public override string Part1(string fileName)
        {
            return GetLines(fileName).Sum(line => line.Max() - line.Min()).ToString();
        }

        public override string Part2(string fileName)
        {
            return GetLines(fileName).Sum(line => (from i in line from j in line where i != j where i % j == 0 select i / j).FirstOrDefault()).ToString();
        }

        private IEnumerable<List<int>> GetLines(string input)
        {
            return base.ImportLines(input, line => line.Split('\t').Select(int.Parse).ToList());
        }
    }
}