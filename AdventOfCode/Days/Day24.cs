namespace AdventOfCode.Days
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Day24 : BaseDay
    {
        public override string Part1(string fileName)
        {
            List<Port> ports = base.ImportLines(fileName, ParsePort).ToList();
            Bridge bridge = new Bridge();

            List<Bridge> bridges = CreateBridges(bridge, ports);

            return bridges.OrderByDescending(b => b.Weight()).First().Weight().ToString();
        }

        public override string Part2(string fileName)
        {
            List<Port> ports = base.ImportLines(fileName, ParsePort).ToList();
            Bridge bridge = new Bridge();

            List<Bridge> bridges = CreateBridges(bridge, ports);

            return bridges.OrderByDescending(b => b.Length()).ThenByDescending(b => b.Weight()).First().Weight().ToString();
        }

        public class Port
        {
            public int End1 { get; set; }

            public int End2 { get; set; }
        }

        private static Port ParsePort(string line)
        {
            string[] parts = line.Split('/');

            return new Port
            {
                End1 = int.Parse(parts[0]),
                End2 = int.Parse(parts[1])
            };
        }

        public class Bridge
        {
            private List<Port> ports = new List<Port>();

            public void AddPort(Port port)
            {
                if (port.End1 == EndPort)
                {
                    ports.Add(port);
                    EndPort = port.End2;
                }
                else if (port.End2 == EndPort)
                {
                    ports.Add(port);
                    EndPort = port.End1;
                }
                else
                {
                    throw new Exception();
                }                
            }

            public bool PortFits(Port port)
            {
                return port.End1 == EndPort || port.End2 == EndPort;
            }

            public int EndPort { get; set; } = 0;

            public Bridge Clone()
            {
                Bridge bridge = new Bridge();

                bridge.ports = ports.ToList();
                bridge.EndPort = EndPort;

                return bridge;
            }

            public int Weight()
            {
                return ports.Sum(p => p.End1 + p.End2);
            }

            public int Length()
            {
                return ports.Count();
            }
        }

        public List<Bridge> CreateBridges(Bridge bridge, List<Port> ports)
        {
            List<Bridge> results = new List<Bridge>();

            foreach (Port port in ports.Where(bridge.PortFits))
            {
                Bridge newBridge = bridge.Clone();
                newBridge.AddPort(port);

                results.Add(newBridge);

                List<Port> newPorts = ports.Where(p => p != port).ToList();
                results.AddRange(CreateBridges(newBridge, newPorts));
            }

            return results;
        }
    }
}