using System.Collections.Generic;
using RecipesApp.Domain.Bases;
using System.ComponentModel.DataAnnotations;

namespace RecipesApp.Domain
{
    public class Recipe : BaseUniqueEntity
    {
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local - required by EF
        // ReSharper disable UnusedAutoPropertyAccessor.Local - required by EF

        [Required(AllowEmptyStrings = false)]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int TotalMinutes { get; set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string Reference { get; set; }

        /* Navigation Properties */

        public List<Ingredient> Ingredients { get; private set; }

        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        private Recipe()
        {
            Ingredients = new List<Ingredient>();
        }

        public Recipe(string name, int totalMinutes, string reference) : this()
        {
            Name = name;
            TotalMinutes = totalMinutes;
            Reference = reference;
        }
    }
}
