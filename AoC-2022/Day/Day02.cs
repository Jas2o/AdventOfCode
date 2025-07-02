namespace AoC.Day
{
    public class Day02
    {
        public static void Run(string file) {
            Console.WriteLine("Day 2: Rock Paper Scissors" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            List<string> runA = new List<string>();
            List<string> runB = new List<string>();

            int partA = 0;
            int partB = 0;
            foreach(string line in lines) {
                RPS play1A = (RPS)(line[0] - 'A' + 1);
                RPS play2A = (RPS)(line[2] - 'X' + 1);

                Result resultA = Result.TBD;
                int scoreA = CalcScore(play1A, play2A, out resultA);
                partA += scoreA;
                runA.Add(string.Format("{0} vs {1}, {2}, score {3}", (RPS)play1A, (RPS)play2A, resultA, scoreA));

                Result resultB = (play2A == RPS.Rock ? Result.Loss : (play2A == RPS.Paper ? Result.Draw : Result.Win));
                RPS play2B = RPS.UNDECIDED;
                if (resultB == Result.Draw)
                    play2B = play1A;
                else if (resultB == Result.Win) {
                    if (play1A == RPS.Rock) play2B = RPS.Paper;
                    else if (play1A == RPS.Paper) play2B = RPS.Scissors;
                    else if (play1A == RPS.Scissors) play2B = RPS.Rock;
                } else if (resultB == Result.Loss) {
                    if (play1A == RPS.Rock) play2B = RPS.Scissors;
                    else if (play1A == RPS.Paper) play2B = RPS.Rock;
                    else if (play1A == RPS.Scissors) play2B = RPS.Paper;
                }

                int scoreB = CalcScore(play1A, play2B, out Result resultB2);
                if (resultB != resultB2)
                    throw new Exception();
                partB += scoreB;

                runB.Add(string.Format("{0} vs {1}, {2}, score {3}", play1A, play2B, resultB, scoreB));
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("# Part 1");
            Console.ResetColor();
            foreach(string run in runA)
                Console.WriteLine(run);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\r\n# Part 2");
            Console.ResetColor();
            foreach (string run in runB)
                Console.WriteLine(run);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 9241
            Console.WriteLine("Part 2: " + partB);
            //Answer: 14610
        }

        private static int CalcScore(RPS play1, RPS play2, out Result result) {
            int score = 0;
            result = Result.TBD;
            if (play1 == RPS.Rock && play2 == RPS.Scissors) {
                result = Result.Loss;
            } else if (play2 > play1 || (play1 == RPS.Scissors && play2 == RPS.Rock)) {
                score += 6;
                result = Result.Win;
            } else if (play1 == play2) {
                score += 3;
                result = Result.Draw;
            } else {
                result = Result.Loss;
            }
            score += (int)play2;
            return score;
        }

        enum RPS {
            UNDECIDED = 0,
            Rock = 1,
            Paper = 2,
            Scissors = 3
        }

        enum Result {
            TBD = 0,
            Win = 1,
            Draw = 2,
            Loss = 3
        }
    }
}
