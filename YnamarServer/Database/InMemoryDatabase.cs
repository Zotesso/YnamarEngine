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
        public static Map[] Maps = new Map[Constants.MAX_MAPS];
        public static Npc[] Npcs = new Npc[Constants.MAX_NPCS];
    }
}
