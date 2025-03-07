using System.Numerics;

namespace AoC.Day
{
    public class Day22
    {
        // Oh no, math.
        // Part 2 is just translated from u/hrunt who credited u/mcpower_

        public static void Run(string file) {
            Console.WriteLine("Day 22: Slam Shuffle" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int limitA = (lines.Length < 20 ? 10 : 10007);
            int partA = ShuffleA(lines, limitA, 2019);

            long partB = -1;
            if (limitA > 10) {
                long limitB = 119315717514047;
                long repeatB = 101741582076661;
                partB = ShuffleB(lines, limitB, repeatB, 2020);
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 6289
            Console.WriteLine("Part 2: " + partB);
            //Answer: 58348342289943
        }

        private static int ShuffleA(string[] lines, int limit, int findCard) {
            int[] table = Enumerable.Range(0, limit).ToArray();
            int[] deck;
            int pos = 0;

            foreach (string line in lines) {
                string[] fields = line.Split(' ');
                if (fields[0] == "cut") {
                    deck = table;
                    table = new int[limit];
                    int cut = int.Parse(fields[1]);
                    if (cut < 0) {
                        cut = Math.Abs(cut);
                        Array.Copy(deck, limit - cut, table, 0, cut);
                        Array.Copy(deck, 0, table, cut, limit - cut);
                    } else {
                        Array.Copy(deck, 0, table, limit - cut, cut);
                        Array.Copy(deck, cut, table, 0, limit - cut);
                    }
                } else if (fields[1] == "into") {
                    Array.Reverse(table);
                } else if (fields[2] == "increment") {
                    deck = table;
                    table = new int[limit];
                    int increment = int.Parse(fields[3]);
                    for (int i = 0; i < limit; i++) {
                        table[pos] = deck[i];
                        deck[i] = -1;

                        pos += increment;
                        pos %= limit;
                    }
                }
            }

            if (limit == 10)
                Console.WriteLine(string.Join(' ', table) + Environment.NewLine);

            return Array.IndexOf(table, findCard);
        }

        private static long ShuffleB(string[] lines, long limitB, long repeatB, long atB) {
            BigInteger increment_mul = 1;
            BigInteger offset_diff = 0;
            foreach (string line in lines) {
                string[] fields = line.Split(' ');
                if (fields[0] == "cut") {
                    int cut = int.Parse(fields[1]);
                    offset_diff += cut * increment_mul;
                } else if (fields[1] == "into") {
                    increment_mul *= -1;
                    offset_diff += increment_mul;
                } else if (fields[2] == "increment") {
                    int increment = int.Parse(fields[3]);
                    increment_mul *= BigInteger.ModPow(increment, limitB - 2, limitB);
                }
                increment_mul %= limitB;
                offset_diff %= limitB;
            }

            BigInteger incrementB = BigInteger.ModPow(increment_mul, repeatB, limitB);
            BigInteger offset = offset_diff * (1 - incrementB) * BigInteger.ModPow((1 - increment_mul) % limitB, limitB - 2, limitB);
            offset %= limitB;

            return (long)((offset + atB * incrementB) % limitB);
        }
    }
}
