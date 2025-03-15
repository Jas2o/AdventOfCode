namespace AoC.Graph {
    public class DNode {
        public int X;
        public int Y;
        public int Distance;
        public DNode? Previous;
        public char Value;
        public bool Ignore;

        public DNode(int x, int y, int distance, char value = '.') {
            X = x;
            Y = y;
            Distance = distance;
            Value = value;
            Ignore = false;
        }

        public override string ToString() {
            return string.Format("{0},{1} {2}", X, Y, Value);
        }

        public static List<List<DNode>> BFS_GetAllPaths(List<DNode> listNodes, DNode start, DNode end, int depthLimit) {
            ResetIgnore(listNodes);
            end.Ignore = true;

            Queue<Tuple<int, List<DNode>>> queueState = new Queue<Tuple<int, List<DNode>>>(); //Depth, state
            queueState.Enqueue(new Tuple<int, List<DNode>>(end.Distance, new List<DNode>() { end }));

            Dictionary<(DNode, DNode), List<DNode>> results = new Dictionary<(DNode, DNode), List<DNode>>();

            while (queueState.Count > 0) {
                Tuple<int, List<DNode>> state = queueState.Dequeue();
                int depth = state.Item1;
                List<DNode> givenState = state.Item2;

                DNode last = givenState.Last();
                if (last == start) {
                    givenState.Reverse();
                    results.TryAdd((givenState[1], end), givenState);
                    continue;
                }

                if (depth > depthLimit)
                    continue;

                last.Ignore = true;
                if (!results.Any()) {
                    List<DNode> neighbours = GetNeighbors(listNodes, last);

                    foreach (DNode neighbour in neighbours) {
                        if (neighbour.Distance >= depth)
                            continue;
                        if (neighbour.Value != '.' && neighbour.Value != start.Value)
                            continue;

                        List<DNode> copy = givenState.ToList();
                        copy.Add(neighbour);
                        queueState.Enqueue(new Tuple<int, List<DNode>>(depth - 1, copy));
                    }
                }
            }

            return results.Values.ToList();
        }

        public static void Dijkstra(List<DNode> listUnvisited, List<DNode> listVisited) {
            //Will move items from Unvisited to Visited
            bool loop = true;
            while (loop) {
                if (listUnvisited.Count == 0) {
                    loop = false;
                    break;
                }
                DNode currentNode = listUnvisited.MinBy(n => n.Distance);
                if (currentNode.Distance == int.MaxValue)
                    break;
                List<DNode> neighbors = DNode.GetNeighbors(listUnvisited, currentNode);
                foreach (DNode nextNode in neighbors) {
                    if (listVisited.Contains(nextNode) || nextNode.Ignore)
                        continue;
                    int distance = currentNode.Distance + 1;
                    if (distance < nextNode.Distance) {
                        nextNode.Distance = distance;
                        nextNode.Previous = currentNode;
                    }
                }
                listVisited.Add(currentNode);
                listUnvisited.Remove(currentNode);
            }
        }

        public static void Dijkstra(List<DNode> listNodes) {
            List<DNode> listVisited = new List<DNode>();
            List<DNode> listUnvisited = new List<DNode>();
            listUnvisited.AddRange(listNodes);
            Dijkstra(listUnvisited, listVisited);
        }

        public static List<DNode> GetNeighbors(List<DNode> listNodes, DNode? currentNode) {
            List<DNode> neighbors = new List<DNode>();

            DNode up = listNodes.Find(n => n.X == currentNode.X && n.Y + 1 == currentNode.Y);
            DNode down = listNodes.Find(n => n.X == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode left = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y == currentNode.Y);
            DNode right = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y == currentNode.Y);

            if (up != null) neighbors.Add(up);
            if (down != null) neighbors.Add(down);
            if (left != null) neighbors.Add(left);
            if (right != null) neighbors.Add(right);

            return neighbors;
        }

        public static List<DNode> GetNeighborsWithDiagonals(List<DNode> listNodes, DNode? currentNode) {
            List<DNode> neighbors = new List<DNode>();

            DNode up        = listNodes.Find(n => n.X     == currentNode.X && n.Y + 1 == currentNode.Y);
            DNode upright   = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y + 1 == currentNode.Y);
            DNode right     = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y     == currentNode.Y);
            DNode downright = listNodes.Find(n => n.X - 1 == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode down      = listNodes.Find(n => n.X     == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode downleft  = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y - 1 == currentNode.Y);
            DNode left      = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y     == currentNode.Y);
            DNode upleft    = listNodes.Find(n => n.X + 1 == currentNode.X && n.Y + 1 == currentNode.Y);

            if (up != null) neighbors.Add(up);
            if (upright != null) neighbors.Add(upright);
            if (right != null) neighbors.Add(right);
            if (downright != null) neighbors.Add(downright);
            if (down != null) neighbors.Add(down);
            if (downleft != null) neighbors.Add(downleft);
            if (left != null) neighbors.Add(left);
            if (upleft != null) neighbors.Add(upleft);

            return neighbors;
        }

        public static void ResetIgnore(List<DNode> nodes) {
            foreach (DNode node in nodes) {
                node.Ignore = false;
            }
        }

        public static void ResetDistances(List<DNode> nodes) {
            foreach (DNode node in nodes) {
                node.Distance = int.MaxValue;
                node.Previous = null;
            }
        }

        public static void ResetDistancesAndIgnore(List<DNode> nodes) {
            foreach (DNode node in nodes) {
                node.Distance = int.MaxValue;
                node.Previous = null;
                node.Ignore = false;
            }
        }

        public static List<DNode> GetPath(DNode? nodeEnd, bool Reverse = false) {
            List<DNode> listPath = new List<DNode>();

            DNode nodeCurrent = nodeEnd;
            while (nodeCurrent != null) {
                listPath.Add(nodeCurrent);
                nodeCurrent = nodeCurrent.Previous;
            }

            if (Reverse)
                listPath.Reverse();

            return listPath;
        }

        public static List<DNode> GetPath(DNode? nodeEnd, char changeValue) {
            List<DNode> listPath = new List<DNode>();

            DNode nodeCurrent = nodeEnd;
            List<DNode> listPathNormal = new List<DNode>();
            while (nodeCurrent != null) {
                nodeCurrent.Value = changeValue;
                listPath.Add(nodeCurrent);
                nodeCurrent = nodeCurrent.Previous;
            }

            return listPath;
        }

        public static List<DNode> GetPath(DNode? nodeEnd, char ifValueIsThis, char changeValue) {
            List<DNode> listPath = new List<DNode>();

            DNode nodeCurrent = nodeEnd;
            List<DNode> listPathNormal = new List<DNode>();
            while (nodeCurrent != null) {
                if (nodeCurrent.Value == ifValueIsThis)
                    nodeCurrent.Value = changeValue;
                listPath.Add(nodeCurrent);
                nodeCurrent = nodeCurrent.Previous;
            }

            return listPath;
        }
    }

    public static class BronKerbosch {
        public static void GetCliques(Dictionary<string, List<string>> graph, List<List<string>> cliques,
            List<string> P, List<string> R = null, List<string> X = null) {
            //Normally the order would be R, P, X however P and X start empty.

            if (R == null)
                R = new List<string>();
            if (X == null)
                X = new List<string>();

            //if P and X are both empty then
            if (P.Count == 0 && X.Count == 0) {
                //report R as a maximal clique
                cliques.Add(R);
                return;
            }

            //for each vertex v in P do
            for (int i = 0; i < P.Count; i++) {
                string v = P[i];
                //BronKerbosch1(R ⋃ { v}, P ⋂ N(v), X ⋂ N(v))
                List<string> newR = new List<string>(R);
                newR.Add(v);
                List<string> newP = new List<string>(P.Intersect(graph[v]));
                List<string> newX = new List<string>(X.Intersect(graph[v]));
                GetCliques(graph, cliques, newP, newR, newX);

                //P:= P \ { v}
                P.Remove(v);

                //X:= X ⋃ { v}
                X.Add(v);
            }
        }
    }
}
