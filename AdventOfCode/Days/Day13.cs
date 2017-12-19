namespace AdventOfCode.Days
{
    using System.Collections.Generic;
    using System.Linq;

    public class Day13 : BaseDay
    {
        public override string Part1(string input)
        {
            List<(int Depth, int Range)> layers = ImportLayers(input).ToList();

            return layers.Sum(layer => layer.Depth % (2 * (layer.Range - 1)) == 0 ? layer.Depth * layer.Range : 0).ToString();
        }

        public override string Part2(string input)
        {
            List<(int Depth, int Range)> layers = ImportLayers(input).ToList();

            int delay = 0;

            while (layers.Any(layer => (delay + layer.Depth) % (2 * (layer.Range - 1)) == 0))
            {
                delay++;
            }

            return delay.ToString();
        }

        private IEnumerable<(int Depth, int Range)> ImportLayers(string input)
        {
            return base.ImportLines(input, line =>
            {
                string[] parts = line.Replace(" ", string.Empty).Split(':');
                return (Depth: int.Parse(parts[0]), Range: int.Parse(parts[1]));
            });
        }
    }
}
