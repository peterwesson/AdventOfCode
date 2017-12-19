namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day16 : BaseDay
    {
        public override string Part1(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);

            List<IDanceMove> danceMoves = input.Split(',').Select(ParseDanceMove).ToList();

            return Solve(danceMoves);
        }

        public override string Part2(string fileName)
        {
            string input = base.ReadFileToEnd(fileName);

            List<IDanceMove> danceMoves = input.Split(',').Select(ParseDanceMove).ToList();

            return Solve(danceMoves, 1000000000);
        }

        private const string InitalPrograms = "abcdefghilkjmnop";

        private static string Solve(List<IDanceMove> danceMoves, int iterations = 1)
        {
            List<string> sequence = new List<string> { InitalPrograms };

            sequence.AddRange(GetSequence(InitalPrograms, danceMoves).TakeWhile(r => r != InitalPrograms).Take(iterations));

            return sequence[iterations % sequence.Count()];
        }

        private static IEnumerable<string> GetSequence(string programs, List<IDanceMove> danceMoves)
        {
            while (true)
            {
                yield return programs = danceMoves.Aggregate(programs, PerformDanceMove);
            }
        }

        private static string PerformDanceMove(string programs, IDanceMove danceMove)
        {
            switch (danceMove.Type)
            {
                case DanceMoveType.Spin:
                    SpinDanceMove spinDanceMove = (SpinDanceMove)danceMove;
                    return string.Join(string.Empty, programs.Skip(programs.Length - spinDanceMove.X))
                        + string.Join(string.Empty, programs.Take(programs.Length - spinDanceMove.X));

                case DanceMoveType.Exchange:
                    ExchangeDanceMove exchangeDanceMove = (ExchangeDanceMove)danceMove;
                    char a = programs[exchangeDanceMove.A];
                    char b = programs[exchangeDanceMove.B];
                    char[] exchangeChars = programs.ToCharArray();
                    exchangeChars[exchangeDanceMove.B] = a;
                    exchangeChars[exchangeDanceMove.A] = b;
                    return string.Join(string.Empty, exchangeChars);

                case DanceMoveType.Partner:
                    PartnerDanceMove partnerDanceMove = (PartnerDanceMove)danceMove;
                    int idxA = programs.IndexOf(partnerDanceMove.A);
                    int idxB = programs.IndexOf(partnerDanceMove.B);
                    char[] partnerChars = programs.ToCharArray();
                    partnerChars[idxA] = programs[idxB];
                    partnerChars[idxB] = programs[idxA];
                    return string.Join(string.Empty, partnerChars);

                default:
                    throw new ArgumentException();
            }
        }

        private static IDanceMove ParseDanceMove(string input)
        {
            List<string> parameters = input.Substring(1).Split('/').ToList();

            switch (input[0])
            {
                case 's':
                    return new SpinDanceMove
                    {
                        X = int.Parse(parameters[0])
                    };

                case 'x':                    
                    return new ExchangeDanceMove
                    {
                        A = int.Parse(parameters[0]),
                        B = int.Parse(parameters[1])
                    };

                case 'p':
                    return new PartnerDanceMove
                    {
                        A = parameters[0],
                        B = parameters[1]
                    };

                default: throw new ArgumentException();
            }
        }

        private interface IDanceMove
        {
            DanceMoveType Type { get; }
        }

        private class SpinDanceMove : IDanceMove            
        {
            public DanceMoveType Type => DanceMoveType.Spin;

            public int X { get; set; }
        }

        private class ExchangeDanceMove : IDanceMove
        {
            public DanceMoveType Type => DanceMoveType.Exchange;

            public int A { get; set; }

            public int B { get; set; }
        }

        private class PartnerDanceMove : IDanceMove
        {
            public DanceMoveType Type => DanceMoveType.Partner;

            public string A { get; set; }

            public string B { get; set; }

            public int X { get; set; }
        }
        
        public enum DanceMoveType
        {
            Spin,
            Exchange,
            Partner
        }
    }
}
