namespace AoC.Day
{
    public class Day24
    {
        //Pretty slow, but got the job done.

        public static void Run(string file) {
            Console.WriteLine("Day 24: Lobby Layout" + Environment.NewLine);

            List<Hex> floor = new List<Hex>();
            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                //Cube coordinates (flat east/west)
                int q = 0;
                int r = 0;
                int s = 0;

                char previous = '-';
                foreach(char c in line) {
                    if(c == 'n')
                        r--;
                    else if (c == 's')
                        r++;
                    else if (c == 'e') {
                        if(previous == 'n')
                            q++;
                        else if(previous == 's')
                            s--;
                        else {
                            q++;
                            s--;
                        }
                    } else if (c == 'w') {
                        if (previous == 's')
                            q--;
                        else if (previous == 'n')
                            s++;
                        else {
                            q--;
                            s++;
                        }
                    }
                    previous = c;
                }

                Hex hex = floor.Find(h => h.Q == q && h.R == r);//&& h.S == s
                if (hex == null) {
                    hex = new Hex(q, r, s);
                    floor.Add(hex);
                } else
                    floor.Remove(hex);
            }
            
            List<Hex> daily = new List<Hex>(floor);
            List<Hex> add = new List<Hex>();
            List<Hex> remove = new List<Hex>();
            for (int day = 1; day <= 100; day++) {
                int minQ = daily.Min(d => d.Q) - 1;
                int maxQ = daily.Max(d => d.Q) + 1;
                int minR = daily.Min(d => d.R) - 1;
                int maxR = daily.Max(d => d.R) + 1;
                int minS = daily.Min(d => d.S) - 1;
                int maxS = daily.Max(d => d.S) + 1;

                for(int q = minQ; q <= maxQ; q++) {
                    for (int r = minR; r <= maxR; r++) {
                        for (int s = minS; s <= maxS; s++) {
                            //Faster to nest another loop than calculate: s = -q - r;
                            if (q + r + s != 0)
                                continue;

                            Hex tile = daily.Find(d => d.Q == q && d.R == r);// && d.S == s

                            Hex nw = daily.Find(d => d.Q == q && d.R == r - 1 && d.S == s + 1);
                            Hex ne = daily.Find(d => d.Q == q + 1 && d.R == r - 1 && d.S == s);
                            Hex e = daily.Find(d => d.Q == q + 1 && d.R == r && d.S == s - 1);
                            Hex w = daily.Find(d => d.Q == q - 1 && d.R == r && d.S == s + 1);
                            Hex se = daily.Find(d => d.Q == q && d.R == r + 1 && d.S == s - 1);
                            Hex sw = daily.Find(d => d.Q == q - 1 && d.R == r + 1 && d.S == s);

                            int adjB = 0;
                            if (nw != null) adjB++;
                            if (ne != null) adjB++;
                            if (e != null) adjB++;
                            if (w != null) adjB++;
                            if (se != null) adjB++;
                            if (sw != null) adjB++;

                            if(tile == null) {
                                //White
                                if (adjB == 2) {
                                    tile = new Hex(q, r, s);
                                    add.Add(tile);
                                }
                            } else {
                                //Black
                                if (adjB == 0 || adjB > 2)
                                    remove.Add(tile);
                            }
                        }
                    }
                }

                foreach (Hex tile in remove)
                    daily.Remove(tile);
                foreach (Hex tile in add)
                    daily.Add(tile);
                remove.Clear();
                add.Clear();

                Console.WriteLine("Day {0}: {1}", day, daily.Count);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + floor.Count);
            //Answer: 266
            Console.WriteLine("Part 2: " + daily.Count);
            //Answer: 3627
        }

        private class Hex {
            public int Q;
            public int R;
            public int S;

            public Hex(int q, int r, int s) {
                Q = q;
                R = r;
                S = s;
            }

            public override string ToString() {
                return string.Format("{0},{1},{2}", Q, R, S);
            }
        }
    }
}
