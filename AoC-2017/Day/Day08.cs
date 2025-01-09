namespace AoC.Day
{
    public class Day08
    {
        public static void Run(string file) {
            Console.WriteLine("Day 8: I Heard You Like Registers" + Environment.NewLine);

            Dictionary<string, int> dRegister = new Dictionary<string, int>();

			string[] lines = File.ReadAllLines(file);
            int highest = 0;
            foreach (string line in lines) {
                string[] fields = line.Split(' ');

                string left = fields[0];
                string leftOp = fields[1];
                int leftNum = int.Parse(fields[2]);
                if (!dRegister.ContainsKey(left))
                    dRegister.Add(left, 0);

                string right = fields[4];
                string rightOp = fields[5];
                int rightNum = int.Parse(fields[6]);
                if (!dRegister.ContainsKey(right))
                    dRegister.Add(right, 0);

                bool check = false;
                switch (rightOp) {
                    case ">": check = dRegister[right] > rightNum; break;
                    case "<": check = dRegister[right] < rightNum; break;
                    case ">=": check = dRegister[right] >= rightNum; break;
                    case "<=": check = dRegister[right] <= rightNum; break;
                    case "==": check = dRegister[right] == rightNum; break;
                    case "!=": check = dRegister[right] != rightNum; break;
                    default:
                        throw new Exception();
                }

                if (check) {
                    if (leftOp == "inc")
                        dRegister[left] += leftNum;
                    else //dec
                        dRegister[left] -= leftNum;
                }

                highest = Math.Max(highest, dRegister[left]);
            }

            int largest = dRegister.Max(r => r.Value);

            Console.WriteLine("Part 1: " + largest);
            //Answer: 4567
            Console.WriteLine("Part 2: " + highest);
            //Answer: 5636
        }
    }
}
