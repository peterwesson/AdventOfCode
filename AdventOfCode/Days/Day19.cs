namespace AdventOfCode.Days
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day19 : BaseDay
    {
        public override string Part1(string fileName)
        {
            Network network = new Network(base.ImportLines(fileName, line => line.ToCharArray().ToList()).ToList());

            return network.Follow();
        }

        public override string Part2(string fileName)
        {
            Network network = new Network(base.ImportLines(fileName, line => line.ToCharArray().ToList()).ToList());

            return network.FollowCount().ToString();
        }

        private class Network
        {
            public Network(List<List<char>> grid)
            {
                this.grid = grid;
                this.position = (this.FindEntrance(), 0);
                this.direction = Direction.Down;
                this.steps = 0;
            }

            public string Follow()
            {
                while (Advance());

                return this.letters;
            }

            public int FollowCount()
            {
                while (Advance());

                return this.steps;
            }

            private Direction direction;

            private (int X, int Y) position;

            private readonly List<List<char>> grid;

            private int FindEntrance()
            {
                return this.grid[0].IndexOf('|');
            }

            private bool Advance()
            {
                switch (direction)
                {
                    case Direction.Up:
                        this.position = (this.position.X, this.position.Y - 1);
                        break;
                    case Direction.Down:
                        this.position = (this.position.X, this.position.Y + 1);
                        break;
                    case Direction.Left:
                        this.position = (this.position.X - 1, this.position.Y);
                        break;
                    case Direction.Right:
                        this.position = (this.position.X + 1, this.position.Y);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (this.grid[this.position.Y][this.position.X] >= 'A' && this.grid[this.position.Y][this.position.X] <= 'Z')
                {
                    letters += this.grid[this.position.Y][this.position.X];
                }

                if (this.grid[this.position.Y][this.position.X] == '+')
                {
                    this.direction = NextDirection();
                }

                steps++;

                return this.grid[this.position.Y][this.position.X] != 32;
            }

            private Direction NextDirection()
            {
                char leftChar = GetCharacter(this.position.X - 1, this.position.Y);

                if (this.direction != Direction.Right && 
                    (leftChar == '-' || leftChar >= 'A' && leftChar <= 'Z'))
                {
                    return Direction.Left;
                }

                char rightChar = GetCharacter(this.position.X + 1, this.position.Y);

                if (this.direction != Direction.Left &&
                    (rightChar == '-' || rightChar >= 'A' && rightChar <= 'Z'))
                {
                    return Direction.Right;
                }

                char upChar = GetCharacter(this.position.X, this.position.Y - 1);

                if (this.direction != Direction.Down &&
                    (upChar == '|' || upChar >= 'A' && upChar <= 'Z'))
                {
                    return Direction.Up;
                }

                char downChar = GetCharacter(this.position.X, this.position.Y + 1);

                if (this.direction != Direction.Up &&
                    (downChar == '|' || downChar >= 'A' && downChar <= 'Z'))
                {
                    return Direction.Down;
                }

                throw new Exception();
            }

            private char GetCharacter(int x, int y)
            {
                return this.grid[y][x];
            }

            private enum Direction
            {
                Up, Down, Left, Right
            }

            private string letters = string.Empty;

            private int steps;
        }
    }
}