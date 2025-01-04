using System.Text;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: One-Time Pad" + Environment.NewLine);

            string salt = File.ReadAllText(file);

            List<Notable> listNotableP1 = new List<Notable>();
            List<Notable> listNotableP2 = new List<Notable>();
            Dictionary<int, Notable> dFoundP1 = new Dictionary<int, Notable>();
            Dictionary<int, Notable> dFoundP2 = new Dictionary<int, Notable>();

            Console.WriteLine("Working on it...");

            MD5Worker md5worker = new MD5Worker();

            bool partA = true;
            bool partB = true;
            int num = 0;
            while (partA || partB) {
                string md5 = md5worker.Get(salt + num);

                if (partA) {
                    //Part 1
                    char triplet = GetFirstTriple(md5);
                    if (triplet != '_') {
                        char[] quintuples = GetQuintuple(md5);
                        foreach (char q in quintuples) {
                            List<Notable> matches = listNotableP1.Where(t => t.Index >= num - 1000 && t.C == q).ToList();
                            foreach (Notable match in matches) {
                                //match.Validated = true;
                                dFoundP1.TryAdd(match.Index, match);
                            }
                        }
                        listNotableP1.Add(new Notable(num, triplet, md5));
                    }

                    if (dFoundP1.Count() >= 64) {
                        partA = false;
                        Console.WriteLine("Part 1 done.");
                    }
                }

                if(partB) {
                    //Part 2
                    for (int stretch = 0; stretch < 2016; stretch++)
                        md5 = md5worker.Get(md5);

                    char triplet = GetFirstTriple(md5);
                    if (triplet != '_') {
                        char[] quintuples = GetQuintuple(md5);
                        foreach (char q in quintuples) {
                            List<Notable> matches = listNotableP2.Where(t => t.Index >= num - 1000 && t.C == q).ToList();
                            foreach (Notable match in matches) {
                                //match.Validated = true;
                                dFoundP2.TryAdd(match.Index, match);
                            }
                        }
                        listNotableP2.Add(new Notable(num, triplet, md5));
                    }

                    if (dFoundP2.Count() >= 64) {
                        partB = false;
                        Console.WriteLine("Part 2 done.");
                    }
                }

                num++;
            }

            dFoundP1 = dFoundP1.OrderBy(f => f.Key).ToDictionary();
            dFoundP2 = dFoundP2.OrderBy(f => f.Key).ToDictionary();
            //foreach(Notable n in dFoundP1.Values) {
                //Console.WriteLine("{0},{1}", n.Index, n.MD5);
            //}

            //Console.WriteLine(dFoundP1.Count);
            Console.WriteLine();
            Console.WriteLine("Part 1: " + dFoundP1.ElementAt(63).Value.Index);
            //Answer: 35186
            Console.WriteLine("Part 2: " + dFoundP2.ElementAt(63).Value.Index);
            //Answer: 22429
        }

        private class Notable {
            public int Index;
            public char C;
            public string MD5;
            //public bool Validated;

            public Notable(int index, char c, string md5) {
                Index = index;
                C = c;
                MD5 = md5;
                //Validated = false;
            }

            public override string ToString() {
                return string.Format("{0} = {1} = {2}", Index, C, MD5);
            }
        }

        private static char GetFirstTriple(string input) {
            for (int i = 2; i < input.Length; i++) {
                if (input[i - 2] == input[i - 1] &&input[i - 1] == input[i]) {
                    return input[i];
                }
            }
            return '_';
        }

        private static char[] GetQuintuple(string input) {
            List<char> found = new List<char>();
            for (int i = 4; i < input.Length; i++) {
                if (input[i - 4] == input[i - 3] &&
                    input[i - 3] == input[i - 2] &&
                    input[i - 2] == input[i - 1] &&
                    input[i - 1] == input[i]) {
                    found.Add(input[i]);
                }
            }
            return found.ToArray();
        }

        private class MD5Worker {
            private System.Security.Cryptography.MD5 md5;
        
            public MD5Worker() {
                md5 = System.Security.Cryptography.MD5.Create();
                //Using create a lot slows it down.
            }

            public string Get(string input) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }
    }
}
