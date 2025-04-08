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

            Console.WriteLine("Db setted up");

            // Register other services
            services.AddSingleton<AccountService>(); // Singleton to ensure reusability
        }
    }
}
