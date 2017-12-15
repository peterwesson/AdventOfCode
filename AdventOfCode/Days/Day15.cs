namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day15 : BaseDay
    {
        public override string Part1(string input)
        {
            IEnumerable<long> sequenceA = Results(679, 16807, 40000000);
            IEnumerable<long> sequenceB = Results(771, 48271, 40000000);

            return SequenceCompare(sequenceA, sequenceB).Count().ToString();
        }

        public override string Part2(string input)
        {
            IEnumerable<long> sequenceA = Results(679, 16807, 5000000, 4);
            IEnumerable<long> sequenceB = Results(771, 48271, 5000000, 8);

            return SequenceCompare(sequenceA, sequenceB).Count().ToString();
        }

        private static IEnumerable<long> Results(int startValue, int factor, int take, int divisor = 1)
        {
            return GenerateSequence(startValue, factor).Where(x => x % divisor == 0).Take(take).Select(i => i & 0xffff);
        }

        private static IEnumerable<long> GenerateSequence(int startValue, int factor)
        {
            long value = startValue;

            while (true)
            {
                yield return value = (value * factor + 2147483647) % 2147483647;
            }
        }

        private static IEnumerable<long> SequenceCompare(IEnumerable<long> sequenceA, IEnumerable<long> sequenceB)
        {
            using (IEnumerator<long> enumeratorA = sequenceA.GetEnumerator())
            using (IEnumerator<long> enumeratorB = sequenceB.GetEnumerator())
            {
                while (enumeratorA.MoveNext() && enumeratorB.MoveNext())
                {
                    if (enumeratorA.Current == enumeratorB.Current)
                    {
                        yield return enumeratorA.Current;
                    }
                }
            }
        }
    }
}
