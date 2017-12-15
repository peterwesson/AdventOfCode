namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Days;

    internal class Program
    {
        private static void Main()
        {
            List<(IDay Day, string Input)> days = new List<(IDay Day, string Input)>
            {
                //(new Day1(), "Day1.txt"),
                //(new Day2(), "Day2.txt"),
                //(new Day4(), "Day4.txt"),
                //(new Day5(), "Day5.txt"),
                //(new Day10(), "94,84,0,79,2,27,81,1,123,93,218,23,103,255,254,243"),
                //(new Day11(), "Day11.txt"),
                //(new Day12(), "Day12.txt"),
                //(new Day13(), "Day13.txt"),
                //(new Day14(), "wenycdww"),
                (new Day15(), null)
            };


            foreach ((IDay Day, string Input) day in days)
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine($"Day {string.Join(string.Empty, day.Day.GetType().Name.Skip(3))}");
                Console.WriteLine($"Part 1: {day.Day.Part1(day.Input)}");
                Console.WriteLine($"Part 2: {day.Day.Part2(day.Input)}");
            }

            Console.WriteLine("---------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
