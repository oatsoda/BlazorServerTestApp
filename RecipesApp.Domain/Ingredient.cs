using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecipesApp.Domain.Bases;

namespace RecipesApp.Domain
{
    public class Ingredient : BaseUniqueEntity
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local - required by EF

        public Guid RecipeId { get; private set; }

        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; private set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public decimal Quantity { get; set; }

        [Required(AllowEmptyStrings = false)]
        [MaxLength(50)]
        public string QuantityType { get; set; }

        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local

        private Ingredient()
        {

        }

        public Ingredient(Recipe recipe)
        {
            Recipe = recipe;
        }
    }
}