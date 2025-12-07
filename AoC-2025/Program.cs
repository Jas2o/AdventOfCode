using AoC.Day;

namespace AoC {
    internal class Program {
        static void Main(string[] args) {
            Console.Title = "Advent of Code 2025";

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
                    //2025 is the first year to have 12 days.
                }

            } else {
                Console.WriteLine("Cannot find day {0} {1} file.", day, (test ? "test" : "input"));
            }
        }
    }
}
