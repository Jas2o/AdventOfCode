namespace AoC.Day {
    public class Day25
    {
        public static void Run(string file) {
            Console.WriteLine("Day 25: Code Chronicle" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);
            List<int[]> listLocks = new List<int[]>();
            List<int[]> listKeys = new List<int[]>();

            if (true) {
                int mode = 0; //1 = lock, 2 = key
                int[] temp = [-1, -1, -1, -1, -1];
                for (int i = 0; i < lines.Length; i++) {
                    string line = lines[i];
                    if (line.Length == 0) {
                        if (mode == 1) listLocks.Add(temp);
                        else if (mode == 2) listKeys.Add(temp);
                        mode = 0;
                        temp = [-1, -1, -1, -1, -1];
                    } else {
                        if (mode == 0) {
                            if (line == "#####")
                                mode = 1;
                            else if (line == ".....")
                                mode = 2;
                        }

                        if (line[0] == '#') temp[0]++;
                        if (line[1] == '#') temp[1]++;
                        if (line[2] == '#') temp[2]++;
                        if (line[3] == '#') temp[3]++;
                        if (line[4] == '#') temp[4]++;
                    }
                }
                if (mode == 1) listLocks.Add(temp);
                else if (mode == 2) listKeys.Add(temp);
            }

            int countFit = 0;
            foreach (int[] alock in listLocks) {
                foreach (int[] akey in listKeys) {
                    string text = "fit";
                    int[] temp = [
                        alock[0]+ akey[0],
                        alock[1]+ akey[1],
                        alock[2]+ akey[2],
                        alock[3]+ akey[3],
                        alock[4]+ akey[4]
                    ];
                    for (int c = 0; c < temp.Length; c++) {
                        if (temp[c] > 5) {
                            text = "overlap in column #" + (c + 1);
                            break;
                        } /*else if (temp[c] < 5) {
                            text = "short in column #" + (c + 1);
                            break;
                        }*/
                    }
                    if (text == "fit")
                        countFit++;
                    Console.WriteLine("Lock {0} and key {1}: {2}", string.Join(',', alock), string.Join(',', akey), text);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + countFit);
            //Answer: 3338
            Console.WriteLine("Part 2: (there isn't one)");
            //Answer: 
        }
    }
}
