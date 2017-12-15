namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public abstract class BaseDay : IDay
    {
        public abstract string Part1(string input);

        public abstract string Part2(string input);

        protected string ReadFileToEnd(string input)
        {
            using (StreamReader streamReader = new StreamReader($"../../Inputs/{input}"))
            {
                return streamReader.ReadToEnd().Replace("\r", string.Empty).Replace("\n", string.Empty);
            }
        }

        protected IEnumerable<T> ImportLines<T>(string input, Func<string, T> parseLine)
        {
            using (StreamReader sr = new StreamReader($"../../Inputs/{input}"))
            {
                while (!sr.EndOfStream)
                {
                    yield return parseLine(sr.ReadLine());
                }
            }
        }
    }
}