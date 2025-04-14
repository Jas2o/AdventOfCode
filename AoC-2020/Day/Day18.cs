namespace AoC.Day
{
    public class Day18
    {
        public static void Run(string file) {
            Console.WriteLine("Day 18: Operation Order" + Environment.NewLine);

            long partA = 0;
            long partB = 0;
            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                List<int> open = new List<int>();
                List<int> close = new List<int>();
                for(int i = 0; i < line.Length; i++) {
                    char c = line[i];
                    if (c == '(')
                        open.Add(i);
                    else if (c == ')')
                        close.Add(i + 1);
                }

                Console.WriteLine("   " + line);

                string workingA = line;
                string workingB = line;

                //Start from the deepest open bracket
                for (int indOpen = open.Count - 1; indOpen >= 0; indOpen--) {
                    int start = open[indOpen];
                    int end = close.Last();
                    int indClose = close.Count - 1;
                    //Find the next close bracket
                    for (; indClose >= 0; indClose--) {
                        if (close[indClose] > open[indOpen])
                            end = close[indClose];
                    }
                    int width = end - start;
                    close.Remove(end);

                    //Replace this bracket section
                    string subA = workingA.Substring(start + 1, width - 2);
                    long replacementA = SolveA(subA);
                    string replacementAS = replacementA.ToString().PadLeft(width);

                    string subB = workingB.Substring(start + 1, width - 2);
                    long replacementB = SolveB(subB);
                    string replacementBS = replacementB.ToString().PadLeft(width);

                    workingA = workingA.Substring(0, start) + replacementAS + workingA.Substring(end);
                    workingB = workingB.Substring(0, start) + replacementBS + workingB.Substring(end);

                    if (workingA == workingB)
                        Console.WriteLine("#  " + workingA);
                    else {
                        Console.WriteLine("1  " + workingA);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("2  " + workingB);
                        Console.ResetColor();
                    }
                }

                long answerA = SolveA(workingA);
                long answerB = SolveB(workingB);
                Console.WriteLine("1  " + answerA);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("2  " + answerB);
                Console.ResetColor();
                partA += answerA;
                partB += answerB;

                Console.WriteLine();
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 650217205854
            Console.WriteLine("Part 2: " + partB);
            //Answer: 20394514442037
        }

        private static long SolveA(string input) {
            long result = 0;
            bool multi = false;

            string[] fields = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (string field in fields) {
                if (field == "+") multi = false;
                else if (field == "*") multi = true;
                else {
                    long num = long.Parse(field);
                    if (multi)
                        result *= num;
                    else
                        result += num;
                }
            }

            return result;
        }

        private static long SolveB(string input) {
            string[] fields = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            //Add first
            for (int i = 0; i < fields.Length; i++) {
                if (fields[i] == "+") {
                    long left = long.Parse(fields[i - 1]);
                    long right = long.Parse(fields[i + 1]);
                    long add = left + right;

                    fields[i - 1] = "+";
                    fields[i + 1] = add.ToString();
                }
            }

            //Multiply second
            long result = 1;
            for (int i = 0; i < fields.Length; i++) {
                if (fields[i] == "+" || fields[i] == "*")
                    continue;

                long num = long.Parse(fields[i]);
                result *= num;
            }

            return result;
        }
    }
}
