using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YnamarServer.Services;


namespace YnamarServer.Database
{
    internal class Database
    {
        public void ConfigureDatabase(IConfiguration contextConfig, IServiceCollection services)
        {
            var connectionString = contextConfig.GetConnectionString("AppDb");

            // Register DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString));

            Console.WriteLine("Db configurado! = " + connectionString);

            // Register other services
            services.AddSingleton<TesteService>(); // Singleton to ensure reusability
        }
    }
}
