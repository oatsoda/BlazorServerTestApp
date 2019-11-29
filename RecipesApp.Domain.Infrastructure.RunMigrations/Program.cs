using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RecipesApp.Domain.Infrastructure.Context;

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

            //      // We have to register a Service Provider to register a Fake Mediator to avoid exceptions in the Context where we use Service Locator
            //      var sc = new ServiceCollection();
            //      sc.AddSingleton<IMediator, MockMediator>();
            //      sc.AddEntityFramework("");
            //      var sp = sc.BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<RecipesContext>()
                //          .UseInternalServiceProvider(sp)
               .UseSqlServer(connString);

            var context = new RecipesContext(builder.Options);
            context.Database.SetCommandTimeout(TimeSpan.FromSeconds(_DATABASE_COMMAND_TIMEOUT_SECONDS));
            context.Database.Migrate();

            Console.WriteLine($"Seeding data for '{environment}'...");

            //TestDataSeeder.SeedData(context, environment).GetAwaiter().GetResult();

#if DEBUG
            Console.WriteLine("Press enter to exit.");
            Console.ReadLine();
#endif
        }
    }
}
