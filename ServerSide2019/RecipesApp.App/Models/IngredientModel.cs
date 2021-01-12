using System;
using RecipesApp.Domain;

namespace RecipesApp.App.Models
{
    public class IngredientModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Quantity { get; set; }

        public string QuantityType { get; set; }

        public Ingredient ToDomainObject(Recipe parentRecipe)
        {
            return new Ingredient(parentRecipe)
                   {
                       Name = Name,
                       Quantity = Quantity,
                       QuantityType = QuantityType
                   };
        }

        public static IngredientModel FromDomainObject(Ingredient ingredient)
        {
            return new IngredientModel
                   {
                       Id = ingredient.Id,
                       Name = ingredient.Name,
                       Quantity = ingredient.Quantity,
                       QuantityType = ingredient.QuantityType
                   };
        }

        public void UpdateDomainObject(Ingredient ingredient)
        {
            ingredient.Name = Name;
            ingredient.Quantity = Quantity;
            ingredient.QuantityType = QuantityType;
        }
    }
}