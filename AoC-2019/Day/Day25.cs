using System.Text;

namespace AoC.Day
{
    public class Day25
    {
        //Currently requires manual entry to get to the answer.

        public static void Run(string file) {
            Console.WriteLine("Day 25: Cryostasis" + Environment.NewLine);

            string input = File.ReadAllText(file);
            if (input.IndexOf(',') == -1) {
                Console.WriteLine("There is no test for this day.");
                return;
            }
            long[] initial = Array.ConvertAll(input.Split(','), long.Parse);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("== Keyboard Controls ==");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("WASD to move north/west/south/east");
            Console.WriteLine("T to take item (you might be prompted to type the item name)");
            Console.WriteLine("Y to drop item (you will be prompted to type the item name)");
            Console.WriteLine("I to check inventory.");
            Console.WriteLine("ESC if you want to quit.");
            Console.ResetColor();
            Console.WriteLine();

            string command = string.Empty;
            IntCode computer = new IntCode(initial, []);
            while (!computer.Halted) {
                if(command.Length > 0) {
                    foreach (char c in command)
                        computer.inputQueue.Enqueue(c);
                    computer.inputQueue.Enqueue(10);
                }

                computer.Run();

                StringBuilder sb = new StringBuilder();
                while (computer.outputQueue.Any()) {
                    long value = computer.outputQueue.Dequeue();
                    if (value < 128)
                        sb.Append((char)value);
                    else {
                        break;
                    }
                }

                string[] lines = sb.ToString().Split("\n", StringSplitOptions.RemoveEmptyEntries);
                bool listingItems = false;
                List<string>? itemsHere = null;
                foreach (string line in lines) {
                    if (line.StartsWith("=="))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (computer.Halted && line.Length > 0) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                    }

                    if (line.StartsWith("Items here:")) {
                        listingItems = true;
                        itemsHere = new List<string>();
                    } else if (listingItems) {
                        if (line.StartsWith('-'))
                            itemsHere.Add(line.Replace("- ", ""));
                        else
                            listingItems = false;
                    }
                    Console.WriteLine(line);

                    Console.ResetColor();
                }

                if (!computer.Halted) {
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    command = string.Empty;
                    if (key.Key == ConsoleKey.W)
                        command = "north";
                    else if (key.Key == ConsoleKey.S)
                        command = "south";
                    if (key.Key == ConsoleKey.A)
                        command = "west";
                    else if (key.Key == ConsoleKey.D)
                        command = "east";
                    else if (key.Key == ConsoleKey.I)
                        command = "inv";
                    else if (key.Key == ConsoleKey.T || key.Key == ConsoleKey.P) {
                        string item = string.Empty;
                        if (itemsHere != null && itemsHere.Count == 1) {
                            item = itemsHere[0];
                            Console.Write("take > " + item);
                        } else {
                            Console.Write("take > ");
                            item = Console.ReadLine();
                        }
                        command = "take " + item;
                        if (item == "infinite loop") {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine("\r\n(This would normally lead to an infinite loop)");
                            Console.ResetColor();
                            break;
                        }
                    } else if (key.Key == ConsoleKey.Y) {
                        //Y for "yeet".
                        //Yes I hate it, no I can't think of a better key than D which is already taken.
                        Console.Write("drop > ");
                        string item = Console.ReadLine();
                        command = "drop " + item;
                    } else if(key.Key == ConsoleKey.Escape) {
                        Console.Write("-EMERGENCY EJECT-\r\n");
                        break;
                    }
                    //--
                    if (command != string.Empty)
                        Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: (this would be written above if you found it)");
            //Press keys: SAAT,DDST,WWDT,SDSDT,WA
            //This will pickup items for my given input (astrolabe, candy cane, food ration, space law space brochure) and pass through checkpoint.
            //Answer: 2415919488
            Console.WriteLine("Part 2: (N/A)");
			//Answer: (there is no Part 2).
        }
    }
}
