namespace AoC.Day
{
    public class Day21
    {
        public static void Run(string file) {
            Console.WriteLine("Day 21: Allergen Assessment" + Environment.NewLine);

            List<Food> foods = new List<Food>();
            Dictionary<string, List<Food>> dIngredientToFood = new Dictionary<string, List<Food>>();
            Dictionary<string, List<Food>> dAllergenToFood = new Dictionary<string, List<Food>>();

            string[] lines = File.ReadAllLines(file);
            foreach (string line in lines) {
                string[] half = line.Substring(0, line.Length - 1).Replace(",", "").Split(" (contains ");
                string[] ingredients = half[0].Split(' ');
                string[] allergens = half[1].Split(' ');
                Food food = new Food(ingredients, allergens);
                foods.Add(food);

                foreach (string i in ingredients) {
                    if (!dIngredientToFood.ContainsKey(i))
                        dIngredientToFood.Add(i, new List<Food>());
                    dIngredientToFood[i].Add(food);
                }

                foreach (string a in allergens) {
                    if (!dAllergenToFood.ContainsKey(a))
                        dAllergenToFood.Add(a, new List<Food>());
                    dAllergenToFood[a].Add(food);
                }
            }

            Dictionary<string, string> solvedAllergenToIngredient = new Dictionary<string, string>();
            bool showUnsolvedText = false;
            int unsolved = dAllergenToFood.Count();
            while (unsolved > 0) {
                foreach (KeyValuePair<string, List<Food>> pair in dAllergenToFood) {
                    string allergen = pair.Key;
                    if (solvedAllergenToIngredient.ContainsKey(allergen))
                        continue;

                    IEnumerable<string> common = pair.Value[0].Ingredients.AsEnumerable();
                    for (int i = 1; i < pair.Value.Count; i++)
                        common = common.Intersect(pair.Value[i].Ingredients);
                    common = common.Except(solvedAllergenToIngredient.Values);

                    if (common.Count() == 1) {
                        string ingredient = common.First();
                        solvedAllergenToIngredient.Add(allergen, ingredient);
                        Console.WriteLine("{0} contains {1}", ingredient, allergen);
                        unsolved--;
                    } else if(showUnsolvedText) {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("unsolved: " + allergen);
                        Console.ResetColor();
                    }
                }
            }

            foreach (KeyValuePair<string, string> pair in solvedAllergenToIngredient)
                dIngredientToFood.Remove(pair.Value);
            int partA = dIngredientToFood.Sum(d => d.Value.Count);

            solvedAllergenToIngredient = solvedAllergenToIngredient.OrderBy(d => d.Key).ToDictionary();
            string partB = string.Join(',', solvedAllergenToIngredient.Values);

            Console.WriteLine();
            Console.WriteLine("Part 1: " + partA);
            //Answer: 1945
            Console.WriteLine("Part 2: " + partB);
            //Answer: pgnpx,srmsh,ksdgk,dskjpq,nvbrx,khqsk,zbkbgp,xzb
        }

        private class Food {
            public List<string> Ingredients;
            public List<string> Allergens;

            public Food(string[] ingredients, string[] allergens) {
                Ingredients = new List<string>(ingredients);
                Allergens = new List<string>(allergens);
            }
        }
    }
}
