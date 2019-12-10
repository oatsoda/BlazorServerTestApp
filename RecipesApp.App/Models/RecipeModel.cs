using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using RecipesApp.Domain;

namespace RecipesApp.App.Models
{
    public class RecipeModel
    {
        public Guid Id { get; private set; }

        public string Name { get; set; }
        
        public int TotalMinutes { get; set; }

        public string Reference { get; set; }

        public List<IngredientModel> Ingredients { get; } = new List<IngredientModel>();

        public DateTimeOffset CreatedAt { get; private set; }

        public DateTimeOffset UpdatedAt { get; private set; }

        public RecipeModel()
        {

        }

        public RecipeModel(IEnumerable<IngredientModel> ingredients) : this()
        {
            Ingredients = new List<IngredientModel>(ingredients); // TODO: optimise
        }

        public Recipe ToDomainObject()
        {
            var recipe = new Recipe(Name, TotalMinutes, Reference);
            recipe.AddIngredients(Ingredients.Select(i => i.ToDomainObject(recipe)));
            return recipe;
        }

        public void UpdateDomainObject(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TotalMinutes = TotalMinutes;
            recipe.Reference = Reference;

            foreach (var ingredientModel in Ingredients)
            {
                var ingredient = recipe.Ingredients.SingleOrDefault(i => i.Id == ingredientModel.Id && i.Id != Guid.Empty);
                if (ingredient != null)
                    ingredientModel.UpdateDomainObject(ingredient);
                else
                    recipe.Ingredients.Add(ingredientModel.ToDomainObject(recipe));
            }


            // TODO: Remove if deleted, but not NEW entries
            //var toDelete = recipe.Ingredients
            //                     .Where(i => Ingredients.All(im => im.Id != i.Id))
            //                     .Select(i => i.Id)
            //                     .ToList();

            //recipe.Ingredients.RemoveAll(i => toDelete.Contains(i.Id));
        }

        public static RecipeModel FromDomainObject(Recipe r)
        {
            return new RecipeModel(r.Ingredients.Select(IngredientModel.FromDomainObject))
                   {
                       Id = r.Id,
                       Name = r.Name,
                       TotalMinutes = r.TotalMinutes,
                       Reference = r.Reference,
                       CreatedAt = r.CreatedAt,
                       UpdatedAt = r.UpdatedAt
                   };
        }
    }
}
