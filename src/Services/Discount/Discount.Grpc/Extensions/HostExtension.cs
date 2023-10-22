using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Discount.Grpc.Extensions
{
    public static class HostExtension
    {
        public static WebApplicationBuilder MigrateDatabase<Tcontext>(this WebApplicationBuilder builder,int? retry=0)
        {
            int retryForAvailability = retry.Value;
            using (var services=builder.Services.BuildServiceProvider())
            {
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger= services.GetRequiredService<ILogger<Tcontext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand { Connection=connection };
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE  TABLE Coupon (
                                                                    Id Serial Primary Key, 
                                                                    ProductName varchar(24) not null , 
                                                                    Description Text ,
                                                                    Amount Int                                                                
                                                                )";
                    command.ExecuteNonQuery();

                    command.CommandText= "Insert into Coupon (ProductName,Description,Amount ) values('IPhone X' , 'Apple Phone Brand' , 100)";
                    command.ExecuteNonQuery();

                    command.CommandText = "Insert into Coupon (ProductName,Description,Amount ) values('Samsung 10' , 'Samsung Phone Brand' , 150)";
                    command.ExecuteNonQuery();

                    logger.LogInformation("Migrated postresql database");

                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An Error Occurred while migrating the postresql database");
                    if(retryForAvailability > 0)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<Tcontext>(builder,retryForAvailability);
                    }
                    throw;
                }
                return builder;

            }
        }
    }
}
