using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecipesApp.App.Models;

namespace RecipesApp.App.Data
{
    public interface IRecipesService
    {
        Task<IEnumerable<RecipeModel>> GetRecipes();
        Task<RecipeModel> GetRecipe(Guid id);
        Task<RecipeModel> AddRecipe(RecipeModel recipeModel);
        Task<RecipeModel> UpdateRecipe(RecipeModel recipeModel);
        Task DeleteRecipe(RecipeModel recipeModel);
    }
}