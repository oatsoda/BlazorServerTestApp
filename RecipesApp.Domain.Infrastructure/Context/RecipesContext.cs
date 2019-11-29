using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RecipesApp.Domain.Bases;

namespace RecipesApp.Domain.Infrastructure.Context
{
    public class RecipesContext : DbContext
    {
        public DbSet<Recipe> Recipes { get; set; }
        
        #region Ctors

        // ReSharper disable once SuggestBaseTypeForParameter - recommended that EF Context fixed to specific context type.
        public RecipesContext(DbContextOptions<RecipesContext> options) : base(options)
        {
            // Ctor required for Migrations
        }

        protected RecipesContext(DbContextOptions options) : base(options)
        {
            // Ctor required for Unit Tests
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

            /* INDEXES: Not supported with Data Annotations in EF.
             * https://docs.microsoft.com/en-us/ef/core/modeling/relational/indexes
             */
            modelBuilder.Entity<Recipe>()
                .HasIndex(a => a.Name)
                .IsUnique();

            /* DELETE BEHAVIOUR: Not supported with Data Annotations in EF.
             * https://docs.microsoft.com/en-us/ef/core/modeling/relationships
             */

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        #endregion

        #region SaveChanges Overrides

        // public override int SaveChanges() - No override as base calls: int SaveChanges(bool acceptAllChangesOnSuccess)

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            SetTimestamps();
            var result = base.SaveChanges(acceptAllChangesOnSuccess);

            return result;
        }

        // public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) - No override as base calls: Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            SetTimestamps();
            var result = base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

            return result;
        }

        #endregion

        protected virtual void SetTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                    ((BaseEntity)entity.Entity).CreatedAt = DateTimeOffset.UtcNow; ;

                ((BaseEntity)entity.Entity).UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
