namespace AoC.Day
{
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Binary Boarding" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            Dictionary<int, string> boardingPasses = new Dictionary<int, string>();

            foreach (string line in lines) {
                int minRow = 0, maxRow = 127;
                int minCol = 0, maxCol = 7;

                for (int i = 0; i < line.Length; i++) {
                    switch (line[i]) {
                        case 'F':
                            maxRow = ((maxRow - minRow) / 2) + minRow;
                            break;
                        case 'B':
                            minRow = ((maxRow - minRow) / 2) + 1 + minRow;
                            break;
                        case 'L':
                            maxCol = ((maxCol - minCol) / 2) + minCol;
                            break;
                        case 'R':
                            minCol = ((maxCol - minCol) / 2) + 1 + minCol;
                            break;
                    }
                }

                int id = (minRow * 8) + minCol;
                boardingPasses.Add(id, string.Format("Seat ID {0} = Row {1}, Col {2}", id, minRow, minCol));
            }

            boardingPasses = boardingPasses.OrderBy(b => b.Key).ToDictionary();

            int partA = boardingPasses.Max(b => b.Key);
            int partB = -1;
            int prev = boardingPasses.Min(b => b.Key) - 1;
            foreach (KeyValuePair<int, string> boardingPass in boardingPasses) {
                if (boardingPass.Key > prev + 1) {
                    partB = prev + 1;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                prev = boardingPass.Key;
                Console.WriteLine(boardingPass.Value);
            }
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 822
            Console.WriteLine("Part 2: " + partB);
            //Answer: 705
        }
    }
}
