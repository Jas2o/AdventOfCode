using AoC.Graph;

namespace AoC.Day {
    public class Day15
    {
        //After doing 2024, then working up from 2015, THIS PUZZLE IS THE ABSOLUTE WORST.
        //It's NOT efficient, but I don't care.

        //If it doesn't get the correct answer, can try increasing the comparison for bestDistance to use BFS more often at the cost of speed.

        public static void Run(string file) {
            Console.WriteLine("Day 15: Beverage Bandits" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            bool verbose = (lines.Length < 20);
            int elfAttackPower = 3;

            int partA = Simulate(lines, verbose, false, elfAttackPower);

            Console.WriteLine("\r\nSimulating for elves to win...");
            int partB = 0;
            do {
                elfAttackPower++;
                partB = Simulate(lines, false, true, elfAttackPower);
            } while (partB < 0);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 218272
            Console.WriteLine("Part 2: {0} (elves win with attack power {1})", partB, elfAttackPower);
            //Answer: 40861
        }

        private static int Simulate(string[] lines, bool verbose, bool elvesMustWin, int elfAttackPower) {
            int goblinAttackPower = 3;

            //Setup
            List<DNode> nodesAll = new List<DNode>();
            List<DNode> nodes = new List<DNode>();
            List<DNode> nodesUnit = new List<DNode>();
            Dictionary<DNode, Unit> dUnit = new Dictionary<DNode, Unit>();
            for (int y = 0; y < lines.Length; y++) {
                for (int x = 0; x < lines[y].Length; x++) {
                    char c = lines[y][x];
                    if (c == ' ')
                        break;
                    DNode node = new DNode(x, y, int.MaxValue, c);
                    nodesAll.Add(node);
                    if (c != '#') {
                        nodes.Add(node);
                        if (c != '.') {
                            nodesUnit.Add(node);
                            dUnit.Add(node, new Unit(node));
                        }
                    }
                }
            }

            int maxX = nodesAll.Max(n => n.X);
            int maxY = nodesAll.Max(n => n.Y);
            if (verbose)
                DrawMap(nodesAll, maxX, maxY, dUnit);

            int rounds = 0;
            bool loop = true;
            while (loop) {
                loop = false;
                IOrderedEnumerable<DNode> units = nodesUnit.OrderBy(u => u.Y).ThenBy(u => u.X);

                //Each unit's turn.
                foreach (DNode unit in units) {
                    if (unit.Value == '.')
                        continue; //This unit was killed

                    //They look out for the closest target(s).
                    DNode.ResetDistancesAndIgnore(nodes);
                    List<DNode> reachable = new List<DNode>();
                    FloodFill(nodes, unit, reachable, unit.Value);

                    //Check if they just couldn't reach a target.
                    if (reachable.Count == 0) {
                        IEnumerable<DNode> potential = nodesUnit.Where(u => u.Value != unit.Value);
                        if (potential.Any())
                            continue;
                        loop = false;
                        break;
                    }

                    int bestDistance = reachable.MinBy(n => n.Distance).Distance;
                    reachable = reachable.Where(n => n.Distance == bestDistance).ToList();
                    DNode step = null;

                    //If the distance isn't 1 (standing next to them), work out the ridiculousness that is the next step and point next to target (by reading order).
                    if (bestDistance == int.MaxValue) {
                        throw new Exception();
                    } else if (bestDistance > 1) {
                        //Unfortuntely, for my given input there would be a point where BFS would be too slow.
                        //While BFS is accurate, in special circumstances the inaccurate Dijkstra was good enough to save time.

                        //Some stopwatch tests I did with changing the number in the below if statement.
                        //- 25 could take 3 seconds and was the minimum for my input.
                        //- 30 could take 4 seconds and was the minimum for another input I found.
                        //- 35 could take 7 seconds
                        //- 40 could take 15 seconds.
                        //- 45 could take 27 seconds (but that other input would finish in 7 seconds).
                        if (bestDistance < 35) {
                            //BFS
                            List<(DNode, DNode)> options = new List<(DNode, DNode)>();
                            foreach (DNode target in reachable) {
                                List<List<DNode>> paths = DNode.BFS_GetAllPaths(nodes, unit, target, bestDistance);
                                List<DNode> path = paths.OrderBy(p => p[1].Y).ThenBy(p => p[1].X).First();
                                options.Add((path[1], path.TakeLast(2).First()));
                            }
                            step = options.OrderBy(n => n.Item2.Y).ThenBy(n => n.Item2.X).First().Item1;
                        } else {
                            //Dijkstra
                            //We test each closest reachable target with each step option.
                            //This can alter which step next to the target is best, which alters which next step to take...
                            //There's probably a way to shortcut this but I'm sick of this puzzle.

                            List<(DNode, DNode)> options = new List<(DNode, DNode)>();

                            List<DNode> optionsU = DNode.GetNeighbors(nodes, unit);
                            foreach (DNode optionU in optionsU) {
                                optionU.Distance = int.MaxValue;
                                optionU.Previous = null;
                            }

                            foreach (DNode optionU in optionsU) {
                                optionU.Distance = 1;
                                optionU.Previous = unit;

                                foreach (DNode target in reachable) {
                                    DijkstraAfterFlood(nodes, target);
                                    List<DNode> path = DNode.GetPath(target);
                                    path.Reverse();
                                    options.Add((optionU, path.TakeLast(2).First()));
                                }

                                optionU.Distance = int.MaxValue;
                                optionU.Previous = null;
                            }
                            step = options.OrderBy(n => n.Item2.Y).ThenBy(n => n.Item2.X).First().Item1;
                        }

                        //If not next to them, take a step.
                        if (bestDistance > 1) {
                            (unit.X, step.X) = (step.X, unit.X);
                            (unit.Y, step.Y) = (step.Y, unit.Y);
                            loop = true;
                        }
                    }

                    //If next to a target(s), swing at the lowest health one.
                    List<DNode> possibleTargets = DNode.GetNeighbors(nodes, unit);
                    List<Unit> targets = new List<Unit>();
                    foreach (DNode possible in possibleTargets) {
                        if (possible.Value == '.' || possible.Value == unit.Value) continue;
                        targets.Add(dUnit[possible]);
                    }
                    if (targets.Any()) {
                        targets = targets.OrderBy(u => u.HitPoints).ThenBy(u => u.Node.Y).ThenBy(u => u.Node.X).ToList();
                        Unit target = targets.First();

                        if (target.IsELf)
                            target.HitPoints -= goblinAttackPower;
                        else
                            target.HitPoints -= elfAttackPower;

                        if (target.HitPoints <= 0) {
                            if (elvesMustWin && target.IsELf) {
                                Console.WriteLine("Simulation failed.");
                                return -1;
                            }
                            target.Node.Value = '.';
                            nodesUnit.Remove(target.Node);
                            dUnit.Remove(target.Node);
                        }
                        loop = true;
                    }
                }

                if (verbose) {
                    if (loop) {
                        rounds++;
                        Console.WriteLine(rounds);
                    } else {
                        Console.WriteLine("DNF");
                    }
                    DrawMap(nodesAll, maxX, maxY, dUnit);
                } else {
                    if (loop)
                        rounds++;
                }
            }

            int score = 0;
            foreach (var u in dUnit.OrderBy(u => u.Value.Node.Y)) {
                if (u.Value.HitPoints > 0) {
                    score += u.Value.HitPoints;
                }
            }
            Console.WriteLine("Rounds: " + rounds);
            Console.WriteLine("Total HP: " + score);
            score *= rounds;

            return score;
        }

        private static void FloodFill(List<DNode> nodes, DNode homeNode, List<DNode> hits, char not) {
            Queue<DNode> queue = new Queue<DNode>();
            homeNode.Distance = 0;
            queue.Enqueue(homeNode);

            while (queue.Any()) {
                DNode currentNode = queue.Dequeue();
                List<DNode> neighbors = DNode.GetNeighbors(nodes, currentNode);

                if (currentNode.Distance != 0 && currentNode.Value != '.') {
                    if (currentNode.Value != not) {
                        if (!hits.Contains(currentNode))
                            hits.Add(currentNode);
                    }
                } else {
                    if (hits.Count == 0) {
                        foreach (DNode n in neighbors) {
                            if (n.Distance == int.MaxValue && !n.Ignore) {
                                queue.Enqueue(n);
                                n.Ignore = true;
                            }
                        }
                    }
                }

                int minDist = neighbors.Min(n => n.Distance);
                if(currentNode != homeNode && currentNode.Value != not)
                    currentNode.Distance = minDist + 1;
            }
        }

        //This is altered from Graph's DNode.Dijkstra method to not look at the ignore flag and not change distance values.
        private static void DijkstraAfterFlood(List<DNode> listUnvisited, List<DNode> listVisited, DNode end) {
            bool loop = true;
            while (loop) {
                if (listUnvisited.Count == 0) {
                    loop = false;
                    break;
                }
                DNode currentNode = listUnvisited.MinBy(n => n.Distance);
                if (currentNode.Distance == int.MaxValue)
                    break;

                if (currentNode == end)
                    break;

                List<DNode> neighbors = DNode.GetNeighbors(listUnvisited, currentNode);
                foreach (DNode nextNode in neighbors) {
                    if (listVisited.Contains(nextNode))
                        continue;
                    if (nextNode.Distance == currentNode.Distance + 1) {
                        nextNode.Previous = currentNode;
                    }
                }
                listVisited.Add(currentNode);
                listUnvisited.Remove(currentNode);
            }
        }

        private static void DijkstraAfterFlood(List<DNode> listNodes, DNode end) {
            List<DNode> listVisited = new List<DNode>();
            List<DNode> listUnvisited = new List<DNode>();
            listUnvisited.AddRange(listNodes);
            DijkstraAfterFlood(listUnvisited, listVisited, end);
        }

        private static void DrawMap(List<DNode> list, int maxX, int maxY, Dictionary<DNode, Unit> dUnit, bool showDistances = false) {
            var ordered = dUnit.OrderBy(d => d.Key.Y).ThenBy(d => d.Key.X);

            for (int y = 0; y <= maxY; y++) {
                //Map
                for (int x = 0; x <= maxX; x++) {
                    DNode node = list.Find(o => o.Y == y && o.X == x);
                    if (node == null) {
                        Console.Write("?");
                    } else {
                        if (node.Value == '#')
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        else if (node.Value == '*')
                            Console.ForegroundColor = ConsoleColor.Green;
                        else if (node.Value != '.')
                            Console.ForegroundColor = ConsoleColor.Cyan;

                        Console.Write(node.Value);
                        if (showDistances) {
                            if (node.Value == '#') {
                                Console.Write(" ");
                            } else {
                                if (node.Distance == int.MaxValue)
                                    Console.Write("n ");
                                else
                                    Console.Write(node.Distance + " ");
                            }
                        }
                        Console.ResetColor();
                    }
                }

                //Unit position and health.
                Console.Write(" ");
                if (y < dUnit.Count) {
                    var u = ordered.ElementAt(y);
                    Console.Write("  {1,-2}   {0,-2}   {2}", u.Key.X, u.Key.Y, u.Value.HitPoints);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private class Unit {
            public DNode Node;
            public int HitPoints;
            public bool IsELf;

            public Unit(DNode node) {
                Node = node;
                HitPoints = 200;
                IsELf = (node.Value == 'E');
            }
        }
    }
}
