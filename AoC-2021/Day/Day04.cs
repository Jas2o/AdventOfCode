namespace AoC.Day
{
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Giant Squid" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int[] notRandom = Array.ConvertAll(lines[0].Split(','), int.Parse);
            List<BingoBoard> boards = new List<BingoBoard>();
            for(int i = 2; i < lines.Length; i += 6) {
                BingoBoard board = new BingoBoard(lines.Skip(i).Take(5).ToArray());
                boards.Add(board);
            }

            List<BingoBoard> winOrder = new List<BingoBoard>();

            int dim = 5;
            foreach (int num in notRandom) {
                foreach (BingoBoard board in boards) {
                    if (board.Score != 0)
                        continue;

                    HashSet<int> markedRow = new HashSet<int>();
                    HashSet<int> markedCol = new HashSet<int>();
                    bool win = false;

                    for (int row = 0; row < dim; row++) {
                        for (int col = 0; col < dim; col++) {
                            if (board.Numbers[row][col] == num) {
                                board.Marked[row][col] = true;
                                markedRow.Add(row);
                                markedCol.Add(col);
                            }
                        }
                    }
                    foreach (int row in markedRow) {
                        if (board.Marked[row].Count(m => m == true) == dim)
                            win = true;
                    }
                    foreach (int col in markedCol) {
                        IEnumerable<bool> marked = board.Marked.Select(r => r[col]);
                        if (marked.Count(m => m == true) == dim)
                            win = true;
                    }

                    if (win) {
                        int sumUnmarked = 0;
                        for (int row = 0; row < dim; row++) {
                            for (int col = 0; col < dim; col++) {
                                if (!board.Marked[row][col])
                                    sumUnmarked += board.Numbers[row][col];
                            }
                        }
                        board.Score = sumUnmarked * num;
                        board.LastNumber = num;
                        winOrder.Add(board);
                    }
                }
            }

            Draw(winOrder.First());
            Draw(winOrder.Last());

            int partA = winOrder.First().Score;
            int partB = winOrder.Last().Score;

            Console.WriteLine("Part 1: " + partA);
            //Answer: 38594
            Console.WriteLine("Part 2: " + partB);
            //Answer: 21184
        }

        private static void Draw(BingoBoard board) {
            int dim = 5;
            for (int row = 0; row < dim; row++) {
                for (int col = 0; col < dim; col++) {
                    if (board.Marked[row][col])
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("{0,2} ", board.Numbers[row][col]);
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            Console.WriteLine("Last number: " + board.LastNumber);
            Console.WriteLine("Score: {0}\r\n", board.Score);
        }

        private class BingoBoard {
            public int[][] Numbers;
            public bool[][] Marked;
            public int Score;
            public int LastNumber;

            public BingoBoard(string[] rows) {
                Numbers = new int[5][];
                Marked = new bool[5][];
                Score = 0;
                LastNumber = 0;

                for (int i = 0; i < rows.Length; i++) {
                    Numbers[i] = Array.ConvertAll(rows[i].Split(' ', StringSplitOptions.RemoveEmptyEntries), int.Parse);
                    Marked[i] = new bool[5];
                }
            }

        }
    }
}
