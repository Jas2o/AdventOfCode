namespace AoC.Day
{
    public class Day07
    {
        private const string target = "shiny gold";

        public static void Run(string file) {
            Console.WriteLine("Day 7: Handy Haversacks" + Environment.NewLine);

            List<Bag> bags = new List<Bag>();

            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                string[] parts = line.Split(" contain ");
                Bag bag = new Bag(parts[0], parts[1]);
                bags.Add(bag);
            }

            foreach(Bag bag in bags) {
                if (bag.ContainsText.StartsWith("no other"))
                    continue;

                string[] contains = bag.ContainsText.Split(", ");
                foreach(string c in contains) {
                    string[] parts = c.Split(' ', 2);
                    int amount = int.Parse(parts[0]);

                    int last = parts[1].LastIndexOf(" bag");
                    string other = parts[1].Substring(0, last);

                    Bag otherBag = bags.Find(b => b.Name == other);
                    bag.Contains.Add(otherBag, amount);
                }
            }

            Bag targetBag = bags.Find(b => b.Name == target);

            int partA = 0;
            foreach (Bag bag in bags) {
                if (bag == targetBag)
                    continue;
                if (CheckBag(bag))
                    partA++;
            }

            int partB = TotalBags(1, targetBag);

            Console.WriteLine("Part 1: " + partA);
            //Answer: 169
            Console.WriteLine("Part 2: " + partB);
            //Answer: 82372
        }

        private static bool CheckBag(Bag bag) {
            if (bag.Name == target)
                return true;

            bool check = false;
            foreach (KeyValuePair<Bag, int> b in bag.Contains) {
                check = check || CheckBag(b.Key);
            }
            return check;
        }

        private static int TotalBags(int mul, Bag bag) {
            int num = 0;
            foreach(var pair in bag.Contains)
                num += TotalBags(pair.Value, pair.Key) + pair.Value;
            return num * mul;
        }

        private class Bag {
            public string Name;
            public Dictionary<Bag, int> Contains;
            public string ContainsText;

            public Bag(string name, string text) {
                Name = name.Replace(" bags", "");
                Contains = new Dictionary<Bag, int>();
                ContainsText = text;
            }
        }
    }
}
