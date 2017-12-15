namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class Day5 : BaseDay
    {
        public override string Part1(string fileName)
        {
            List<int> offsets = GetOffsets(fileName).ToList();

            return GetStep(offsets, x => x + 1).ToString();
        }

        public override string Part2(string fileName)
        {
            List<int> offsets = GetOffsets(fileName).ToList();

            return GetStep(offsets, x => x >= 3 ? x - 1 : x + 1).ToString();
        }

        private static int GetStep(IList<int> offsets, Func<int, int> adjust)
        {
            int index = 0;
            int step = 0;

            while (index < offsets.Count)
            {
                int offset = offsets[index];
                offsets[index] = adjust(offsets[index]);
                index += offset;
                step++;
            }

            return step;
        }

        private IEnumerable<int> GetOffsets(string input)
        {
            return base.ImportLines(input, int.Parse);
        }
    }
}