using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerTestApp.Data
{
    public class RecipesService : IRecipesService
    {
        private readonly List<Recipe> m_Recipes = new List<Recipe>
        {
            new Recipe { Id = Guid.NewGuid(), Name = "Spaghetti Bolognese", Reference = "Public Domain", TotalMinutes = 40, CreatedAt = DateTimeOffset.UtcNow.AddDays(-2), UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1) },
            new Recipe { Id = Guid.NewGuid(), Name = "Pizza", Reference = "Public Domain", TotalMinutes = 60, CreatedAt = DateTimeOffset.UtcNow.AddDays(-1), UpdatedAt = DateTimeOffset.UtcNow.AddDays(-1).AddHours(4) }
        };

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            return m_Recipes;
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            recipe.CreatedAt = DateTimeOffset.UtcNow;
            m_Recipes.Add(recipe);
            return recipe;
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            var existing = m_Recipes.Single(r => r.Id == recipe.Id);
            m_Recipes.Remove(existing);

            recipe.UpdatedAt = DateTimeOffset.UtcNow;
            m_Recipes.Add(recipe);

            return recipe;
        }

        public async Task DeleteRecipe(Recipe recipe)
        {
            var existing = m_Recipes.Single(r => r.Id == recipe.Id);
            m_Recipes.Remove(existing);
        }
    }
}
