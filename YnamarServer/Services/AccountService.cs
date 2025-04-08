using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using static BCrypt.Net.BCrypt;

namespace YnamarServer.Services
{
    internal class AccountService
    {
        private const int WorkFactor = 14;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AccountService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task RegisterUserAsync(string username, string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                string hashedPassword = HashPassword(password, WorkFactor);
                Account accountToSave = new Account();
                accountToSave.Username = username;
                accountToSave.CharGender = false;
                accountToSave.CharId = 0;
                accountToSave.PasswordHash = hashedPassword;
                // Agora você pode usar dbContext normalmente
                Console.WriteLine("Service is running...");
                await dbContext.Accounts.AddAsync(accountToSave);
                var data = await dbContext.SaveChangesAsync();

                Console.WriteLine($" {data}.");
            }; // O escopo será descartado aqui automaticamente
        }

        public async Task<bool> Login(string username, string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                Account? userAccount = await dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Username == username);
            
                if (userAccount == null)
                {
                    return false;
                } else
                {
                    bool isPasswordCorret = Verify(password, userAccount.PasswordHash);
                    return isPasswordCorret;
                }

                // Agora você pode usar dbContext normalmente

            }; // O escopo será descartado aqui automaticamente
        }
    }
}
