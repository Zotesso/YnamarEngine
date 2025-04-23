using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database;
using YnamarServer.Database.Models;

namespace YnamarServer.GameLogic
{
    internal class MapLogicHandler
    {
        public static void UpdateAllMaps()
        {
            Random rnd = new Random();
            bool didWalk = false;
            int targetY = rnd.Next(0, 20); ;
            int targetX = rnd.Next(0, 20); ;

            foreach (var (map, mapIndex) in InMemoryDatabase.Maps.Select((value, i) => (value, i)))
            {
                foreach (var (layer, layerIndex) in map.Layer.Select((value, i) => (value, i)))
                {
                    foreach (MapNpc mapNpc in layer.MapNpc)
                    {
                        if (mapNpc != null)
                        {
                            
                            int i = rnd.Next(0, 5); // Generates 0 to 4 inclusive

                            // Let's move the NPC
                            switch (i)
                            {
                                case 0:
                                    // Up
                                    if (mapNpc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_UP);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (mapNpc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_DOWN);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (mapNpc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_LEFT);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (mapNpc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_RIGHT);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 1:
                                    // Right
                                    if (mapNpc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_RIGHT);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (mapNpc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_LEFT);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (mapNpc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_DOWN);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (mapNpc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_UP);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 2:
                                    // Down
                                    if (mapNpc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_DOWN);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (mapNpc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_UP);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (mapNpc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_RIGHT);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (mapNpc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_LEFT);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 3:
                                    // Left
                                    if (mapNpc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_LEFT);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (mapNpc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_RIGHT);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (mapNpc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_UP);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (mapNpc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, mapNpc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(mapIndex, layerIndex, mapNpc, Constants.DIR_DOWN);
                                            didWalk = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}
