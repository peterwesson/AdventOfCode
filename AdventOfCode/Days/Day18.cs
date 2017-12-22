namespace AdventOfCode.Days
{
    using System.Linq;
    using System.Collections.Generic;

    public class Day18 : BaseDay
    {
        public override string Part1(string input)
        {
            Dictionary<char, long> registers = new Dictionary<char, long>();

            for (char register = 'a'; register <= 'z'; register++)
            {
                registers[register] = 0;
            }

            List<string> instructions = base.ImportLines(input, line => line).ToList();

            bool finished = false;
            int pc = 0;
            long value = 0;

            while (!finished)
            {
                value = ParseInstruction(instructions[pc], registers, out int jump);

                if (value != 0)
                {
                    finished = true;
                }

                pc += jump;
            }

            return value.ToString();
        }

        public override string Part2(string input)
        {
            List<string> instructions = base.ImportLines(input, line => line).ToList();

            Queue<long> queue1 = new Queue<long>();
            Queue<long> queue2 = new Queue<long>();

            Program program0 = new Program(0, queue1, queue2, instructions);
            Program program1 = new Program(1, queue2, queue1, instructions);

            bool program0IsRunning = true;
            bool program1IsRunning = true;

            while (program0IsRunning || program1IsRunning)
            {
                program0IsRunning = program0.ProcessInstruction();
                program1IsRunning = program1.ProcessInstruction();
            }

            return program1.SendCount.ToString();
        }  

        private class Program
        {
            public Program(int programId, Queue<long> inputQueue, Queue<long> outputQueue, List<string> instructions)
            {
                this.registers = this.CreateEmptyRegisters();
                this.inputQueue = inputQueue;
                this.outputQueue = outputQueue;
                this.instructions = instructions;

                this.registers['p'] = programId;
            }

            private readonly Dictionary<char, long> registers;

            private readonly Queue<long> inputQueue;

            private readonly Queue<long> outputQueue;

            private readonly List<string> instructions;

            private int programCounter;

            public int SendCount { get; private set; }

            private int deadlockCounter;

            public bool ProcessInstruction()
            {
                if (this.programCounter < 0 || this.programCounter >= this.instructions.Count)
                {
                    return false;
                }

                List<string> parts = this.instructions[this.programCounter].Split(' ').ToList();

                this.programCounter++;

                switch (parts[0])
                {
                    case "snd":
                        this.outputQueue.Enqueue(this.GetValue(parts[1]));
                        this.SendCount++;
                        this.deadlockCounter = 0;
                        break;
                    case "set":
                        this.registers[parts[1][0]] = this.GetValue(parts[2]);
                        break;
                    case "add":
                        this.registers[parts[1][0]] += this.GetValue(parts[2]);
                        break;
                    case "mul":
                        this.registers[parts[1][0]] *= this.GetValue(parts[2]);
                        break;
                    case "mod":
                        this.registers[parts[1][0]] %= this.GetValue(parts[2]);
                        break;
                    case "rcv":
                        if (inputQueue.Count > 0)
                        {
                            this.registers[parts[1][0]] = this.inputQueue.Dequeue();
                        }
                        else
                        {
                            this.programCounter--;
                            this.deadlockCounter++;

                            if (deadlockCounter > 1000000)
                            {
                                return false;
                            }
                        }

                        break;
                    case "jgz":
                        if (this.GetValue(parts[1]) > 0)
                        {
                            this.programCounter += (int)this.GetValue(parts[2]) - 1;
                        }
                        break;                        
                }

                return true;
            }

            private Dictionary<char, long> CreateEmptyRegisters()
            {
                Dictionary<char, long> dictionary = new Dictionary<char, long>();

                for (char register = 'a'; register <= 'z'; register++)
                {
                    dictionary[register] = 0;
                }

                return dictionary;
            }

            private long GetValue(string input)
            {
                if (int.TryParse(input, out int result))
                {
                    return result;
                }

                return this.registers[input[0]];
            }
        }



        private static long ParseInstruction(string line, Dictionary<char, long> registers, out int jump)
        {
            List<string> parts = line.Split(' ').ToList();

            jump = 1;

            switch (parts[0])
            {
                case "snd":
                    registers['*'] = GetValue(parts[1], registers);
                    break;
                case "set":
                    registers[parts[1][0]] = GetValue(parts[2], registers);
                    break;
                case "add":
                    registers[parts[1][0]] += GetValue(parts[2], registers);
                    break;
                case "mul":
                    registers[parts[1][0]] *= GetValue(parts[2], registers);
                    break;
                case "mod":
                    registers[parts[1][0]] %= GetValue(parts[2], registers);
                    break;
                case "rcv":
                    if (GetValue(parts[1], registers) != 0)
                    {
                        return registers['*'];
                    }
                    break;
                case "jgz":
                    if (GetValue(parts[1], registers) > 0)
                    {
                        jump = (int)GetValue(parts[2], registers);
                    }
                    break;
            }

            return 0;
        }

        private static long GetValue(string input, Dictionary<char, long> registers)
        {
            if (int.TryParse(input, out int result))
            {
                return result;
            }

            return registers[input[0]];
        }
    }
}