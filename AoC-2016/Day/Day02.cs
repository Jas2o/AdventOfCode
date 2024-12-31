using System.Drawing;
using System.Text;

namespace AoC.Day {
    public class Day02
    {
        private static string[] padNumA = {"     ",
                                           " 123 ",
                                           " 456 ",
                                           " 789 ",
                                           "     "};
        private static string[] padNumB = {"       ",
                                           "   1   ",
                                           "  234  ",
                                           " 56789 ",
                                           "  ABC  ",
                                           "   D   ",
                                           "       "};

        public static void Run(string file) {
            Console.WriteLine("Day 2: Bathroom Security" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            StringBuilder codeA = new StringBuilder();
            StringBuilder codeB = new StringBuilder();

            //Starting at '5', these are x,y
            Point posA = new Point(2,2);
            Point posB = new Point(1,3);

            foreach (string line in lines) {
                for(int i = 0; i < line.Length; i++) {
                    char c = line[i];
                    switch(c) {
                        case 'U':
                            KeyMove(padNumA, ref posA, 0, -1);
                            KeyMove(padNumB, ref posB, 0, -1);
                            break;
                        case 'D':
                            KeyMove(padNumA, ref posA, 0, 1);
                            KeyMove(padNumB, ref posB, 0, 1);
                            break;
                        case 'L':
                            KeyMove(padNumA, ref posA, -1, 0);
                            KeyMove(padNumB, ref posB, -1, 0);
                            break;
                        case 'R':
                            KeyMove(padNumA, ref posA, 1, 0);
                            KeyMove(padNumB, ref posB, 1, 0);
                            break;
                    }
                }
                codeA.Append(padNumA[posA.Y][posA.X]);
                codeB.Append(padNumB[posB.Y][posB.X]);
            }

            Console.WriteLine("Part 1: " + codeA.ToString());
            //Answer: 38961
            Console.WriteLine("Part 2: " + codeB.ToString());
            //Answer: 46C92
        }

        private static bool KeyMove(string[] pad, ref Point pos, int diffX, int diffY) {
            int x = pos.X + diffX;
            int y = pos.Y + diffY;
            if (pad[y][x] == ' ')
                return false;
            pos = new Point(x, y);
            return true;
        }
    }
}
