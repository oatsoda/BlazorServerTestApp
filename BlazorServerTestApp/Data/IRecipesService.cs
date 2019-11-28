using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorServerTestApp.Data
{
    public interface IRecipesService
    {
        Task<IEnumerable<Recipe>> GetRecipes();
        Task<Recipe> GetRecipe(Guid id);
        Task<Recipe> AddRecipe(Recipe recipe);
        Task<Recipe> UpdateRecipe(Recipe recipe);
        Task DeleteRecipe(Recipe recipe);
    }
}