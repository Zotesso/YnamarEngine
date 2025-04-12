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
                accountToSave.PasswordHash = hashedPassword;
                // Agora você pode usar dbContext normalmente
                Console.WriteLine("Service is running...");
                await dbContext.Accounts.AddAsync(accountToSave);
                var data = await dbContext.SaveChangesAsync();

                await CreateCharacter(username, dbContext, accountToSave);

                Console.WriteLine($" {data}.");
            }; // O escopo será descartado aqui automaticamente
        }

        public async Task CreateCharacter(string username, AppDbContext dbContext, Account acc)
        {
            Character newCharacter = new Character();
            newCharacter.Account = acc;
            newCharacter.YOffset = 0;
            newCharacter.XOffset = 0;
            newCharacter.Dir = 0;
            newCharacter.Name = username;
            newCharacter.EXP = 0;
            newCharacter.Access = 0;
            newCharacter.Map = 0;
            newCharacter.Sprite = 0;
            newCharacter.X = 0;
            newCharacter.Y = 0;
            newCharacter.Level = 0;

            await dbContext.Characters.AddAsync(newCharacter);
            await dbContext.SaveChangesAsync();
        }

        public async Task<int> Login(string username, string password)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                Account? userAccount = await dbContext.Accounts.FirstOrDefaultAsync(acc => acc.Username == username);
            
                if (userAccount == null)
                {
                    throw new ArgumentOutOfRangeException("Account or Password Invalid.");

                }
                else
                {
                    bool isPasswordCorret = Verify(password, userAccount.PasswordHash);
                    return isPasswordCorret 
                        ? userAccount.Id 
                        : throw new ArgumentOutOfRangeException("Account or Password Invalid.");
                    ;
                }
            };
        }

        public async Task<Character> GetCharacterAsync(int id)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                return await dbContext.Characters.FindAsync(id);
            };
        }
    }
}
