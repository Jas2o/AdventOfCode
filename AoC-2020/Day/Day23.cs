using System.Text;

namespace AoC.Day
{
    public class Day23
    {
        public static void Run(string file) {
            Console.WriteLine("Day 23: Crab Cups" + Environment.NewLine);

            string input = File.ReadAllText(file);

            List<int> numbers = new List<int>();
            Console.WriteLine("-- start --");
            Console.Write("cups: ");
            for (int i = 0; i < input.Length; i++) {
                int num = (int)Char.GetNumericValue(input[i]);
                numbers.Add(num);
                Console.Write("{0} ", num);
            }
            Console.WriteLine("\r\n");

            CrabGame gameA = new CrabGame(numbers, false);
            gameA.Run();
            string partA = gameA.GetA();

            CrabGame gameB = new CrabGame(numbers, true);
            gameB.Run(10000000);
            (int num1, int num2, long partB) = gameB.GetB();

            Console.WriteLine("Part 1: " + partA);
            //Answer: 82934675
            Console.WriteLine("Part 2: {0} ({1} and {2})", partB, num1, num2);
            //Answer: 474600314018
        }

        private class CrabGame {
            private Dictionary<int, LinkedNode<int>> numToCup;
            private LinkedNode<int> root;
            private LinkedNode<int> current;

            public CrabGame(List<int> numbers, bool isPartB) {
                numToCup = new Dictionary<int, LinkedNode<int>>();
                foreach (int num in numbers) {
                    LinkedNode<int> addedCup = new LinkedNode<int>(num);
                    numToCup.Add(num, addedCup);
                }

                if(isPartB) {
                    for (int num = 10; num <= 1000000; num++) {
                        LinkedNode<int> addedCup = new LinkedNode<int>(num);
                        numToCup.Add(num, addedCup);
                    }
                }

                root = numToCup[numbers[0]];
                root.Next = root;
                current = root;
                foreach (KeyValuePair<int, LinkedNode<int>> cup in numToCup.Skip(1)) {
                    current.AddAfter(cup.Value);
                    current = cup.Value;
                }
                current = root;
            }

            public void Run(int moveLimit = 100) {
                int numMin = numToCup.Keys.Min();
                int numMax = numToCup.Keys.Max();
                int count = numToCup.Count();

                StringBuilder sb = new StringBuilder();

                int move = 0;
                bool textShow = (moveLimit == 10);
                while (move < moveLimit) {
                    move++;
                    if (textShow) {
                        Console.WriteLine("-- move {0} --", move);
                        sb.Clear();
                        LinkedNode<int> temp = root;
                        for (int i = 0; i < count; i++) {
                            if (temp == current)
                                sb.Append(string.Format("({0}) ", temp.Value));
                            else
                                sb.Append(string.Format("{0} ", temp.Value));
                            temp = temp.Next;
                        }
                    }

                    LinkedNode<int>[] pickup = new LinkedNode<int>[3];
                    pickup[0] = current.Next;
                    pickup[1] = current.Next.Next;
                    pickup[2] = current.Next.Next.Next;
                    IEnumerable<int> pickupNums = pickup.Select(p => p.Value);

                    int destNum = current.Value;
                    while (true) {
                        destNum--;
                        if (destNum < numMin)
                            destNum = numMax;
                        if (!pickupNums.Contains(destNum))
                            break;
                    }

                    LinkedNode<int> destCup = numToCup[destNum];
                    LinkedNode<int> destCupAfter = destCup.Next;

                    if (textShow) {
                        Console.WriteLine("cups: " + sb.ToString());
                        Console.WriteLine("pickup: " + string.Join(", ", pickupNums));
                        Console.WriteLine("destination: " + destNum);
                        Console.WriteLine();
                    }

                    if (current.Next == pickup[0])
                        current.Next = pickup[2].Next;

                    pickup[2].Next = destCupAfter;
                    destCup.Next = pickup[0];
                    current = current.Next;
                }
            }

            public string GetA() {
                string partA = string.Empty;

                int count = numToCup.Count();
                StringBuilder sb = new StringBuilder();
                LinkedNode<int> temp = numToCup[1];
                for (int i = 0; i < count; i++) {
                    if (temp == current)
                        sb.Append(string.Format("({0}) ", temp.Value));
                    else
                        sb.Append(string.Format("{0} ", temp.Value));
                    temp = temp.Next;
                }

                Console.WriteLine("-- final --");
                Console.WriteLine("cups: " + sb.ToString());
                Console.WriteLine();

                partA = sb.ToString().Replace(" ", "").Replace("1", "").Replace("(", "").Replace(")", "");
                return partA;
            }

            public (int, int, long) GetB() {
                int num1 = numToCup[1].Next.Value;
                int num2 = numToCup[1].Next.Next.Value;
                return (num1, num2, Math.BigMul(num1, num2));
            }
        }

        //Based on 2018 Day 9.
        private class LinkedNode<T> {
            public T Value;
            public LinkedNode<T> Next;

            public LinkedNode(T data) {
                Value = data;
            }

            public void AddAfter(LinkedNode<T> after) {
                LinkedNode<T> orgNext = Next;
                Next = after;
                Next.Next = orgNext;
            }
        }
    }
}
