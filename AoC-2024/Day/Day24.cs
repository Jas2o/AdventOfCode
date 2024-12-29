using System;
using System.Collections;
using System.Text;

namespace AoC.Day
{
    public class Day24
    {
        //This one was SUPER difficult for me to comprehend.

        public static void Run(string file, bool isTest) {
            Console.WriteLine("Day 24: Crossed Wires" + Environment.NewLine);

            List<string> outputWires = new List<string>();
            Dictionary<string, int> knownWires = new Dictionary<string, int>();

            string[] lines = File.ReadAllLines(file);
            Queue<string[]> queueInitial = new Queue<string[]>();
            bool mode = false;
            foreach (string line in lines) {
                if (line.Length == 0) {
                    mode = true;
                    continue;
                }

                if (mode) {
                    string[] fields = line.Split(' ');
                    queueInitial.Enqueue(new string[] { fields[0], fields[1], fields[2], fields[4] });
                    outputWires.Add(fields[4]);
                } else {
                    string[] fields = line.Split(": ");
                    knownWires.Add(fields[0], int.Parse(fields[1]));
                }
            }

            //--Part 1
            Queue<string[]> queue = new Queue<string[]>(queueInitial);
            long part1 = Simulate(queue, knownWires, new Dictionary<string, string>());

            //--Part 2
            string part2 = string.Empty;
            //Binary X and Y
            KeyValuePair<string, int>[] xs = knownWires.Where(w => w.Key.StartsWith('x')).OrderBy(w => w.Key).ToArray();
            KeyValuePair<string, int>[] ys = knownWires.Where(w => w.Key.StartsWith('y')).OrderBy(w => w.Key).ToArray();
            BitArray xb = new BitArray(xs.Length);
            BitArray yb = new BitArray(ys.Length);
            for (int i = 0; i < xs.Length; i++) {
                xb[i] = xs[i].Value == 1;
                yb[i] = ys[i].Value == 1;
            }
            //Binary Z
            KeyValuePair<string, int>[] zs = knownWires.Where(w => w.Key.StartsWith('z')).OrderBy(w => w.Key).ToArray();
            BitArray zb = new BitArray(zs.Length);
            for (int i = 0; i < zs.Length; i++)
                zb[i] = zs[i].Value == 1;
            //Turn them into numbers.
            long[] xyz = [
                GetIntFromBitArray(xb),
                GetIntFromBitArray(yb),
                GetIntFromBitArray(zb), //this should be same as part1
            ];
            Console.WriteLine("     X = {0} // {1}", xyz[0], Convert.ToString(xyz[0], 2));
            Console.WriteLine("     Y = {0} // {1}", xyz[1], Convert.ToString(xyz[1], 2));

            //For some tests we can't go on.
            if (zb.Length == 0)
                return;

            string required = string.Empty;
            long z = 0;
            int pairs = 4;
            List<string> possible;
            //The part 2 example and the real input work differently.
            if (isTest) {
                z = xyz[0] & xyz[1];
                required = Convert.ToString(z, 2);
                Console.WriteLine(" And Z = {0} // {1}", z, required);

                pairs = 2;
                possible = outputWires.ToList();
            } else {
                z = xyz[0] + xyz[1];
                required = Convert.ToString(z, 2);
                Console.WriteLine(" Sum Z = {0} // {1}", z, required);

                possible = new List<string>();

                //Have u/jewelcodesxo to thank for this part.
                foreach (string[] line in queueInitial) {
                    bool xy0 = line[0][0] == 'x' || line[0][0] == 'y';
                    bool xy2 = line[2][0] == 'x' || line[2][0] == 'y';
                    bool z3 = line[3][0] == 'z';
                    bool valid = true;

                    if (line[1] == "XOR") {
                        //XOR gates should take two intermediate (gibberish) inputs, and output a Z bit.

                        //If not, they should take a pair of X/Y bits and output an intermediate wire.
                        //In this case, the intermediate wire must eventually be forwarded to an AND gate, an XOR gate, or both.
                        if (!xy0 && !xy2 && z3) { }
                        else if(xy0 && xy2) {
                            if (!z3) {
                                string[] findAND = queueInitial.FirstOrDefault(n => n[1] == "AND" && (n[0] == line[3] || n[2] == line[3]));
                                string[] findOR = queueInitial.FirstOrDefault(n => n[1] == "XOR" && (n[0] == line[3] || n[2] == line[3]));
                                if (findAND == null && findOR == null)
                                    valid = false;
                            } else if (line[3] == "z00") {
                            } else {
                                valid = false;
                            }
                        } else {
                            valid = false;
                        }
                    } else if (line[1] == "OR") {
                        //OR gates should take two intermediate inputs and output a third intermediate wire.
                        //The output wire must eventually be forwarded to both an AND and an XOR gate.
                        if (!xy0 && !xy2) {
                            if (!z3) {
                                string[] findAND = queueInitial.FirstOrDefault(n => n[1] == "AND" && (n[0] == line[3] || n[2] == line[3]));
                                string[] findXOR = queueInitial.FirstOrDefault(n => n[1] == "XOR" && (n[0] == line[3] || n[2] == line[3]));
                                if (findAND == null || findXOR == null)
                                    valid = false;
                            } else if (line[3] == "z45") {
                            } else
                                valid = false;
                        } else
                            valid = false;
                    } else if (line[1] == "AND") {
                        //AND gates can take either a pair of intermediate inputs or a pair of X/Y inputs.
                        //In both cases, the output is an intermediate wire that must eventually be forwarded to an OR gate.
                        if (!xy0 && !xy2) {
                        } else if (xy0 && xy2) {
                            if (line[0].EndsWith("00") && line[2].EndsWith("00"))
                                continue;
                        } else
                            valid = false;
                        if (z3)
                            valid = false;

                        if (valid) {
                            string[] findOR = queueInitial.FirstOrDefault(n => n[1] == "OR" && (n[0] == line[3] || n[2] == line[3]));
                            if (findOR == null)
                                valid = false;
                        }
                    }

                    if (!valid && !possible.Contains(line[3]))
                        possible.Add(line[3]);
                }
            }

            //If the input has issues, then use random shenanigans until finding an answer.
            bool loop = (xyz[2] != z);
            Random rand = new Random();
            List<string> possibleCopy = new List<string>(possible);
            while (loop) {
                Console.Write('.');
                Dictionary<string, string> swap = new Dictionary<string, string>();
                for (int i = 0; i < pairs; i++) {
                    //Super dumb
                    int index = rand.Next(0, possible.Count);
                    string swap1 = possible[index];
                    possible.RemoveAt(index);

                    index = rand.Next(0, possible.Count);
                    string swap2 = possible[index];
                    possible.RemoveAt(index);

                    swap.Add(swap1, swap2);
                    swap.Add(swap2, swap1);
                }

                if (swap.Count != pairs * 2) {
                    Console.WriteLine("This ain't going to work.");
                    break;
                }

                Queue<string[]> newQueue = new Queue<string[]>(queueInitial);
                long test = Simulate(newQueue, knownWires, swap);
                if (test == z) {
                    loop = false;
                    part2 = string.Join(',', swap.Keys.Order().ToArray());
                    Console.WriteLine();
                    Console.WriteLine("Test Z = {0,2} // {1}", test, Convert.ToString(test, 2).PadLeft(required.Length, '0'));
                }

                //Reset before trying again.
                possible = new List<string>(possibleCopy);
            }

            Console.WriteLine(" Was Z = {0,2} // {1}", xyz[2], Convert.ToString(xyz[2], 2).PadLeft(required.Length, '0'));
            if (z == xyz[2])
                Console.WriteLine("Wires are fine.");
            
            //--End of Part 2

            Console.WriteLine();
            Console.WriteLine("Part 1: " + part1);
            //Answer: 66055249060558
            Console.WriteLine("Part 2: " + part2);
            //Answer: fcd,fhp,hmk,rvf,tpc,z16,z20,z33
            if (isTest)
                Console.WriteLine(" (test can have multiple matching answers)");
        }

