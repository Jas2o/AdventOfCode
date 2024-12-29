using System;
using System.Globalization;
using System.Security.Claims;
using System.Text;

namespace AoC.Day
{
    public class Day14
    {
        public static void Run(string file) {
            Console.WriteLine("Day 14: Restroom Redoubt" + Environment.NewLine);

            int spaceWidth = 101;
            int spaceHeight = 103;

            string[] lines = File.ReadAllLines(file);
            if(lines.Length < 20) {
                spaceWidth = 11;
                spaceHeight = 7;
            }

            // Prepare the robots!
            List<Robot> robots = new List<Robot>();
            foreach(string line in lines) {
                Robot r = new Robot(line);
                robots.Add(r);
            }

            // Prepare a blank simulation space!
            int[][] simulation = new int[spaceHeight][];
            for(int y = 0; y < spaceHeight; y++) {
                simulation[y] = new int[spaceWidth];
            }

            // Simulate!
            foreach (Robot robot in robots) {
                robot.Simulate(100, spaceWidth, spaceHeight);
                simulation[robot.PosY][robot.PosX]++;
                robot.Reset();
            }

            // Check all them quads for that safety factor!
            int[] quad = SimOutSafetyQuad(simulation);
            int safetyFactor = quad[0] * quad[1] * quad[2] * quad[3];

            Console.WriteLine("Part 1: " + safetyFactor);
            //Answer: 208437768

            Console.WriteLine();
            if (spaceHeight != 103) {
                Console.WriteLine("Cannot apply Part 2 to the test.");
                return;
            }

            // Alrighty we'll restart the simulation and step through until we find something suspicious.
            int totalSeconds = 0;
            int treeStartY = -1;
            int treeEndY = -1;
            bool suspectedTree = false;
            while (suspectedTree == false) {
                for (int y = 0; y < spaceHeight; y++) {
                    string test = string.Join("", simulation[y]);
                    //Look down
                    if (test.Contains("1111111111")) {
                        suspectedTree = true;
                        treeStartY = y;

                        for (int noty = spaceHeight - 1; noty > y; noty--) {
                            string test2 = string.Join("", simulation[noty]);
                            //Look up
                            if (test2.Contains("1111111111")) {
                                treeEndY = noty+1;
                                break;
                            }
                        }

                        break;
                    }
                }

                if (suspectedTree) {
                    //Console.Clear();
                    //Console.WriteLine("\x1b[3J");
                    //Console.WriteLine(totalSeconds);
                    DrawSim(simulation, treeStartY, treeEndY);
                } else {
                    totalSeconds++;

                    for (int y = 0; y < spaceHeight; y++) {
                        simulation[y] = new int[spaceWidth];
                    }
                    foreach (Robot robot in robots) {
                        robot.Simulate(1, spaceWidth, spaceHeight);
                        simulation[robot.PosY][robot.PosX]++;
                    }
                }
            }
            
            Console.WriteLine();
            Console.WriteLine("Part 1: " + safetyFactor);
            Console.WriteLine("Part 2: " + totalSeconds);
            //Answer: 7492
        }

        private static int[] SimOutSafetyQuad(int[][] simulation) {
            int spaceHeight = simulation.Length;
            int spaceWidth = simulation[0].Length;

            int midW = spaceWidth / 2;
            int midH = spaceHeight / 2;

            int[] quad = new int[4];

            for (int y = 0; y < spaceHeight; y++) {
                if (y == midH) {
                    //Console.WriteLine();
                    continue;
                }

                int quadIndex = (y > midH ? 2 : 0);
                for (int x = 0; x < spaceWidth; x++) {
                    if (x == midW) {
                        quadIndex++;
                        //Console.Write(' ');
                        continue;
                    }

                    /*
                    if (simulation[y][x] == 0)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else if(simulation[y][x] == 1)
                        Console.ForegroundColor = ConsoleColor.Green;
                    else
                        Console.ForegroundColor = ConsoleColor.Red;
                    */

                    quad[quadIndex] += simulation[y][x];
                    //Console.Write("{0}", simulation[y][x]);
                    ////Console.Write("{0}", quadIndex);
                }
                //Console.WriteLine();
            }
            //Console.ResetColor();
            //Console.WriteLine();

            return quad;
        }

        private static void DrawSim(int[][] simulation, int start = 0, int end = -1) {
            int spaceHeight = simulation.Length;
            int spaceWidth = simulation[0].Length;
            if (end == -1)
                end = spaceHeight;

            for (int y = start; y < end; y++) {
                for (int x = 0; x < spaceWidth; x++) {
                    if (simulation[y][x] == 0)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("{0}", simulation[y][x]);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        private class Robot {
            public int InitialPosX;
            public int InitialPosY;
            public int PosX;
            public int PosY;
            public int VelocityX;
            public int VelocityY;

            public Robot(string line) {
                int firstEq = line.IndexOf('=') + 1;
                int firstComma = line.IndexOf(',');
                int firstSpace = line.IndexOf(' ');
                int lastEq = line.LastIndexOf('=') + 1;
                int lastComma = line.LastIndexOf(',');

                string pX = line.Substring(firstEq, firstComma - firstEq);
                string pY = line.Substring(firstComma + 1, firstSpace - firstComma - 1);
                string vX = line.Substring(lastEq, lastComma - lastEq);
                string vY = line.Substring(lastComma + 1);

                PosX = InitialPosX = int.Parse(pX);
                PosY = InitialPosY = int.Parse(pY);
                VelocityX = int.Parse(vX);
                VelocityY = int.Parse(vY);
            }

            public void Reset() {
                PosX = InitialPosX;
                PosY = InitialPosY;
            }

            public void Simulate(int seconds, int spaceWidth, int spaceHeight) {
                PosX += VelocityX * seconds;
                PosY += VelocityY * seconds;

                if(PosX >= spaceWidth || PosX < 0)
                    PosX = PosX % spaceWidth;
                if(PosY >= spaceHeight || PosY < 0)
                    PosY = PosY % spaceHeight;

                if (PosX >= spaceWidth || PosX < 0)
                    PosX = spaceWidth + PosX;
                if (PosY >= spaceHeight || PosY < 0)
                    PosY = spaceHeight + PosY;
            }
        }
    }
}
