namespace AoC.Day
{
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: Electromagnetic Moat" + Environment.NewLine);

            List<(int, int)> list = new List<(int, int)>();
			string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int[] nums = Array.ConvertAll(line.Split('/'), int.Parse);
                list.Add((nums[0], nums[1]));
            }

            int partA = 0;
            int lengthA = 0;
            int partB = 0;
            int longestB = 0;
            Recursive(list, 0, new List<(int, int)>(), ref partA, ref lengthA, ref partB, ref longestB);

            Console.WriteLine("Part 1: {0} (with length {1})", partA, lengthA);
            //Answer: 1859
            Console.WriteLine("Part 2: {0} (with length {1})", partB, longestB);
            //Answer: 1799
        }

        private static void Recursive(List<(int, int)> list, int match, List<(int, int)> inProgress, ref int highestStrength, ref int lenOfHighestStr, ref int longStrong, ref int longest) {
            List<(int, int)> list2 = list.ToList();
            List<(int, int)> options = list.Where(x => x.Item1 == match || x.Item2 == match).ToList();

            //if(options.Count == 0) {
                //Console.WriteLine(string.Join("--", inProgress));
            //}

            foreach((int, int) option in options) {
                inProgress.Add(option);
                list2.Remove(option);

                int length = inProgress.Count();

                int strength = inProgress.Sum(x => x.Item1) + inProgress.Sum(x => x.Item2);
                if (strength > highestStrength) {
                    highestStrength = strength;
                    lenOfHighestStr = length;
                }

                if (length > longest) {
                    longest = length;
                    longStrong = strength;
                } else if(length == longest && strength > longStrong) {
                    longStrong = strength;
                }

                //Console.WriteLine("{0} = {1}", string.Join("--", inProgress), strength);

                if (option.Item1 == match)
                    Recursive(list2, option.Item2, inProgress, ref highestStrength, ref lenOfHighestStr, ref longStrong, ref longest);
                else
                    Recursive(list2, option.Item1, inProgress, ref highestStrength, ref lenOfHighestStr, ref longStrong, ref longest);
                
                inProgress.Remove(option);
                list2.Add(option);
            }
        }
    }
}
