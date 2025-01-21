namespace AoC.Day {
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Chocolate Charts" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            //The real input only has a single line.
            foreach (string input in lines) {
                int num = int.Parse(input);
                int numEnd = num + 10;
                List<int> scores = new List<int>() { 3, 7 };
                int pos1 = 0;
                int pos2 = 1;
                while(scores.Count < numEnd) {
                    AddScore(scores, ref pos1, ref pos2);
                }

                string partA = string.Join("", scores.Skip(num).Take(10).ToArray());

                int sStart = 100000;
                int seqLength = 6;
                int seqN = num;
                int[] seq;
                if (seqN < sStart) {
                    //For the examples
                    sStart = 10000;
                    seqLength = 5;
                    seqN = int.Parse(partA.Substring(0, seqLength));
                }
                seq = new int[seqLength];
                int p = 0;
                for(int s = sStart; s > 0; s /= 10) {
                    int n = seqN / s;
                    seqN %= s;
                    seq[p++] = n;
                }

                int partB = -1;
                int i = 0;
                while (partB == -1) {
                    for(; i < scores.Count - seqLength; i++) {
                        int[] segment = scores.Skip(i).Take(seqLength).ToArray();
                        if(segment.SequenceEqual(seq)) {
                            partB = i;
                            break;
                        }
                    }

                    if (partB == -1) {
                        Console.Write(".");
                        for(int k = 0; k < 10000; k++)
                            AddScore(scores, ref pos1, ref pos2);
                    }
                }

                if(i > 330000)
                    Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Part 1: " + partA);
                //Answer: 3410710325
                Console.WriteLine("Part 2: " + partB);
                //Answer: 20216138
                Console.WriteLine();
            }
        }

        private static void AddScore(List<int> scores, ref int pos1, ref int pos2) {
            int n1 = scores[pos1];
            int n2 = scores[pos2];
            int n3 = n1 + n2;

            if (n3 < 10) {
                scores.Add(n3);
            } else {
                int r1 = n3 / 10;
                int r2 = n3 % 10;
                scores.Add(r1);
                scores.Add(r2);
            }

            pos1 += 1 + n1;
            pos2 += 1 + n2;
            pos1 %= scores.Count;
            pos2 %= scores.Count;
        }
    }
}
