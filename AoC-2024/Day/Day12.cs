namespace AoC.Day {
    public class Day12 {
        public static void Run(string file) {
            Console.WriteLine("Day 12: Garden Groups" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            char[] plants = string.Join("", lines).Distinct().ToArray();

            bool verbose = true;
            bool verboseDraw = lines.Length < 50;

            int totalArea = 0;
            //Add a border to the outside, and also give each plant a "0" region ID.
            string[][] garden = new string[lines.Length + 2][];
            garden[0] = new string[lines[0].Length + 2];
            garden[lines.Length + 1] = new string[lines[0].Length + 2];
            for (int y = 0; y < garden.Length; y++) {
                garden[y] = new string[garden[0].Length];
                for (int x = 0; x < garden[0].Length; x++) {
                    garden[y][x] = "..";
                    if (y > 0 && x > 0 && y < garden.Length - 1 && x < garden.Length - 1) {
                        garden[y][x] = string.Format("{0}{1}", lines[y - 1][x - 1], 0);
                    }
                }
            }

            int partA = 0;
            int partB = 0;
            // For each of the plants, find their regions, area, perimeter, corners, 
            foreach (char plant in plants) {
                if(verbose)
                    Console.WriteLine("Plant: " + plant);

                List<Tuple<char, int, int>> regionInfo = new List<Tuple<char, int, int>>();

                string lookFor = string.Format("{0}{1}", plant, 0);
                
                int islandNum = 1;
                for (int y = 0; y < garden.Length; y++) {
                    for (int x = 0; x < garden[y].Length; x++) {
                        if (garden[y][x] == lookFor) {
                            int area = 0;
                            int perim = 0;
                            string becomes = string.Format("{0}{1}", plant, islandNum);
                            DFS_Area(lines, garden, y, x, lookFor, becomes, ref area, ref perim);
                            perim = Math.Max(perim, 4);

                            if (area > 0) {
                                totalArea += area;

                                if (verbose)
                                    Console.WriteLine("A region of {0} plants with price a:{1} * p:{2} = {3}.", plant, area, perim, area * perim);
                                partA += area * perim;

                                regionInfo.Add(new Tuple<char, int, int>(plant, islandNum, area));

                                islandNum++;
                            }
                        }
                    }
                }

                //--

                for (int island = 1; island < islandNum; island++) {
                    var region = regionInfo.First(x => x.Item2 == island);
                    string pi = string.Format("{0}{1}", region.Item1, region.Item2);

                    List<int[]> corners = new List<int[]>();
                    for (int y = 0; y < garden.Length; y++) {
                        for (int x = 0; x < garden[0].Length; x++) {
                            if (garden[y][x][0] != '.')
                                CornerCheck(lines, garden, corners, y, x, pi);
                        }
                    }

                    if (verbose)
                        Console.WriteLine("[P2] A region of {0} plants with price a:{1} * c:{2} = {3}.", plant, region.Item3, corners.Count, region.Item3 * corners.Count);
                    partB += region.Item3 * corners.Count;

                    //--

                    if (verboseDraw) {
                        for (int y = 0; y < garden.Length; y++) {
                            for (int x = 0; x < garden[0].Length; x++) {
                                if (garden[y][x] == pi) {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                    if (corners.Any(c => c[0] == y && c[1] == x))
                                        Console.ForegroundColor = ConsoleColor.Blue;
                                } else
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write(garden[y][x].Substring(0, 2));
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }
            }

            if(totalArea != lines.Length * lines[0].Length)
                Console.WriteLine("Incorrect area: " + totalArea + " vs " + (lines.Length * lines[0].Length));

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1473620
            Console.WriteLine("Part 2: " + partB);
            //Answer: 902620
        }

        private static void DFS_Area(string[] lines, string[][] visited, int y, int x, string lookFor, string becomes, ref int area, ref int perimeter) {
            //DepthFirstSearch

            if (y == 0 || y >= visited.Length || x == 0 || x >= visited[y].Length) {
                perimeter++;
                return;
            }
            

            string lookingAt = visited[y][x];

            if (lookingAt == lookFor) {
                visited[y][x] = becomes;
                area++;
            } else if (lookingAt == becomes) {
                return;
            } else {
                perimeter++;
                return;
            }

            DFS_Area(lines, visited, y - 1, x, lookFor, becomes, ref area, ref perimeter); // Up
            DFS_Area(lines, visited, y, x + 1, lookFor, becomes, ref area, ref perimeter); // Right
            DFS_Area(lines, visited, y + 1, x, lookFor, becomes, ref area, ref perimeter); // Down
            DFS_Area(lines, visited, y, x - 1, lookFor, becomes, ref area, ref perimeter); // Left
        }

        private static void CornerCheck(string[] lines, string[][] visited, List<int[]> corners, int y, int x, string region) {
            if (y == 0 || x == 0 || y > lines.Length || x > lines[0].Length)
                return;

            string center = visited[y][x];
            if (center != region)
                return;

            int[] here = [y, x];

            bool up = visited[y - 1][x] == region;
            bool down = visited[y + 1][x] == region;
            bool left = visited[y][x - 1] == region;
            bool right = visited[y][x + 1] == region;

            bool upLeft = visited[y - 1][x - 1] == region;
            bool upRight = visited[y - 1][x + 1] == region;
            bool downLeft = visited[y + 1][x - 1] == region;
            bool downRight = visited[y + 1][x + 1] == region;

            if (!upLeft) {
                if (!up && !left)
                    corners.Add(here);
                if (up && left)
                    corners.Add(here);
            } else {
                //Interior
                if (!up && !left)
                    corners.Add(here);
            }

            if (!upRight) {
                if (!up && !right)
                    corners.Add(here);
                if (up && right)
                    corners.Add(here);
            } else {
                //Interior
                if (!up && !right)
                    corners.Add(here);
            }

            if (!downLeft) {
                if (!down && !left)
                    corners.Add(here);
                if (down && left)
                    corners.Add(here);
            } else {
                //Interior
                if (!down && !left)
                    corners.Add(here);
            }

            if (!downRight) {
                if (!down && !right)
                    corners.Add(here);
                if (down && right)
                    corners.Add(here);
            } else {
                //Interior
                if (!down && !right) {
                    corners.Add(here);
                }
            }
        }
    }
}
