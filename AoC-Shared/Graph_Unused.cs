namespace AoC.Graph {
    public class DNode {

        public static List<List<DNode>> BFS_GetAllPaths(List<DNode> listNodes, DNode start, DNode end, int depthLimit) {
            ResetIgnore(listNodes);
            start.Ignore = true;

            Queue<Tuple<int, List<DNode>>> queueState = new Queue<Tuple<int, List<DNode>>>(); //Depth, state
            queueState.Enqueue(new Tuple<int, List<DNode>>(0, new List<DNode>() { start }));

            Dictionary<(DNode, DNode), List<DNode>> results = new Dictionary<(DNode, DNode), List<DNode>>();

            while (queueState.Count > 0) {
                Tuple<int, List<DNode>> state = queueState.Dequeue();
                int depth = state.Item1;
                List<DNode> givenState = state.Item2;

                DNode last = givenState.Last();
                if (last == end) {
                    results.TryAdd((givenState[1], end), givenState);
                    continue;
                }

                if (depth >= depthLimit)
                    continue;

                last.Ignore = true;
                List<DNode> neighbors = GetNeighbors(listNodes, last);

                foreach (DNode neighbour in neighbors) {
                    if (neighbour.Ignore)
                        continue;
                    if (neighbour.Distance < depth)
                        continue;
                    if (neighbour.Value != '.' && neighbour.Value != end.Value)
                        continue;

                    List<DNode> copy = givenState.ToList();
                    copy.Add(neighbour);
                    queueState.Enqueue(new Tuple<int, List<DNode>>(depth + 1, copy));
                }

            }

            return results.Values.ToList();
        }

        public static List<DNode>? BFS_GetPath(List<DNode> listNodes, DNode start, DNode end, char valueMatch, int depthLimit) {
            Queue<Tuple<int, List<DNode>>> queueState = new Queue<Tuple<int, List<DNode>>>(); //Depth, state
            queueState.Enqueue(new Tuple<int, List<DNode>>(0, new List<DNode>() { start }));

            while (queueState.Count > 0) {
                Tuple<int, List<DNode>> state = queueState.Dequeue();
                int depth = state.Item1;
                List<DNode> givenState = state.Item2;

                DNode last = givenState.Last();
                if (last == end)
                    return givenState;

                if (depth >= depthLimit)
                    return null;

                List<DNode> neighbors = GetNeighbors(listNodes, last);

                foreach (DNode neighbour in neighbors) {
                    //if (givenState.Contains(neighbour) || neighbour.Value != valueMatch)
                    //continue;
                    if (neighbour.Distance != last.Distance + 1)
                        continue;

                    if (neighbour.Value != valueMatch)
                        continue;

                    List<DNode> copy = givenState.ToList();
                    copy.Add(neighbour);
                    queueState.Enqueue(new Tuple<int, List<DNode>>(depth + 1, copy));
                }
            }

            return null;
        }

        public static int BFS_GetDepth(List<DNode> listNodes, DNode start, DNode end, char valueMatch, int depthLimit) {
            List<DNode>? path = BFS_GetPath(listNodes, start, end, valueMatch, depthLimit);
            if (path != null)
                return path.Count;
            return -1;
        }
    }
}
