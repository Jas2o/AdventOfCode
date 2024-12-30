namespace AoC.Day {
    public class Day01
    {
        public static void Run(string file) {
            Console.WriteLine("Day 1: ???" + Environment.NewLine);

            int current = 0;
            int charOfFirstBasement = 0;

            string text = File.ReadAllText(file);
            for (int i = 0; i < text.Length; i++) {
                if (text[i] == '(')
                    current++;
                else if (text[i] == ')')
                    current--;
                else
                    Console.WriteLine("Help?? " + text[i]);

                if (current == -1 && charOfFirstBasement == 0)
                    charOfFirstBasement = i + 1;
            }

            Console.WriteLine("Part 1: " + current);
            //Answer: 74
            Console.WriteLine("Part 2: " + charOfFirstBasement);
            //Answer: 1795
        }
    }
}
