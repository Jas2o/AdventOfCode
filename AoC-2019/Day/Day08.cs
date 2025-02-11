using System.Drawing;

namespace AoC.Day {
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Space Image Format" + Environment.NewLine);

			string input = File.ReadAllText(file);
            Point size = new Point(25, 6);
            if (input.Length < 20)
                (size.X, size.Y) = (3, 2);
            
            int partA = 0;
            int partA_zeroes = int.MaxValue;
            Dictionary<(int, int), char> dImage = new Dictionary<(int, int), char>();

            int layer = 1;
            int pos = 0;
            while (pos < input.Length) {
                Console.WriteLine("Layer {0}:", layer);
                int count0 = 0;
                int count1 = 0;
                int count2 = 0;
                for (int y = 0; y < size.Y; y++) {
                    for (int x = 0; x < size.X; x++) {
                        char c = input[pos];
                        Console.Write(c);
                        switch(c) {
                            case '0': count0++; break;
                            case '1': count1++; break;
                            case '2': count2++; break;
                        }
                        if(c != '2')
                            dImage.TryAdd((x, y), c);
                        pos++;
                    }
                    Console.WriteLine();
                }

                if(count0 < partA_zeroes) {
                    partA_zeroes = count0;
                    partA = count1 * count2;
                }

                layer++;
                Console.WriteLine();
            }

            Console.WriteLine("Decoded:");
            for (int y = 0; y < size.Y; y++) {
                for (int x = 0; x < size.X; x++) {
                    char pixel = dImage[(x, y)];
                    if (pixel == '0') {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write('.');
                    } else if (pixel == '1') {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write('#');
                    } else {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(pixel);
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 2460
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: LRFKU
        }
    }
}
