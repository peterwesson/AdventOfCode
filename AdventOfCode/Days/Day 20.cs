namespace AdventOfCode.Days
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class Day20 : BaseDay
    {
        public override string Part1(string fileName)
        {
            List<Particle> particles = base.ImportLines(fileName, ExtractParticle).ToList();

            Particle particle = particles.OrderBy(p => p.AccelerationSize).ThenBy(p => p.VelocitySize).ThenBy(p => p.ManhattonDistance).First();

            return particles.IndexOf(particle).ToString();
        }

        public override string Part2(string fileName)
        {
            List<Particle> particles = base.ImportLines(fileName, ExtractParticle).ToList();

            for (int i = 0; i < 50; i++)
            {
                particles.ForEach(p => p.Update());

                particles.RemoveAll(particle =>
                    particles.Where(p => p != particle).Any(p =>
                        p.Position.X == particle.Position.X && p.Position.Y == particle.Position.Y && p.Position.Z == particle.Position.Z));
            }

            return particles.Count.ToString();
        }

        private class Particle
        {
            public (int X, int Y, int Z) Position { get; set; }

            public (int X, int Y, int Z) Velocity { get; set; }

            public (int X, int Y, int Z) Acceleration { get; set; }

            public void Update()
            {
                this.Velocity = (this.Velocity.X + this.Acceleration.X, this.Velocity.Y + this.Acceleration.Y, this.Velocity.Z + this.Acceleration.Z);
                this.Position = (this.Position.X + this.Velocity.X, this.Position.Y + this.Velocity.Y, this.Position.Z + this.Velocity.Z);
            }

            public int ManhattonDistance => Math.Abs(this.Position.X) + Math.Abs(this.Position.Y) + Math.Abs(this.Position.Z);

            public int AccelerationSize => Math.Abs(this.Acceleration.X) + Math.Abs(this.Acceleration.Y) + Math.Abs(this.Acceleration.Z);

            public int VelocitySize => Math.Abs(this.Velocity.X) + Math.Abs(this.Velocity.Y) + Math.Abs(this.Velocity.Z);
        }

        private static Particle ExtractParticle(string line)
        {
            List<string> parts = line.Split(new [] {", "}, StringSplitOptions.None).Select(i => string.Join(string.Empty, i.Skip(3).Take(i.Length - 4))).ToList();

            List<int> position = parts[0].Split(',').Select(int.Parse).ToList();
            List<int> velocity = parts[1].Split(',').Select(int.Parse).ToList();
            List<int> acceleration = parts[2].Split(',').Select(int.Parse).ToList();

            return new Particle
            {
                Position = (position[0], position[1], position[2]),
                Velocity = (velocity[0], velocity[1], velocity[2]),
                Acceleration = (acceleration[0], acceleration[1], acceleration[2])
            };
        }
    }
}