namespace AoC.Day {
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: I Was Told There Would Be No Math" + Environment.NewLine);

            int partA = 0;
            int partB = 0;

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                int[] num = Array.ConvertAll(line.Split('x'), int.Parse);

                int side1 = num[0] * num[1];
                int side2 = num[1] * num[2];
                int side3 = num[2] * num[0];
                int slack = Math.Min(Math.Min(side1, side2), side3);
                int totalwrap = (2 * side1) + (2 * side2) + (2 * side3) + slack;

                int small1 = num.Min();
                int small2 = num.OrderBy(num => num).ElementAt(1);
                int ribbon = small1 + small1 + small2 + small2;
                int bow = num[0] * num[1] * num[2];

                int totalribbon = ribbon + bow;
                
                partA += totalwrap;
                partB += totalribbon;
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 1586300
            Console.WriteLine("Part 2: " + partB);
            //Answer: 3737498
        }
    }
}
