using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipesApp.Domain.Infrastructure.Context;
using System;

namespace RecipesApp.Domain.Infrastructure.Startup
{
    public static class ServiceCollectionExtensionsForEntityFramework
    {
        public static IServiceCollection AddEntityFramework(this IServiceCollection services, string connectionString)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            /*
             * Use this if the Context requires a dependency injected
             */

            // services.AddEntityFrameworkSqlServer(); // Must register this manually first due to UseInternalServiceProvider call below.
            // 
            // services.AddDbContextPool<MessageGatewayDbContext>((serviceProvider, options) =>
            // {
            //     options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
            //     options.UseInternalServiceProvider(serviceProvider); // This is required for internal Service Locator (Pooled Context must not have ctor injection)
            // });

            /*
             * Use this is the Context doesn't have dependencies.
             */

            // services.AddDbContextPool<RecipesContext>((serviceProvider, options) =>
            //                                            {
            //                                                options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
            //                                            });

            /*
             * Use this if you don't care about Pooling the Context objects
             */

            //services.AddDbContext<RecipesContext>((serviceProvider, options) =>
            //                                       {
            //                                           options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
            //                                       });


             services.AddDbContext<RecipesContext>((serviceProvider, options) =>
             {
                 options.UseCosmos(
                                   "https://localhost:8081",
                                   "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==",
                                   databaseName: "RecipesDb");
             });


             services.AddDefaultIdentity<IdentityUser>()
                     .AddEntityFrameworkStores<RecipesContext>();


             services.BuildServiceProvider().GetService<RecipesContext>().Database.EnsureCreatedAsync().GetAwaiter().GetResult();

            return services;
        }
    }
}
