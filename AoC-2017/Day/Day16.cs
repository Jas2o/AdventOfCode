namespace AoC.Day
{
    public class Day16
    {
        private static string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static void Run(string file) {
            Console.WriteLine("Day 16: Permutation Promenade" + Environment.NewLine);

			string input = File.ReadAllText(file);
            string[] parts = input.Split(',');

            int numPrograms = (parts.Length < 10 ? 5 : 16);
            char[] programs = alphabet.ToCharArray(0, numPrograms);
            char[] programsStart = programs.ToArray();

            Dance(parts, ref programs);
            string partA = new string(programs);

            int times = 1000000000;
            for (int i = 1; i < times; i++) {
                if (programs.SequenceEqual(programsStart)) {
                    int rem = (times / i) - 1;
                    i += rem * i;
                }
                Dance(parts, ref programs);
            }
            string partB = new string(programs);

            Console.WriteLine("Part 1: " + partA);
            //Answer: kpbodeajhlicngmf
            Console.WriteLine("Part 2: " + partB);
            //Answer: ahgpjdkcbfmneloi
        }

        //2016, Day 21
        private static IEnumerable<T> ShiftRight<T>(IList<T> values, int shift) {
            IEnumerable<T> newBegin = values.Skip(values.Count - shift);
            IEnumerable<T> newEnd = values.Take(values.Count - shift);
            return newBegin.Concat(newEnd);
        }

        private static void Dance(string[] parts, ref char[] programs) {
            foreach (string part in parts) {
                if (part[0] == 's') {
                    //Spin
                    int num = int.Parse(part.Substring(1));
                    programs = ShiftRight(programs, num).ToArray();
                } else if (part[0] == 'x') {
                    //Exchange
                    int[] find = Array.ConvertAll(part.Substring(1).Split('/'), int.Parse);
                    (programs[find[0]], programs[find[1]]) = (programs[find[1]], programs[find[0]]);
                } else if (part[0] == 'p') {
                    //Partner
                    char find1 = part[1];
                    char find2 = part[3];
                    int pos1 = Array.IndexOf(programs, find1);
                    int pos2 = Array.IndexOf(programs, find2);
                    (programs[pos1], programs[pos2]) = (programs[pos2], programs[pos1]);
                } else
                    throw new Exception();
            }
        }
    }
}
