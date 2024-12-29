using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AoC.Day
{
    public class Day15
    {
        public static void Run(string file) {
            Console.WriteLine("Day 15: Science for Hungry People" + Environment.NewLine);

            List<Ingredient> list = new List<Ingredient>();
            string[] lines = File.ReadAllLines(file);
            foreach(string line in lines) {
                Ingredient i = new Ingredient(line);
                list.Add(i);
            }

            int bestScore = 0;
            int okayScore = 0;
            int okayCalorieTarget = 500;
            Cookie bestCookie = new Cookie();

            int reqTotal = 100;
            int upper = reqTotal * list.Count;

            List<int[]> combinations = new List<int[]>();

            Console.WriteLine("Generating combinations...");

            int[] array = new int[list.Count];
            Recursive(ref list, ref combinations, array, 0, reqTotal, upper);

            Console.WriteLine("Rating cookies...");

            foreach (int[] combo in combinations) {
                Cookie cookie = new Cookie(list, combo);
                if (cookie.Score > bestScore) {
                    bestCookie = cookie;
                    bestScore = cookie.Score;
                }

                if(cookie.Calories == okayCalorieTarget) {
                    if (cookie.Score > okayScore)
                        okayScore = cookie.Score;
                }
            }

            Console.WriteLine();

            Console.WriteLine("Part 1: {0} with {1} calories!", bestScore, bestCookie.Calories);
            //Answer: 13882464
            Console.WriteLine("Part 2: {0} with {1} calories!", okayScore, okayCalorieTarget);
            //Answer: 11171160
        }
        private static void Recursive(ref List<Ingredient> list, ref List<int[]> combination, int[] array, int index, int reqTotal, int upper) {
            if (index >= list.Count)
                return;

            for (int y = 0; y < upper; y++) {
                array[index] = y;                

                if (array.Sum() == reqTotal) {
                    int[] clone = (int[])array.Clone();
                    combination.Add(clone);
                    //Console.WriteLine("{0}", string.Join(", ", array));
                } else {
                    Recursive(ref list, ref combination, array, index + 1, reqTotal, upper - y);
                }
            }
        }

        private class Ingredient {
            public string Name;
            public int Capacity;
            public int Durability;
            public int Flavor;
            public int Texture;
            public int Calories;

            public Ingredient(string input) {
                string[] fields = input.Replace(",", "").Split(' ');
                Name = fields[0].Substring(0, fields[0].Length - 1);
                Capacity = int.Parse(fields[2]);
                Durability = int.Parse(fields[4]);
                Flavor = int.Parse(fields[6]);
                Texture = int.Parse(fields[8]);
                Calories = int.Parse(fields[10]);
            }
        }

        private class Cookie {
            public int Capacity;
            public int Durability;
            public int Flavor;
            public int Texture;
            public int Calories;

            public int Score;

            public Cookie() { }

            public Cookie(List<Ingredient> ingredients, int[] amounts) {
                if (amounts.Sum() != 100)
                    throw new Exception("InvalidCookieException");

                for(int i = 0; i < amounts.Length; i++) {
                    Capacity += amounts[i] * ingredients[i].Capacity;
                    Durability += amounts[i] * ingredients[i].Durability;
                    Flavor += amounts[i] * ingredients[i].Flavor;
                    Texture += amounts[i] * ingredients[i].Texture;
                    Calories += amounts[i] * ingredients[i].Calories;
                }

                if (Capacity < 0) Capacity = 0;
                if (Durability < 0) Durability = 0;
                if (Flavor < 0) Flavor = 0;
                if (Texture < 0) Texture = 0;

                Score = Capacity * Durability * Flavor * Texture;
            }
        }
    }
}
