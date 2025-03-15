using AoC.Graph;
using System.Text;
using System.Text.RegularExpressions;

namespace AoC.Day {
    public class Day17
    {
        //This is only solved enough for the example and my given input to work.

        public static void Run(string file) {
            Console.WriteLine("Day 17: Set and Forget" + Environment.NewLine);

            string input = File.ReadAllText(file); //Used by real
			string[] lines = File.ReadAllLines(file); //Used by test, replaced by real's first output.
            long[] initial = [];
            if (input.IndexOf(',') == -1)
                Console.WriteLine("Skipping Part 1's IntCode\r\n");
            else
                initial = Array.ConvertAll(input.Split(','), long.Parse);

            //Part 1 - real only
            IntCode computerA = new IntCode(initial, []);
            if (initial.Length > 0) {
                computerA.Run();

                StringBuilder sb = new StringBuilder();
                while (computerA.outputQueue.Any()) {
                    char c = (char)computerA.outputQueue.Dequeue();
                    sb.Append(c);
                }
                string input2 = sb.ToString();
                lines = input2.Split("\n"); //This will be the camera output and some blanks at the bottom.
            }

            //Part 1 - Convert the real/example camera output to graph.
            List<DNode> nodes = new List<DNode>();
            for (int y = 0; y < lines.Length; y++) {
                if (lines[y].Length < lines[0].Length) {
                    //Console.WriteLine("Extra: " + lines2[y]);
                    continue;
                }
                //Console.WriteLine(lines2[y]);
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == '.')
                        continue;
                    DNode node = new DNode(x, y, 0, c);
                    nodes.Add(node);
                }
            }

            //Part 1 - Solve for the intersections in graph.
            int partA = 0;
            foreach (DNode node in nodes) {
                List<DNode> neighbors = DNode.GetNeighbors(nodes, node);
                if (neighbors.Count == 4) {
                    node.Value = 'O';
                    partA += (node.X * node.Y);
                }
            }

            //Visualization
            int maxY = nodes.Max(n => n.Y);
            int maxX = nodes.Max(n => n.X);
            DrawMap(nodes, maxX, maxY);

            //Part 2 - Come up with greedy instructions to get from start to end.
            DNode start = nodes.Find(n => n.Value == '^');
            start.Distance = 1;
            int RobotX = start.X;
            int RobotY = start.Y;
            Direction dir = Direction.Up;

            int turn = 0;
            int forward = 0;
            List<string> pathRaw = new List<string>();
            while (nodes.Any(n => n.Distance == 0)) {
                int tryX = RobotX;
                int tryY = RobotY;

                if (dir == Direction.Up) tryY--;
                else if (dir == Direction.Left) tryX--;
                else if (dir == Direction.Right) tryX++;
                else if (dir == Direction.Down) tryY++;

                DNode test = nodes.Find(n => n.X == tryX && n.Y == tryY);
                if(test == null) {
                    if(forward != 0) {
                        pathRaw.Add(forward.ToString());
                        forward = 0;
                    }
                    //Turn towards anything with distance 0
                    DNode current = nodes.Find(n => n.X == RobotX && n.Y == RobotY);
                    IEnumerable<DNode> neighbors = DNode.GetNeighbors(nodes, current).Where(n => n.Distance == 0);
                    int c = neighbors.Count();
                    if (c == 0) {
                        throw new Exception("Stuck");
                    } else if(c == 1) {
                        turn++;
                        if (turn == 1)
                            dir++;
                        else
                            dir--;
                        if (dir == Direction.None)
                            dir = Direction.Left;
                        else if (dir == Direction.Imaginary)
                            dir = Direction.Up;
                    } else {
                        throw new Exception("Too many");
                    }
                } else {
                    if(turn != 0) {
                        if (turn == 1)
                            pathRaw.Add("R");
                        else
                            pathRaw.Add("L");
                        turn = 0;
                    }
                    RobotX = tryX;
                    RobotY = tryY;
                    test.Distance = 1;
                    forward++;
                }
            }
            if (forward != 0) {
                pathRaw.Add(forward.ToString());
                forward = 0;
            }
            string pathUncompressed = string.Join(',', pathRaw);
            Console.WriteLine("Uncompressed: " + pathUncompressed + "\r\n");

