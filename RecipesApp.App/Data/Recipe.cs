using System;

namespace RecipesApp.App.Data
{
    public class Recipe
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public int TotalMinutes { get; set; }

        public string Reference { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public Recipe()
        {
            Id = Guid.NewGuid();
        }
    }
}