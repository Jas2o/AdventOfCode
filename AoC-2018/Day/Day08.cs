namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Memory Maneuver" + Environment.NewLine);

			string input = File.ReadAllText(file);
            int[] nums = Array.ConvertAll(input.Split(' '), int.Parse);

            List<int> metadataEntries = new List<int>();
            TNode rootNode = Recursive(nums, 0, metadataEntries);

            int partA = metadataEntries.Sum();
            int partB = rootNode.Value;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 35852
            Console.WriteLine("Part 2: " + partB);
            //Answer: 33422
        }

        private static TNode Recursive(int[] nums, int pos, List<int> metadataEntries) {
            TNode node = new TNode(pos, nums[pos++], nums[pos++]);
            for (int c = 0; c < node.QuantityChild; c++) {
                TNode child = Recursive(nums, pos, metadataEntries);
                node.Children.Add(child);
                pos += child.Length;
            }
            node.Length = pos + node.QuantityMeta - node.StartPos;

            for (int m = 0; m < node.QuantityMeta; m++)
                node.Metadata.Add(nums[pos + m]);
            metadataEntries.AddRange(node.Metadata);

            //Part 2
            if (node.QuantityChild == 0) {
                node.Value = node.Metadata.Sum();
            } else {
                foreach(int i in node.Metadata) {
                    int index = i - 1;
                    if (index < node.Children.Count)
                        node.Value += node.Children[index].Value;
                }
            }

            return node;
        }

        private class TNode {
            public int StartPos;
            public int QuantityChild;
            public int QuantityMeta;
            public int Length;
            public int Value;
            public List<TNode> Children;
            public List<int> Metadata;

            public TNode(int pos, int qChild, int qMeta) {
                StartPos = pos;
                QuantityChild = qChild;
                QuantityMeta = qMeta;
                Length = 0;
                Value = 0;
                Children = new List<TNode>();
                Metadata = new List<int>();
            }
        }
    }
}
