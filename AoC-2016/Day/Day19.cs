namespace AoC.Day
{
    public class Day19
    {
        // Thanks to u/Smylers as I was not willing to put in effort to solve this.

        public static void Run(string file) {
            Console.WriteLine("Day 19: An Elephant Named Joseph" + Environment.NewLine);

            int last = int.Parse(File.ReadAllText(file));

            int partA = SolveA_Fast(last);
            int partB = SolveB(last);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1815603
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1410630
        }

        private static int SolveA(int last) {
            Queue<int> queue = new Queue<int>();
            for (int i = 1; i <= last; i++) {
                queue.Enqueue(i);
            }

            while (queue.Count > 0) {
                int stealer = queue.Dequeue();
                if (queue.Count == 0)
                    return stealer;
                int loser = queue.Dequeue();
                queue.Enqueue(stealer);
            }
            return 0;
        }

        private static int SolveA_Fast(int last) {
            //Josephus problem using binary.
            string binary = Convert.ToString(last, 2);
            binary = binary.Substring(1) + binary[0];
            int winner = Convert.ToInt32(binary, 2);
            return winner;
        }

        private static int SolveB(int last) {
            Queue<int> queue1 = new Queue<int>();
            Queue<int> queue2 = new Queue<int>();
            int mid = last / 2;
            for (int i = 1; i <= mid; i++)
                queue1.Enqueue(i);
            for (int i = mid + 1; i <= last; i++)
                queue2.Enqueue(i);

            while (queue1.Count > 0) {
                int loser = queue2.Dequeue();

                if (queue1.Count == queue2.Count) {
                    int move = queue2.Dequeue();
                    queue1.Enqueue(move);
                }

                int stealer = queue1.Dequeue();
                queue2.Enqueue(stealer);
            }

            return queue2.Dequeue();
        }
    }
}
