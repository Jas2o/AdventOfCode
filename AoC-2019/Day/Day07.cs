using AoC.Shared;

namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: Amplification Circuit" + Environment.NewLine);

            string input = File.ReadAllText(file);
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            int partA = -1;
            string partA_seq = string.Empty;
            int[] baseA = [0, 1, 2, 3, 4];
            List<IList<int>> permsA = Permutations.Get(baseA);
            foreach (IList<int> perm in permsA) {
                int nextInput = 0;
                for (int s = 0; s < baseA.Length; s++) {
                    IntCode amp = new IntCode(initial, [perm[s], nextInput]);
                    bool completed = amp.Run();
                    if (completed)
                        nextInput = (int)amp.OutputLast;
                    else {
                        Console.WriteLine("A - Did not complete\r\n");
                        break;
                    }
                }

                if (nextInput == 0)
                    break;
                if (nextInput > partA) {
                    partA = nextInput;
                    partA_seq = string.Join(',', perm);
                }
            }

            int partB = -1;
            string partB_seq = string.Empty;
            int[] baseB = [5, 6, 7, 8, 9];
            List<IList<int>> permsB = Permutations.Get(baseB);
            foreach (IList<int> perm in permsB) {
                Queue<long> queue0 = new Queue<long>([perm[0], 0]);
                Queue<long> queue1 = new Queue<long>([perm[1]]);
                Queue<long> queue2 = new Queue<long>([perm[2]]);
                Queue<long> queue3 = new Queue<long>([perm[3]]);
                Queue<long> queue4 = new Queue<long>([perm[4]]);

                IntCode amp0 = new IntCode(initial, queue0, queue1);
                IntCode amp1 = new IntCode(initial, queue1, queue2);
                IntCode amp2 = new IntCode(initial, queue2, queue3);
                IntCode amp3 = new IntCode(initial, queue3, queue4);
                IntCode amp4 = new IntCode(initial, queue4, queue0);

                while(!(amp0.Halted && amp1.Halted && amp2.Halted && amp3.Halted && amp4.Halted)) {
                    amp0.Run();
                    amp1.Run();
                    amp2.Run();
                    amp3.Run();
                    amp4.Run();
                }

                if (amp4.OutputLast > partB) {
                    partB = (int)amp4.OutputLast;
                    partB_seq = string.Join(',', perm);
                }
            }

            Console.WriteLine("Part 1: {0} ({1})", partA, partA_seq);
            //Answer: 30940 (3,0,4,2,1)
            Console.WriteLine("Part 2: {0} ({1})", partB, partB_seq);
            //Answer: 76211147 (8,9,6,7,5)
        }
    }
}
