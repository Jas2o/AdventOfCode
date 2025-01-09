namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Stream Processing" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                //There's only one line in the real input.

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(line);
                Console.ResetColor();
                int groupLevel = 0;
                int groups = 0;
                int score = 0;
                int garbageCount = 0;
                bool garbageMode = false;
                bool ignoreMode = false;
                for (int i = 0; i < line.Length; i++) {
                    if(ignoreMode) {
                        ignoreMode = false;
                        continue;
                    }
                    char c = line[i];

                    if (c == '!') {
                        ignoreMode = true;
                        continue;
                    }

                    if(garbageMode) {
                        if (c == '>')
                            garbageMode = false;
                        else
                            garbageCount++;
                    } else {
                        if (c == '<')
                            garbageMode = true;
                        else {
                            if (c == '{') {
                                groupLevel++;
                            } else if (c == '}') {
                                score += groupLevel;
                                groups++;
                                groupLevel--;
                            }
                            Console.Write(line[i]);
                        }
                    }
                }

                Console.WriteLine("\r\n");
                Console.WriteLine("Part 1: " + score);
                //Answer: 21037
                Console.WriteLine("Part 2: " + garbageCount);
                //Answer: 9495

                Console.WriteLine();
            }
        }
    }
}
