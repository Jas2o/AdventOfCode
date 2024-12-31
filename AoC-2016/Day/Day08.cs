using System.Drawing;

namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: Two-Factor Authentication" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            List<Pixel> display = new List<Pixel>();
            Point bottomRight = new Point(50, 6);
            if (lines.Length < 10) {
                bottomRight.X = 7;
                bottomRight.Y = 3;
            }
            for (int y = 0; y < bottomRight.Y; y++) {
                for (int x = 0; x < bottomRight.X; x++) {
                    Pixel pixel = new Pixel(x, y);
                    display.Add(pixel);
                }
            }

            foreach (string line in lines) {
                string[] parts = line.Split(' ', 2);
                if (parts[0] == "rect")
                    Rectangle(display, parts[1]);
                else if (parts[0] == "rotate")
                    Rotate(display, parts[1], bottomRight);
            }

            DrawDisplay(display, bottomRight);

            Console.WriteLine("Part 1: " + display.Count(p => p.Status == true));
            //Answer: 116
            Console.WriteLine("Part 2: (you'll need to use your eyes to read the display above)");
            //Answer: UPOJFLBCEZ
        }

        private static void Rectangle(List<Pixel> display, string input) {
            string[] num = input.Split('x');
            Point rect = new Point(int.Parse(num[0]), int.Parse(num[1]));
            for (int y = 0; y < rect.Y; y++) {
                for (int x = 0; x < rect.X; x++) {
                    Pixel pixel = display.Find(p => p.X == x && p.Y == y);
                    pixel.Status = true;
                }
            }
        }

        private static void Rotate(List<Pixel> display, string input, Point bottomRight) {
            string[] parts = input.Split(' ');

            if (parts[0] == "column") {
                int rotateX = int.Parse(parts[1].Substring(2));
                int rotateAmount = int.Parse(parts[3]);
                List<Pixel> groupRotate = display.FindAll(p => p.X == rotateX);
                foreach (Pixel pixel in groupRotate) {
                    pixel.Y += rotateAmount;
                    if (pixel.Y >= bottomRight.Y) {
                        pixel.Y = pixel.Y % bottomRight.Y;
                    }
                }
            } else if (parts[0] == "row") {
                int rotateY = int.Parse(parts[1].Substring(2));
                int rotateAmount = int.Parse(parts[3]);
                List<Pixel> groupRotate = display.FindAll(p => p.Y == rotateY);
                foreach (Pixel pixel in groupRotate) {
                    pixel.X += rotateAmount;
                    if (pixel.X >= bottomRight.X) {
                        pixel.X = pixel.X % bottomRight.X;
                    }
                }
            }
        }


        private static void DrawDisplay(List<Pixel> display, Point bottomRight) {
            for (int y = 0; y < bottomRight.Y; y++) {
                for (int x = 0; x < bottomRight.X; x++) {
                    Pixel pixel = display.Find(p => p.X == x && p.Y == y);
                    if (pixel.Status)
                        Console.Write('#');
                    else
                        Console.Write(' ');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        private class Pixel {
            public int X;
            public int Y;
            public bool Status;

            public Pixel(int x, int y) {
                X = x;
                Y = y;
            }

            public override string ToString() {
                return string.Format("x:{0} y:{1} is {2}", X, Y, (Status ? "On!" : "Off"));
            }
        }
    }
}
