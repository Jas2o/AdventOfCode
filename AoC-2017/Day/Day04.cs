namespace AoC.Day
{
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: High-Entropy Passphrases" + Environment.NewLine);

            int numValid_A = 0;
            int numValid_B = 0;

			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] parts = line.Split(' ');

                //Part 1
                bool valid_A = true; 
                Array.Sort(parts); 
                for (int i = 1; i < parts.Length; i++) {
                    if (parts[i] == parts[i - 1]) {
                        valid_A = false;
                        break;
                    }
                }
                if (valid_A) {
                    numValid_A++;
                    //Console.WriteLine(line);
                }

                //Part 2
                bool valid_B = true; 
                for (int i = 0; i < parts.Length; i++) {
                    parts[i] = new string(parts[i].ToCharArray().Order().ToArray());
                }
                Array.Sort(parts);
                for (int i = 1; i < parts.Length; i++) {
                    if (parts[i] == parts[i - 1]) {
                        valid_B = false;
                        break;
                    }
                }
                if (valid_B) {
                    numValid_B++;
                    Console.WriteLine(string.Join(' ', parts));
                }
            }

			Console.WriteLine();
            Console.WriteLine("Part 1: " + numValid_A);
            //Answer: 451
            Console.WriteLine("Part 2: " + numValid_B);
            //Answer: 223
        }
    }
}
