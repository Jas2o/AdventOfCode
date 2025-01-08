using AoC.Shared;

namespace AoC.Day {
    public class Day22
    {
        //private const bool AllowInputWhenQueueEmpty = false;

        public static void Run(string file) {
            Console.WriteLine("Day 22: Wizard Simulator 20XX" + Environment.NewLine);

            Unit player = new Unit("player", 50, 0, 500);
            Unit boss = new Unit("boss", int.MaxValue, int.MaxValue, 0);

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int num = int.Parse(line.Substring(line.IndexOf(':') + 2));
                if (line.Contains("Hit Points"))
                    boss.HP = num;
                else if (line.Contains("Damage"))
                    boss.Damage = num;
            }

            int manaSpentA = 0;
            int manaSpentB = 0;
            if (boss.HP < 20) {
                player.HP = 10;
                player.Mana = 250;

                string[] playerActions13 = {
                    "Poison",
                    "Magic Missile"
                };

                string[] playerActions14 = {
                    "Recharge",
                    "Shield",
                    "Drain",
                    "Poison",
                    "Magic Missile"
                };

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Running with the first test action set.");
                Console.ResetColor();
                manaSpentA = Simulate(player, boss, playerActions13, true, false); //Verbose, easy mode
                Console.WriteLine("\r\nPart 1: " + manaSpentA + "\r\n");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Adding 1 HP to boss and running with the second test action set.");
                Console.ResetColor();
                boss.HP++;
                manaSpentA = Simulate(player, boss, playerActions14, true, false);
            } else {
                manaSpentA = int.MaxValue;
                manaSpentB = int.MaxValue;
                Dictionary<char, Tuple<string, int>> dIntToSpell = new Dictionary<char, Tuple<string, int>>() {
                    {'0', new Tuple<string, int>("Magic Missile", 53)},
                    {'1', new Tuple<string, int>("Drain", 73)},
                    {'2', new Tuple<string, int>("Shield", 113)},
                    {'3', new Tuple<string, int>("Poison", 173)},
                    {'4', new Tuple<string, int>("Recharge", 229)}
                };

                int ops = 0;
                Console.WriteLine("Easy mode:");
                while (true) {
                    string mask = Number.IntToString(ops, 5).PadLeft(0, '0');
                    if (mask.Length > 9)
                        break;

                    int calcCost = 0;
                    string[] actions = new string[mask.Length];
                    for(int i = 0; i < mask.Length; i++) {
                        Tuple<string, int> spell = dIntToSpell[mask[i]];
                        actions[i] = spell.Item1;
                        calcCost += spell.Item2;
                    }

                    if (calcCost < manaSpentA) {
                        int trial = Simulate(player, boss, actions, false, false); //No verbose, easy mode.
                        if (trial > 0) {
                            //Simulate(player, boss, actions, true); //If need to see the output of the last run.
                            if (trial < manaSpentA) {
                                manaSpentA = trial;
                                Console.WriteLine("- Used {0} mana with spells: {1}", trial, string.Join(", ", actions));
                            }
                        }
                    }
                    ops++;
                }
                Console.WriteLine();

                //Part 2
                ops = 5;
                Console.WriteLine("Hard mode:");
                while (true) {
                    string mask = Number.IntToString(ops, 5).PadLeft(0, '0');
                    if (mask.Length > 9)
                        break;

                    int calcCost = 0;
                    string[] actions = new string[mask.Length];
                    for (int i = 0; i < mask.Length; i++) {
                        Tuple<string, int> spell = dIntToSpell[mask[i]];
                        actions[i] = spell.Item1;
                        calcCost += spell.Item2;
                    }

                    if (calcCost < manaSpentB) {
                        int trial = Simulate(player, boss, actions, false, true); //No verbose, hard mode.
                        if (trial > 0) {
                            //Simulate(player, boss, actions, true, true);
                            if (trial < manaSpentB) {
                                manaSpentB = trial;
                                Console.WriteLine("- Used {0} mana with spells: {1}", trial, string.Join(", ", actions));
                            }
                        }
                    }
                    ops++;
                }
                
            }

            /*
            if(manaSpentA == int.MaxValue)
                Console.WriteLine("No solution found.");
            else if(manaSpentA > 0)
                Console.WriteLine("Success, spent {0} mana.", manaSpentA);
            else if (manaSpentA == 0)
                Console.WriteLine("You died.");
            else if (manaSpentA < 0)
                Console.WriteLine("You forgot any spell names.");
            */

            Console.WriteLine();
            Console.WriteLine("Part 1: " + manaSpentA);
            //Answer: 953
            Console.WriteLine("Part 2: " + manaSpentB);
            //Answer: 1289
        }

        private class Unit {
            public string Name;
            public int HP;
            public int Damage; //Boss only
            public int Armor; //Boss starts with, player can get from effect.
            public int Mana; //Player only, has no max limit.

            public int EffectShield;
            public int EffectPoison;
            public int EffectRecharge;

            public Unit(string name, int hp, int damage, int mana) {
                Name = name;
                HP = hp;
                Damage = damage;
                Mana = mana;
                Armor = 0;
            }

            public Unit(Unit b) {
                Name = b.Name;
                HP = b.HP;
                Damage = b.Damage;
                Mana = b.Mana;
                Armor = b.Armor;
            }

