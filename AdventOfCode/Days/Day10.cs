namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day10 : BaseDay
    {
        public override string Part1(string input)
        {
            List<int> lengths = input.Split(',').Select(int.Parse).ToList();
            List<int> numbers = Enumerable.Range(0, 256).ToList();

            int currentPosition = 0;
            int skipSize = 0;

            numbers = KnotHashRound(numbers, lengths, ref currentPosition, ref skipSize);

            return (numbers[0] * numbers[1]).ToString();
        }

        public override string Part2(string input)
        {
            List<int> lengths = input.Select(c => (int)c).ToList();
            lengths.AddRange(new List<int> { 17, 31, 73, 47, 23 });

            List<int> numbers = Enumerable.Range(0, 256).ToList();

            int currentPosition = 0;
            int skipSize = 0;

            for (int i = 0; i < 64; i++)
            {
                numbers = KnotHashRound(numbers, lengths, ref currentPosition, ref skipSize);
            }

            string output = string.Empty;

            for (int index = 0; index < numbers.Count; index += 16)
            {
                output += numbers.Skip(index).Take(16).Aggregate((x, y) => x ^ y).ToString("x2");
            }

            return output;
        }

        private static List<int> KnotHashRound(List<int> numbers, IEnumerable<int> lengths, ref int currentPosition, ref int skipSize)
        {
            foreach (int length in lengths)
            {
                List<int> offset = CircularOffset(numbers, currentPosition).ToList();
                List<int> reversed = new List<int>();
                reversed.AddRange(offset.Take(length).Reverse());
                reversed.AddRange(offset.Skip(length));
                numbers = CircularOffset(reversed, -currentPosition).ToList();
                currentPosition = (currentPosition + length + skipSize++ + numbers.Count) % numbers.Count;
            }

            return numbers;
        }

        private static IEnumerable<int> CircularOffset(IReadOnlyList<int> input, int offset)
        {
            int index = (offset + input.Count) % input.Count;

            do
            {
                yield return input[index];
                index = (index + 1 + input.Count) % input.Count;
            } while (index != (offset + input.Count) % input.Count);
        }
    }
}
