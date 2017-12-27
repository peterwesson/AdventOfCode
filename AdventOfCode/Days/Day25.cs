namespace AdventOfCode.Days
{
    using System.IO;
    using System.Collections.Generic;
    using System.Linq;

    public class Day25 : BaseDay
    {
        public override string Part1(string fileName)
        {
            BluePrint blueprint = ParseFile(fileName);

            Dictionary<int, int> tape = new Dictionary<int, int>();

            int position = 0;

            for (int i = 0; i < blueprint.StepsRemaining; i++)
            {
                Instruction instruction = blueprint.Instructions.First(x => x.State == blueprint.State);

                int value = GetValue(tape, position);

                if (value == 0)
                {
                    tape[position] = instruction.WriteIf0;
                    position = position + (instruction.DirectionIf0 == Direction.Left ? -1 : +1);
                    blueprint.State = instruction.StateIf0;
                }
                else if (value == 1)
                {
                    tape[position] = instruction.WriteIf1;
                    position = position + (instruction.DirectionIf1 == Direction.Left ? -1 : +1);
                    blueprint.State = instruction.StateIf1;
                }
            }

            return tape.Count(kvp => kvp.Value == 1).ToString();
        }

        public override string Part2(string fileName)
        {
            return "";
        }

        private int GetValue(Dictionary<int, int> tape, int position)
        {
            if (!tape.ContainsKey(position))
            {
                tape[position] = 0;
            }

            return tape[position];
        }

        private class BluePrint
        {
            public char State { get; set; }

            public int StepsRemaining { get; set; }

            public List<Instruction> Instructions = new List<Instruction>();
        }

        private class Instruction
        {
            public char State { get; set; }

            public Direction DirectionIf0 { get; set; }

            public Direction DirectionIf1 { get; set; }

            public int WriteIf0 { get; set; }

            public int WriteIf1 { get; set; }

            public char StateIf0 { get; set; }

            public char StateIf1 { get; set; }
        }

        private enum Direction { Left, Right }

        private BluePrint ParseFile(string fileName)
        {
            BluePrint output = new BluePrint();

            using (StreamReader sr = new StreamReader($"../../Inputs/{fileName}"))
            {
                string initialStateline = sr.ReadLine();
                output.State = initialStateline.Split(' ').Last().First();

                string stepsLine = sr.ReadLine();
                output.StepsRemaining = int.Parse(stepsLine.Split(' ')[5]);

                while (!sr.EndOfStream)
                {
                    Instruction instruction = new Instruction();

                    sr.ReadLine();
                    instruction.State = sr.ReadLine().Split(' ').Last().First();
                    sr.ReadLine();
                    instruction.WriteIf0 = int.Parse(sr.ReadLine().Split(' ').Last().First().ToString());
                    instruction.DirectionIf0 = sr.ReadLine().Split(' ').Last() == "left." ? Direction.Left : Direction.Right;
                    instruction.StateIf0 = sr.ReadLine().Split(' ').Last().First();
                    sr.ReadLine();
                    instruction.WriteIf1 = int.Parse(sr.ReadLine().Split(' ').Last().First().ToString());
                    instruction.DirectionIf1 = sr.ReadLine().Split(' ').Last() == "left." ? Direction.Left : Direction.Right;
                    instruction.StateIf1 = sr.ReadLine().Split(' ').Last().First();

                    output.Instructions.Add(instruction);
                }
            }

            return output;
        }
    }
}