namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day4 : BaseDay
    {
        public override string Part1(string fileName)
        {
            return CountUnique(GetLines(fileName), word => word).ToString();
        }

        public override string Part2(string fileName)
        {
            return CountUnique(GetLines(fileName), word => string.Concat(word.OrderBy(c => c))).ToString();
        }

        private static int CountUnique(IEnumerable<IEnumerable<string>> lines, Func<string, string> select)
        {
            return lines.Count(line =>
            {
                List<string> enumeratedLine = line.ToList();
                return enumeratedLine.Select(select).Distinct().Count() == enumeratedLine.Count;
            });
        }

        private IEnumerable<IEnumerable<string>> GetLines(string input)
        {
            return base.ImportLines(input, line => line.Split(' '));
        }
    }
}