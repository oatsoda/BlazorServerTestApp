using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipesApp.Domain.Bases;

namespace RecipesApp.Domain.Infrastructure.Context
{
    public class RecipesContext : IdentityDbContext
    {
        private readonly AuthenticationStateProvider m_AuthenticationStateProvider;

        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredient { get; set; }
        
        #region Ctors

        // ReSharper disable once SuggestBaseTypeForParameter - recommended that EF Context fixed to specific context type.
        public RecipesContext(DbContextOptions<RecipesContext> options, AuthenticationStateProvider authenticationStateProvider) : base(options)
        {
            // Ctor required for Migrations
            m_AuthenticationStateProvider = authenticationStateProvider;
        }

        protected RecipesContext(DbContextOptions options, AuthenticationStateProvider authenticationStateProvider) : base(options)
        {
            // Ctor required for Unit Tests
            m_AuthenticationStateProvider = authenticationStateProvider;
        }

        #endregion


        #region OnModelCreating Override

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*
             * IMPORTANT: Use Data Annotations wherever possible where defining business logic.
             * Any cases that are not possible with Data Annotations, or which are "data store specific" should go
             * here in a section which clearly defines why or what is not supported yet.
             */

            /* DELETE BEHAVIOUR: Not supported with Data Annotations in EF.
             * https://docs.microsoft.com/en-us/ef/core/modeling/relationships
             */

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            /* INDEXES: Not supported with Data Annotations in EF.
             * https://docs.microsoft.com/en-us/ef/core/modeling/relational/indexes
             */
            modelBuilder.Entity<Recipe>()
                        .HasIndex(a => a.Name)
                        .IsUnique();

            /* DATA TYPES: Mappings from .NET to Data Store specific
             * https://docs.microsoft.com/en-us/ef/core/modeling/relational/data-types
             */
            modelBuilder.Entity<Ingredient>()
                        .Property(nameof(Domain.Ingredient.Quantity))
                        .HasColumnType("decimal(10, 2)");

            modelBuilder.Entity<Recipe>()
                        .HasMany(r => r.Ingredients)
                        .WithOne(i => i.Recipe)
                        .OnDelete(DeleteBehavior.Cascade);


        }

        #endregion

        #region SaveChanges Overrides

        // public override int SaveChanges() - No override as base calls: int SaveChanges(bool acceptAllChangesOnSuccess)

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetCreatedUpdated().GetAwaiter().GetResult();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            return result;
        }

        // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) - No override as base calls: Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            await SetCreatedUpdated();
            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        #endregion

        protected virtual async Task  SetCreatedUpdated()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            var authState = await m_AuthenticationStateProvider.GetAuthenticationStateAsync();

            foreach (var entity in entities)
            {
                var baseEntity = (BaseEntity)entity.Entity;

                baseEntity.UpdatedAt = DateTimeOffset.UtcNow;
                baseEntity.UpdatedBy = authState.User.UserId();

                if (entity.State != EntityState.Added)
                    continue;

                baseEntity.CreatedAt = baseEntity.UpdatedAt;
                baseEntity.CreatedBy = baseEntity.UpdatedBy;
            }
        }
    }


    public static class ClaimsPrincipalExtensions
    {
        public static string UserId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Identities.Single().Claims.Single(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
        }
    }
}
