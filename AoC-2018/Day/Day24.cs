namespace AoC.Day
{
    public class Day24
    {
        public static void Run(string file) {
            Console.WriteLine("Day 24: Immune System Simulator 20XX" + Environment.NewLine);

			string[] lines = File.ReadAllLines(file);

            int mode = 0;
            List<Group> immuneSys = new List<Group>();
            List<Group> infection = new List<Group>();
            foreach (string line in lines) {
                if (line.Length < 5)
                    continue;
                else if (line.StartsWith("Immune"))
                    mode = 1;
                else if (line.StartsWith("Infection"))
                    mode = 2;
                else {
                    if (mode == 1) {
                        Group g = new Group(false, line, immuneSys.Count() + 1);
                        immuneSys.Add(g);
                    } else if (mode == 2) {
                        Group g = new Group(true, line, infection.Count() + 1);
                        infection.Add(g);
                    }
                }
            }

            (bool partAres, int partA) = Solve(immuneSys, infection, true, 0);
            int boost = 0;
            int partB = 0;
            while(true) {
                boost++;
                (bool immuneWin, int units) = Solve(immuneSys, infection, false, boost);
                if(immuneWin) {
                    partB = units;
                    if (false) {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("With immune system having a boost of {0}", boost);
                        Console.ResetColor();
                        Solve(immuneSys, infection, true, boost);
                    }
                    break;
                }
            }

            Console.WriteLine("Part 1: " + partA);
            //Answer: 9878
            Console.WriteLine("Part 2: {0} (boost of {1})", partB, boost);
            //Answer: 10954
        }

        private static (bool immuneSysWins, int unitsRem) Solve(List<Group> base_immuneSys, List<Group> base_infection, bool verbose, int immuneSysBoost) {
            //Copy the original state and reset if needed
            List<Group> immuneSys = new List<Group>(base_immuneSys);
            List<Group> infection = new List<Group>(base_infection);
            if (immuneSysBoost != 0) {
                foreach (Group g in immuneSys) {
                    g.Reset();
                    g.AttackAmount += immuneSysBoost;
                }
                foreach (Group g in infection) {
                    g.Reset();
                }
            }

            int round = 0;
            while (true) {
                round++;
                if (verbose) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Round {0}", round);
                    Console.ResetColor();
                    foreach (Group g in immuneSys)
                        Console.WriteLine("Immune System group {0} contains {1} units", g.ID, g.UnitsNum);
                    foreach (Group g in infection)
                        Console.WriteLine("Infection group {0} contains {1} units", g.ID, g.UnitsNum);
                }

                if (immuneSys.Count() == 0 || infection.Count() == 0)
                    break;

                if (verbose)
                    Console.WriteLine("\r\n# Targetting");
                Dictionary<(bool, int), Consideration> chosen = new Dictionary<(bool, int), Consideration>();
                TargetSelection(chosen, infection, immuneSys, verbose);
                TargetSelection(chosen, immuneSys, infection, verbose);

                if (!chosen.Any())
                    break;

                if (verbose)
                    Console.WriteLine("\r\n# Attacking");
                chosen = chosen.OrderByDescending(c => c.Value.Attacker.Initiative).ToDictionary();
                foreach (KeyValuePair<(bool, int), Consideration> c in chosen) {
                    //The damage value might have changed if units were killed between consideration and turn to attack.
                    int damage = c.Value.Attacker.EffectivePower;
                    if (c.Value.Defender.ImmuneTo.Contains(c.Value.Attacker.AttackType))
                        damage = 0;
                    if (c.Value.Defender.WeakTo.Contains(c.Value.Attacker.AttackType))
                        damage *= 2;

                    decimal remaining = (c.Value.Defender.UnitsHP * c.Value.Defender.UnitsNum) - damage;
                    remaining = Math.Ceiling(remaining / c.Value.Defender.UnitsHP);
                    int killed = Math.Min(c.Value.Defender.UnitsNum - (int)remaining, c.Value.Defender.UnitsNum);
                    if (killed > 0)
                        c.Value.Defender.UnitsNum = Math.Max(0, (int)remaining);
                    if (verbose) {
                        if (killed == 0)
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("{0} group {1} attacks defending group {2} ({3} damage), killing {4} units, {5} remaining", c.Value.Team, c.Value.Attacker.ID, c.Value.Defender.ID, damage, killed, c.Value.Defender.UnitsNum);
                        Console.ResetColor();
                    }
                }

                immuneSys = immuneSys.Where(i => i.UnitsNum > 0).ToList();
                infection = infection.Where(i => i.UnitsNum > 0).ToList();

                if (verbose)
                    Console.WriteLine();
            }

            if (verbose)
                Console.WriteLine();

            if (infection.Any())
                return (false, infection.Sum(i => i.UnitsNum));
            else
                return (true, immuneSys.Sum(i => i.UnitsNum)); 
        }

        private static void TargetSelection(Dictionary<(bool, int), Consideration> chosen, List<Group> attackers, List<Group> defenders, bool verbose) {
            IOrderedEnumerable<Group> attackersOrder = attackers.OrderByDescending(a => a.EffectivePower).ThenBy(a => a.Initiative);

            foreach (Group attacker in attackersOrder) {
                string team = attacker.IsInfection ? "Infection" : "Immune System";
                int ep = attacker.EffectivePower;
                Dictionary<Consideration, int> considerations = new Dictionary<Consideration, int>();
                foreach (Group defender in defenders) {
                    int damage = ep;
                    if (defender.ImmuneTo.Contains(attacker.AttackType))
                        damage = 0;
                    if (defender.WeakTo.Contains(attacker.AttackType))
                        damage *= 2;

                    Consideration c = new Consideration(team, attacker, defender, damage);
                    if(damage > 0)
                        considerations.Add(c, damage);
                }

                if (!considerations.Any())
                    continue;

                bool canChoose = true;
                int highest = considerations.Max(t => t.Value);
                considerations = considerations.OrderByDescending(t => t.Value).ThenByDescending(t => t.Key.Defender.EffectivePower).ToDictionary();
                foreach(KeyValuePair<Consideration, int> consideration in considerations) {
                    if(canChoose) {
                        if (chosen.TryAdd((attacker.IsInfection, consideration.Key.Defender.ID), consideration.Key)) {
                            canChoose = false;
                            if (verbose)
                                Console.WriteLine(consideration.Key.ToString());
                        }
                    }
                }
            }
        }

        private class Consideration {
            public string Team;
            public Group Attacker;
            public Group Defender;
            public int Damage;

            public Consideration(string team, Group attacker, Group defender, int damage) {
                Team = team;
                Attacker = attacker;
                Defender = defender;
                Damage = damage;
            }

            public override string ToString() {
                return string.Format("{0} group {1} would deal defending group {2} {3} damage", Team, Attacker.ID, Defender.ID, Damage);
            }

        }

        private class Group {
            public bool IsInfection;
            public int ID;
            public int UnitsNumBase;
            public int UnitsNum;
            public int UnitsHP;
            public string[] WeakTo;
            public string[] ImmuneTo;
            public int AttackAmountBase;
            public int AttackAmount;
            public string AttackType;
            public int Initiative;

            public Group(bool isInfection, string line, int id) {
                IsInfection = isInfection;
                ID = id;
                int iNumUnits = line.IndexOf(' ');
                int iWith = line.IndexOf(" with") + 5;
                int iHit = line.IndexOf(" hit");
                int iDoes = line.IndexOf(" does") + 6;
                int iDamage = line.IndexOf(" damage");
                int iInitiative = line.IndexOf(" initiative") + 11;

                UnitsNumBase = UnitsNum = int.Parse(line.Substring(0, iNumUnits));
                UnitsHP = int.Parse(line.Substring(iWith, iHit - iWith));

                int bLeft = line.IndexOf('(') + 1;
                int bRight = line.IndexOf(')');
                if (bLeft > 0) {
                    string[] bracket = line.Substring(bLeft, bRight - bLeft).Split("; ");
                    foreach (string b in bracket) {
                        string[] parts = b.Replace(",", "").Split(' ');
                        if (parts[0] == "weak")
                            WeakTo = parts.Skip(2).ToArray();
                        else if (parts[0] == "immune")
                            ImmuneTo = parts.Skip(2).ToArray();
                        else
                            throw new Exception();
                    }
                }
                if (WeakTo == null) WeakTo = new string[0];
                if (ImmuneTo == null) ImmuneTo = new string[0];

                string[] damage = line.Substring(iDoes, iDamage - iDoes).Split(' ');
                AttackAmountBase = AttackAmount = int.Parse(damage[0]);
                AttackType = damage[1];
                Initiative = int.Parse(line.Substring(iInitiative));
            }

            public void Reset() {
                UnitsNum = UnitsNumBase;
                AttackAmount = AttackAmountBase;
            }
            
            public int EffectivePower {
                get { return UnitsNum * AttackAmount; }
            }
        }
    }
}
