using System.Globalization;

namespace AoC.Day
{
    public class Day04
    {
        public static void Run(string file) {
            Console.WriteLine("Day 4: Passport Processing" + Environment.NewLine);

			List<string> lines = File.ReadAllLines(file).ToList();
            lines.Add(string.Empty); //To trigger last getting added to list
            
            List<Dictionary<string, string>> passports = new List<Dictionary<string, string>>();
            Dictionary<string, string> inprogress = new Dictionary<string, string>();
            foreach (string line in lines) {
                if(line.Length == 0) {
                    if (inprogress.Any()) {
                        passports.Add(inprogress);
                        inprogress = new Dictionary<string, string>();
                    }
                } else {
                    string[] fields = line.Split(' ');
                    foreach(string field in fields) {
                        string[] pair = field.Split(':');
                        inprogress.Add(pair[0], pair[1]);
                    }
                }
            }

            string[] required = { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "ecl", "pid" }; //Not "cid"
            string[] validEyeColors = ["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];

            int partA = 0;
            int partB = 0;
            foreach (Dictionary<string, string> passport in passports) {
                bool pA = true;
                foreach (string r in required) {
                    if (!passport.ContainsKey(r)) {
                        pA = false;
                        break;
                    }
                }
                if (pA) {
                    partA++;
                    bool pB = true;

                    int birthYear = int.Parse(passport["byr"]);
                    if (birthYear < 1920 || birthYear > 2002)
                        pB = false;

                    int issueYear = int.Parse(passport["iyr"]);
                    if (issueYear < 2010 || issueYear > 2020)
                        pB = false;

                    int expirationYear = int.Parse(passport["eyr"]);
                    if (expirationYear < 2020 || expirationYear > 2030)
                        pB = false;

                    string height = passport["hgt"];
                    int.TryParse(height.Substring(0, height.Length - 2), out int heightN);
                    if (height.Contains("cm")) {
                        if (heightN < 150 || heightN > 193)
                            pB = false;
                    } else if (height.Contains("in")) {
                        if (heightN < 59 || heightN > 76)
                            pB = false;
                    } else
                        pB = false;

                    string hairColor = passport["hcl"];
                    if (hairColor[0] != '#' || hairColor.Length != 7)
                        pB = false;
                    else {
                        bool vv = int.TryParse(hairColor.Substring(1), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int v);
                        if (!vv) {
                            //This never happened in my given input.
                            pB = false;
                        }
                    }

                    string eyeColor = passport["ecl"];
                    if (!validEyeColors.Contains(eyeColor))
                        pB = false;

                    string pid = passport["pid"];
                    bool pidParse = int.TryParse(pid, out int pidN);
                    if (pidParse) {
                        string pidVerify = pidN.ToString().PadLeft(9, '0');
                        if(pid != pidVerify || pid.Length != 9)
                            pB = false;
                    } else
                        pB = false;

                    passport.TryGetValue("cid", out string cid);

                    if (pB) {
                        Console.WriteLine("{0} {1} {2} {3} {4} {5} {6} {7}",
                            birthYear, issueYear, expirationYear, height.PadLeft(5, ' '), hairColor, eyeColor, pid, cid);
                        partB++;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 190
            Console.WriteLine("Part 2: " + partB);
            //Answer: 121
        }
    }
}
