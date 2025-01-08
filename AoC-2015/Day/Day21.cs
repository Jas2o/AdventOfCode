using AoC.Shared;

namespace AoC.Day {
    public class Day21
    {
        public static void Run(string file) {
            Console.WriteLine("Day 21: RPG Simulator 20XX" + Environment.NewLine);

            Unit player = new Unit("player", 100, 0, 0);
            Unit boss = new Unit("boss", int.MaxValue, int.MaxValue, int.MaxValue);

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                int num = int.Parse(line.Substring(line.IndexOf(':') + 2));
                if (line.Contains("Hit Points"))
                    boss.HP = num;
                else if (line.Contains("Damage"))
                    boss.Damage = num;
                else if (line.Contains("Armor"))
                    boss.Armor = num;
            }

            //Item Shop
            List<Item> shop = new List<Item>() {
                new Item(ItemType.Weapon, "Dagger", 8, 4, 0),
                new Item(ItemType.Weapon, "Shortsword", 10, 5, 0),
                new Item(ItemType.Weapon, "Warhammer", 25, 6, 0),
                new Item(ItemType.Weapon, "Longsword", 40, 7, 0),
                new Item(ItemType.Weapon, "Greataxe", 74, 8, 0),

                new Item(ItemType.Armor, "NONE", 0, 0, 0),
                new Item(ItemType.Armor, "Leather", 13, 0, 1),
                new Item(ItemType.Armor, "Chainmail", 31, 0, 2),
                new Item(ItemType.Armor, "Splintmail", 53, 0, 3),
                new Item(ItemType.Armor, "Bandedmail", 75, 0, 4),
                new Item(ItemType.Armor, "Platemail", 102, 0, 5),

                new Item(ItemType.Ring, "NONEv1", 0, 0, 0),
                new Item(ItemType.Ring, "NONEv2", 0, 0, 0),
                new Item(ItemType.Ring, "Damage + 1", 25, 1, 0),
                new Item(ItemType.Ring, "Damage + 2", 50, 2, 0),
                new Item(ItemType.Ring, "Damage + 3", 100, 3, 0),
                new Item(ItemType.Ring, "Defense + 1", 20, 0, 1),
                new Item(ItemType.Ring, "Defense + 2", 40, 0, 2),
                new Item(ItemType.Ring, "Defense + 3", 80, 0, 3)
            };

            List<Item> weapons = shop.FindAll(x => x.Type == ItemType.Weapon);
            List<Item> armors = shop.FindAll(x => x.Type == ItemType.Armor);
            List<Item> rings = shop.FindAll(x => x.Type == ItemType.Ring);

            //Part 1
            int minCostAndWin = int.MaxValue;
            int maxCostAndLose = 0;
            if (lines[0].Contains("Test")) {
                minCostAndWin = 0;

                player.HP = 8;
                player.Damage = 5;
                player.Armor = 5;

                Console.WriteLine("Player stats: {0} HP, {1} atk, {2} def", player.HP, player.Damage, player.Armor);

                bool result = Simulate(player, boss);
                Console.WriteLine(result);
            } else {
                //Console.WriteLine("Player starting stats: {0} HP, {1} atk, {2} def", player.HP, player.Damage, player.Armor);
                //Console.WriteLine();

                int ops = 0;
                while (true) {
                    ops++;
                    string mask = Number.IntToString(ops, 8).PadLeft(4, '0');
                    if (mask.Length > 4)
                        throw new Exception();
                    if (mask[0] == '5')
                        break;
                    if (mask[1] == '6' || mask[1] == '7' || mask[2] == mask[3])
                        continue;

                    int nw = (int)char.GetNumericValue(mask[0]);
                    int na = (int)char.GetNumericValue(mask[1]);
                    int nr1 = (int)char.GetNumericValue(mask[2]);
                    int nr2 = (int)char.GetNumericValue(mask[3]);

                    Item w = weapons[nw];
                    Item a = armors[na];
                    Item r1 = rings[nr1];
                    Item r2 = rings[nr2];

                    player.Damage = w.Damage + a.Damage + r1.Damage + r2.Damage;
                    player.Armor = w.Armor + a.Armor + r1.Armor + r2.Armor;
                    int cost = w.Cost + a.Cost + r1.Cost + r2.Cost;

                    bool result = Simulate(player, boss);
                    if (result) {
                        //Console.WriteLine("Success with cost of {4} using {0}, {1}, {2}, {3}", w.Name, a.Name, r1.Name, r2.Name, cost.ToString().PadLeft(3));
                        if (cost < minCostAndWin)
                            minCostAndWin = cost;
                    } else {
                        //if(!a.Name.Contains("NONE") && !r1.Name.Contains("NONE") && !r2.Name.Contains("NONE"))
                        if(cost > 140)
                            Console.WriteLine("Fail with cost of {4} using {0}, {1}, {2}, {3}", w.Name, a.Name, r1.Name, r2.Name, cost.ToString().PadLeft(3));
                        //Funny to see that if you buy the two expensive damage rings, cheapest weapon, and no/leather armor you will lose.

                        if (cost > maxCostAndLose)
                            maxCostAndLose = cost;
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Part 1: " + minCostAndWin);
            //Answer: 91
            Console.WriteLine("Part 2: " + maxCostAndLose);
            //Answer: 158
        }

        private class Unit {
            public string Name;
            public int HP;
            public int Damage;
            public int Armor;

            public Unit(string name, int hp, int damage, int armor) {
                Name = name;
                HP = hp;
                Damage = damage;
                Armor = armor;
            }

            public Unit(Unit b) {
                Name = b.Name;
                HP = b.HP;
                Damage = b.Damage;
                Armor = b.Armor;
            }
        }

        private enum ItemType {
            Weapon,
            Armor,
            Ring
        };

        private class Item {
            public ItemType Type;
            public string Name;
            public int Cost;
            public int Damage;
            public int Armor;

            public Item(ItemType type, string name, int cost, int damage, int armor) {
                Type = type;
                Name = name;
                Cost = cost;
                Damage = damage;
                Armor = armor;
            }

            public override string ToString() {
                return string.Format("{0} : {1}", Type, Name);
            }
        }

        private static bool Simulate(Unit baseplayer, Unit baseboss) {
            Unit player = new Unit(baseplayer);
            Unit boss = new Unit(baseboss);

            while (true) {
                int pDamageDealt = Math.Max(player.Damage - boss.Armor, 1);
                boss.HP -= pDamageDealt;
                //Console.WriteLine("The player deals {0}-{1} = {2} damage; the boss goes down to {3} hit points.", player.Damage, boss.Armor, pDamageDealt, boss.HP);

                if (boss.HP <= 0)
                    return true;

                int bDamageDealt = Math.Max(boss.Damage - player.Armor, 1);
                player.HP -= bDamageDealt;
                //Console.WriteLine("The boss deals {0}-{1} = {2} damage; the player goes down to {3} hit points.", boss.Damage, player.Armor, bDamageDealt, player.HP);

                if (player.HP <= 0)
                    return false;
            }
        }
    }
}
