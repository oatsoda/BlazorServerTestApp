using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RecipesApp.Domain.Infrastructure.Context;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipesApp.Domain.Infrastructure.RunMigrations
{
    class Program
    {
        private const int _DATABASE_COMMAND_TIMEOUT_SECONDS = 300;

        static void Main(string[] args)
        {
            if (args == null || args.Length < 1)
            {
                Console.WriteLine("Please specify connection string");

#if DEBUG
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();
#endif
                return;
            }

            var connString = args[0];
            var environment = args[1];

            var connStringBuilder = new SqlConnectionStringBuilder(connString);

            Console.WriteLine($"Running migration on DB '{connStringBuilder.InitialCatalog}' on server '{connStringBuilder.DataSource}'");

            // If using Pooled Context and Service Locator
            // var sc = new ServiceCollection();
            // sc.AddSingleton<AuthenticationStateProvider, MockAuthStateProvider>();
            // sc.AddEntityFramework("");
            // var sp = sc.BuildServiceProvider();
            
            var builder = new DbContextOptionsBuilder<RecipesContext>()
            //  .UseInternalServiceProvider(sp)
                .UseSqlServer(connString);

            var context = new RecipesContext(builder.Options, new MockAuthStateProvider());
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(_DATABASE_COMMAND_TIMEOUT_SECONDS));
            context.Database.Migrate();

            Console.WriteLine($"Seeding data for '{environment}'...");

#if DEBUG
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
#endif
        }
    }

    public class MockAuthStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => Task.FromResult(new AuthenticationState(new ClaimsPrincipal())); // Migrations don't access the Principal, so just pass empty one.
    }
}
