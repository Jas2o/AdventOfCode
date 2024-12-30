namespace AoC.Day {
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Matchsticks" + Environment.NewLine);

            int totalCodeA = 0;
            int totalMemoryA = 0;
            int totalCodeB = 0;
            //int totalMemoryB = 0;

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int lenCodeA = line.Length;
                int lenInsideA = 0;

                int lenCodeB = lenCodeA + 4;
                //int lenInsideB = 0;
                for (int i = 1; i < lenCodeA-1; i++) {
                    char c = line[i];
                    if (c == '\\') {
                        char cn = line[i+1];
                        i++;

                        if (cn == '\\' || cn == '\"') {
                            lenInsideA++;
                            lenCodeB += 2;
                        } else if (cn == 'x') {
                            i += 2;
                            //char next = line[i];
                            lenInsideA++;
                            lenCodeB += 1;
                        } else {
                            throw new Exception();
                        }
                    } else {
                        lenInsideA++;
                        //Console.WriteLine();
                    }
                }

                totalCodeA += lenCodeA;
                totalMemoryA += lenInsideA;
                totalCodeB += lenCodeB;
                //totalMemoryB += lenInsideB;
                Console.WriteLine("{0} : {1} or {2}", lenCodeA, lenInsideA, lenCodeB);
            }

            int partA = totalCodeA - totalMemoryA;
            int partB = totalCodeB - totalCodeA;

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1342
            Console.WriteLine("Part 2: " + partB);
            //Answer: 2074
        }
    }
}
