namespace AoC.Day {
    public class Day10
    {
        public static void Run(string file) {
            Console.WriteLine("Day 10: Balance Bots" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            Queue<string> queue = new Queue<string>();
            foreach (string line in lines)
                queue.Enqueue(line);

            Dictionary<int, int> output = new Dictionary<int, int>();
            List<Robot> robots = new List<Robot>();
            while(queue.Count > 0) {
                bool resolved = false;
                string line = queue.Dequeue();
                string[] parts = line.Split(' ');
                int indexRobotNum = Array.IndexOf(parts, "bot") + 1;
                int robotNum = int.Parse(parts[indexRobotNum]);
                Robot r = robots.Find(x => x.ID == robotNum);
                if (r == null) {
                    r = new Robot(robotNum);
                    robots.Add(r);
                }

                if (line.StartsWith("value")) {
                    int value = int.Parse(parts[1]);
                    r.Give(value);
                    resolved = true;
                } else if (line.StartsWith("bot")) {
                    if(r.WaitingToGive) {
                        int low = Math.Min(r.ValueA, r.ValueB);
                        int pass1 = int.Parse(parts[6]);
                        if (parts[5] == "output") {
                            output.Add(pass1, low);
                        } else {
                            Robot r2 = robots.Find(x => x.ID == pass1);
                            r2.Give(low);
                        }

                        int high = Math.Max(r.ValueA, r.ValueB);
                        int pass2 = int.Parse(parts[11]);
                        if (parts[10] == "output") {
                            output.Add(pass2, high);
                        } else {
                            Robot r2 = robots.Find(x => x.ID == pass2);
                            r2.Give(high);
                        }

                        r.SetDone();
                        resolved = true;
                    }
                } else {
                    throw new Exception();
                }

                if (!resolved)
                    queue.Enqueue(line);
            }

            int partA = 0;
            int partB = output[0] * output[1] * output[2];

            int outputID_Max = output.MaxBy(x => x.Key).Key;
            for(int i = 0; i < outputID_Max; i++) {
                Console.WriteLine("output bin {0} contains a value-{1} microchip.", i, output[i]);
            }
            robots = robots.OrderBy(x => x.ID).ToList();
            foreach(Robot robot in robots) {
                if ((robot.ValueA == 61 || robot.ValueB == 61) && (robot.ValueA == 17 || robot.ValueB == 17)) {
                    partA = robot.ID;
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("bot {0} compares value-{1} microchips with value-{2} microchips.", robot.ID, robot.ValueA, robot.ValueB);
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 116
            Console.WriteLine("Part 2: " + partB);
            //Answer: 23903
        }

        private class Robot {
            public int ID { get; private set; }
            public int ValueA { get; private set; }
            public int ValueB { get; private set; }
            public bool WaitingToGive { get; private set; }
            public bool Done { get; private set; }

            public Robot(int id) {
                ID = id;
                ValueA = 0;
                ValueB = 0;
            }

            public void Give(int value) {
                if (ValueA == 0) {
                    ValueA = value;
                } else if (ValueB == 0) {
                    ValueB = value;
                    WaitingToGive = true;
                } else
                    throw new Exception();
            }

            public void SetDone() {
                WaitingToGive = false;
                Done = true;
            }
        }
    }
}