        private static long GetIntFromBitArray(BitArray bitArray) {
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToInt64(array, 0);
        }

        private static long Simulate(Queue<string[]> queue, Dictionary<string, int> knownWires, Dictionary<string, string> swap) {
            while (queue.Any()) {
                bool resolved = false;
                string[] line = queue.Dequeue();
                bool left = int.TryParse(line[0], out int nleft);
                if (!left && knownWires.ContainsKey(line[0])) {
                    nleft = knownWires[line[0]];
                    left = true;
                }
                bool right = int.TryParse(line[2], out int nright);
                if (!right && knownWires.ContainsKey(line[2])) {
                    nright = knownWires[line[2]];
                    right = true;
                }

                //--

                string output = line[3];
                if (swap.ContainsKey(output))
                    output = swap[output];

                if (left && right) {
                    resolved = true;
                    switch (line[1]) {
                        case "AND":
                            knownWires[output] = nleft & nright;
                            break;
                        case "OR":
                            knownWires[output] = nleft | nright;
                            break;
                        case "XOR":
                            knownWires[output] = nleft ^ nright;
                            break;
                        default:
                            Console.WriteLine(line[1]);
                            break;
                    }
                }

                if (!resolved)
                    queue.Enqueue(line);
            }

            string[] zs = knownWires.Keys.Where(x => x.StartsWith('z')).OrderDescending().ToArray();
            StringBuilder sbBin = new StringBuilder();
            foreach (string z in zs) {
                sbBin.Append(knownWires[z]);
            }
            if (sbBin.Length > 0)
                return Convert.ToInt64(sbBin.ToString(), 2);
            return 0;
        }
    }
}
