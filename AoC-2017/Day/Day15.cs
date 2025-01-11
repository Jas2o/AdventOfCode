namespace AoC.Day {
    public class Day15
    {
        public static void Run(string file) {
            Console.WriteLine("Day 15: Dueling Generators" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);
            int startA = int.Parse(lines[0].Substring(24));
            int startB = int.Parse(lines[1].Substring(24));
            int limitA = 40000000;
            int limitB = 5000000;
            int factorA = 16807;
            int factorB = 48271;
            int matchA = 0;
            int matchB = 0;

            //Part 1
            long genA = startA;
            long genB = startB;
            //Console.WriteLine("--Gen. A--  --Gen. B--");
            for (int i = 0; i < limitA; i++) {
                genA = (genA * factorA) % 2147483647;
                genB = (genB * factorB) % 2147483647;
                long compareA = genA & 0xFFFF;
                long compareB = genB & 0xFFFF;
                if (compareA == compareB)
                    matchA++;
                //Console.WriteLine("{0,10} {1,10}", genA, genB);
                //string binA = Convert.ToString(genA, 2).PadLeft(32, '0');
                //string binB = Convert.ToString(genB, 2).PadLeft(32, '0');
                //Console.WriteLine(binA);
                //Console.WriteLine(binB);
                //Console.WriteLine();
            }

            //Part 2
            genA = startA;
            genB = startB;
            for (int i = 0; i < limitB; i++) {
                do {
                    genA = (genA * factorA) % 2147483647;
                } while (genA % 4 != 0);
                do {
                    genB = (genB * factorB) % 2147483647;
                } while (genB % 8 != 0);
                long compareA = genA & 0xFFFF;
                long compareB = genB & 0xFFFF;
                if (compareA == compareB)
                    matchB++;
            }

            Console.WriteLine("Part 1: " + matchA);
            //Answer: 609
            Console.WriteLine("Part 2: " + matchB);
            //Answer: 253
        }
    }
}
