using System.Drawing;

namespace AoC.Day
{
    public class Day03
    {
        public static void Run(string file) {
            Console.WriteLine("Day 3: No Matter How You Slice It" + Environment.NewLine);

            List<Claim> claims = new List<Claim>();
			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                Claim c = new Claim(line);
                claims.Add(c);
            }

            Dictionary<(int, int), bool> overlap = new Dictionary<(int, int), bool>();
            for(int i = 0; i < claims.Count; i++) {
                for (int k = i + 1; k < claims.Count; k++) {
                    if (i == k)
                        continue;
                    Rectangle o = claims[i].Rect;
                    Rectangle r = new Rectangle(o.X, o.Y, o.Width, o.Height);
                    r.Intersect(claims[k].Rect);
                    if(r.Width != 0 && r.Height != 0) {
                        claims[i].Overlaps++;
                        claims[k].Overlaps++;
                        for (int rx = 0; rx < r.Width; rx++) {
                            for (int ry = 0; ry < r.Height; ry++) {
                                overlap[(r.X + rx, r.Y + ry)] = true;
                            }
                        }
                    }
                }
            }

            string partB = claims.Find(x => x.Overlaps == 0).ID;

            Console.WriteLine("Part 1: " + overlap.Count);
            //Answer: 111326
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1019
        }

        private class Claim {
            public string ID;
            public Rectangle Rect;
            public int Overlaps;

            public Claim(string line) {
                string[] s = line.Replace(":", "").Replace("x", ",").Split(' ');
                int[] pos = Array.ConvertAll(s[2].Split(','), int.Parse);
                int[] dim = Array.ConvertAll(s[3].Split(','), int.Parse);
                ID = s[0].Substring(1);
                Rect = new Rectangle(pos[0], pos[1], dim[0], dim[1]);
                Overlaps = 0;
            }
        }
    }
}
