namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day22 : BaseDay
    {
        public override string Part1(string fileName)
        {
            Dictionary<Node, NodeState> nodes = ParseNodes(fileName, out int x, out int y);

            return Solve(nodes, x, y, n => (NodeState)(((int)n + 2) % 4), 10000).ToString();
        }

        public override string Part2(string fileName)
        {
            Dictionary<Node, NodeState> nodes = ParseNodes(fileName, out int x, out int y);

            return Solve(nodes, x, y, n => (NodeState)(((int)n + 1) % 4), 10000000, NodeState.Weakned).ToString();
        }

        private Dictionary<Node, NodeState> ParseNodes(string fileName, out int x, out int y)
        {
            List<List<NodeState>> lines = base.ImportLines(fileName, line => line.ToCharArray().Select(c => c == '#' ? NodeState.Infected : NodeState.Clean).ToList()).ToList();

            Dictionary<Node, NodeState> nodes = new Dictionary<Node, NodeState>();

            for (int i = 0; i < lines.Count; i++)
            {
                List<NodeState> line = lines[i];

                for (int j = 0; j < line.Count; j++)
                {
                    Node node = new Node(j, i);
                    nodes[node] = line[j];
                }
            }

            x = Math.Max((lines.First().Count - 1) / 2, 0);
            y = Math.Max((lines.Count - 1) / 2, 0);

            return nodes;
        }

        private int Solve(IDictionary<Node, NodeState> nodes, int x, int y, Func<NodeState, NodeState> processNode, int bursts, NodeState countState = NodeState.Clean)
        {
            int count = 0;

            Direction direction = Direction.Up;
            
            for (int i = 0; i < bursts; i++)
            {
                switch (GetNode(nodes, new Node(x, y)))
                {
                    case NodeState.Clean:
                        direction = (Direction)(((int)direction + 3) % 4);
                        break;
                    case NodeState.Weakned:
                        break;
                    case NodeState.Infected:
                        direction = (Direction)(((int)direction + 5) % 4);
                        break;
                    case NodeState.Flagged:
                        direction = (Direction)(((int)direction + 2) % 4);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (GetNode(nodes, new Node(x, y)) == countState)
                {
                    count++;
                }

                nodes[new Node(x, y)] = processNode(nodes[new Node(x, y)]);

                switch (direction)
                {
                    case Direction.Up:
                        y--;
                        break;
                    case Direction.Right:
                        x++;
                        break;
                    case Direction.Down:
                        y++;
                        break;
                    case Direction.Left:
                        x--;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return count;
        }

        private enum Direction
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3
        }

        private enum NodeState
        {
            Clean = 0,
            Weakned = 1,
            Infected = 2,
            Flagged = 3
        }

        private static NodeState GetNode(IDictionary<Node, NodeState> nodes, Node node)
        {
            if (!nodes.ContainsKey(node))
            {
                nodes[node] = NodeState.Clean;
            }

            return nodes[node];
        }

        private struct Node
        {
            public Node(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            private readonly int x;

            private readonly int y;

            private bool Equals(Node other)
            {
                return this.x == other.x && this.y == other.y;
            }

            public override bool Equals(object obj)
            {
                if (obj is null)
                {
                    return false;
                }

                return obj is Node castNode && Equals(castNode);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return x.GetHashCode() * 397 ^ y.GetHashCode();
                }
            }
        }
    }
}