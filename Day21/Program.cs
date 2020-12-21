using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
{
    class Food
    {
        public List<string> Ingredients { get; set; }
        public List<string> Allergens { get; set; }

        public static Food Parse(string x)
        {
            var regex = new Regex(@"((?<ingredient>\w+)\s)+\(contains(\s(?<allergen>\w+)[,)])+");
            var match = regex.Match(x);

            var ingredients = match.Groups["ingredient"].Captures.Cast<Capture>().Select(c => c.Value).ToList();
            var allergens = match.Groups["allergen"].Captures.Cast<Capture>().Select(c => c.Value).ToList();

            return new Food
            {
                Ingredients = ingredients,
                Allergens = allergens
            };
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var food = input.Select(Food.Parse).ToList();

            var candidates = new Dictionary<string, List<string>>();

            foreach (var item in food)
            {
                foreach (var allergen in item.Allergens)
                {
                    if (!candidates.ContainsKey(allergen))
                    {
                        candidates[allergen] = item.Ingredients;
                        continue;
                    }

                    candidates[allergen] = candidates[allergen].Intersect(item.Ingredients).ToList();
                }
            }

            var allergenToIngredient = new Dictionary<string, string>();

            while (candidates.Count > 0)
            {
                var item = candidates.First(kv => kv.Value.Count == 1);

                var allergen = item.Key;
                var ingredient = item.Value[0];

                allergenToIngredient[allergen] = ingredient; 

                candidates.Remove(allergen);

                foreach (var candidate in candidates)
                {
                    candidate.Value.Remove(ingredient);
                }
            }

            var ingredientsWithAllergen = allergenToIngredient.Values.ToHashSet();

            var answer1 = food.SelectMany(f => f.Ingredients).Where(i => !ingredientsWithAllergen.Contains(i)).Count();

            Console.WriteLine($"Answer 1: {answer1}");


            var answer2 = string.Join(",", allergenToIngredient.OrderBy(kv => kv.Key).Select(kv => kv.Value).ToArray());

            Console.WriteLine($"Answer 2: {answer2}");
        }
    }
}
