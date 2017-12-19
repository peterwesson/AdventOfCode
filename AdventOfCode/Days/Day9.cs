namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day9 : BaseDay
    {
        public override string Part1(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);

            string reduced = Reduce(input);
            string garbageRemoved = RemoveGarbage(reduced);

            List<Group> groups = FindGroups(garbageRemoved);

            return groups.First().GetTotalScore().ToString();
        }

        public override string Part2(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);

            string reduced = Reduce(input);
            return RemoveGarbageCount(reduced).ToString();
        }

        private static string Reduce(string input)
        {
            string output = string.Empty;

            for (int index = 0; index < input.Length; index++)
            {
                if (input[index] == '!')
                {
                    index++;
                }
                else
                {
                    output += input[index];
                }
            }

            return output;
        }

        private static string RemoveGarbage(string input)
        {
            string output = string.Empty;

            bool inGarbage = false;
            int garbageStart = 0;

            int garbageCount = 0;

            for (int index = 0; index < input.Length; index++)
            {
                if (input[index] == '<')
                {
                    if (!inGarbage)
                    {
                        garbageStart = index + 1;
                    }

                    inGarbage = true;
                }

                if (!inGarbage)
                {
                    output += input[index];
                }

                if (input[index] == '>')
                {
                    inGarbage = false;
                    garbageCount += index - garbageStart;
                }
            }

            return output;
        }

        private static int RemoveGarbageCount(string input)
        {
            string output = string.Empty;

            bool inGarbage = false;
            int garbageStart = 0;

            int garbageCount = 0;

            for (int index = 0; index < input.Length; index++)
            {
                if (input[index] == '<')
                {
                    if (!inGarbage)
                    {
                        garbageStart = index + 1;
                    }

                    inGarbage = true;
                }

                if (!inGarbage)
                {
                    output += input[index];
                }

                if (input[index] == '>')
                {
                    inGarbage = false;
                    garbageCount += index - garbageStart;
                }
            }

            return garbageCount;
        }

        private static List<Group> FindGroups(string input, int level = 1)
        {
            List<Group> groups = new List<Group>();

            bool inGroup = false;

            int groupStartIndex = 0;
            int bracketCount = 0;

            for (int index = 0; index < input.Length; index++)
            {
                if (input[index] == '{')
                {
                    if (inGroup)
                    {
                        bracketCount++;
                    }
                    else
                    {
                        inGroup = true;
                        groupStartIndex = index + 1;
                        bracketCount = 1;
                    }
                }

                if (input[index] == '}')
                {
                    bracketCount--;

                    if (bracketCount == 0)
                    {
                        groups.Add(new Group
                        {
                            Groups = FindGroups(string.Join(string.Empty, input.Skip(groupStartIndex).Take(index - groupStartIndex).Select(c => c.ToString())), level + 1),
                            Score = level
                        });

                        inGroup = false;
                        groupStartIndex = 0;
                    }
                }
            }

            return groups;
        }

        private class Group
        {
            public List<Group> Groups { get; set; }

            public int Score { get; set; }

            public int GetTotalScore() => Score + Groups.Sum(group => group.GetTotalScore());
        }
    }
}