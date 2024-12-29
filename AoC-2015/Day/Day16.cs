using System;
using System.Globalization;
using System.Text;

namespace AoC.Day
{
    public class Day16
    {
        public static void Run(string file) {
            Console.WriteLine("Day 16: Aunt Sue" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            if(lines.Length < 5) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            List<Sue> sues = new List<Sue>();
            foreach(string line in lines) {
                Sue sue = new Sue(line);
                sues.Add(sue);
            }

            int children = 3;
            int cats = 7; //greater than
            int samoyeds = 2;
            int pomeranians = 3; //fewer than
            int akitas = 0;
            int vizslas = 0;
            int goldfish = 5; //fewer than
            int trees = 3; //greater than
            int cars = 2;
            int perfumes = 1;

            Console.WriteLine("Part 1: "); //373
            foreach (Sue sue in sues) {
                if (sue.children != -1 && sue.children != children) continue;
                if (sue.cats != -1 && sue.cats != cats) continue;
                if (sue.samoyeds != -1 && sue.samoyeds != samoyeds) continue;
                if (sue.pomeranians != -1 && sue.pomeranians != pomeranians) continue;
                if (sue.akitas != -1 && sue.akitas != akitas) continue;
                if (sue.vizslas != -1 && sue.vizslas != vizslas) continue;
                if (sue.goldfish != -1 && sue.goldfish != goldfish) continue;
                if (sue.trees != -1 && sue.trees != trees) continue;
                if (sue.cars != -1 && sue.cars != cars) continue;
                if (sue.perfumes != -1 && sue.perfumes != perfumes) continue;

                Console.WriteLine("It was probably Sue #{0}", sue.ID);
            }

            Console.WriteLine();
            Console.WriteLine("Part 2: "); //260
            foreach (Sue sue in sues) {
                if (sue.children != -1 && sue.children != children) continue;
                if (sue.samoyeds != -1 && sue.samoyeds != samoyeds) continue;
                if (sue.akitas != -1 && sue.akitas != akitas) continue;
                if (sue.vizslas != -1 && sue.vizslas != vizslas) continue;
                if (sue.cars != -1 && sue.cars != cars) continue;
                if (sue.perfumes != -1 && sue.perfumes != perfumes) continue;

                if (sue.cats != -1 && sue.cats <= cats) continue; //Needs to be greater
                if (sue.trees != -1 && sue.trees <= trees) continue;

                if (sue.pomeranians != -1 && sue.pomeranians >= pomeranians) continue; //Needs to be less
                if (sue.goldfish != -1 && sue.goldfish >= goldfish) continue;

                Console.WriteLine("It was actually Sue #{0}", sue.ID);
            }
        }

        private class Sue {
            public int ID = -1;

            public int children = -1;
            public int cats = -1;
            public int samoyeds = -1;
            public int pomeranians = -1;
            public int akitas = -1;
            public int vizslas = -1;
            public int goldfish = -1;
            public int trees = -1;
            public int cars = -1;
            public int perfumes = -1;

            public Sue(string line) {
                string[] fields = line.Replace(":", "").Replace(",", "").Split(' ');
                ID = int.Parse(fields[1]);

                for (int i = 2; i < fields.Length; i+=2) {
                    int next = int.Parse(fields[i + 1]);
                    switch (fields[i]) {
                        case "children":
                            children = next;
                            break;
                        case "cats":
                            cats = next;
                            break;
                        case "samoyeds":
                            samoyeds = next;
                            break;
                        case "pomeranians":
                            pomeranians = next;
                            break;
                        case "akitas":
                            akitas = next;
                            break;
                        case "vizslas":
                            vizslas = next;
                            break;
                        case "goldfish":
                            goldfish = next;
                            break;
                        case "trees":
                            trees = next;
                            break;
                        case "cars":
                            cars = next;
                            break;
                        case "perfumes":
                            perfumes = next;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }
    }
}
