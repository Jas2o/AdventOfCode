namespace AoC.Day {
    public class Day11
    {
        private static char[] banned = ['i', 'o', 'l'];

        public static void Run(string file) {
            Console.WriteLine("Day 11: Corporate Policy");

            /*
            string next = GetNext("xx");
            Console.WriteLine(next); //xy
            next = GetNext(next);
            Console.WriteLine(next); //xz
            next = GetNext(next);
            Console.WriteLine(next); //ya
            next = GetNext(next);
            Console.WriteLine(next); //yb
            */

            string[] lines = File.ReadAllLines(file);
            //The real file only has a single line

            //int testA = (int)'a'; 97
            //int testZ = (int)'z'; 122

            foreach(string line in lines) {
                Console.WriteLine("\r\nFor: " + line);
                bool isValid = CheckIsValid(line, true);
                if (isValid) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Valid");
                    Console.ResetColor();
                }

                isValid = false;
                string input = line;
                while (!isValid) {
                    input = GetNext(input);

                    bool noBannedChars = !banned.Any(input.Contains);
                    if (noBannedChars) {
                        isValid = CheckIsValid(input, false);
                    }
                }
                Console.WriteLine("Next is: " + input);
            }

            Console.WriteLine();
            //Console.WriteLine("Part 1: " + 0);
            //Answer: hepxxyzz
            //Console.WriteLine("Part 2: " + 0);
            //Answer: heqaabcc
        }

        private static bool CheckIsValid(string input, bool verbose) {
            if(verbose)
                Console.WriteLine(input);

            //Check if it meets criteria
            int previous = -1;
            int straight = 0;
            bool hasStraight = false;
            bool double1 = false;
            bool double2 = false;
            bool noBannedChars = !banned.Any(input.Contains);
            for (int i = 0; i < input.Length; i++) {
                // 1=a  2=b  3=c  4=d  5=e  6=f  7=g  8=h  9=i 10=j
                //11=k 12=l 13=m 14=n 15=o 16=p 17=q 18=r 19=s 20=t
                //21=u 22=v 23=w 24=x 25=y 26=z
                int num = (int)input[i] - 96;
                if(verbose)
                    Console.Write(num + "\t");

                if (previous == num) {
                    straight = 0;
                    if (double1 && input[i] != input[i - 2])
                        double2 = true;
                    else
                        double1 = true;
                } else if (previous + 1 == num) {
                    straight++;
                    if (straight == 2)
                        hasStraight = true;
                } else {
                    straight = 0;
                }

                previous = num;
            }

            if (verbose) {
                Console.WriteLine();
                if (noBannedChars)
                    Console.Write("NoBanned\t");
                if (hasStraight)
                    Console.Write("HasStraight\t");
                if (double1)
                    Console.Write("D1\t");
                if (double2)
                    Console.Write("D2\t");
            }
            bool isValid = noBannedChars && hasStraight && double1 && double2;

            if (verbose)
                Console.WriteLine();

            return isValid;
        }

        private static string GetNext(string input) {
            char[] text = input.ToArray();

            for (int i = text.Length - 1; i >= 0; i--) {
                int num = (int)text[i] - 96;
                if (num < 26) {
                    text[i]++;
                    i = 0;
                } else {
                    text[i] = 'a';
                }
            }

            return new string(text);
        }
    }
}
