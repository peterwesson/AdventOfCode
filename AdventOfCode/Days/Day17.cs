namespace AdventOfCode.Days
{
    using System.Linq;
    using System.Collections.Generic;

    public class Day17 : BaseDay
    {
        public override string Part1(string input)
        {
            int steps = int.Parse(input);

            LinkedList<int> buffer = new LinkedList<int>();

            LinkedListNode<int> currentNode = buffer.AddFirst(0);

            for (int i = 1; i <= 2017; i++)
            {
                for (int j = 0; j < steps; j++)
                {
                    currentNode = currentNode == buffer.Last ? buffer.First : currentNode.Next;
                }

                currentNode = buffer.AddAfter(currentNode, i);
            }

            List<int> bufferList = buffer.ToList();

            return bufferList[bufferList.IndexOf(2017) + 1].ToString();
        }

        public override string Part2(string input)
        {
            int steps = int.Parse(input);
            int output = 0;
            int currentPosition = 0;

            for (int i = 1; i <= 50000000; i++)
            {
                currentPosition = (currentPosition + steps) % i + 1;

                if (currentPosition == 1)
                {
                    output = i;
                }
            }

            return output.ToString();
        }     
    }
}