namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day8 : BaseDay
    {
        public override string Part1(string fileName)
        {
            IEnumerable<string> instructions = base.ImportLines(fileName, line => line);

            Dictionary<string, int> registers = new Dictionary<string, int>();

            foreach (string instruction in instructions)
            {
                ParseInstructions(instruction, registers);
            }
                      
            return registers.Max(kvp => kvp.Value).ToString();
        }

        public override string Part2(string fileName)
        {
            IEnumerable<string> instructions = base.ImportLines(fileName, line => line);

            Dictionary<string, int> registers = new Dictionary<string, int>();

            int max = 0;

            foreach (string instruction in instructions)
            {
                ParseInstructions(instruction, registers);

                if (registers.Max(kvp => kvp.Value) > max)
                {
                    max = registers.Max(kvp => kvp.Value);
                }
            }

            return max.ToString();
        }

        private static void ParseInstructions(string instruction, IDictionary<string, int> registers)
        {
            List<string> parts = instruction.Split(' ').ToList();

            int r = GetRegisterValue(registers, parts[0]);

            if (Condition(registers, parts[4], int.Parse(parts[6]), parts[5]))
            {
                switch (parts[1])
                {
                    case "inc":
                        registers[parts[0]] = r + int.Parse(parts[2]);
                        break;
                    case "dec":
                        registers[parts[0]] = r - int.Parse(parts[2]);
                        break;
                }
            }
        }

        private static bool Condition(IDictionary<string, int> registers, string register, int value, string comparison)
        {
            int registerValue = GetRegisterValue(registers, register);

            switch (comparison)
            {
                case "<=": return registerValue <= value;
                case ">=": return registerValue >= value;
                case "!=": return registerValue != value;
                case "==": return registerValue == value;
                case "<": return registerValue < value;
                case ">": return registerValue > value;
                default: throw new ArgumentException();
            }
        }

        private static int GetRegisterValue(IDictionary<string, int> registers, string register)
        {
            if (!registers.ContainsKey(register))
            {
                registers[register] = 0;
            }

            return registers[register];
        }
    }
}