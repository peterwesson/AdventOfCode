namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day9 : BaseDay
    {
        public override string Part1(string fileName)
        {
            return FindGroups(RemoveGarbage(Reduce(base.ReadFileToEnd(fileName)))).First().GetTotalScore().ToString();
        }

        public override string Part2(string fileName)
        {
            return RemoveGarbageCount(Reduce(base.ReadFileToEnd(fileName))).ToString();
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
 
            foreach (char t in input)
            {
                if (t == '<')
                {
                    inGarbage = true;
                }

                if (!inGarbage)
                {
                    output += t;
                }

                if (t == '>')
                {
                    inGarbage = false;
                }
            }

            return output;
        }

        private static int RemoveGarbageCount(string input)
        {
            bool inGarbage = false;
            int garbageStart = 0;

            int garbageCount = 0;

            for (int index = 0; index < input.Length; index++)
            {
                switch (input[index])
                {
                    case '<':
                        if (!inGarbage)
                        {
                            garbageStart = index + 1;
                        }

                        inGarbage = true;
                        break;
                    case '>':
                        inGarbage = false;
                        garbageCount += index - garbageStart;
                        break;
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
                switch (input[index])
                {
                    case '{':
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
                        break;
                    case '}':
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
                        break;
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