using System;
using RecipesApp.Domain;

namespace RecipesApp.App.Models
{
    public class RecipeModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public int TotalMinutes { get; set; }

        public string Reference { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public Recipe ToDomainObject()
        {
            return new Recipe(Name, TotalMinutes, Reference);
        }

        public void UpdateDomainObject(Recipe r)
        {
            r.Name = Name;
            r.TotalMinutes = TotalMinutes;
            r.Reference = Reference;
        }

        public static RecipeModel FromDomainObject(Recipe r)
        {
            return new RecipeModel
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
