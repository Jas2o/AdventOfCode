namespace AoC.Day
{
    public class Day20
    {
        //What a crazy puzzle. There's probably incomplete parts and duplicate code, but my given input is solved so I don't care.

        public static void Run(string file) {
            Console.WriteLine("Day 20: Jurassic Jigsaw" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            Puzzle puzzle = new Puzzle(lines);
            long partA = puzzle.SolvePartA(true);
            int partB = puzzle.SolvePartB(true);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 107399567124539
            Console.WriteLine("Part 2: {0} (sea monsters: {1})", partB, puzzle.NumSeaMonsters);
            //Answer: 1555 (sea monsters: 38)
        }

        private static void Draw2D(char[][] map) {
            foreach (char[] row in map)
                Console.WriteLine(new string(row));
            Console.WriteLine();
        }

        private static void Draw2DFancy(char[][] map) {
            foreach (char[] row in map) {
                foreach (char c in row) {
                    if (c == 'O')
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    Console.Write(c);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        //2016 Day 21
        private static IEnumerable<T> ShiftRight<T>(IList<T> values, int shift) {
            IEnumerable<T> newBegin = values.Skip(values.Count - shift);
            IEnumerable<T> newEnd = values.Take(values.Count - shift);
            return newBegin.Concat(newEnd);
        }

        private static char[][] RotateRight(char[][] input) {
            if (input[0].Length != input.Length)
                throw new Exception();

            int dim = input.Length;
            char[][] output = new char[dim][];
            for (int y = 0; y < dim; y++) {
                output[y] = new char[dim];

                for (int x = 0; x < dim; x++)
                    output[y][x] = input[dim - 1 - x][y];
            }

            return output;
        }

        private static char[][] FlipY(char[][] input) {
            if (input[0].Length != input.Length)
                throw new Exception();

            int dim = input.Length;
            char[][] output = new char[dim][];
            for (int y = 0; y < dim; y++) {
                output[y] = new char[dim];
            }

            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++)
                    output[dim - 1 - y][x] = input[y][x];
            }

            return output;
        }

        private static char[][] FlipX(char[][] input) {
            if (input[0].Length != input.Length)
                throw new Exception();

            int dim = input.Length;
            char[][] output = new char[dim][];
            for (int y = 0; y < dim; y++) {
                output[y] = new char[dim];
            }

            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++)
                    output[y][dim - 1 - x] = input[y][x];
            }

            return output;
        }

        private static char[][] InvertForTL(char[][] input) {
            if (input[0].Length != input.Length)
                throw new Exception();

            int dim = input.Length;
            char[][] output = new char[dim][];
            for (int y = 0; y < dim; y++) {
                output[y] = new char[dim];
            }

            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++)
                    output[dim - 1 - y][x] = input[x][dim - 1 - y];
            }

            return output;
        }

        private static char[][] InvertForTR(char[][] input) {
            if (input[0].Length != input.Length)
                throw new Exception();

            int dim = input.Length;
            char[][] output = new char[dim][];
            for (int y = 0; y < dim; y++) {
                output[y] = new char[dim];
            }

            for (int y = 0; y < dim; y++) {
                for (int x = 0; x < dim; x++)
                    output[y][dim - 1 - x] = input[x][dim - 1 - y];
            }

            return output;
        }

        private class Puzzle {
            private List<PuzzlePiece> pieces;
            private List<int> listCorner;
            private List<int> listBorder;
            private List<int> listMid;
            private int dim = 0;

            private PuzzlePiece[][] puzzle;
            PuzzlePiece? topLeft;
            PuzzlePiece? topRight;
            PuzzlePiece? bottomLeft;
            PuzzlePiece? bottomRight;

            char[][] complete;
            public int NumSeaMonsters { get; private set; }

            public Puzzle(string[] lines) {
                pieces = new List<PuzzlePiece>();
                for (int i = 0; i < lines.Length; i += 12) {
                    int num = int.Parse(lines[i].Substring(5, 4));
                    string[] tile = lines.Skip(i + 1).Take(10).ToArray();
                    PuzzlePiece j = new PuzzlePiece(num, tile);
                    pieces.Add(j);
                }

                listCorner = new List<int>();
                listBorder = new List<int>();
                listMid = new List<int>();

                dim = (int)Math.Sqrt(pieces.Count());
                puzzle = new PuzzlePiece[dim][];
                for (int d = 0; d < dim; d++)
                    puzzle[d] = new PuzzlePiece[dim];

                complete = new char[dim * 8][];
                for (int row = 0; row < complete.Length; row++)
                    complete[row] = new char[dim * 8];
                NumSeaMonsters = 0;
            }

            public long SolvePartA(bool output) {
                FindNeighbors();
                FindTypes(output);

                if (listCorner.Count != 4) {
                    Console.WriteLine("Unfortunately found {0} corners.\r\n", listCorner.Count);
                    return 0;
                }

                if(output)
                    Console.WriteLine("\r\nCorners: {0}\r\n", string.Join(", ", listCorner));

                long partA = 1;
                foreach (int corner in listCorner)
                    partA *= corner;
                return partA;
            }

            public int SolvePartB(bool outputExtra) {
                //Place the pieces into their locations.
                PuzzleFillEdgesTopAndLeft();
                PuzzleFillEdgesBottomAndRight();
                PuzzleFillMiddle();

                if (outputExtra)
                    DrawMapIDs();

                PuzzleCornersRotate();
                PuzzleFixCornerTopLeft();
                PuzzleFixEdgesTopAndLeft();
                PuzzleFixCornerTopRight();
                PuzzleFixCornerBottomLeft();
                PuzzleFixEdgesBottomAndRight();
                PuzzleFixCornerBottomRight();
                PuzzleMiddleRotate();

                if (outputExtra)
                    DrawTilesWithBorders();

                MergeCompleteBoard();
                if (outputExtra)
                    Draw2D(complete);

                return TheSeaMonstersPart(outputExtra);
            }

            private void FindNeighbors() {
                //Work out which pieces would fit next to each other.
                foreach (PuzzlePiece piece in pieces) {
                    foreach (PuzzlePiece other in pieces) {
                        if (piece == other)
                            continue;

                        for (int i = 0; i < 4; i++) {
                            if (other.Edges.Contains(piece.Edges[i])) {
                                piece.Seen[i]++;
                                piece.Seen2.Add(other.ID);
                            }
                            if (other.EdgesFlipped.Contains(piece.Edges[i])) {
                                piece.Seen[i]++;
                                piece.Seen2.Add(other.ID);
                            }
                            if (other.EdgesFlipped.Contains(piece.EdgesFlipped[i])) {
                                piece.Seen[i]++;
                                piece.Seen2.Add(other.ID);
                            }
                        }
                    }
                }
            }

            private void FindTypes(bool output) {
                //Find types of pieces, need corner IDs for Part 1.
                foreach (PuzzlePiece piece in pieces.OrderBy(p => p.Seen2.Count).ThenBy(p => p.ID)) {
                    int c = piece.Seen.Count(s => s == 0);
                    if (c == 2) {
                        listCorner.Add(piece.ID);
                        if (output)
                            Console.ForegroundColor = ConsoleColor.Blue;
                    } else if (c == 1) {
                        listBorder.Add(piece.ID);
                        if (output)
                            Console.ForegroundColor = ConsoleColor.Cyan;
                    } else
                        listMid.Add(piece.ID);

                    if (output) {
                        Console.WriteLine("{0} =   {1}   = {2} = {3}",
                            piece.ID, string.Join("   ", piece.Edges), string.Join(",", piece.Seen), string.Join(",", piece.Seen2));
                        Console.ResetColor();
                    }
                }
            }

            private void PuzzleFillEdgesTopAndLeft() {
                PuzzlePiece left = puzzle[0][0] = pieces.Find(p => p.ID == listCorner.First());
                PuzzlePiece above = left;
                listCorner.Remove(left.ID);

                for (int i = 1; i < dim; i++) {
                    if (i == dim - 1) {
                        //Corner
                        IEnumerable<int> optionsBelow = listCorner.Intersect(above.Seen2);
                        int numBelow = optionsBelow.First();
                        listCorner.Remove(numBelow);
                        above.Seen2.Remove(numBelow);
                        PuzzlePiece below = pieces.Find(p => p.ID == numBelow);
                        below.Seen2.Remove(above.ID);

                        IEnumerable<int> optionsRight = listCorner.Intersect(left.Seen2);
                        int numRight = optionsRight.First();
                        listCorner.Remove(numRight);
                        left.Seen2.Remove(numRight);
                        PuzzlePiece right = pieces.Find(p => p.ID == numRight);
                        right.Seen2.Remove(left.ID);

                        puzzle[0][i] = right;
                        puzzle[i][0] = below;

                        left = right;
                        above = below;
                    } else {
                        //Border
                        IEnumerable<int> optionsBelow = listBorder.Intersect(above.Seen2);
                        int numBelow = optionsBelow.First();
                        listBorder.Remove(numBelow);
                        above.Seen2.Remove(numBelow);
                        PuzzlePiece below = pieces.Find(p => p.ID == numBelow);
                        below.Seen2.Remove(above.ID);

                        IEnumerable<int> optionsRight = listBorder.Intersect(left.Seen2);
                        int numRight = optionsRight.First();
                        listBorder.Remove(numRight);
                        left.Seen2.Remove(numRight);
                        PuzzlePiece right = pieces.Find(p => p.ID == numRight);
                        right.Seen2.Remove(left.ID);

                        puzzle[0][i] = right;
                        puzzle[i][0] = below;

                        left = right;
                        above = below;
                    }
                }
            }

            private void PuzzleFillEdgesBottomAndRight() {
                puzzle[dim - 1][dim - 1] = pieces.Find(p => p.ID == listCorner.First());
                listCorner.Remove(listCorner.First());

                PuzzlePiece above = puzzle[0][dim - 1];
                PuzzlePiece left = puzzle[dim - 1][0];

                for (int xy = 1; xy < dim - 1; xy++) {
                    IEnumerable<int> optionsBelow = listBorder.Intersect(above.Seen2);
                    if (optionsBelow.Count() != 1)
                        throw new Exception();

                    int numBelow = optionsBelow.First();
                    listBorder.Remove(numBelow);
                    above.Seen2.Remove(numBelow);
                    PuzzlePiece below = pieces.Find(p => p.ID == numBelow);
                    below.Seen2.Remove(above.ID);

                    IEnumerable<int> optionsRight = listBorder.Intersect(left.Seen2);
                    int numRight = optionsRight.First();
                    listBorder.Remove(numRight);
                    left.Seen2.Remove(numRight);
                    PuzzlePiece right = pieces.Find(p => p.ID == numRight);
                    right.Seen2.Remove(left.ID);

                    puzzle[dim - 1][xy] = right;
                    puzzle[xy][dim - 1] = below;

                    left = right;
                    above = below;
                }
            }

            private void PuzzleFillMiddle() {
                for (int x = 1; x < dim - 1; x++) {
                    for (int y = 1; y < dim - 1; y++) {
                        PuzzlePiece left = puzzle[y][x - 1];
                        PuzzlePiece above = puzzle[y - 1][x];
                        PuzzlePiece leftAbove = puzzle[y - 1][x - 1];

                        List<int> options = left.Seen2.Intersect(above.Seen2).ToList();
                        options.Remove(leftAbove.ID);

                        if (options.Count() != 1)
                            throw new Exception();

                        int num = options.First();
                        listMid.Remove(num);
                        left.Seen2.Remove(num);
                        above.Seen2.Remove(num);
                        leftAbove.Seen2.Remove(num);
                        PuzzlePiece here = pieces.Find(p => p.ID == num);
                        puzzle[y][x] = here;
                    }
                }
            }

            private void PuzzleCornersRotate() {
                //Rotate the corners. They might not be flipped right yet.
                topLeft = puzzle[0][0];
                topRight = puzzle[0][dim - 1];
                bottomLeft = puzzle[dim - 1][0];
                bottomRight = puzzle[dim - 1][dim - 1];

                if (topLeft.Seen[0] != 0 || topLeft.Seen[3] != 0) {
                    do {
                        topLeft.Seen = ShiftRight(topLeft.Seen, 1).ToArray();
                        topLeft.Raw = RotateRight(topLeft.Raw);
                    } while (topLeft.Seen[0] != 0 || topLeft.Seen[3] != 0);
                    topLeft.GenEdges();
                }

                if (topRight.Seen[0] != 0 || topRight.Seen[1] != 0) {
                    do {
                        topRight.Seen = ShiftRight(topRight.Seen, 1).ToArray();
                        topRight.Raw = RotateRight(topRight.Raw);
                    } while (topRight.Seen[0] != 0 || topRight.Seen[1] != 0);
                    topRight.GenEdges();
                }

                if (bottomLeft.Seen[2] != 0 || bottomLeft.Seen[3] != 0) {
                    do {
                        bottomLeft.Seen = ShiftRight(bottomLeft.Seen, 1).ToArray();
                        bottomLeft.Raw = RotateRight(bottomLeft.Raw);
                    } while (bottomLeft.Seen[2] != 0 || bottomLeft.Seen[3] != 0);
                    bottomLeft.GenEdges();
                }

                if (bottomRight.Seen[1] != 0 || bottomRight.Seen[2] != 0) {
                    do {
                        bottomRight.Seen = ShiftRight(bottomRight.Seen, 1).ToArray();
                        bottomRight.Raw = RotateRight(bottomRight.Raw);
                    } while (bottomRight.Seen[1] != 0 || bottomRight.Seen[2] != 0);
                    bottomRight.GenEdges();
                }
            }

            private void PuzzleFixCornerTopLeft() {
                if (!puzzle[0][1].Edges.Contains(topLeft.Edges[1]) && !puzzle[1][0].Edges.Contains(topLeft.Edges[2])) {
                    //Console.WriteLine("Top-Left invert");
                    topLeft.Raw = InvertForTL(topLeft.Raw);
                    topLeft.GenEdges();
                }
            }

            private void PuzzleFixEdgesTopAndLeft() {
                for (int xy = 1; xy < dim - 1; xy++) {
                    PuzzlePiece left = puzzle[xy][0];
                    if (left.Seen[3] != 0) {
                        while (left.Seen[3] != 0) {
                            left.Seen = ShiftRight(left.Seen, 1).ToArray();
                            left.Raw = RotateRight(left.Raw);
                        }
                        left.GenEdges();
                    }

                    if (puzzle[xy - 1][0].Edges[2] != left.Edges[0]) {
                        if (puzzle[xy - 1][0].Edges[2] == left.Edges[2]) {
                            left.Raw = FlipY(left.Raw);
                            left.GenEdges();
                        } else {
                            Console.WriteLine("{0},{1}", 0, xy);
                        }
                    }

                    PuzzlePiece up = puzzle[0][xy];
                    if (up.Seen[0] != 0) {
                        while (up.Seen[0] != 0) {
                            up.Seen = ShiftRight(up.Seen, 1).ToArray();
                            up.Raw = RotateRight(up.Raw);
                        }
                        up.GenEdges();
                    }
                    if (puzzle[0][xy - 1].Edges[1] != up.Edges[3]) {
                        if (puzzle[0][xy - 1].Edges[1] == up.Edges[1]) {
                            up.Raw = FlipX(up.Raw);
                            up.GenEdges();
                        } else {
                            Console.WriteLine("{0},{1}", 0, xy);
                        }
                    }
                }
            }

            private void PuzzleFixCornerTopRight() {
                if (!puzzle[0][dim - 2].Edges.Contains(topRight.Edges[3])) {
                    //Console.WriteLine("Top-Right invert");
                    topRight.Raw = InvertForTR(topRight.Raw);
                    topRight.GenEdges();
                }
            }

            private void PuzzleFixCornerBottomLeft() {
                //I never needed this for the example and my given input.
                if (!puzzle[dim - 2][0].Edges.Contains(bottomLeft.Edges[0])) {
                    throw new Exception();
                    //Console.WriteLine("Bottom-Left invert");
                    //bottomLeft.Raw = InvertForBR(bottomLeft.Raw); //Not tested
                    //bottomLeft.GenEdges();
                }
            }

            private void PuzzleFixEdgesBottomAndRight() {
                for (int xy = 1; xy < dim - 1; xy++) {
                    PuzzlePiece right = puzzle[xy][dim - 1];
                    if (right.Seen[1] != 0) {
                        while (right.Seen[1] != 0) {
                            right.Seen = ShiftRight(right.Seen, 1).ToArray();
                            right.Raw = RotateRight(right.Raw);
                        }
                        right.GenEdges();
                    }

                    if (puzzle[xy - 1][dim - 1].Edges[2] != right.Edges[0]) {
                        if (puzzle[xy - 1][dim - 1].Edges[2] == right.Edges[2]) {
                            right.Raw = FlipY(right.Raw);
                            right.GenEdges();
                        } else {
                            Console.WriteLine("{0},{1}", 0, xy);
                        }
                    }

                    //--

                    PuzzlePiece bottom = puzzle[dim - 1][xy];
                    if (bottom.Seen[2] != 0) {
                        while (bottom.Seen[2] != 0) {
                            bottom.Seen = ShiftRight(bottom.Seen, 1).ToArray();
                            bottom.Raw = RotateRight(bottom.Raw);
                        }
                        bottom.GenEdges();
                    }

                    if (puzzle[dim - 1][xy - 1].Edges[1] != bottom.Edges[3]) {
                        if (puzzle[dim - 1][xy - 1].Edges[1] == bottom.Edges[1]) {
                            bottom.Raw = FlipX(bottom.Raw);
                            bottom.GenEdges();
                        } else {
                            Console.WriteLine("{0},{1}", 0, xy);
                        }
                    }
                }
            }

            private void PuzzleFixCornerBottomRight() {
                if (!puzzle[dim - 1][dim - 2].Edges.Contains(bottomRight.Edges[3]) && !puzzle[dim - 2][dim - 1].Edges.Contains(bottomRight.Edges[0])) {
                    throw new Exception();
                    //Console.WriteLine("Bottom-right invert");
                    //bottomRight.Raw = InvertForBR(bottomRight.Raw);
                    //bottomRight.GenEdges();
                }
            }

            private void PuzzleMiddleRotate() {
                //This might be incomplete.
                for (int x = 1; x < dim - 1; x++) {
                    for (int y = 1; y < dim - 1; y++) {
                        PuzzlePiece above = puzzle[y - 1][x];
                        PuzzlePiece left = puzzle[y][x - 1];
                        PuzzlePiece here = puzzle[y][x];

                        while (true) {
                            int mAbove = Array.IndexOf(here.Edges, above.Edges[2]);
                            int mLeft = Array.IndexOf(here.Edges, left.Edges[1]);
                            //int mAboveF = Array.IndexOf(here.EdgesFlipped, above.Edges[2]);
                            //int mLeftF = Array.IndexOf(here.EdgesFlipped, left.Edges[1]);

                            if (mAbove == 0 && mLeft == 3)
                                break;

                            if (mAbove > 0) {
                                int times = 4 - mAbove;
                                for (int t = 0; t < times; t++)
                                    here.Raw = RotateRight(here.Raw);
                                here.GenEdges();
                            } else if (mAbove == -1 && mLeft == -1) {
                                here.Raw = InvertForTR(here.Raw);
                                here.GenEdges();
                            } else if (mAbove == -1) {
                                here.Raw = FlipX(here.Raw);
                                here.GenEdges();
                            } else
                                throw new Exception();
                        }
                    }
                }
            }

            private void DrawMapIDs() {
                for (int y = 0; y < dim; y++) {
                    for (int x = 0; x < dim; x++) {
                        PuzzlePiece jig = puzzle[y][x];
                        if (jig != null)
                            Console.Write("{0} ", jig.ID);
                        else
                            Console.Write(" ??  ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

            private void DrawTilesWithBorders() {
                for (int y = 0; y < dim; y++) {

                    for (int x = 0; x < dim; x++)
                        Console.Write("{0,-10} ", string.Join(',', puzzle[y][x].Seen));
                    Console.WriteLine();
                    for (int rawY = 0; rawY < 10; rawY++) {
                        for (int x = 0; x < dim; x++)
                            Console.Write("{0} ", new string(puzzle[y][x].Raw[rawY]));
                        Console.WriteLine();
                    }
                    Console.WriteLine();
                }
            }

            private void MergeCompleteBoard() {
                //This cuts off the borders of each tile.
                for (int y = 0; y < dim; y++) {
                    for (int rawY = 1; rawY < 9; rawY++) {
                        for (int x = 0; x < dim; x++)
                            Array.Copy(puzzle[y][x].Raw[rawY], 1, complete[(y * 8) + (rawY - 1)], (x * 8), 8);
                    }
                }
            }

            private int TheSeaMonstersPart(bool outputExtra) {
                string[] monsterS = {
                    "                  # ",
                    "#    ##    ##    ###",
                    " #  #  #  #  #  #   "
                };
                List<(int Y, int X)> monsterYX = new List<(int, int)>();
                for (int y = 0; y < monsterS.Length; y++) {
                    for (int x = 0; x < monsterS[y].Length; x++) {
                        if (monsterS[y][x] == '#')
                            monsterYX.Add((y, x));
                    }
                }

                int limitY = complete.Length - monsterS.Length + 1;
                int limitX = complete[0].Length - monsterS[0].Length + 1;

                NumSeaMonsters = 0;
                int rotations = 0;
                bool flipX = false;

                while (NumSeaMonsters == 0) {
                    for (int y = 0; y < limitY; y++) {
                        for (int x = 0; x < limitX; x++) {
                            int count = 0;
                            foreach ((int Y, int X) point in monsterYX) {
                                if (complete[y + point.Y][x + point.X] == '#')
                                    count++;
                            }
                            if (count == monsterYX.Count) {
                                NumSeaMonsters++;
                                foreach ((int Y, int X) point in monsterYX)
                                    complete[y + point.Y][x + point.X] = 'O';
                            }
                        }
                    }

                    if (NumSeaMonsters == 0) {
                        if (rotations < 4) {
                            rotations++;
                            complete = RotateRight(complete);
                        } else {
                            if (!flipX) {
                                flipX = true;
                                complete = FlipX(complete);
                                rotations = 0;
                            } else {
                                Console.WriteLine("No monsters found :(");
                                throw new Exception();
                            }
                        }
                    }
                }

                if(outputExtra)
                    Console.WriteLine("Rotations (right): {0}, flipX {1}\r\n", rotations, (flipX ? "yes" : "no"));

                Draw2DFancy(complete);

                int partB = 0;
                for (int y = 0; y < complete.Length; y++)
                    partB += complete[y].Count(c => c == '#');
                return partB;
            }
        }

        private class PuzzlePiece {
            public int ID;
            public char[][] Raw;
            public string[] Edges;
            public string[] EdgesFlipped;
            public int[] Seen;
            public HashSet<int> Seen2;

            public PuzzlePiece(int num, string[] tile) {
                ID = num;
                Raw = new char[tile.Length][];
                for (int i = 0; i < Raw.Length; i++)
                    Raw[i] = tile[i].ToCharArray();
                Seen = new int[4];
                Seen2 = new HashSet<int>();
                GenEdges();
            }

            public void GenEdges() {
                Edges = new string[4];
                Edges[0] = new string(Raw.First());
                Edges[1] = new string(Raw.Select(s => s.Last()).ToArray());
                Edges[2] = new string(Raw.Last());
                Edges[3] = new string(Raw.Select(s => s.First()).ToArray());

                EdgesFlipped = new string[4];
                EdgesFlipped[0] = new string(Edges[0].Reverse().ToArray());
                EdgesFlipped[1] = new string(Edges[1].Reverse().ToArray());
                EdgesFlipped[2] = new string(Edges[2].Reverse().ToArray());
                EdgesFlipped[3] = new string(Edges[3].Reverse().ToArray());
            }

            public override string ToString() {
                return ID.ToString();
            }
        }
    }
}
