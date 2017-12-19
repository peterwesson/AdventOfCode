namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day12 : BaseDay
    {
        public override string Part1(string input)
        {
            List<Node> programs = GetPrograms(input).ToList();

            foreach (Node program in programs)
            {
                program.ConnectedPrograms = program.ConnectedString.Replace(" ", string.Empty).Split(',').Select(c => programs.First(p => p.Id == int.Parse(c))).ToList();
            }

            return FindGroup(programs).Count.ToString();
        }

        public override string Part2(string input)
        {
            List<Node> programs = GetPrograms(input).ToList();

            foreach (Node program in programs)
            {
                program.ConnectedPrograms = program.ConnectedString.Replace(" ", string.Empty).Split(',').Select(c => programs.First(p => p.Id == int.Parse(c))).ToList();
            }

            List<List<Node>> groups = new List<List<Node>>();

            while (programs.Any())
            {
                groups.Add(FindGroup(programs));
            }

            return groups.Count.ToString();
        }

        private IEnumerable<Node> GetPrograms(string input)
        {
            return base.ImportLines(input, line =>
            {
                int breakIndex = line.IndexOf(" <-> ");
                return new Node
                {
                    Id = int.Parse(string.Join(string.Empty, line.Take(breakIndex))),
                    ConnectedString = string.Join(string.Empty, line.Skip(breakIndex + 5))
                };
            });
        }

        private class Node
        {
            public int Id { get; set; }

            public string ConnectedString { get; set; }

            public List<Node> ConnectedPrograms { get; set; }
        }

        private static bool Connected(ICollection<Node> connected, ICollection<Node> unconnected)
        {
            bool output = false;

            foreach (Node node in unconnected.ToList())
            {
                if (node.ConnectedPrograms.Any(connected.Contains))
                {
                    output = true;
                    unconnected.Remove(node);
                    connected.Add(node);
                }
            }

            return output;
        }

        private static List<Node> FindGroup(ICollection<Node> unconnectedPrograms)
        {
            List<Node> connectedPrograms = new List<Node> { unconnectedPrograms.First() };
            unconnectedPrograms.Remove(unconnectedPrograms.First());

            while (Connected(connectedPrograms, unconnectedPrograms)) ;

            return connectedPrograms;
        }
    }
}
