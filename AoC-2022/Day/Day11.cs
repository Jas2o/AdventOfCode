namespace AoC.Day
{
    public class Day11
    {
        public static void Run(string file) {
            Console.WriteLine("Day 11: Monkey in the Middle" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Console.WriteLine("20 rounds, divide by 3:");
            long partA = Solve(lines, 20, false, true);
            Console.WriteLine("10000 rounds, modulo by LCM:");
            long partB = Solve(lines, 10000, true, false);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 107822
            Console.WriteLine("Part 2: " + partB);
            //Answer: 27267163742
        }

        private static long Solve(string[] lines, int rounds, bool isPartB, bool detailedOutput) {
            //Setup
            Queue<string> queue = new Queue<string>(lines);
            Dictionary<int, Monkey> monkeys = new Dictionary<int, Monkey>();
            while (queue.Any()) {
                string lineNum = queue.Dequeue();
                string lineStarting = queue.Dequeue();
                string lineOperation = queue.Dequeue();
                string lineTest = queue.Dequeue();
                string lineTrue = queue.Dequeue();
                string lineFalse = queue.Dequeue();

                int monkeyNum = (int)char.GetNumericValue(lineNum, 7);

                long[] starting = Array.ConvertAll(lineStarting.Substring(lineStarting.IndexOf(':') + 2).Split(", "), long.Parse);
                string[] operation = lineOperation.Substring(lineOperation.IndexOf('=') + 6).Split(' ');
                int testDiv = int.Parse(lineTest.Substring(lineTest.LastIndexOf(' ') + 1));
                int throwTrue = (int)char.GetNumericValue(lineTrue, lineTrue.LastIndexOf(' ') + 1);
                int throwFalse = (int)char.GetNumericValue(lineFalse, lineFalse.LastIndexOf(' ') + 1);

                Monkey m = new Monkey(monkeyNum, starting, operation, testDiv, throwTrue, throwFalse);
                monkeys.Add(monkeyNum, m);

                if (queue.Any())
                    queue.Dequeue(); //Blank lines in between.
            }

            //For Part 2
            long common = LCM(monkeys.Select(m => m.Value.TestDiv).ToArray());
            if(isPartB)
                Console.WriteLine("LCM: {0}", common);

            //Simulate
            for (int round = 1; round <= rounds; round++) {
                foreach (Monkey monkey in monkeys.Values) {
                    if(detailedOutput)
                        Console.WriteLine("Monkey {0}:", monkey.ID);

                    while (monkey.Items.Any()) {
                        long item = monkey.Items.Dequeue();
                        monkey.Inspected++;
                        if (detailedOutput)
                            Console.WriteLine("  Monkey inspects an item with a worry level of {0}.", item);

                        if (monkey.OpOld)
                            monkey.OpNum = item;
                        if (monkey.Operation[0] == "*") {
                            item *= monkey.OpNum;
                            if (detailedOutput)
                                Console.WriteLine("  Worry level is multiplied by {0} to {1}.", monkey.OpNum, item);
                        } else if (monkey.Operation[0] == "+") {
                            item += monkey.OpNum;
                            if (detailedOutput)
                                Console.WriteLine("  Worry level increases by {0} to {1}.", monkey.OpNum, item);
                        }

                        if (isPartB) {
                            item %= common;
                            if (detailedOutput)
                                Console.WriteLine("    Monkey gets bored with item. Worry level now {0}.", item);
                        } else {
                            item /= 3;
                            if (detailedOutput)
                                Console.WriteLine("    Monkey gets bored with item. Worry level is divided by 3 to {0}.", item);
                        }

                        int throwTo = -1;
                        if (item % monkey.TestDiv == 0) {
                            throwTo = monkey.ThrowTrue;
                            if (detailedOutput)
                                Console.WriteLine("    Current worry level is divisible by {0}.", monkey.TestDiv);
                        } else {
                            throwTo = monkey.ThrowFalse;
                            if (detailedOutput)
                                Console.WriteLine("    Current worry level is not divisible by {0}.", monkey.TestDiv);
                        }

                        monkeys[throwTo].Items.Enqueue(item);
                        if (detailedOutput)
                            Console.WriteLine("    Item with worry level {0} is thrown to monkey {1}.", item, throwTo);
                    }
                }
                if (detailedOutput)
                    Console.WriteLine();
            }

            //Answer and display
            int[] top = monkeys.Select(m => m.Value.Inspected).OrderDescending().Take(2).ToArray();
            long result = (long)top[0] * (long)top[1];
            foreach (Monkey monkey in monkeys.Values) {
                if (top.Contains(monkey.Inspected))
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Monkey {0} inspected items {1} times.", monkey.ID, monkey.Inspected);
                Console.ResetColor();
            }
            Console.WriteLine();

            return result;
        }

        private class Monkey {
            public int ID;
            public Queue<long> Items;
            public string[] Operation;
            public bool OpOld;
            public long OpNum;
            public int TestDiv;
            public int ThrowTrue;
            public int ThrowFalse;

            public int Inspected;

            public Monkey(int monkeyNum, long[] starting, string[] operation, int testDiv, int throwTrue, int throwFalse) {
                ID = monkeyNum;
                Items = new Queue<long>(starting);
                Operation = operation;
                OpOld = (Operation[1] == "old");
                if (!OpOld)
                    OpNum = int.Parse(Operation[1]);
                TestDiv = testDiv;
                ThrowTrue = throwTrue;
                ThrowFalse = throwFalse;

                Inspected = 0;
            }
        }

        private static long LCM(int[] numbers) {
            long[] array = numbers.Select(i => (long)i).ToArray();
            return array.Aggregate(lcm);
        }

        private static long lcm(long a, long b) {
            return Math.Abs(a * b) / GCD(a, b);
        }

        private static long GCD(long a, long b) {
            while (a != 0 && b != 0) {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            return a | b;
        }
    }
}