            public void EffectsTick(bool verbose) {
                if(EffectShield > 0) {
                    EffectShield--;
                    if(verbose)
                        Console.WriteLine("Shield's timer is now {0}.", EffectShield);
                    if (EffectShield == 0) {
                        Armor -= 7;
                        if (verbose)
                            Console.WriteLine("Shield wears off, decreasing armor by 7.");
                    }
                }

                if (EffectPoison > 0) {
                    EffectPoison--;
                    HP -= 3;
                    if (verbose)
                        Console.WriteLine("Poison deals 3 damage; its timer is now {0}.", EffectPoison);
                    if (EffectPoison == 0) {
                        if (verbose)
                            Console.WriteLine("Poison wears off.");
                    }
                }

                if (EffectRecharge > 0) {
                    EffectRecharge--;
                    Mana += 101;
                    if(verbose)
                        Console.WriteLine("Recharge provides 101 mana; its timer is now {0}.", EffectRecharge);
                    if (EffectRecharge == 0) {
                        if (verbose)
                            Console.WriteLine("Recharge wears off.");
                    }
                }
            }
        }

        private static int Simulate(Unit baseplayer, Unit baseboss, string[]? playerActions, bool verbose, bool hardMode) {
            Unit player = new Unit(baseplayer);
            Unit boss = new Unit(baseboss);
            Queue<string> actions = new Queue<string>();
            if (playerActions != null) {
                foreach (string action in playerActions)
                    actions.Enqueue(action);
            }

            int manaSpent = 0;

            while (true) {
                if (hardMode) {
                    player.HP -= 1;
                    if (player.HP <= 0) {
                        if (verbose) {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("Life is too hard.");
                            Console.ResetColor();
                        }
                        return 0;
                    }
                }

                if (verbose) {
                    Console.WriteLine("-- Player turn --");
                    if (hardMode) {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Player lost 1 hit point because life is hard.");
                        Console.ResetColor();
                    }
                    Console.WriteLine("- Player has {0} hit points, {1} armor, {2} mana", player.HP, player.Armor, player.Mana);
                    Console.WriteLine("- Boss has {0} hit points", boss.HP);
                }
                player.EffectsTick(verbose);
                boss.EffectsTick(verbose);

                if (player.Mana < 53) {
                    if (verbose) {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("You died from not casting anything, I guess?");
                        Console.ResetColor();
                    }
                    return 0;
                }

                bool castedSomething = false;
                while (!castedSomething) {
                    string action = string.Empty;
                    bool queuedAction = actions.TryDequeue(out action);
                    if (!queuedAction) {
                        /*
                        if (AllowInputWhenQueueEmpty && verbose) {
                            Console.Write("Spell> ");
                            action = Console.ReadLine();
                        } else {
                        */
                            if (verbose)
                                Console.WriteLine("Player ran out of spells.");
                            return -1;
                        //}
                    }

                    if (action == "Magic Missile") {
                        if (player.Mana >= 53) {
                            castedSomething = true;
                            manaSpent += 53;
                            player.Mana -= 53;
                            boss.HP -= 4;
                            if (verbose)
                                Console.WriteLine("Player casts {0}, dealing {1} damage.", action, 4);
                        }
                    } else if (action == "Drain") {
                        if (player.Mana >= 73) {
                            castedSomething = true;
                            manaSpent += 73;
                            player.Mana -= 73;
                            boss.HP -= 2;
                            player.HP += 2;
                            if (verbose)
                                Console.WriteLine("Player casts {0}, dealing 2 damage, and healing 2 hit points.", action);
                        }
                    } else if (action == "Shield") {
                        if (player.Mana >= 113 && player.EffectShield == 0) {
                            castedSomething = true;
                            manaSpent += 113;
                            player.Mana -= 113;
                            player.EffectShield = 6;
                            player.Armor = 7; //This is temporary.
                            if (verbose)
                                Console.WriteLine("Player casts {0}, increasing armor by 7.", action);
                        }
                    } else if (action == "Poison") {
                        if (player.Mana >= 173 && boss.EffectRecharge == 0) {
                            castedSomething = true;
                            manaSpent += 173;
                            player.Mana -= 173;
                            boss.EffectPoison = 6;
                            //boss.HP -= 3; //This is supposed to tick start of each turn.
                            if (verbose)
                                Console.WriteLine("Player casts {0}.", action);
                        }
                    } else if (action == "Recharge") {
                        if (player.Mana >= 229 && player.EffectRecharge == 0) {
                            castedSomething = true;
                            manaSpent += 229;
                            player.Mana -= 229;
                            player.EffectRecharge = 5;
                            //player.Mana += 101; //This is supposed to tick start of each turn.
                            if (verbose)
                                Console.WriteLine("Player casts {0}.", action);
                        }
                    } else {
                        //Console.ForegroundColor = ConsoleColor.Red;
                        //Console.WriteLine("Using an invalid spell would upset your mentor.");
                        //Console.ResetColor();
                    }
                }

                if (verbose)
                    Console.WriteLine();

                if (boss.HP <= 0) {
                    if (verbose) {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Boss killed by direct spell.");
                        Console.ResetColor();
                    }
                    return manaSpent;
                }

                if (verbose) {
                    Console.WriteLine("-- Boss turn --");
                    Console.WriteLine("- Player has {0} hit points, {1} armor, {2} mana", player.HP, player.Armor, player.Mana);
                    Console.WriteLine("- Boss has {0} hit points", boss.HP);
                }
                player.EffectsTick(verbose);
                boss.EffectsTick(verbose);

                if (boss.HP <= 0) {
                    if (verbose) {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Boss killed by poison effect.");
                        Console.ResetColor();
                    }
                    return manaSpent;
                }

                int bDamageDealt = Math.Max(boss.Damage - player.Armor, 1);
                player.HP -= bDamageDealt;
                if (verbose) {
                    Console.WriteLine("Boss attacks for {0} damage, player now at {1}.", bDamageDealt, player.HP);
                }

                if (player.HP <= 0)
                    return 0;

                if (verbose)
                    Console.WriteLine();
            }
        }
    }
}
