namespace AdventOfCode.Days
{
    public class Day15 : BaseDay
    {
        public override string Part1(string input)
        {
            Generator generatorA = new Generator(16807, 679);
            Generator generatorB = new Generator(48271, 771);

            int count = 0;

            for (int i = 0; i < 40000000; i++)
            {
                generatorA.Next();
                generatorB.Next();

                long binaryA = generatorA.Value & 0xffff;
                long binaryB = generatorB.Value & 0xffff;

                if (binaryA == binaryB)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        public override string Part2(string input)
        {
            Generator generatorA = new Generator(16807, 679) { Divisor = 4 };
            Generator generatorB = new Generator(48271, 771) { Divisor = 8 };

            int count = 0;

            for (int i = 0; i < 5000000; i++)
            {
                generatorA.NextValid();
                generatorB.NextValid();

                long binaryA = generatorA.Value & 0xffff;
                long binaryB = generatorB.Value & 0xffff;

                if (binaryA == binaryB)
                {
                    count++;
                }
            }

            return count.ToString();
        }

        private class Generator
        {
            public Generator(int factor, int value)
            {
                this.factor = factor;
                this.Value = value;
            }

            private readonly long factor;

            public long Value { get; private set; }

            public long Divisor { private get; set; }

            public void Next()
            {
                this.Value = (this.Value * factor + 2147483647) % 2147483647;
            }

            public void NextValid()
            {
                do
                {
                    this.Next();
                } while (this.Value % this.Divisor != 0);
            }
        }
    }
}
