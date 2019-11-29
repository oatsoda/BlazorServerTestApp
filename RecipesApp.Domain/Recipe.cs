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
        public string Name { get; private set; }

        [Required]
        public int TotalMinutes { get; private set; }

        [Required(AllowEmptyStrings = true)]
        [MaxLength(50)]
        public string Reference { get; private set; }

        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Local
        // ReSharper restore UnusedAutoPropertyAccessor.Local

        public Recipe(string name, int totalMinutes, string reference)
        {
            Name = name;
            TotalMinutes = totalMinutes;
            Reference = reference;
        }
    }
}
