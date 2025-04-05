using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;

namespace YnamarServer.Services
{
    internal class TesteService
    {
        private readonly AppDbContext _context;

        public TesteService(AppDbContext context)
        {
            _context = context;
        }

        public void DoSomething()
        {
            Console.WriteLine("Service is running...");
            var data = _context.TesteEntities.ToList();
            Console.WriteLine($"Fetched {data.Count} records.");
        }

    }
}
