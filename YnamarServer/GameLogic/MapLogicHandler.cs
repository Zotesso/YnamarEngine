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

            foreach (var map in InMemoryDatabase.Maps.Values)
            {
                
                foreach (var npc in map.Npcs)
                    {
                        if (npc != null && npc.RespawnWait == 0)
                        {
                            
                            int i = rnd.Next(0, 5); // Generates 0 to 4 inclusive
                            int targetY = npc.Y + rnd.Next(-1, 2);
                            int targetX = npc.X + rnd.Next(-1, 2);

                            // Let's move the NPC
                            switch (i)
                            {
                                case 0:
                                    // Up
                                    if (npc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_UP);
                                            //didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (npc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_DOWN);
                                            //didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (npc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_LEFT);
                                            //didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (npc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_RIGHT);
                                            //didWalk = true;
                                        }
                                    }
                                    break;

                                case 1:
                                    // Right
                                    if (npc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_RIGHT);
                                            //didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (npc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_LEFT);
                                           // didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (npc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_DOWN);
                                            //didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (npc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_UP);
                                            //didWalk = true;
                                        }
                                    }
                                    break;

                                case 2:
                                    // Down
                                    if (npc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_DOWN);
                                            //didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (npc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_UP);
                                           // didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (npc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_RIGHT);
                                            //didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (npc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_LEFT);
                                            //didWalk = true;
                                        }
                                    }
                                    break;

                                case 3:
                                    // Left
                                    if (npc.X > targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_LEFT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_LEFT);
                                            //didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (npc.X < targetX && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_RIGHT))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_RIGHT);
                                            //didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (npc.Y > targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_UP))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_UP);
                                            //didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (npc.Y < targetY && !didWalk)
                                    {
                                        if (NpcLogicHandler.CanNpcMove(map, npc, Constants.DIR_DOWN))
                                        {
                                            NpcLogicHandler.NpcMove(map.Id, npc, Constants.DIR_DOWN);
                                           // didWalk = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                
            }
        }
    
        public static MapNpc? CheckForNpcInRange(int mapNum, int targetX, int targetY)
        {
            if (InMemoryDatabase.Maps[mapNum].Npcs.Count == 0) return null;

            return InMemoryDatabase.Maps[mapNum].Npcs
                .Select((npc, i) => new { npc, i })
                .FirstOrDefault(a => a.npc.X == targetX && a.npc.Y == targetY).npc;
        }
    }
}
