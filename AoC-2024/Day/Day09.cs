using System;
using System.Globalization;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace AoC.Day
{
    public class Day09
    {
        public static void Run(string file) {
            Console.WriteLine("Day 9: Disk Fragmenter" + Environment.NewLine);

            List<int> listBlocks = new List<int>();

            string input = File.ReadAllText(file);

            int verbose = 2;
            if (input.Length > 100)
                verbose--;

            bool checkFree = false;
            int id = 0;
            for (int i = 0; i < input.Length; i++) {
                int width = int.Parse(input[i].ToString());

                if (checkFree) {
                    for (int c = 0; c < width; c++)
                        listBlocks.Add(-1);
                } else {
                    for (int c = 0; c < width; c++)
                        listBlocks.Add(id);
                    id++;
                }

                checkFree = !checkFree;
            }

            ulong part1checksum = 0;
            ulong part2checksum = 0;
            List<int> part1 = new List<int>(listBlocks);
            List<int> part2 = new List<int>(listBlocks);

            if (verbose > 0) {
                Console.WriteLine("Starting with:");
                foreach (int block in listBlocks) {
                    if (block == -1) {
                        Console.Write('.');
                    } else {
                        Console.Write("[{0:X}]", block);
                    }
                }
            }

            if (verbose > 0)
                Console.WriteLine("\r\n\r\nCalculating for Part 1");

            for (int fromStart = 0; fromStart < part1.Count; fromStart++) {
                if (part1[fromStart] == -1) {
                    for (int fromEnd = part1.Count - 1; fromEnd > fromStart; fromEnd--) {
                        if (part1[fromEnd] != -1) {
                            //char temp = sb[fromEnd];
                            part1[fromStart] = part1[fromEnd];
                            part1[fromEnd] = -1;
                            break;
                        }
                    }
                }
            }

            if (verbose == 2)
                Console.WriteLine("\r\nAfter (1):");
            for (int i = 0; i < part1.Count; i++) {
                if (part1[i] == -1) {
                    if (verbose == 2)
                        Console.Write('.');
                } else {
                    part1checksum += (uint)(i * part1[i]);
                    if (verbose == 2)
                        Console.Write("[{0:X}]", part1[i]);
                }
            }
            if (verbose == 2)
                Console.WriteLine();

            if (verbose > 0)
                Console.WriteLine("\r\nCalculating for Part 2");

            int lastID = part2.Last(x => x != -1);
            for (int indexEnd = part2.Count - 1; indexEnd > -1; indexEnd--) {
                int curID = part2[indexEnd];
                //if(curID != -1 && verbose)
                    //Console.WriteLine("Looking for {0} got {1}", lastID, curID);

                if (curID == lastID) {
                    int indexStart = part2.IndexOf(lastID);
                    int indexDiff = (indexEnd - indexStart) + 1;

                    int fromStart;
                    int found = 0;
                    for (fromStart = 0; fromStart < indexEnd; fromStart++) {
                        int earlyID = part2[fromStart];
                        if (earlyID == -1) {
                            found++;
                            if (found == indexDiff) {
                                fromStart -= found - 1;
                                break;
                            }
                        } else {
                            found = 0;
                        }
                    }
                    if (found == indexDiff) {
                        for (int x = 0; x < found; x++) {
                            part2[fromStart + x] = lastID;
                            part2[indexStart + x] = -1;
                        }
                        lastID--;
                        if (verbose > 0) {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" " + curID + " Moved ");
                            Console.ResetColor();
                        }
                    } else {
                        lastID--;
                        //indexEnd = indexStart; //optional
                        if (verbose > 0) {
                            Console.Write(" " + curID + " Not ");
                        }
                    }
                }
            }
            if (verbose > 0)
                Console.WriteLine();

            if (verbose == 2)
                Console.WriteLine("\r\nAfter (2):");
            for (int i = 0; i < part1.Count; i++) {
                if (part2[i] == -1) {
                    if (verbose == 2)
                        Console.Write('.');
                } else {
                    part2checksum += (uint)(i * part2[i]);
                    if (verbose == 2)
                        Console.Write("[{0:X}]", part2[i]);
                }
            }
            if (verbose == 2)
                Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine("Part 1: " + part1checksum);
            //Answer: 6211348208140
            Console.WriteLine("Part 2: " + part2checksum);
            //Answer: 6239783302560
        }
    }
}
