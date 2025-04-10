namespace AoC.Day
{
    public class Day16
    {
        public static void Run(string file) {
            Console.WriteLine("Day 16: Ticket Translation" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            //For Part 1
            List<int> approvedNums = new List<int>();
            List<int[]> validTickets = new List<int[]>();
            int[] yourTicket = new int[0];
            int partA = 0;

            //For Part 2
            Dictionary<string, List<int>> ticketFields = new Dictionary<string, List<int>>();
            Dictionary<string, Dictionary<int, int>> ticketFieldsMatch = new Dictionary<string, Dictionary<int, int>>();
            Dictionary<int, int> template = new Dictionary<int, int>();

            int mode = 0;
            foreach (string line in lines) {
                if(line.Contains(':')) {
                    if (line.StartsWith("your"))
                        mode = 1;
                    else if (line.StartsWith("nearby"))
                        mode = 2;
                    else {
                        string[] fields = line.Split(':', StringSplitOptions.TrimEntries);
                        string[] ranges = fields[1].Split(' ');

                        List<int> fieldNums = new List<int>();
                        ticketFields.Add(fields[0], fieldNums);

                        foreach (string range in ranges) {
                            if (range == "or")
                                continue;
                            int[] nums = Array.ConvertAll(range.Split('-'), int.Parse);

                            IEnumerable<int> r = Enumerable.Range(nums[0], nums[1] - nums[0] + 1);
                            fieldNums.AddRange(r);
                        }

                        approvedNums.AddRange(fieldNums);
                    }
                } else if (line.Contains(',')) {
                    if (mode == 1) {
                        yourTicket = Array.ConvertAll(line.Split(','), int.Parse);
                        approvedNums.Sort();

                        for (int i = 0; i < yourTicket.Length; i++)
                            template.Add(i, 0);
                        foreach(KeyValuePair<string, List<int>> fields in ticketFields)
                            ticketFieldsMatch.Add(fields.Key, template.ToDictionary());
                    } else if (mode == 2) {
                        int[] nums = Array.ConvertAll(line.Split(','), int.Parse);
                        bool valid = true;
                        foreach(int n in nums) {
                            if(!approvedNums.Contains(n)) {
                                valid = false;
                                partA += n;
                            }
                        }
                        if (valid)
                            validTickets.Add(nums);
                    }
                }
            }

            //Part 2
            //Count up how many times each field matches
            foreach(int[] ticket in validTickets) {
                for(int i = 0; i < ticket.Length; i++) {
                    foreach (KeyValuePair<string, List<int>> pair in ticketFields) {
                        if (pair.Value.Contains(ticket[i])) {
                            ticketFieldsMatch[pair.Key][i]++;
                        }
                    }
                }
            }

            //Check which of those counts equals the amount of tickets and cleanup.
            int count = validTickets.Count;
            foreach (var pair in ticketFieldsMatch) {
                foreach (var match in pair.Value) {
                    if (match.Value != count)
                        ticketFieldsMatch[pair.Key].Remove(match.Key);
                }
            }

            //Find each of the fields that only match one, remove from all and repeat.
            Dictionary<string, int> matchedUp = new Dictionary<string, int>();
            while(ticketFieldsMatch.Any(t => t.Value.Count == 1)) {
                KeyValuePair<string, Dictionary<int, int>> find = ticketFieldsMatch.First(t => t.Value.Count == 1);
                int taken = find.Value.First().Key;
                matchedUp.Add(find.Key, taken);

                foreach(KeyValuePair<string, Dictionary<int, int>> match in ticketFieldsMatch)
                    match.Value.Remove(taken);
            }

            //Show which position was which field and calculate answer.
            long partB = 1;
            matchedUp = matchedUp.OrderBy(m => m.Value).ToDictionary();
            foreach(var m in matchedUp) {
                int onYourTicket = yourTicket[m.Value];
                if (m.Key.StartsWith("departure")) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    partB *= onYourTicket;
                }
                Console.WriteLine("{0,2} : {1} => {2}", m.Value, m.Key, onYourTicket);
                Console.ResetColor();
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 22057
            Console.WriteLine("Part 2: " + partB);
            //Answer: 1093427331937
        }
    }
}
