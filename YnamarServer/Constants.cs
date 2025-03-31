using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer
{
    internal class Constants
    {
        //Player Constants
        public const int MAX_PLAYERS = 100;


        public const byte DIR_UP = 0;
        public const byte DIR_DOWN = 1;
        public const byte DIR_LEFT = 2;
        public const byte DIR_RIGHT = 3;

        //Map Constants
        public const int MAX_MAPS = 100;
        public const int MAX_MAP_X = 50;
        public const int MAX_MAP_Y = 50;

        public const int MAX_MAP_NPCS = 15;

        //NPC Constants
        public const int MAX_NPCS = 100;
    }
}
