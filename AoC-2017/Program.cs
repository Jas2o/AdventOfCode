using AoC.Day;

namespace AoC {
    internal class Program {
        static void Main(string[] args) {
            Console.Title = "Advent of Code 2017";

            int daynum = 1;
            bool test = true;

            bool loop = true;
            string? input = string.Format("{0} {1}", daynum, (test ? "Test" : ""));
            while (loop) {
                if (input != null && input.Length > 0) {
                    if (input.Contains("exit", StringComparison.CurrentCultureIgnoreCase)) {
                        loop = false;
                        break;
                    }

                    test = input.Contains('t', StringComparison.CurrentCultureIgnoreCase);
                    string digits = new string(input.TakeWhile(c => Char.IsDigit(c)).ToArray());
                    int newdaynum = 0;
                    int.TryParse(digits, out newdaynum);
                    if (newdaynum > 0)
                        daynum = newdaynum;
                }
                if (test)
                    Console.WriteLine("TEST");

                Run(daynum, test);

                Console.Write(Environment.NewLine + "> ");
                input = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("\x1b[3J");
            }
        }

        private static void Run(int daynum, bool test) {
            string day = daynum.ToString("00");

            string file = "..\\..\\..\\" + (test ? "Test" : "Input") + "\\" + day + ".txt";
            if (File.Exists(file)) {
                switch (daynum) {
                    case 1: Day01.Run(file); break;
                    case 2: Day02.Run(file); break;
                    case 3: Day03.Run(file); break;
                    case 4: Day04.Run(file); break;
                    case 5: Day05.Run(file); break;
                    case 6: Day06.Run(file); break;
                    case 7: Day07.Run(file); break;
                    case 8: Day08.Run(file); break;
                    case 9: Day09.Run(file); break;
                    case 10: Day10.Run(file); break;
                    case 11: Day11.Run(file); break;
                    case 12: Day12.Run(file); break;
                    case 13: Day13.Run(file); break;
                    case 14: Day14.Run(file); break;
                    case 15: Day15.Run(file); break;
                    case 16: Day16.Run(file); break;
                    case 17: Day17.Run(file); break;
                    case 18: Day18.Run(file); break;
                    case 19: Day19.Run(file); break;
                    case 20: Day20.Run(file); break;
                    case 21: Day21.Run(file); break;
                    case 22: Day22.Run(file); break;
                    case 23: Day23.Run(file); break;
                    case 24: Day24.Run(file); break;
                    case 25: Day25.Run(file); break;
                }

            } else {
                Console.WriteLine("Cannot find day {0} {1} file.", day, (test ? "test" : "input"));
            }
        }
    }
}
