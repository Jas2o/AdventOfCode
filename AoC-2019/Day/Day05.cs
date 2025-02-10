namespace AoC.Day
{
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Sunny with a Chance of Asteroids" + Environment.NewLine);

            string input = File.ReadAllText(file);
            int[] initial = Array.ConvertAll(input.Split(','), int.Parse);
            int[] intcode = new int[initial.Length];

            Array.Copy(initial, intcode, initial.Length);
            int partA = Run(intcode, 1);
            
            Array.Copy(initial, intcode, initial.Length);
            int partB = Run(intcode, 5);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 5821753
            Console.WriteLine("Part 2: " + partB);
            //Answer: 11956381
        }

        private static int Run(int[] intcode, int systemID) {
            int pos = 0;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Input will be: {0}", systemID);
            Console.ResetColor();

            int output = -1;

            bool loop = true;
            while (loop) {
                int read = intcode[pos];
                int opcode = read % 100;
                int mode1 = read % 1000 / 100;
                int mode2 = read % 10000 / 1000;
                int mode3 = read % 100000 / 10000;

                if (opcode == 99) {
                    loop = false;
                    break;
                }

                int v1 = (mode1 == 1 ? intcode[pos + 1] : intcode[intcode[pos + 1]]);
                int v2 = 0;
                if(opcode != 3 && opcode != 4)
                    v2 = (mode2 == 1 ? intcode[pos + 2] : intcode[intcode[pos + 2]]);
                //int v3 = (mode3 == 1 ? intcode[pos + 3] : intcode[intcode[pos + 3]]);

                switch (opcode) {
                    case 1: //Add
                        int addD = intcode[pos + 3];
                        intcode[addD] = v1 + v2;
                        pos += 4;
                        break;
                    case 2: //Multiply
                        int mulD = intcode[pos + 3];
                        intcode[mulD] = v1 * v2;
                        pos += 4;
                        break;

                    case 3: //Input
                        int input = systemID;
                        int setD = intcode[pos + 1];
                        intcode[setD] = input;
                        pos += 2;
                        break;
                    case 4: //Output
                        output = v1;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Out: {0}", v1);
                        Console.ResetColor();
                        pos += 2;
                        break;

                    case 5: //Jump-if-true
                        if (v1 != 0)
                            pos = v2;
                        else
                            pos += 3;
                        break;
                    case 6: //Jump-if-false
                        if (v1 == 0)
                            pos = v2;
                        else
                            pos += 3;
                        break;
                    case 7: //Less than
                        int lessD = intcode[pos + 3];
                        intcode[lessD] = (v1 < v2 ? 1 : 0);
                        pos += 4;
                        break;
                    case 8: //Equals
                        int eqD = intcode[pos + 3];
                        intcode[eqD] = (v1 == v2 ? 1 : 0);
                        pos += 4;
                        break;

                    default:
                        throw new Exception();
                }
            }

            Console.WriteLine();
            return output;
        }
    }
}
