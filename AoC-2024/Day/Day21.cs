using AoC.Graph;
using System.Drawing;
using System.Text;

namespace AoC.Day {
    //Many thanks go to u/Prof_McBurney for this: https://www.reddit.com/r/adventofcode/comments/1hjgyps/2024_day_21_part_2_i_got_greedyish/

    public class Day21
    {
        private static char[] dirs = ['A', '^', '<', 'v', '>'];
        private static char[] nums = ['A', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
        private static char[,] padNum = { { '7', '8', '9' },
                                          { '4', '5', '6' },
                                          { '1', '2', '3' },
                                          { ' ', '0', 'A' } };
        private static char[,] padDir = { { ' ', '^', 'A' },
                                          { '<', 'v', '>' } };
        private static Point padNumActivate = new Point(2, 3);
        private static Point padDirActivate = new Point(2, 0);
        private static Point padNumGap = new Point(0, 3);
        private static Point padDirGap = new Point(0, 0);

        private static Dictionary<string, string> genNum = new Dictionary<string, string>();
        private static Dictionary<string, string> genDir = new Dictionary<string, string>();

        public static void Run(string file, bool interactive) {
            Console.WriteLine("Day 21: Keypad Conundrum" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            int numOfRobotsUsingDirPad = 2;
            int numOfRobotsUsingDirPad_Part2 = 25;
            List<Point> robots = new List<Point>() { new Point(padNumActivate.X, padNumActivate.Y) };
            for (int r = 0; r < numOfRobotsUsingDirPad; r++) {
                robots.Add(new Point(padDirActivate.X, padDirActivate.Y));
            }

            #region Preparation
            List<DNode> nodeVisited = new List<DNode>();
            List<DNode> nodeNums = new List<DNode>();
            for (int y = 0; y < padNum.GetLength(0); y++) {
                for (int x = 0; x < padNum.GetLength(1); x++) {
                    char c = padNum[y,x];
                    if (c == ' ')
                        continue;

                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodeNums.Add(node);
                }
            }
            List<DNode> nodeDirs = new List<DNode>();
            for (int y = 0; y < padDir.GetLength(0); y++) {
                for (int x = 0; x < padDir.GetLength(1); x++) {
                    char c = padDir[y, x];
                    if (c == ' ')
                        continue;

                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodeDirs.Add(node);
                }
            }

            Dictionary<(string, int), long> genDirCache = new Dictionary<(string pair, int level), long>();

            foreach (char num1 in nums) {
                foreach (char num2 in nums) {
                    string key = string.Format("{0}{1}", num1, num2);
                    StringBuilder path = new StringBuilder();

                    if (num1 != num2) {
                        DNode nodeEnd = nodeNums.Find(n => n.Value == num2);
                        DNode nodeStart = nodeNums.Find(n => n.Value == num1);
                        nodeStart.Distance = 0;

                        DNode.Dijkstra(nodeNums, nodeVisited);

                        int movesLeft = 0;
                        int movesRight = 0;
                        int movesUp = 0;
                        int movesDown = 0;

                        DNode nodeCurrent = nodeEnd;
                        List<DNode> listPathNormal = new List<DNode>();
                        while (nodeCurrent != null) {
                            if(listPathNormal.Count > 10)
                                Console.Write('?');

                            if (nodeCurrent.Previous != null) {
                                if (nodeCurrent.X < nodeCurrent.Previous.X)
                                    movesLeft++;
                                else if (nodeCurrent.X > nodeCurrent.Previous.X)
                                    movesRight++;
                                else if (nodeCurrent.Y < nodeCurrent.Previous.Y)
                                    movesUp++;
                                else if (nodeCurrent.Y > nodeCurrent.Previous.Y)
                                    movesDown++;
                                else
                                    throw new Exception();
                            }

                            listPathNormal.Add(nodeCurrent);
                            nodeCurrent = nodeCurrent.Previous;
                        }

                        string updo = "";
                        if (movesUp > 0) updo += '^'.ToString().PadLeft(movesUp, '^');
                        if (movesDown > 0) updo += 'v'.ToString().PadLeft(movesDown, 'v');
                        string leri = "";
                        if (movesLeft > 0) leri += '<'.ToString().PadLeft(movesLeft, '<');
                        if (movesRight > 0) leri += '>'.ToString().PadLeft(movesRight, '>');

                        if ((num1 == 'A' || num1 == '1') && (num2 == '1' || num2 == '4' || num2 == '7')) {
                            path.Append(updo);
                            path.Append(leri);
                        } else if ((num1 == '7' || num1 == '4' || num1 == '1') && (num2 == '0' || num2 == 'A')) {
                            path.Append(leri);
                            path.Append(updo);
                        } else {
                            if (movesLeft > 0)
                                path.Append(leri);
                            path.Append(updo);
                            if (movesRight > 0)
                                path.Append(leri);
                        }

                        //Reset
                        nodeNums.AddRange(nodeVisited);
                        nodeVisited.Clear();
                        DNode.ResetDistances(nodeNums);
                    }

                    path.Append('A');
                    genNum.TryAdd(key, path.ToString());
                }
            }

            foreach(char dir1 in dirs) {
                foreach (char dir2 in dirs) {
                    string key = string.Format("{0}{1}", dir1, dir2);
                    StringBuilder path = new StringBuilder();

                    if (dir1 != dir2) {
                        DNode nodeEnd = nodeDirs.Find(n => n.Value == dir2);
                        DNode nodeStart = nodeDirs.Find(n => n.Value == dir1);
                        nodeStart.Distance = 0;

                        DNode.Dijkstra(nodeDirs, nodeVisited);

                        int movesLeft = 0;
                        int movesRight = 0;
                        int movesUp = 0;
                        int movesDown = 0;

                        DNode nodeCurrent = nodeEnd;
                        List<DNode> listPathNormal = new List<DNode>();
                        while (nodeCurrent != null) {
                            if (listPathNormal.Count > 10)
                                Console.Write('?');

                            if (nodeCurrent.Previous != null) {
                                if (nodeCurrent.X < nodeCurrent.Previous.X)
                                    movesLeft++;
                                else if (nodeCurrent.X > nodeCurrent.Previous.X)
                                    movesRight++;
                                else if (nodeCurrent.Y < nodeCurrent.Previous.Y)
                                    movesUp++;
                                else if (nodeCurrent.Y > nodeCurrent.Previous.Y)
                                    movesDown++;
                                else
                                    throw new Exception();
                            }

                            listPathNormal.Add(nodeCurrent);
                            nodeCurrent = nodeCurrent.Previous;
                        }

                        string updo = "";
                        if (movesUp > 0) updo += '^'.ToString().PadLeft(movesUp, '^');
                        if (movesDown > 0) updo += 'v'.ToString().PadLeft(movesDown, 'v');
                        string leri = "";
                        if (movesLeft > 0) leri += '<'.ToString().PadLeft(movesLeft, '<');
                        if (movesRight > 0) leri += '>'.ToString().PadLeft(movesRight, '>');

                        if (dir1 == '<') {
                            path.Append(leri);
                            path.Append(updo); 
                        } else if (dir2 == '<') {
                            path.Append(updo);
                            path.Append(leri);
                        } else {
                            if (movesLeft > 0)
                                path.Append(leri);
                            path.Append(updo);
                            if (movesRight > 0)
                                path.Append(leri);
                        }

                        //Reset
                        nodeDirs.AddRange(nodeVisited);
                        nodeVisited.Clear();
                        DNode.ResetDistances(nodeDirs);
                    }

                    path.Append('A');
                    genDir.TryAdd(key, path.ToString());
                }
            }
            #endregion

            long partA = 0;
            long partB = 0;
            List<string> expanded = new List<string>(); //Part 1 only
            foreach (string line in lines) {
                Console.WriteLine(line);
                int complexity_num = int.Parse(line.Substring(0, 3));

                //Numpad to door robot
                string pA1 = string.Format("A{0}", line[0]);
                string p12 = string.Format("{0}{1}", line[0], line[1]);
                string p23 = string.Format("{0}{1}", line[1], line[2]);
                string p3A = string.Format("{0}{1}", line[2], line[3]);

                //Part 1, the original slow way.
                //Door robot to layer 0
                string doorA1 = genNum[pA1];
                string door12 = genNum[p12];
                string door23 = genNum[p23];
                string door3A = genNum[p3A];
                for (int r = 0; r < numOfRobotsUsingDirPad; r++) {
                    doorA1 = DirToDir(doorA1);
                    door12 = DirToDir(door12);
                    door23 = DirToDir(door23);
                    door3A = DirToDir(door3A);
                }
                genDirCache.TryAdd((pA1, numOfRobotsUsingDirPad), doorA1.Length);
                genDirCache.TryAdd((p12, numOfRobotsUsingDirPad), door12.Length);
                genDirCache.TryAdd((p23, numOfRobotsUsingDirPad), door23.Length);
                genDirCache.TryAdd((p3A, numOfRobotsUsingDirPad), door3A.Length);
                string joined = string.Format("{0}{1}{2}{3}", doorA1, door12, door23, door3A);
                expanded.Add(joined);
                int complexity_len = joined.Length;
                partA += (complexity_len * complexity_num);
                Console.WriteLine("With {0} robots using direction pads: {1}", numOfRobotsUsingDirPad, complexity_len);
                //Console.WriteLine("{0}={1}, {2}={3}, {4}={5}, {6}={7}", pA1, doorA1.Length, p12, door12.Length, p23, door23.Length, p3A, door3A.Length);

                //Part 2, the fast way. This would work for Part 1 as well without the visual.
                long cA1 = GetCost(pA1, numOfRobotsUsingDirPad_Part2, genDirCache);
                long c12 = GetCost(p12, numOfRobotsUsingDirPad_Part2, genDirCache);
                long c23 = GetCost(p23, numOfRobotsUsingDirPad_Part2, genDirCache);
                long c3A = GetCost(p3A, numOfRobotsUsingDirPad_Part2, genDirCache);
                long complexity_lenB = cA1 + c12 + c23 + c3A;
                partB += (complexity_lenB * complexity_num);
                Console.WriteLine("With {0} robots using direction pads: {1}", numOfRobotsUsingDirPad_Part2, complexity_lenB);
                //Console.WriteLine("{0}={1}, {2}={3}, {4}={5}, {6}={7}", pA1, cA1, p12, c12, p23, c23, p3A, c3A);

                Console.WriteLine();
            }

            if(interactive) {
                int pos = 0;
                int posExpanded = 0;
                StringBuilder sb = new StringBuilder();
                List<string> history = new List<string>();
                
                Console.Beep(150, 100);
                Console.Beep(200, 100);

                Console.WriteLine("Part 1: " + partA);
                //Answer: 219254
                Console.WriteLine("Part 2: " + partB);
                //Answer: 264518225304496

                Console.WriteLine("\r\nPress ENTER to continue with the interactive experience.");
                Console.ReadLine();

                bool loop = true;
                bool useExpanded = true;
                while (loop) {
                    if (numOfRobotsUsingDirPad < 3) {
                        Console.Clear();
                        Console.WriteLine("\x1b[3J");

                        Console.WriteLine("// door //");
                        DrawLine();
                        DrawKeypad(padNum, robots[0]);
                        if (numOfRobotsUsingDirPad > 0) {
                            DrawLine();
                            DrawKeypad(padDir, robots[1]);
                        }
                        if (numOfRobotsUsingDirPad > 1) {
                            DrawLine();
                            DrawKeypad(padDir, robots[2]);
                        }
                        DrawLine();
                    }

                    if (useExpanded) {
                        //Visualization
                        if (pos >= expanded[posExpanded].Length) {
                            if (numOfRobotsUsingDirPad < 3) {
                                history.Add(lines[posExpanded]);
                                history.Add(sb.ToString());
                                history.Add("");
                            }
                            sb.Clear();
                            pos = 0;
                            posExpanded++;
                        }
                        if (posExpanded >= expanded.Count) {
                            useExpanded = false;
                            continue;
                        }

                        char e = expanded[posExpanded][pos];
                        pos++;

                        if (e == ' ')
                            continue;

                        bool moved = Inception(robots, numOfRobotsUsingDirPad, e);
                        sb.Append(e);
                        if (numOfRobotsUsingDirPad < 3) {
                            Console.WriteLine(expanded[posExpanded].Substring(0, pos));
                            Console.WriteLine();
                            foreach (string h in history) {
                                Console.WriteLine(h);
                            }
                            Thread.Sleep(10);
                        }
                        if (!moved) {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Fail");
                            Console.ResetColor();
                            break;
                        }
                    } else {
                        //Manual control
                        Console.WriteLine("You can use control with arrow keys and A. Exit with ESC.");
                        Console.WriteLine(sb.ToString());
                        bool moved = true;
                        ConsoleKeyInfo key = Console.ReadKey(intercept: true);
                        if (key.Key == ConsoleKey.Escape) {
                            loop = false;
                        } else if (key.Key == ConsoleKey.A || key.Key == ConsoleKey.Spacebar) {
                            if (numOfRobotsUsingDirPad == 0) {
                                moved = Inception(robots, 0, 'A', sb);
                            } else {
                                char pressed = padDir[robots[numOfRobotsUsingDirPad].Y, robots[numOfRobotsUsingDirPad].X];
                                moved = Inception(robots, numOfRobotsUsingDirPad - 1, pressed, sb);
                            }
                        } else if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                            moved = Inception(robots, numOfRobotsUsingDirPad, '^');
                        else if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                            moved = Inception(robots, numOfRobotsUsingDirPad, '<');
                        else if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                            moved = Inception(robots, numOfRobotsUsingDirPad, '>');
                        else if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                            moved = Inception(robots, numOfRobotsUsingDirPad, 'v');

                        if (!moved) {
                            for (int i = 200; i > 100; i -= 20)
                                Console.Beep(i, 50);
                        }
                    }
                }
                //Loop has ended

                Console.WriteLine();
                foreach (string h in history) {
                    Console.WriteLine(h);
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 219254
            Console.WriteLine("Part 2: " + partB);
            //Answer: 264518225304496

            Console.WriteLine("\r\nYou can run the interactive version by including letter 'i' in the command.");
        }

        #region Only needed for visualization/validation
        private static bool Inception(List<Point> robots, int layer, char pressed, StringBuilder sb = null) {
            if (pressed == 'A') {
                char pressed2 = (layer == 0 ? padNum[robots[layer].Y, robots[layer].X] : padDir[robots[layer].Y, robots[layer].X]);
                if (layer == 0) {
                    if (sb != null)
                        sb.Append(pressed2);
                    return true;
                }
                return Inception(robots, layer - 1, pressed2, sb);
            } else if (pressed == '^')
                return KeyMove(robots, layer, 0, -1);
            else if (pressed == '<')
                return KeyMove(robots, layer, -1, 0);
            else if (pressed == '>')
                return KeyMove(robots, layer, 1, 0);
            else if (pressed == 'v')
                return KeyMove(robots, layer, 0, 1);
            else
                return false;
        }

        private static bool KeyMove(List<Point> robots, int robotNum, int diffX, int diffY) {
            int x = robots[robotNum].X + diffX;
            int y = robots[robotNum].Y + diffY;
            if (robotNum == 0) {
                if (x < 0 || y < 0 || x > 2 || y > 3 || x == padNumGap.X && y == padNumGap.Y)
                    return false;
            } else {
                if (x < 0 || y < 0 || x > 2 || y > 1 || x == padDirGap.X && y == padDirGap.Y)
                    return false;
            }
            robots[robotNum] = new Point(x, y);
            return true;
        }

        private static void DrawKeypad(char[,] pad, Point current) {
            for (int y = 0; y < pad.GetLength(0); y++) {
                for (int x = 0; x < pad.GetLength(1); x++) {
                    if (current.X == x && current.Y == y) {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    Console.Write(" {0} ", pad[y, x]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private static void DrawLine() {
            Console.WriteLine("---------\r\n");
        }
        #endregion

        private static string DirToDir(string subdir) {
            //Will lead to memory issues when there's a lot of robots.
            StringBuilder sb = new StringBuilder();
            char prev = 'A';
            foreach(char c in subdir) {
                sb.Append(genDir[string.Format("{0}{1}", prev, c)]);
                prev = c;
            }
            return sb.ToString();
        }

        private static long GetSubCost(char prev, string steps, int robot, int numRobots, Dictionary<(string, int), long> cache) {
            if (cache.TryGetValue((steps, robot), out long cVal))
                return cVal;

            if (robot == numRobots)
                return steps.Length;

            long value = 0;
            foreach (char c in steps) {
                string two = string.Format("{0}{1}", prev, c);
                string firstMoves = genDir[two];
                value += GetSubCost('A', firstMoves, robot + 1, numRobots, cache);
                prev = c;
            }

            cache.TryAdd((steps, robot), value);
            return value;
        }

        private static long GetCost(string pair, int numRobots, Dictionary<(string, int), long> cache) {
            if(cache.TryGetValue((pair, numRobots), out long cVal))
                return cVal;

            string firstMoves = genNum[pair];
            long value = GetSubCost('A', firstMoves, 0, numRobots, cache);
            cache.Add((pair, numRobots), value);
            return value;
        }
    }
}
