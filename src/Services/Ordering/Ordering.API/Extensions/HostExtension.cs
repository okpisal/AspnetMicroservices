using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Ordering.API.Extensions
{
    public static class HostExtension
    {
        public static WebApplicationBuilder MigrationDatabase<TContext>(this WebApplicationBuilder builder, Action<TContext, IServiceProvider> seeder, int? retry = 0) where TContext : DbContext
        {
            int retryForAvilability = retry.Value;
            using (var services = builder.Services.BuildServiceProvider())
            {
                //var services = scope.ServiceProvider;
                var logger=services.GetRequiredService<ILogger<TContext>>();
                var context=services.GetService<TContext>();
                try
                {
                    logger.LogInformation("Migrating database associated with context {DbContextName}",typeof(TContext));

                    InvokeSeeder(seeder,context,services);

                    logger.LogInformation("Migrated database associated with context {DbContextName}",typeof (TContext));
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "An Error occured while migrating the database used on context");
                    if (retryForAvilability < 50)
                    {
                        retryForAvilability ++;
                        System.Threading.Thread.Sleep(2000);
                        MigrationDatabase<TContext>(builder, seeder, retryForAvilability);
                    }
                    
                }
            }
            return builder;
        }

        private static void InvokeSeeder<TContext>(Action<TContext, IServiceProvider> seeder, TContext? context, IServiceProvider services) where TContext : DbContext
        {
            context.Database.Migrate();
            seeder(context, services);
        }
    }
}
