namespace AoC.Day
{
    public class Day07
    {
        public static void Run(string file) {
            Console.WriteLine("Day 7: No Space Left On Device" + Environment.NewLine);

            string[] lines = File.ReadAllLines(file);

            List<FakeFile> fakefiles = new List<FakeFile>();
            List<string> path = new List<string>();
            Dictionary<string, int> knownPaths = new Dictionary<string, int>();
            knownPaths.Add("/", 0);

            foreach (string line in lines) {
                if (line[0] == '$') {
                    if (line == "$ ls")
                        continue;
                    else if (line == "$ cd /")
                        path.Clear();
                    else if (line == "$ cd ..")
                        path.RemoveAt(path.Count() - 1);
                    else {
                        string part = line.Substring(5);
                        path.Add(part);
                        knownPaths.TryAdd('/' + string.Join('/', path) + '/', 0);
                    }
                } else {
                    string[] fields = line.Split(' ', 2);
                    int num = 0;
                    if (fields[0] != "dir")
                        num = int.Parse(fields[0]);
                    FakeFile fake = new FakeFile(fields[1], '/' + string.Join('/', path), num);
                    fakefiles.Add(fake);
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach(var fake in fakefiles) {
                Console.WriteLine(fake.ToString());
            }
            Console.ResetColor();
            Console.WriteLine();

            //Part 1
            foreach (KeyValuePair<string, int> known in knownPaths) {
                IEnumerable<FakeFile> collection = fakefiles.Where(f => f.Path.StartsWith(known.Key));
                int sum = collection.Sum(f => f.Size);
                knownPaths[known.Key] = sum;
                Console.WriteLine("{1,8} = {0}", known.Key, sum);
                /*
                if (sum > 100000)
                    continue;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                foreach (var c in collection)
                    Console.WriteLine(c.ToString());
                Console.ResetColor();
                */
            }
            int partA = knownPaths.Values.Where(v => v <= 100000).Sum();

            //Part 2
            int diskTotal = 70000000;
            int diskNeeded = 30000000;
            int diskUsed = diskTotal - knownPaths["/"];
            int diff = diskNeeded - diskUsed;
            IEnumerable<KeyValuePair<string, int>> deleteOptions = knownPaths.OrderBy(s => s.Value).Where(s => s.Value > diff);
            KeyValuePair<string, int> toDelete = deleteOptions.MinBy(x => x.Value);
            Console.WriteLine("\r\nCurrently using {0}, need {1} more.", diskUsed, diff);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Delete {0} to free up {1}.", toDelete.Key, toDelete.Value);
            Console.ResetColor();
            int partB = toDelete.Value;

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1307902
            Console.WriteLine("Part 2: " + partB);
            //Answer: 7068748
        }

        private class FakeFile {
            public string Name;
            public string Path;
            public int Size;

            public FakeFile(string name, string path, int size) {
                Name = name;
                if(!path.EndsWith('/'))
                    Path = path + '/';
                else
                    Path = path;
                Size = size;
            }

            public override string ToString() {
                return string.Format("{2,8} = {0}{1}", Path, Name, Size);
            }
        }
    }
}
