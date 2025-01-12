namespace AoC.Day
{
    public class Day20
    {
        private const int X = 0;
        private const int Y = 1;
        private const int Z = 2;

        public static void Run(string file) {
            Console.WriteLine("Day 20: Particle Swarm" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            List<Particle> particlesP1 = Setup(lines);
            List<Particle> particlesP2 = Setup(lines);

            int numTicks = 1000; //329 was the min for my input., but 1000 doesn't take too long.
            //Part 1
            for (int t = 0; t < numTicks; t++) {
                for (int p = 0; p < particlesP1.Count; p++)
                    particlesP1[p].Tick();
            }
            Particle closest = particlesP1.MinBy(p => p.Manhattan);

            //Part 2
            int lastCollisionTick = 0;
            for (int t = 0; t < numTicks; t++) {
                if (t >= lastCollisionTick + 20) //Just end the search earlier.
                    break;

                for (int look = 0; look < particlesP2.Count; look++) {
                    int[] pos = particlesP2[look].Position;

                    List<Particle> collision = particlesP2.Where(p => p.Position.SequenceEqual(pos)).ToList();
                    if (collision.Count > 1) {
                        lastCollisionTick = t;
                        foreach (Particle p in collision) {
                            Console.WriteLine("Destroy #" + p.Num);
                            particlesP2.Remove(p);
                        }
                    }
                }

                for (int p = 0; p < particlesP2.Count; p++)
                    particlesP2[p].Tick();
            }

			Console.WriteLine();
            Console.WriteLine("Part 1: " + closest.Num);
            //Answer: 170
            Console.WriteLine("Part 2: " + particlesP2.Count);
            //Answer: 571
        }

        private class Particle {
            public int Num;

            public int[] Position;
            public int[] Velocity;
            public int[] Acceleration;
            public int Manhattan;

            public Particle(int num, int[] p, int[] v, int[] a) {
                Num = num;
                Position = p;
                Velocity = v;
                Acceleration = a;
                UpdateManhattan();
            }

            public void Tick() {
                Velocity[X] += Acceleration[X];
                Velocity[Y] += Acceleration[Y];
                Velocity[Z] += Acceleration[Z];
                Position[X] += Velocity[X];
                Position[Y] += Velocity[Y];
                Position[Z] += Velocity[Z];
                UpdateManhattan();
            }

            private void UpdateManhattan() {
                Manhattan = Math.Abs(Position[X]) + Math.Abs(Position[Y]) + Math.Abs(Position[Z]);
            }

            public override string ToString() {
                return string.Format("{0},{1},{2}", Position[X], Position[Y], Position[Z]);
            }
        }

        private static List<Particle> Setup(string[] lines) {
            List<Particle> particles = new List<Particle>();
            foreach (string line in lines) {
                string[] split = line.Split('<').Skip(1).ToArray();
                split[0] = split[0].Split('>').First();
                split[1] = split[1].Split('>').First();
                split[2] = split[2].Split('>').First();

                int[] p = Array.ConvertAll(split[0].Split(','), int.Parse);
                int[] v = Array.ConvertAll(split[1].Split(','), int.Parse);
                int[] a = Array.ConvertAll(split[2].Split(','), int.Parse);

                Particle particle = new Particle(particles.Count, p, v, a);
                particles.Add(particle);
            }
            return particles;
        }
    }
}
