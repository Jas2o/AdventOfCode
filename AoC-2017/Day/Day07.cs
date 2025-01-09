namespace AoC.Day
{
    public class Day07
    {
        //Not proud of this solution, was just throwing stuff at the wall and seeing what sticks.

        public static void Run(string file) {
            Console.WriteLine("Day 7: Recursive Circus" + Environment.NewLine);

            Dictionary<string, string> childParent = new Dictionary<string, string>();
            Dictionary<string, int> weight = new Dictionary<string, int>();
            Dictionary<string, int> weight2 = new Dictionary<string, int>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] fields = line.Replace(",", "").Split(' ');

                string parent = fields[0];
                string num = fields[1].Substring(1, fields[1].Length - 2);

                childParent.TryAdd(parent, string.Empty);
                weight[parent] = int.Parse(num);

                if(fields.Length > 3) {
                    string[] children = fields.Skip(3).ToArray();
                    foreach(string child in children) {
                        childParent[child] = parent;
                    }
                }
            }

            string[] bottom = childParent.Where(x => x.Value == string.Empty).Select(x => x.Key).ToArray();
            if (bottom.Length != 1)
                throw new Exception();
            string part1 = bottom.First();

            int part2 = 0;
            Recursive(childParent, weight, weight2, bottom, 1, ref part2);

			Console.WriteLine();
            Console.WriteLine("Part 1: " + part1);
            //Answer: eugwuhl
            Console.WriteLine("Part 2: " + part2);
            //Answer: 420
        }

        private static int Recursive(Dictionary<string, string> childParent, Dictionary<string, int> weight,
            Dictionary<string, int> weight2, string[] thisLayer, int depth, ref int part2) {
            string spacing = "-".PadLeft(depth, '-');
            int total = 0;
            foreach (string t in thisLayer) {
                int w = weight[t];
                string[] children = childParent.Where(x => x.Value == t).Select(x => x.Key).ToArray();
                int[] childrenW = new int[children.Length];
                for (int c = 0; c < children.Length; c++) {
                    childrenW[c] = weight[children[c]];
                }
                int holding = Recursive(childParent, weight, weight2, children, depth + 1, ref part2);
                int check = w + holding;
                weight2[t] = check;

                if (part2 == 0) {
                    int[] childrenW2 = new int[children.Length];
                    for (int c = 0; c < children.Length; c++) {
                        childrenW2[c] = weight2[children[c]];
                    }
                    if (childrenW2.Length > 0) {
                        int check2 = childrenW2.Sum() / childrenW2.Length;
                        if (childrenW2[0] != check2) {
                            //Find most common number
                            int mid = childrenW2.GroupBy(item => item).OrderByDescending(g => g.Count()).Select(g => g.Key).First();

                            //Find which one doesn't match.
                            //We couldn't sort because we need the index to match another array.
                            for (int c = 0; c < children.Length; c++) {
                                if (childrenW2[c] != mid) {
                                    int diff = childrenW2[c] - mid;
                                    int answer = childrenW[c] - diff;
                                    part2 = answer;
                                }
                            }
                        }
                    }
                }

                total += check;
                Console.WriteLine("{0}({1}) {2} ({3}) + {4} = {5}", spacing, depth, t, w, holding, check);
            }
            return total;
        }
    }
}
