namespace AoC.Day
{
    public class Day12 {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Passage Pathing" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            bool outputText = (lines.Length == 7);

            //Setup
            Dictionary<string, CNode> nodes = new Dictionary<string, CNode>();
            foreach (string line in lines) {
                string[] side = line.Split('-');
                if (!nodes.ContainsKey(side[0])) {
                    CNode node = new CNode(side[0]);
                    nodes.Add(side[0], node);
                }
                if (!nodes.ContainsKey(side[1])) {
                    CNode node = new CNode(side[1]);
                    nodes.Add(side[1], node);
                }
            }

            foreach (string line in lines) {
                string[] side = line.Split('-');
                CNode left = nodes[side[0]];
                CNode right = nodes[side[1]];

                left.Connections.Add(right);
                right.Connections.Add(left);
            }

            //Part 1
            List<CNode[]> pathsA = new List<CNode[]>();
            RecursiveA(pathsA, new List<CNode>() { nodes["start"] });
            int partA = pathsA.Count();

            if (outputText) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 1");
                Console.ResetColor();
                foreach (CNode[] path in pathsA)
                    Console.WriteLine(string.Join(',', path.Select(p => p.ID)));
                Console.WriteLine();
            }

            //Part 2
            List<CNode[]> pathsB = new List<CNode[]>();
            RecursiveB(pathsB, new List<CNode>() { nodes["start"] }, false);
            int partB = pathsB.Count();

            if (outputText) {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Part 2");
                Console.ResetColor();
                foreach (CNode[] path in pathsB)
                    Console.WriteLine(string.Join(',', path.Select(p => p.ID)));
                Console.WriteLine();
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 4691
            Console.WriteLine("Part 2: " + partB);
            //Answer: 140718
        }

        private static void RecursiveA(List<CNode[]> paths, List<CNode> inProgress) {
            CNode current = inProgress.Last();
            if(current.ID == "end") {
                paths.Add(inProgress.ToArray());
                return;
            }

            foreach(CNode other in current.Connections) {
                if (!other.IsBigCave && inProgress.Contains(other))
                    continue;

                inProgress.Add(other);
                RecursiveA(paths, inProgress);
                inProgress.Remove(other);
            }
        }

        private static void RecursiveB(List<CNode[]> paths, List<CNode> inProgress, bool twice) {
            CNode current = inProgress.Last();
            if (current.ID == "end") {
                paths.Add(inProgress.ToArray());
                return;
            }

            foreach (CNode other in current.Connections) {
                bool twiceNow = twice;
                if (other.ID == "start")
                    continue;
                if(!other.IsBigCave) {
                    int already = inProgress.Count(p => p == other);
                    if (already == 2)
                        continue;
                    if (already == 1) {
                        if(twice)
                            continue;
                        twiceNow = true;
                    }
                }

                inProgress.Add(other);
                RecursiveB(paths, inProgress, twiceNow);
                inProgress.Remove(other);
            }
        }

        private class CNode {
            public string ID;
            public bool IsBigCave;
            public List<CNode> Connections;

            public CNode(string id) {
                ID = id;
                IsBigCave = char.IsUpper(ID[0]);
                Connections = new List<CNode>();
            }
        }
    }
}
