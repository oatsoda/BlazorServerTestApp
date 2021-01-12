using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipesApp.App.Models;
using RecipesApp.Domain.Infrastructure.Context;

namespace RecipesApp.App.Data
{
    /*
     * Not really the ideal use of Entity Framework - need to refactor away from this Service.  Perhaps dispatchers/handlers/mediators instead.
     */

    public class RecipesService : IRecipesService
    {
        private readonly RecipesContext m_Context;

        public RecipesService(RecipesContext context)
        {
            m_Context = context;
        }

        public async Task<IEnumerable<RecipeModel>> GetRecipes()
        {
            var recipes = await m_Context.Recipes.ToListAsync();

            return recipes.Select(RecipeModel.FromDomainObject);
        }

        public async Task<RecipeModel> GetRecipe(Guid id)
        {
            var recipe = await m_Context.Recipes
                                        .Include(r => r.Ingredients)
                                        .FirstOrDefaultAsync(r => r.Id == id);
            // TODO: Null check
            return RecipeModel.FromDomainObject(recipe);
        }

        public async Task<RecipeModel> AddRecipe(RecipeModel recipeModel)
        {
            var r = recipeModel.ToDomainObject();
            m_Context.Recipes.Add(r);
            await m_Context.SaveChangesAsync();
            return RecipeModel.FromDomainObject(r);
        }

        public async Task<RecipeModel> UpdateRecipe(RecipeModel recipeModel)
        {
            var recipe = await m_Context.Recipes
                                        .Include(r => r.Ingredients)
                                        .FirstOrDefaultAsync(r => r.Id == recipeModel.Id);
            // TODO: Null check
            recipeModel.UpdateDomainObject(recipe);
            await m_Context.SaveChangesAsync();
            return RecipeModel.FromDomainObject(recipe);
        }

        public async Task DeleteRecipe(RecipeModel recipeModel)
        {
            var recipe = await m_Context.Recipes.FindAsync(recipeModel.Id);
            // TODO: Null check
            m_Context.Recipes.Remove(recipe);
            await m_Context.SaveChangesAsync();
        }
    }
}
