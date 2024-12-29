using System;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Reindeer Olympics" + Environment.NewLine);

            //int puzzleTime = 1000; //Test
            int puzzleTime = 2503;

            List<Reindeer> reindeer = new List<Reindeer>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] fields = line.Split(' ');
                string name = fields[0];
                int kms = int.Parse(fields[3]);
                int seconds = int.Parse(fields[6]);
                int restTime = int.Parse(fields[13]);

                reindeer.Add(new Reindeer(name, kms, seconds, restTime));
            }

            for (int z = 0; z < puzzleTime; z++) {
                foreach(Reindeer deer in reindeer) {
                    if (deer.StateResting) {
                        if (deer.Current == deer.RestTime - 2) {
                            deer.StateResting = false;
                            deer.Current = -1;
                        }
                    } else {
                        if (deer.Current == deer.RaceTime) {
                            deer.StateResting = true;
                            deer.Current = -1;
                        } else {
                            deer.Travelled += deer.Speed;
                        }
                    }

                    deer.Current++;

                    //Console.WriteLine("At {0} of {3} seconds has reached {1} km {2}", z+1, deer.Travelled, deer.StateResting ? ("(resting " + (deer.Current+1) + " of " + restTime + ")") : "", puzzleTime);
                }

                int highest = reindeer.Max(x => x.Travelled);
                List<Reindeer> pointEarners = reindeer.FindAll(x => x.Travelled == highest);
                foreach(Reindeer deer in pointEarners) {
                    deer.Points++;
                }
            }

            Reindeer? fastest = reindeer.MaxBy(x => x.Travelled);
            Reindeer? stylish = reindeer.MaxBy(x => x.Points);

            foreach (Reindeer deer in reindeer) {
                Console.WriteLine("{0} got {1} points and travelled {2} km.", deer.Name, deer.Points, deer.Travelled);
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: {0} travelled {1} km.", fastest.Name, fastest.Travelled);
            //Answer: 2585 is too low
            Console.WriteLine("Part 2: {0} got {1} points.", stylish.Name, stylish.Points);
            //Answer: 
        }

        private class Reindeer {
            public string Name;
            public int Speed;
            public int RaceTime;
            public int RestTime;

            public bool StateResting;
            public int Travelled;
            public int Current;
            public int Points;

            public Reindeer(string n, int kms, int seconds, int restTime) {
                Name = n;
                Speed = kms;
                RaceTime = seconds;
                RestTime = restTime;

                StateResting = false;
                Travelled = 0;
                Current = 0;
                Points = 0;
            }
        }
    }
}
