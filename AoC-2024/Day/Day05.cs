namespace AoC.Day {
    public class Day05
    {
        public static void Run(string file) {
            Console.WriteLine("Day 5: Print Queue" + Environment.NewLine);

            bool verbose = false;

            List<int[]> listPageBeforePage = new List<int[]>();

            int middleTotal_A = 0;
            int middleTotal_B = 0;

            string[] lines = File.ReadAllLines(file);
            bool changedMode = false;
            foreach (string line in lines)
            {
                if (line == "")
                {
                    changedMode = true;
                    continue;
                }

                if (!changedMode)
                {
                    string[] pages = line.Split('|');
                    listPageBeforePage.Add([int.Parse(pages[0]), int.Parse(pages[1])]);
                }
                else
                {
                    bool valid = true;

                    int[] pageNums = Array.ConvertAll(line.Split(','), int.Parse);
                    int mid = pageNums.Length / 2;

                    List<int[]> applicableRules = new List<int[]>();
                    foreach (int[] rule in listPageBeforePage)
                    {
                        if (pageNums.Intersect(rule).Any())
                            applicableRules.Add(rule);
                    }

                    bool check = CheckAndSwapAround(applicableRules, pageNums);
                    if (!check)
                    {
                        valid = false;
                        if (verbose)
                            Console.Write(line + " >> ");
                    }
                    while (!check)
                    {
                        if (verbose)
                            Console.Write(".");
                        check = CheckAndSwapAround(applicableRules, pageNums);
                    }

                    //--

                    if (valid)
                    {
                        //Part 1
                        middleTotal_A += pageNums[mid];
                    }
                    else
                    {
                        //Part 2
                        middleTotal_B += pageNums[mid];
                        if (verbose)
                            Console.WriteLine(line + " >> " + string.Join(',', pageNums));
                    }
                }
                //End of foreach
            }

            if (verbose)
                Console.WriteLine();
            Console.WriteLine("Part 1: " + middleTotal_A);
            //Answer: 4905
            Console.WriteLine("Part 2: " + middleTotal_B);
            //Answer: 6204
        }

        private static bool CheckAndSwapAround(List<int[]> applicableRules, int[] array)
        {
            bool valid = true;

            foreach (int[] rule in applicableRules)
            {
                int i1 = Array.IndexOf(array, rule[0]);
                int i2 = Array.IndexOf(array, rule[1]);

                if (i1 > -1 && i2 > -1 && i1 > i2)
                {
                    valid = false;
                    array[i1] = rule[1];
                    array[i2] = rule[0];
                }
            }

            return valid;
        }
    }
}
