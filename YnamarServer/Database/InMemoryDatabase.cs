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
        public static Dictionary<int, MapRuntime> Maps = new();
        public static Npc[] Npcs = new Npc[Constants.MAX_NPCS];
        public static Item[] Items = new Item[Constants.MAX_ITEMS];
    }
}
