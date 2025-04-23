using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;
using YnamarServer.Services;
using static System.Net.Mime.MediaTypeNames;

namespace YnamarServer.GameLogic
{
    internal class NpcLogicHandler
    {
        public static bool CanNpcMove(Map map, MapNpc mapNpc, byte direction)
        {
            if(map.Id < 0 || map.Id > Constants.MAX_MAPS || mapNpc.Id < 0 || mapNpc.Id > Constants.MAX_MAP_NPCS || direction < Constants.DIR_UP || direction > Constants.DIR_RIGHT)
               return false;

            int x = mapNpc.X;
            int y = mapNpc.Y;
            bool canNpcMove = true;

            switch (direction)
            {
                case Constants.DIR_UP:

                if (y > 0)
                {
                   canNpcMove = true;
                } else
                {
                   canNpcMove = false;
                }
                    break;
                // to do - Check to make sure that there is not a player in the way


                // to do - Check to make sure that there is not another npc in the way



                // to do - check if Directional blocking

                case Constants.DIR_DOWN:
                if (y < map.MaxMapY)
                    {
                        canNpcMove = true;
                    }
                    else
                    {
                        canNpcMove = false;
                    }
                    // to do - Check to make sure that there is not a player in the way


                    // to do - Check to make sure that there is not another npc in the way
                    break;



                // to do - check if Directional blocking

                case Constants.DIR_LEFT:

                    if (x > 0)
                    {
                        canNpcMove = true;
                    }
                    else
                    {
                        canNpcMove = false;
                    }
                    // to do - Check to make sure that there is not a player in the way


                    // to do - Check to make sure that there is not another npc in the way


                    break;

                // to do - check if Directional blocking

                case Constants.DIR_RIGHT:

                    if (x < map.MaxMapX)
                    {
                        canNpcMove = true;
                    }
                    else
                    {
                        canNpcMove = false;
                    }
                    // to do - Check to make sure that there is not a player in the way


                    // to do - Check to make sure that there is not another npc in the way


                    break;

                    // to do - check if Directional blocking

            }

            return canNpcMove;
        }

        public static void NpcMove(int mapNum, int layerNum, MapNpc mapNpc, byte direction)
        {
            mapNpc.Dir = direction;

            switch (direction)
            {
                case Constants.DIR_UP:
                    mapNpc.Y -= 1;
                    break;
                case Constants.DIR_DOWN:
                    mapNpc.Y += 1;
                    break;
                case Constants.DIR_LEFT:
                    mapNpc.X -= 1;
                    break;

                case Constants.DIR_RIGHT:
                    mapNpc.X += 1;
                    break;
            }

            MapService mapService = Program.mapService;
            mapService.SendMapNpcToMap(mapNum, layerNum, mapNpc);
        }
    }
}
