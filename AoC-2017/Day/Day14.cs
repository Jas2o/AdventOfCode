using AoC.Graph;
using System.Text;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Defragmentation" + Environment.NewLine);

			string input = File.ReadAllText(file);

            List<DNode> nodes = new List<DNode>();

            int gridDim = 128;
            int used = 0;
            for (int y = 0; y < gridDim; y++) {
                string hashinput = string.Format("{0}-{1}", input, y);
                string hexstring = Day10Part2(hashinput);
                //Console.WriteLine(hexstring);

                for (int x4 = 0; x4 < hexstring.Length; x4++) {
                    string c = hexstring.Substring(x4, 1);
                    string b = Convert.ToString(Convert.ToInt32(c, 16), 2).PadLeft(4, '0');
                    used += b.Replace("0", "").Length;
                    //Console.WriteLine("{0} -> {1}", hexstring[k], b);
                    //Console.Write(b);

                    if (b[0] == '1') {
                        DNode node0 = new DNode(x4 * 4 + 0, y, 0, b[0]);
                        nodes.Add(node0);
                    }
                    if (b[1] == '1') {
                        DNode node1 = new DNode(x4 * 4 + 1, y, 0, b[1]);
                        nodes.Add(node1);
                    }
                    if (b[2] == '1') {
                        DNode node2 = new DNode(x4 * 4 + 2, y, 0, b[2]);
                        nodes.Add(node2);
                    }
                    if (b[3] == '1') {
                        DNode node3 = new DNode(x4 * 4 + 3, y, 0, b[3]);
                        nodes.Add(node3);
                    }
                }
            }

            int region = 0;
            for(int y = 0; y < gridDim; y++) {
                for (int x = 0; x < gridDim; x++) {
                    DNode node = nodes.Find(n => n.X == x && n.Y == y);
                    if (node == null || node.Distance != 0)
                        continue;
                    FloodFill(nodes, node, ++region);
                }
            }

            Console.WriteLine("Part 1: " + used);
            //Answer: 8148
            Console.WriteLine("Part 2: " + region);
            //Answer: 1180
        }

        private static void FloodFill(List<DNode> nodes, DNode node, int regionID) {
            List<DNode> neighbours = DNode.GetNeighbors(nodes, node);
            foreach (DNode n in neighbours) {
                if (n.Distance != 0)
                    continue;

                n.Distance = regionID;
                FloodFill(nodes, n, regionID);
            }
        }

        private static string Day10Part2(string input) {
            List<int> nums = new List<int>(Enumerable.Range(0, 256));

            int[] seqLength = new int[input.Length + 5];
            for (int i = 0; i < input.Length; i++) {
                seqLength[i] = (byte)input[i];
            }
            int[] end = [17, 31, 73, 47, 23];
            Array.Copy(end, 0, seqLength, input.Length, end.Length);

            int currentPos = 0;
            int skipSize = 0;
            int rounds = 64;
            for (int r = 0; r < rounds; r++) {
                //Day10's Common
                foreach (int seq in seqLength) {
                    int[] subsection = new int[seq];
                    for (int i = 0; i < seq; i++) {
                        int actualPos = (currentPos + i) % nums.Count;
                        subsection[i] = nums[actualPos];
                    }
                    Array.Reverse(subsection);
                    for (int i = 0; i < seq; i++) {
                        int actualPos = (currentPos + i) % nums.Count;
                        nums[actualPos] = subsection[i];
                    }

                    currentPos += seq + skipSize;
                    skipSize++;
                }
            }

            StringBuilder sb = new StringBuilder();
            if (nums.Count == 256) {
                int offset = 0;
                while (offset < 256) {
                    int previous = 0;
                    for (int i = 0; i < 16; i++) {
                        previous ^= nums[offset + i];
                    }
                    offset += 16;
                    sb.Append(previous.ToString("x2"));
                }
            }

            return sb.ToString();
        }
    }
}
