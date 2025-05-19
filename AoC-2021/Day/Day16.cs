using System.Text;

namespace AoC.Day
{
    public class Day16
    {
        public static void Run(string file) {
            Console.WriteLine("Day 16: Packet Decoder");

			string[] lines = File.ReadAllLines(file);
            //The real input only has a single line.
            foreach (string input in lines) {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(input);
                Console.ResetColor();

                byte[] bytes = Convert.FromHexString(input);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
                string str = sb.ToString();

                int partA = 0;
                int offset = 0;
                long partB = ReadPacket(str, ref offset, ref partA);

                Console.WriteLine();
                Console.WriteLine("Part 1: " + partA);
                //Answer: 821
                Console.WriteLine("Part 2: " + partB);
                //Answer: 2056021084691
            }
        }

        private static long ReadPacket(string str, ref int offset, ref int versionSum) {
            int packetVersion = Convert.ToInt32(str.Substring(offset, 3), 2);
            versionSum += packetVersion;
            int packetTypeID = Convert.ToInt32(str.Substring(offset+3, 3), 2);
            offset += 6;

            if (packetTypeID == 4) {
                StringBuilder sb = new StringBuilder();
                while (true) {
                    char first = str[offset++];
                    string part = str.Substring(offset, 4);
                    sb.Append(part);
                    offset += 4;
                    if (first == '0')
                        break;
                }
                long num = Convert.ToInt64(sb.ToString(), 2);
                return num;
            }

            List<long> numbers = new List<long>();
            bool lengthTypeIDis0 = (str[offset++] == '0');
            if (lengthTypeIDis0) {
                string s15 = str.Substring(offset, 15);
                int totalLengthInBits = Convert.ToInt32(s15, 2);
                offset += 15;

                int endoffset = offset + totalLengthInBits;
                while (offset < endoffset) {
                    long num = ReadPacket(str, ref offset, ref versionSum);
                    numbers.Add(num);
                }
            } else {
                string s11 = str.Substring(offset, 11);
                int numSubPackets = Convert.ToInt32(s11, 2);
                offset += 11;

                for(int sub = 0; sub < numSubPackets; sub++) {
                    long num = ReadPacket(str, ref offset, ref versionSum);
                    numbers.Add(num);
                }
            }

            long result = 0;
            switch (packetTypeID) {
                case 0:
                    result = numbers.Sum();
                    break;
                case 1:
                    result = numbers[0];
                    for (int i = 1; i < numbers.Count; i++)
                        result *= numbers[i];
                    break;
                case 2:
                    result = numbers.Min();
                    break;
                case 3:
                    result = numbers.Max();
                    break;
                case 5:
                    result = (numbers[0] > numbers[1] ? 1 : 0);
                    break;
                case 6:
                    result = (numbers[0] < numbers[1] ? 1 : 0);
                    break;
                case 7:
                    result = (numbers[0] == numbers[1] ? 1 : 0);
                    break;
            }

            string type = ((PacketTypeID)packetTypeID).ToString();
            Console.WriteLine("{0} {1} = {2}", type, string.Join(',', numbers), result);

            return result;
        }

        enum PacketTypeID {
            Sum = 0,
            Product = 1,
            Minimum = 2,
            Maximum = 3,
            Literal = 4,
            GreaterThan = 5,
            LessThan = 6,
            EqualTo = 7
        }
    }
}
