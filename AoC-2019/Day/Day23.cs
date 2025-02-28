namespace AoC.Day
{
    public class Day23
    {
        public static void Run(string file) {
            Console.WriteLine("Day 23: Category Six" + Environment.NewLine);

            string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            long partA = Solve(initial, false);
            long partB = Solve(initial, true);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 19473
            Console.WriteLine("Part 2: " + partB);
            //Answer: 12475
        }

        private static long Solve(long[] initial, bool partB) {
            Dictionary<int, IntCode> network = new Dictionary<int, IntCode>();
            IEnumerable<int> range = Enumerable.Range(0, 50);
            foreach (int address in range) {
                IntCode nic = new IntCode(initial, [address]);
                network[address] = nic;
            }

            long natX = 0;
            long natY = 0;
            long natY_prevSent = -1;

            while (true) {
                bool idle = true;

                foreach (KeyValuePair<int, IntCode> nic in network) {
                    nic.Value.Run();
                    if (!nic.Value.outputQueue.Any()) {
                        nic.Value.inputQueue.Enqueue(-1);
                        nic.Value.Run();
                    }
                    while (nic.Value.outputQueue.Any()) {
                        idle = false;
                        int dest = (int)nic.Value.outputQueue.Dequeue();
                        long x = nic.Value.outputQueue.Dequeue();
                        long y = nic.Value.outputQueue.Dequeue();
                        if (dest == 255) {
                            if(!partB)
                                return y;
                            natX = x;
                            natY = y;
                            continue;
                        }
                        network[dest].inputQueue.Enqueue(x);
                        network[dest].inputQueue.Enqueue(y);
                    }
                }

                if(idle) {
                    network[0].inputQueue.Enqueue(natX);
                    network[0].inputQueue.Enqueue(natY);
                    if (natY == natY_prevSent)
                        return natY;
                    natY_prevSent = natY;
                }
            }
        }
    }
}
