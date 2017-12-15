namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day14 : BaseDay
    {
        public override string Part1(string input)
        {
            bool[,] grid = Grid(input);

            int count = 0;


            for (int row = 0; row < 128; row++)
            {
                for (int column = 0; column < 128; column++)
                {
                    if (grid[row, column])
                    {
                        count++;
                    }
                }
            }

            return count.ToString();
        }

        public override string Part2(string input)
        {
            bool[,] grid = Grid(input);

            return NumberOfRegions(grid).ToString();
        }
        private static bool[,] Grid(string input)
        {
            bool[,] grid = new bool[128, 128];

            for (int row = 0; row < 128; row++)
            {
                List<bool> result = HexToBinary(KnotHash($"{input}-{row}")).ToList();

                for (int column = 0; column < 128; column++)
                {
                    grid[row, column] = result[column];
                }
            }

            return grid;
        }

        private static IEnumerable<bool> HexToBinary(string input)
        {
            return string.Join(string.Empty, input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))).Select(c => c == '1');
        }

        private static string KnotHash(string input)
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

        private static int NumberOfRegions(bool[,] input)
        {
            int[,] grid = CreateRegions(input, out int regionCount);

            for (int row = 0; row < 128; row++)
            {
                for (int column = 0; column < 128; column++)
                {
                    if (row > 0)
                    {
                        if (grid[row, column] != grid[row - 1, column] &&
                            grid[row, column] != 0 &&
                            grid[row - 1, column] != 0)
                        {
                            MergeGroups(grid, grid[row, column], grid[row - 1, column]);
                            regionCount--;
                        }
                    }

                    if (column > 0)
                    {
                        if (grid[row, column] != grid[row, column - 1] &&
                            grid[row, column] != 0 &&
                            grid[row, column - 1] != 0)
                        {
                            MergeGroups(grid, grid[row, column], grid[row, column - 1]);
                            regionCount--;
                        }
                    }
                }
            }

            return regionCount;
        }

        private static void MergeGroups(int[,] grid, int group1, int group2)
        {
            for (int row = 0; row < 128; row++)
            {
                for (int column = 0; column < 128; column++)
                {
                    if (grid[row, column] == group2)
                    {
                        grid[row, column] = group1;
                    }
                }
            }
        }

        private static int[,] CreateRegions(bool[,] input, out int regionCount)
        {
            int[,] grid = new int[128, 128];

            regionCount = 0;

            for (int row = 0; row < 128; row++)
            {
                for (int column = 0; column < 128; column++)
                {
                    if (input[row, column])
                    {
                        grid[row, column] = ++regionCount;
                    }
                }
            }

            return grid;
        }
    }
}