            //Part 2 - Compress the instructions so that each requested function is up to 20 characters.
            //This will probably not work for some inputs.
            string main = pathUncompressed;
            string A = string.Empty;
            string B = string.Empty;
            string C = string.Empty;
            for (int posStart = 0; posStart < main.Length; posStart++) {
                char first = main[posStart];
                if (first == ',' || !(first == 'L' || first == 'R'))
                    continue;

                bool didReplace = false;
                for(int posEnd = 20; posEnd > 2; posEnd--) {
                    if (posStart + posEnd >= main.Length)
                        continue;
                    char last = main[posStart + posEnd - 1];
                    if (last == ',' || last == 'R' || last == 'L' || last == '1')
                        continue;
                    string section = main.Substring(posStart, posEnd);
                    if (section.IndexOf('A') > -1 || section.IndexOf('B') > -1 || section.IndexOf('C') > -1)
                        continue;

                    int matches = Regex.Matches(main, section).Count;
                    if(matches > 1) {
                        if(A == string.Empty) {
                            A = section;
                            main = main.Replace(A, "A");
                        } else if(B == string.Empty) {
                            B = section;
                            main = main.Replace(B, "B");
                        } else {
                            C = section;
                            main = main.Replace(C, "C");
                        }
                        didReplace = true;
                        break;
                    }
                }

                if (didReplace) {
                    posStart = 0;
                } else if(A != string.Empty) {
                    //This is not exhaustive, but it's "good enough" for my given input.
                    //Might result in an infinite loop.
                    if(B != string.Empty) {
                        int bL = B.LastIndexOf('L');
                        int bR = B.LastIndexOf('R');
                        if(bL > bR) {
                            string remainder = B.Substring(bL);
                            main = main.Replace("B", "B," + remainder);
                            main = main.Replace("C", C);
                            B = B.Substring(0, bL - 1);
                            C = string.Empty;
                            main = main.Replace(B, "B");
                        } else {
                            string remainder = B.Substring(bR);
                            main = main.Replace("B", "B," + remainder);
                            main = main.Replace("C", C);
                            B = B.Substring(0, bR - 1);
                            C = string.Empty;
                            main = main.Replace(B, "B");
                        }
                    } else {
                        throw new Exception();
                    }
                }
            }

            if(main.Length > 20 || A.Length > 20 || B.Length > 20 || C.Length > 20) {
                Console.WriteLine("Did not come up with a compressed solution.");
                return;
            }

            string validate = main.Replace("A", A).Replace("B", B).Replace("C", C);
            if(validate != pathUncompressed) {
                Console.WriteLine("Compressed solution was invalid.");
                return;
            }

            long partB = 0;
            if (initial.Length > 0) {
                IntCode computerB = new IntCode(initial, []);
                computerB.memory[0] = 2;

                while (!computerB.Halted) {
                    computerB.Run();

                    StringBuilder sb = new StringBuilder();
                    while (computerB.outputQueue.Any()) {
                        long value = computerB.outputQueue.Dequeue();
                        if (value < 128)
                            sb.Append((char)value);
                        else {
                            partB = value;
                            break;
                        }
                    }

                    string input2 = sb.ToString();

                    string[] sections = input2.Split("\n\n");
                    //if(sections.Length > 1) {
                        //With continuous video feed all the frames will be here.
                    //}

                    if (partB != 0)
                        break;

                    string last = sections.Last().Replace("\n", " ");
                    Console.Write(last);
                    string copy = string.Empty;

                    if (last.StartsWith("Main"))
                        copy = main;
                    else if (last.StartsWith("Function A"))
                        copy = A;
                    else if (last.StartsWith("Function B"))
                        copy = B;
                    else if (last.StartsWith("Function C"))
                        copy = C;
                    else if (last.StartsWith("Continuous video feed?"))
                        copy = "n"; //Will work if enabled, but we won't show the frames.
                    //--
                    if (copy != string.Empty) {
                        Console.Write(copy);
                        foreach(char c in copy) {
                            computerB.inputQueue.Enqueue(c);
                        }
                        computerB.inputQueue.Enqueue(10);
                        Console.WriteLine();
                    }
                }
            } else {
                Console.WriteLine("Main: " + main);
                Console.WriteLine("Function A: " + A);
                Console.WriteLine("Function B: " + B);
                Console.WriteLine("Function C: " + C);
                Console.WriteLine("Skip run? y");
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 4220
            Console.WriteLine("Part 2: " + partB);
            //main = "A,B,B,C,B,C,B,C,A,A";
            //A = "L,6,R,8,L,4,R,8,L,12";
            //B = "L,12,R,10,L,4";
            //C = "L,12,L,6,L,4,L,4";
            //Answer: 809736
        }

        private enum Direction {
            None, Up, Right, Down, Left, Imaginary
        }

        private static void DrawMap(List<DNode> nodes, int maxX, int maxY) {
            for (int y = 0; y <= maxY; y++) {
                for (int x = 0; x <= maxX; x++) {
                    DNode node = nodes.Find(o => o.Y == y && o.X == x);
                    if (node != null) {
                        if(node.Value != '#' && node.Value != 'O')
                            Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(node.Value);
                    } else
                        Console.Write(" ");

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
