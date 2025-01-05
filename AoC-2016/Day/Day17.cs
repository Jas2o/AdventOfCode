using System.Drawing;
using System.Text;

namespace AoC.Day {
    public class Day17
    {
        public static void Run(string file) {
            Console.WriteLine("Day 17: Two Steps Forward" + Environment.NewLine);

            string input = File.ReadAllText(file);
            Console.WriteLine(input);

            Tuple<string, int> result = SolveWithQueue(input);
                
            Console.WriteLine();
            Console.WriteLine("Part 1: {0} ({1})", result.Item1, result.Item1.Length);
            //Answer: DURLDRRDRD
            Console.WriteLine("Part 2: " + result.Item2);
            //Answer: 650
        }

        private class MD5Worker {
            private System.Security.Cryptography.MD5 md5;

            public MD5Worker() {
                md5 = System.Security.Cryptography.MD5.Create();
                //Using create a lot slows it down.
            }

            public string Get(string input) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes).ToLower();
            }
        }

        private static Tuple<string, int> SolveWithQueue(string input) {
            MD5Worker md5worker = new MD5Worker();
            char[] open = ['b', 'c', 'd', 'e', 'f'];

            string shortestPath = string.Empty;
            int shortestLen = int.MaxValue;
            int longestLen = int.MinValue;

            Queue<Tuple<string, Point>> queue = new Queue<Tuple<string, Point>>();
            queue.Enqueue(new Tuple<string, Point>("", new Point(0, 0)));

            while (queue.Count > 0) {
                Tuple<string, Point> state = queue.Dequeue();
                string path = state.Item1;
                Point point = state.Item2;

                //Console.WriteLine(input + " " + path);

                if (point.X == 3 && point.Y == 3) {
                    int len = path.Length;
                    if (len < shortestLen) {
                        shortestPath = path;
                        shortestLen = len;
                    }

                    if (len > longestLen)
                        longestLen = len;

                    continue;
                }

                string doors = md5worker.Get(input + path).Substring(0, 4);
                //Console.WriteLine(doors);

                if(point.Y > 0 && open.Contains(doors[0]))
                    queue.Enqueue(new Tuple<string, Point>(path+"U", new Point(point.X, point.Y - 1)));
            
                if (point.Y < 3 && open.Contains(doors[1]))
                    queue.Enqueue(new Tuple<string, Point>(path + "D", new Point(point.X, point.Y + 1)));
            
                if (point.X > 0 && open.Contains(doors[2]))
                    queue.Enqueue(new Tuple<string, Point>(path + "L", new Point(point.X - 1, point.Y)));
            
                if (point.X < 3 && open.Contains(doors[3]))
                    queue.Enqueue(new Tuple<string, Point>(path + "R", new Point(point.X + 1, point.Y)));
            
                //Console.WriteLine(doors);
                //Console.WriteLine();
            }

            return new Tuple<string, int>(shortestPath, longestLen);
        }
    }
}
