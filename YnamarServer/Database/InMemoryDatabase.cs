using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;

namespace YnamarServer.Database
{
    internal class InMemoryDatabase
    {
        public static Character[] Player = new Character[Constants.MAX_PLAYERS];

    }
}
