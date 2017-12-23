namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day23 : BaseDay
    {
        public override string Part1(string fileName)
        {
            List<string> instructions = base.ImportLines(fileName, line => line).ToList();

            Program program = new Program(instructions);

            bool loop = true;
            int mulCount = 0;

            while (loop)
            {
                if (program.ProgramCounter < instructions.Count && instructions[program.ProgramCounter].StartsWith("mul"))
                {
                    mulCount++;
                }

                loop = program.ProcessInstruction();
            }

            return mulCount.ToString();
        }

        public override string Part2(string fileName)
        {
            int h = 0;

            for (int b = 107900; b <= 124900; b += 17)
            {
                if (!IsPrime(b))
                {
                    h++;
                }
            }

            return h.ToString();
        }

        private static bool IsPrime(int b)
        {
            for (int d = 2; d <= (int)Math.Sqrt(b); d++)
            {
                if (b % d == 0)
                {
                    return false;
                }
            }

            return true;
        }


        private class Program
        {
            public Program(List<string> instructions, bool debug = false)
            {
                this.registers = this.CreateEmptyRegisters(debug);
                this.instructions = instructions;
            }

            private readonly Dictionary<char, long> registers;

            private readonly List<string> instructions;

            public int ProgramCounter { get; private set; }
            
            public bool ProcessInstruction()
            {
                if (this.ProgramCounter < 0 || this.ProgramCounter >= this.instructions.Count)
                {
                    return false;
                }

                List<string> parts = this.instructions[this.ProgramCounter].Split(' ').ToList();

                this.ProgramCounter++;

                switch (parts[0])
                {
                    case "set":
                        this.registers[parts[1][0]] = this.GetValue(parts[2]);

                        if (parts[1][0] == 'h')
                        {
                            Console.WriteLine(this.GetValue("h"));
                        }
                        break;
                    case "sub":
                        this.registers[parts[1][0]] -= this.GetValue(parts[2]);

                        if (parts[1][0] == 'h')
                        {
                            Console.WriteLine(this.GetValue("h"));
                        }
                        break;
                    case "mul":
                        this.registers[parts[1][0]] *= this.GetValue(parts[2]);

                        if (parts[1][0] == 'h')
                        {
                            Console.WriteLine(this.GetValue("h"));
                        }
                        break;
                    case "jnz":
                        if (this.GetValue(parts[1]) != 0)
                        {
                            this.ProgramCounter += (int)this.GetValue(parts[2]) - 1;
                        }
                        break;
                }

                return true;
            }

            private Dictionary<char, long> CreateEmptyRegisters(bool debug)
            {
                Dictionary<char, long> dictionary = new Dictionary<char, long>();

                for (char register = 'a'; register <= 'h'; register++)
                {
                    dictionary[register] = 0;
                }

                if (debug)
                {
                    dictionary['a'] = 1;
                }

                return dictionary;
            }

            public long GetValue(string input)
            {
                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                return this.registers[input[0]];
            }
        }
    }
}