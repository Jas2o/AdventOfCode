namespace AoC.Day {
    public class Day22
    {
        //This is currently super slow

        public static void Run(string file) {
            Console.WriteLine("Day 22: Monkey Market" + Environment.NewLine);

            List<Buyer> buyers = new List<Buyer>();

            long total = 0;
            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                long input = long.Parse(line);
                Buyer b = new Buyer(input);
                buyers.Add(b);
                for (int i = 0; i < 2000; i++) {
                    b.Seq();
                }
                total += b.Secret;
                Console.WriteLine("{0}: {1}", input, b.Secret);
            }

            int bananas = 0;
            string bestsequence = string.Empty;
            if (true) {
                Console.WriteLine("\r\nThis is really slow...");

                int[] lookFor = { -9, -9, -9, -9 };

                while (true) {
                    int testbananas = 0;
                    for (int i = 0; i < buyers.Count; i++) {
                        int[] aDiff = buyers[i].ListDiff.ToArray();

                        for (int t = 3; t < buyers[i].ListDiff.Count; t++) {
                            if (buyers[i].ListDiff[t - 3] == lookFor[0] &&
                                buyers[i].ListDiff[t - 2] == lookFor[1] &&
                                buyers[i].ListDiff[t - 1] == lookFor[2] &&
                                buyers[i].ListDiff[t] == lookFor[3]) {
                                testbananas += buyers[i].ListOnes[t + 1];
                                break;
                            }
                        }
                    }

                    if (testbananas > bananas) {
                        bananas = testbananas;
                        bestsequence = string.Join(',', lookFor);
                    }

                    //--

                    lookFor[3]++;
                    if (lookFor[3] == 10) {
                        lookFor[2]++;
                        if (lookFor[2] == 10) {
                            lookFor[1]++;
                            Console.WriteLine("Upping second to: " + lookFor[1]);
                            if (lookFor[1] == 10) {
                                lookFor[0]++;
                                Console.WriteLine("Upping first to: " + lookFor[0]);
                                if (lookFor[0] == 10) {
                                    break;
                                }
                                lookFor[1] = -9;
                            }
                            lookFor[2] = -9;
                        }
                        lookFor[3] = -9;
                    }
                }
            }

            if (false) {
                Console.WriteLine();

                int[] lookFor = { -1, 0, -1, 8 };

                for (int i = 0; i < buyers.Count; i++) {
                    bool found = false;
                    Console.Write("Secret: {0} // ", buyers[i].SecretInitial);
                    for (int t = 3; t < buyers[i].ListDiff.Count; t++) {
                        //Console.WriteLine("{0,10}: {1} ({2})", buyers[i].ListSecret[t], buyers[i].ListOnes[t+1], buyers[i].ListDiff[t]);

                        if (buyers[i].ListDiff[t - 3] == lookFor[0] &&
                            buyers[i].ListDiff[t - 2] == lookFor[1] &&
                            buyers[i].ListDiff[t - 1] == lookFor[2] &&
                            buyers[i].ListDiff[t] == lookFor[3]) {

                            found = true;
                        }

                        //Console.Write("{0,2} ({1,2}) ", buyers[i].ListOnes[t+1], buyers[i].ListDiff[t]);

                        if (found) {
                            Console.Write("{0},{1},{2},{3} then ", buyers[i].ListDiff[t - 3], buyers[i].ListDiff[t - 2], buyers[i].ListDiff[t - 1], buyers[i].ListDiff[t]);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("{0}", buyers[i].ListOnes[t + 1]);
                            Console.ResetColor();
                            bananas += buyers[i].ListOnes[t + 1];
                            break;
                        }
                        //if (t % 9 == 0) Console.WriteLine();
                    }

                    if (!found) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("not possible");
                        Console.ResetColor();
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + total);
            //Answer: 13185239446
            Console.WriteLine("Part 2: " + bananas + " with sequence: " + bestsequence);
            //Answer: 1501 with -1,2,0,0
        }

        private class Buyer {
            public long SecretInitial;
            public long Secret;
            public List<long> ListSecret; //--
            public List<int> ListOnes;
            public List<int> ListDiff;

            public Buyer(long secret) {
                SecretInitial = Secret = secret;
                ListSecret = new List<long>();
                ListOnes = new List<int>();
                ListDiff = new List<int>();

                int ones = (int)(secret % 10);
                ListOnes.Add(ones);
            }

            public void Seq() {
                Secret = Sequence(Secret);
                int ones = (int)(Secret % 10);
                int diff = ones - ListOnes.Last();
                ListSecret.Add(Secret); //--
                ListOnes.Add(ones);
                ListDiff.Add(diff);
            }
        }

        private static long Sequence(long secret) {
            secret ^= secret * 64; //Multi and mix
            secret %= 16777216;//Prune

            secret ^= secret / 32; //Divide and mix
            secret %= 16777216;//Prune

            secret ^= (secret * 2048); //Multi and mix
            secret %= 16777216;//Prune

            return secret;
        }

        /*
        public static string BetweenNegPos9(int value, int toBase) {
            string result = string.Empty;
            do {
                result = "ABCDEFGHI0123456789"[value % toBase] + result;
                value /= toBase;
            }
            while (value > 0);

            return result;
        }
        */
    }
}
