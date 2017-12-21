namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day21 : BaseDay
    {
        public override string Part1(string fileName)
        {
            List<(Grid input, Grid output)> enhancements = base.ImportLines(fileName, ParseGrids).SelectMany(FullListOfGrids).ToList();

            Grid grid = CreateInitialGrid();

            for (int i = 0; i < 5; i++)
            {
                grid = grid.Enhance(enhancements);
            }

            return grid.Count().ToString();
        }

        public override string Part2(string fileName)
        {
            List<(Grid input, Grid output)> enhancements = base.ImportLines(fileName, ParseGrids).SelectMany(FullListOfGrids).ToList();

            Grid grid = CreateInitialGrid();

            for (int i = 0; i < 18; i++)
            {
                grid = grid.Enhance(enhancements);
            }

            return grid.Count().ToString();
        }

        private struct Grid
        {
            private bool Equals(Grid other)
            {
                return this == other;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }

                return obj is Grid castGrid && Equals(castGrid);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((grid != null ? grid.GetHashCode() : 0) * 397) ^ size;
                }
            }

            public Grid(int size)
            {
                this.grid = new bool[size, size];
                this.size = size;
            }

            private readonly bool[,] grid;

            private readonly int size;

            public void SetPixel(int x, int y, bool value)
            {
                this.grid[x, y] = value;
            }

            public int Count()
            {
                int count = 0;

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        if (this.grid[x, y])
                        {
                            count++;
                        }
                    }
                }

                return count;
            }

            private bool this[int x, int y] => this.grid[x, y];

            public Grid Enhance(IReadOnlyCollection<(Grid input, Grid output)> enhancements)
            {
                int numberOfSubGridsPerRow = this.size % 2 == 0 ?
                    this.size / 2 :
                    this.size / 3;

                int sizeOfSubGrids = this.size % 2 == 0 ? 3 : 4;

                Grid output = new Grid(numberOfSubGridsPerRow * sizeOfSubGrids);

                for (int x = 0; x < numberOfSubGridsPerRow; x++)
                {
                    for (int y = 0; y < numberOfSubGridsPerRow; y++)
                    {
                        Grid subGrid = new Grid(sizeOfSubGrids - 1);

                        subGrid.SetPixel(0, 0, this.grid[x * (sizeOfSubGrids - 1), y * (sizeOfSubGrids - 1)]);
                        subGrid.SetPixel(0, 1, this.grid[x * (sizeOfSubGrids - 1), y * (sizeOfSubGrids - 1) + 1]);
                        subGrid.SetPixel(1, 0, this.grid[x * (sizeOfSubGrids - 1) + 1, y * (sizeOfSubGrids - 1)]);
                        subGrid.SetPixel(1, 1, this.grid[x * (sizeOfSubGrids - 1) + 1, y * (sizeOfSubGrids - 1) + 1]);

                        if (sizeOfSubGrids == 4)
                        {
                            subGrid.SetPixel(0, 2, this.grid[x * (sizeOfSubGrids - 1), y * (sizeOfSubGrids - 1) + 2]);
                            subGrid.SetPixel(1, 2, this.grid[x * (sizeOfSubGrids - 1) + 1, y * (sizeOfSubGrids - 1) + 2]);
                            subGrid.SetPixel(2, 0, this.grid[x * (sizeOfSubGrids - 1) + 2, y * (sizeOfSubGrids - 1)]);
                            subGrid.SetPixel(2, 1, this.grid[x * (sizeOfSubGrids - 1) + 2, y * (sizeOfSubGrids - 1) + 1]);
                            subGrid.SetPixel(2, 2, this.grid[x * (sizeOfSubGrids - 1) + 2, y * (sizeOfSubGrids - 1) + 2]);
                        }

                        Grid enhancedSubgrid = subGrid.EnhanceSubGrid(enhancements);

                        for (int i = 0; i < enhancedSubgrid.size; i++)
                        {
                            for (int j = 0; j < enhancedSubgrid.size; j++)
                            {
                                output.SetPixel(x * sizeOfSubGrids + i, y * sizeOfSubGrids + j, enhancedSubgrid[i, j]);
                            }
                        }
                    }
                }

                return output;
            }

            public static bool operator ==(Grid g1, Grid g2)
            {
                if (g1.size != g2.size)
                {
                    return false;
                }

                for (int x = 0; x < g1.size; x++)
                {
                    for (int y = 0; y < g1.size; y++)
                    {
                        if (g1[x, y] != g2[x, y])
                        {
                            return false;
                        }
                    }
                }

                return true;
            }

            private Grid EnhanceSubGrid(IEnumerable<(Grid input, Grid output)> enhancements)
            {
                int mySize = this.size;

                foreach ((Grid input, Grid output) in enhancements.Where(enhancement => enhancement.input.size == mySize))
                {
                    if (this == input)
                    {
                        return output;
                    }
                }

                throw new Exception("Unable to match");
            }

            public static bool operator !=(Grid g1, Grid g2)
            {
                return !(g1 == g2);
            }

            public Grid FlipVertical()
            {
                Grid output = new Grid(this.size);

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        output.SetPixel(x, y, this[x, this.size - y - 1]);
                    }
                }

                return output;
            }

            public Grid FlipHorizontal()
            {
                Grid output = new Grid(this.size);

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        output.SetPixel(x, y, this[this.size - x - 1, y]);
                    }
                }

                return output;
            }

            public Grid FlipBoth()
            {
                Grid output = new Grid(this.size);

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        output.SetPixel(x, y, this[this.size - x - 1, this.size - y - 1]);
                    }
                }

                return output;
            }

            public Grid FlipDiagonal()
            {
                Grid output = new Grid(this.size);

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        output.SetPixel(x, y, this[y, x]);
                    }
                }

                return output;
            }

            public Grid Rotate90()
            {
                Grid output = new Grid(this.size);

                for (int x = 0; x < this.size; x++)
                {
                    for (int y = 0; y < this.size; y++)
                    {
                        output.SetPixel(x, y, this[y, this.size - x - 1]);
                    }
                }

                return output;
            }

            public Grid Rotate90(int number)
            {
                Grid output = this;

                for (int i = 0; i < number; i++)
                {
                    output = output.Rotate90();
                }

                return output;
            }
        }


        private static Grid CreateInitialGrid()
        {
            Grid grid =  new Grid(3);

            grid.SetPixel(0, 2, true);
            grid.SetPixel(1, 0, true);
            grid.SetPixel(1, 2, true);
            grid.SetPixel(2, 1, true);
            grid.SetPixel(2, 2, true);

            return grid;
        }

        private static (Grid, Grid) ParseGrids(string input)
        {
            List<Grid> grids = input.Split(new[] {" => "}, StringSplitOptions.None).Select(ParseGrid).ToList();

            return (grids[0], grids[1]);
        }

        private static Grid ParseGrid(string input)
        {
            string[] lines = input.Split('/');

            Grid grid = new Grid(lines.Length);

            for (int y = 0; y < lines.Length; y++)
            {
                string line = lines[y];

                for (int x = 0; x < line.Length; x++)
                {
                    grid.SetPixel(x, y, line[x] == '#');
                }
            }

            return grid;
        }

        private static IEnumerable<(Grid input, Grid output)> FullListOfGrids((Grid input, Grid output) value)
        {
            for (int i = 0; i < 4; i++)
            {
                yield return (value.input.Rotate90(i), value.output);
            }

            yield return (value.input.FlipVertical().Rotate90(), value.output);
            yield return (value.input.FlipHorizontal().Rotate90(), value.output);
            yield return (value.input.FlipDiagonal().Rotate90(), value.output);
            yield return (value.input.FlipBoth().Rotate90(), value.output);
        }
    }
}