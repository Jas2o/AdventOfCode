namespace AoC.Day {
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Resonant Collinearity" + Environment.NewLine);

            //Build map
            string[] all = File.ReadAllLines(file);
            char[][] mapAntenna = new char[all.Length][];
            byte[][] mapAntinode = new byte[all.Length][];
            Dictionary<char, List<int[]>> dFrequency = new Dictionary<char, List<int[]>>();

            for (int i = 0; i < all.Length; i++) {
                mapAntenna[i] = new char[all[i].Length];
                mapAntinode[i] = new byte[all[i].Length];

                for (int j = 0; j < all[i].Length; j++) {
                    mapAntenna[i][j] = all[i][j];
                    if (all[i][j] == '.')
                        continue;

                    if(!dFrequency.ContainsKey(all[i][j]))
                        dFrequency.Add(all[i][j], new List<int[]>());

                    dFrequency[all[i][j]].Add(new int[] { i, j });
                }
            }

            //Check for antinodes
            Check(mapAntenna, mapAntinode, dFrequency);
            DrawMap(mapAntenna, mapAntinode, false);
            Console.WriteLine();

            int partA = 0;
            for (int y = 0; y < mapAntinode.Length; y++) {
                partA += mapAntinode[y].Count(x => x == 1);
            }

            DrawMap(mapAntenna, mapAntinode, true);

            int partB = 0;
            for (int y = 0; y < mapAntinode.Length; y++) {
                partB += mapAntinode[y].Count(x => x > 0);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 344
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1182
        }

        private static void Check(char[][] mapAntenna, byte[][] mapAntinode, Dictionary<char, List<int[]>> dFrequency) {
            foreach (KeyValuePair<char, List<int[]>> pair in dFrequency) {
                //Console.WriteLine(pair.Key);

                for (int a = 0; a < pair.Value.Count; a++) {
                    for (int b = 0; b < pair.Value.Count; b++) {
                        if (a == b)
                            continue;

                        int minY = Math.Min(pair.Value[a][0], pair.Value[b][0]);
                        int minX = Math.Min(pair.Value[a][1], pair.Value[b][1]);

                        int diffY = pair.Value[b][0] - pair.Value[a][0];
                        int diffX = pair.Value[b][1] - pair.Value[a][1];

                        int aY = pair.Value[a][0] - diffY;
                        int aX = pair.Value[a][1] - diffX;
                        int bY = pair.Value[b][0] + diffY;
                        int bX = pair.Value[b][1] + diffX;

                        if (aY >= 0 && aY < mapAntenna.Length && aX >= 0 && aX < mapAntenna[0].Length)
                            mapAntinode[aY][aX] = 1;
                        if (bY >= 0 && bY < mapAntenna.Length && bX >= 0 && bX < mapAntenna[0].Length)
                            mapAntinode[bY][bX] = 1;

                        if (mapAntinode[pair.Value[a][0]][pair.Value[a][1]] == 0)
                            mapAntinode[pair.Value[a][0]][pair.Value[a][1]] = 2;
                        if (mapAntinode[pair.Value[b][0]][pair.Value[b][1]] == 0)
                            mapAntinode[pair.Value[b][0]][pair.Value[b][1]] = 2;

                        while (true) {
                            aY = aY - diffY;
                            aX = aX - diffX;

                            if (aY >= 0 && aY < mapAntenna.Length && aX >= 0 && aX < mapAntenna[0].Length) {
                                if (mapAntinode[aY][aX] == 1)
                                    continue;
                                mapAntinode[aY][aX] = 2;
                            } else
                                break;
                        }

                        while (true) {
                            bY = bY + diffY;
                            bX = bX + diffX;

                            if (bY >= 0 && bY < mapAntenna.Length && bX >= 0 && bX < mapAntenna[0].Length) {
                                if (mapAntinode[bY][bX] == 1)
                                    continue;
                                mapAntinode[bY][bX] = 2;
                            } else
                                break;
                        }
                    }
                }
            }
        }

        private static void DrawMap(char[][] antenna, byte[][] antinode, bool isPart2) {
            for (int y = 0; y < antenna.Length; y++) {
                for (int x = 0; x < antenna[y].Length; x++) {
                    char c = antenna[y][x];
                    byte a = antinode[y][x];
                    bool checkPart2 = (isPart2 && a == 2);

                    if (c == '.') {
                        if (a == 1 || checkPart2)
                            c = '#';
                    } else
                        Console.ForegroundColor = ConsoleColor.Cyan;

                    if (a == 1)
                        Console.ForegroundColor = ConsoleColor.Red;
                    if (checkPart2)
                        Console.ForegroundColor = ConsoleColor.Green;

                    //c = antinode[y][x].ToString()[0];
                    Console.Write(c);

                    Console.ResetColor();
                }
                Console.WriteLine();
            }
        }
    }
}
