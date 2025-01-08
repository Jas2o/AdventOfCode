using AoC.Graph;

namespace AoC.Day
{
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: RAM Run" + Environment.NewLine);

            List<DNode> listNodeAll = new List<DNode>();
            List<DNode> listNode = new List<DNode>();
            List<DNode> listCorrupted = new List<DNode>();

            string[] lines = File.ReadAllLines(file);
            (int range, int sims) = (71, 1024);
            if (lines.Length < 30)
                (range,sims) = (7, 13);
            for (int y = 0; y < range; y++) {
                for (int x = 0; x < range; x++) {
                    DNode node = new DNode(x, y, int.MaxValue);
                    listNodeAll.Add(node);
                    listNode.Add(node);
                }
            }

            DNode nodeStart = listNode.Find(n => n.X == 0 && n.Y == 0);
            DNode nodeEnd = listNode.Find(n => n.X == (range - 1) && n.Y == (range - 1));
            nodeStart.Distance = 0;
            nodeStart.Value = 'S';
            nodeEnd.Value = 'E';

            for (int i = 0; i < sims; i++) {
                string[] fields = lines[i].Split(',');
                int x = int.Parse(fields[0]);
                int y = int.Parse(fields[1]);
                DNode node = listNode.Find(n => n.X == x && n.Y == y);
                node.Value = '#';
                listCorrupted.Add(node);
                listNode.Remove(node);
            }

            Console.WriteLine("Dijkstra's algorithm start to end...\r\n");
            DNode.Dijkstra(listNode);
            List<DNode> listSafePath = DNode.GetPath(nodeEnd, 'O');
            int steps = listSafePath.Count() - 1;

            for (int y = 0; y < range; y++) {
                for (int x = 0; x < range; x++) {
                    DNode node = listNodeAll.Find(n => n.X == x && n.Y == y);
                    if (node.Value == '#')
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write(node.Value);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine("\r\nStart the madness!\r\n");

            //Part 2
            string partB = string.Empty;
            for (int i = sims; i < lines.Length; i++) {
                //Get the next corrupted coordinate and take it out of the pool.
                string[] fields = lines[i].Split(',');
                int x = int.Parse(fields[0]);
                int y = int.Parse(fields[1]);
                DNode simNode = listNode.Find(n => n.X == x && n.Y == y);
                simNode.Value = '#';
                listCorrupted.Add(simNode);
                listNode.Remove(simNode);

                //This new corruption doesn't impact the path.
                if (!listSafePath.Contains(simNode))
                    continue;

                //It does impact, reset things and run again.
                DNode.ResetDistances(listNode);
                nodeStart.Distance = 0;
                DNode.Dijkstra(listNode);
                DNode testEnd = nodeEnd;
                while(true) {
                    if (testEnd.Previous == null)
                        break;
                    //Might be a new safe path
                    if(!listSafePath.Contains(testEnd))
                        listSafePath.Add(testEnd);
                    testEnd = testEnd.Previous;
                }
                if(testEnd != nodeStart) {
                    partB = string.Format("{0},{1}", x, y);
                    DNode node = listNodeAll.Find(n => n.X == x && n.Y == y);
                    node.Value = 'X';
                    break;
                }
            }

            for (int y = 0; y < range; y++) {
                for (int x = 0; x < range; x++) {
                    DNode node = listNodeAll.Find(n => n.X == x && n.Y == y);
                    if (node.Value == '#')
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    else if (node.Value == 'X')
                        Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(node.Value);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + steps);
            //Answer: 318
            Console.WriteLine("Part 2: " + partB);
            //Answer: 56,29
        }
    }
}
