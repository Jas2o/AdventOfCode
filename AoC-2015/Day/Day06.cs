using System;
using System.Globalization;
using System.Numerics;
using System.Text;

namespace AoC.Day
{
    public class Day06
    {
        public static void Run(string file) {
            Console.WriteLine("Day 6: Probably a Fire Hazard" + Environment.NewLine);

            int width = 1000;
            int height = 1000;

            bool[][] mapLight1 = new bool[height][]; // Part 1: on/off
            int[][] mapLight2 = new int[height][]; // Part 2: brightness
            for (int i = 0; i < height; i++) {
                mapLight1[i] = new bool[width];
                mapLight2[i] = new int[width];
            }

            string[] instructions = File.ReadAllLines(file);
            foreach (string instruction in instructions) {
                int firstComma = instruction.IndexOf(',');
                int lastComma = instruction.LastIndexOf(',');

                int firstSpace = instruction.Substring(0, firstComma).LastIndexOf(' ');
                int secondSpace = instruction.Substring(firstComma).IndexOf(' ') + firstComma;
                int thirdSpace = instruction.Substring(0, lastComma).LastIndexOf(' ');

                //--

                int num1 = int.Parse(instruction.Substring(firstSpace, firstComma - firstSpace));
                int num2 = int.Parse(instruction.Substring(firstComma + 1, secondSpace - firstComma));

                int num3 = int.Parse(instruction.Substring(thirdSpace, lastComma - thirdSpace));
                int num4 = int.Parse(instruction.Substring(lastComma + 1));

                //--

                bool turnOn = instruction.StartsWith("turn on ");
                bool turnOff = instruction.StartsWith("turn off ");
                bool toggle = instruction.StartsWith("toggle");

                for (int y = num2; y <= num4; y++) {
                    for (int x = num1; x <= num3; x++) {
                        if (turnOn) {
                            mapLight1[y][x] = true;
                            mapLight2[y][x]++;
                        } else if (turnOff) {
                            mapLight1[y][x] = false;
                            mapLight2[y][x]--;
                            if (mapLight2[y][x] < 0)
                                mapLight2[y][x] = 0;
                        } else if (toggle) {
                            mapLight1[y][x] = !mapLight1[y][x];
                            mapLight2[y][x] += 2;
                        }
                    }
                }
            }

            //DrawMap(ref mapLight1);

            int partA = 0;
            int partB = 0;
            for (int y = 0; y < mapLight1.Length; y++) {
                partA += mapLight1[y].Count(x => x == true);
                partB += mapLight2[y].Sum();
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 377891
            Console.WriteLine("Part 2: " + partB);
            //Answer: 14110788
        }

        private static void DrawMap(ref bool[][] map) {
            for (int y = 0; y < map.Length; y++) {

                //Console.BackgroundColor = ConsoleColor.Red;
                //Console.Write(' ');

                for (int x = 0; x < map[y].Length; x++) {
                    bool lit = map[y][x];

                    if (lit) {
                        //Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write('#');
                    } else {
                        //Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.Write(' ');
                    }
                }

                /*
                Console.BackgroundColor = ConsoleColor.Red;
                Console.Write(' ');
                Console.ResetColor();
                Console.Write(" {0}", y);
                */
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
