namespace AoC.Day
{
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: 1202 Program Alarm" + Environment.NewLine);

			string input = File.ReadAllText(file);
            int[] initial = Array.ConvertAll(input.Split(','), int.Parse);
            
            int[] intcode = new int[initial.Length];
            Array.Copy(initial, intcode, initial.Length);
            bool real = (intcode.Length > 10);
            if (real) {
                intcode[1] = 12;
                intcode[2] = 2;
            }
            int partA = Run(intcode);

            int partB = 0;
            if (real) {
                for (int noun = 0; noun < 255; noun++) {
                    try {
                        for (int verb = 0; verb < 255; verb++) {
                            Array.Copy(initial, intcode, initial.Length);
                            intcode[1] = noun;
                            intcode[2] = verb;
                        
                            int result = Run(intcode);
                            if (result == 19690720) {
                                partB = 100 * noun + verb;
                                noun = 255;
                                verb = 255;
                            }
                        }
                    } catch (Exception) {
                    }
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 5482655
            Console.WriteLine("Part 2: " + partB);
            //Answer: 4967
        }

        private static int Run(int[] intcode) {
            int pos = 0;

            bool loop = true;
            while (loop) {
                int read = intcode[pos];
                switch (read) {
                    case 1:
                        int add1 = intcode[intcode[pos + 1]];
                        int add2 = intcode[intcode[pos + 2]];
                        int addD = intcode[pos + 3];
                        intcode[addD] = add1 + add2;
                        pos += 4;
                        break;
                    case 2:
                        int mul1 = intcode[intcode[pos + 1]];
                        int mul2 = intcode[intcode[pos + 2]];
                        int mulD = intcode[pos + 3];
                        intcode[mulD] = mul1 * mul2;
                        pos += 4;
                        break;
                    case 99:
                        loop = false;
                        break;
                    default:
                        throw new Exception();
                }
            }

            //Console.WriteLine(string.Join(',', intcode));
            return intcode[0];
        }
    }
}
