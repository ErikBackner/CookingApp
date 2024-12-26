using System;
using CookingApp.Models;
using CookingApp.Services;

namespace CookingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var recipeService = new RecipeService();

            while (true)
            {
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("1. Lägg till recept");
                Console.WriteLine("2. Visa alla recept");
                Console.WriteLine("3. Betygsätt ett recept");
                Console.WriteLine("4. Kommentera ett recept");
                Console.WriteLine("5. Uppdatera recept");
                Console.WriteLine("6. Ta bort recept");
                Console.WriteLine("7. Avsluta");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRecipe(recipeService);
                        break;
                    case "2":
                        ShowRecipes(recipeService);
                        break;
                    case "3":
                        RateRecipe(recipeService);
                        break;
                    case "4":
                        AddComment(recipeService);
                        break;
                    case "5":
                        UpdateRecipe(recipeService);
                        break;
                    case "6":
                        DeleteRecipe(recipeService);
                        break;
                    case "7":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt alternativ, försök igen.");
                        break;
                }
            }
        }

        static Recipe SelectRecipeByIndex(RecipeService recipeService)
        {
            var recipes = recipeService.GetAllRecipes();
            if (recipes.Count == 0)
            {
                Console.WriteLine("Inga recept finns i databasen.");
                return null;
            }

            Console.WriteLine("Välj ett recept:");
            for (int i = 0; i < recipes.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {recipes[i].Name}");
            }

            Console.Write("Ange nummer för receptet: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= recipes.Count)
            {
                return recipes[index - 1];
            }
            else
            {
                Console.WriteLine("Ogiltigt val. Försök igen.");
                return null;
            }
        }

        static void AddRecipe(RecipeService recipeService)
        {
            Console.WriteLine("Ange receptets namn:");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Namn får inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange beskrivning:");
            string description = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("Beskrivning får inte vara tomt.");
                return;
            }

            var recipe = new Recipe
            {
                Name = name,
                Description = description
            };

            recipeService.AddRecipe(recipe);
            Console.WriteLine($"Receptet har lagts till med ID: {recipe.CustomId}");
        }

        static void ShowRecipes(RecipeService recipeService)
        {
            var recipes = recipeService.GetAllRecipes();
            if (recipes.Count == 0)
            {
                Console.WriteLine("Inga recept finns i databasen.");
                return;
            }
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Recept ID: {recipe.CustomId}, Namn: {recipe.Name}, Beskrivning: {recipe.Description}");
            }
        }

        static void RateRecipe(RecipeService recipeService)
        {
            var recipe = SelectRecipeByIndex(recipeService);
            if (recipe == null)
            {
                return;
            }

            Console.WriteLine("Ange betyg (1-5): ");
            if (int.TryParse(Console.ReadLine(), out int rating) && rating >= 1 && rating <= 5)
            {
                recipeService.AddRatingToRecipe(recipe.CustomId, rating);
                Console.WriteLine("Betyg har lagts till.");
            }
            else
            {
                Console.WriteLine("Ogiltigt betyg. Betyget måste vara mellan 1 och 5.");
            }
        }

        static void AddComment(RecipeService recipeService)
        {
            var recipe = SelectRecipeByIndex(recipeService);
            if (recipe == null)
            {
                return;
            }

            Console.WriteLine("Skriv din kommentar:");
            string comment = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(comment))
            {
                Console.WriteLine("Kommentar får inte vara tom.");
                return;
            }

            recipeService.AddCommentToRecipe(recipe.CustomId, comment);
            Console.WriteLine("Kommentar har lagts till.");
        }

        static void UpdateRecipe(RecipeService recipeService)
        {
            var recipe = SelectRecipeByIndex(recipeService);
            if (recipe == null)
            {
                return;
            }

            Console.WriteLine("Ange nytt namn:");
            string newName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newName))
            {
                Console.WriteLine("Namn får inte vara tomt.");
                return;
            }

            Console.WriteLine("Ange ny beskrivning:");
            string newDescription = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                Console.WriteLine("Beskrivning får inte vara tomt.");
                return;
            }

            recipe.Name = newName;
            recipe.Description = newDescription;

            recipeService.UpdateRecipe(recipe.CustomId, recipe);
            Console.WriteLine("Receptet har uppdaterats.");
        }

        static void DeleteRecipe(RecipeService recipeService)
        {
            var recipe = SelectRecipeByIndex(recipeService);
            if (recipe == null)
            {
                return;
            }

            recipeService.DeleteRecipe(recipe.CustomId);
        }
    }
}
