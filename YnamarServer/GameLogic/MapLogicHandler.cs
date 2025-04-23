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
            bool didWalk = false;

            foreach (Map map in InMemoryDatabase.Maps)
            {
                foreach (MapLayer layer in map.Layer)
                {
                    foreach (MapNpc mapNpc in layer.MapNpc)
                    {
                        if (mapNpc != null)
                        {
                            Random rnd = new Random();
                            int i = rnd.Next(0, 5); // Generates 0 to 4 inclusive

                            // Let's move the NPC
                            switch (i)
                            {
                                case 0:
                                    // Up
                                    if (mapNpc.Y > targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Up))
                                        {
                                            NpcMove(mapNum, x, Direction.Up, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (MapNpc[mapNum].Npc[x].Y < targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Down))
                                        {
                                            NpcMove(mapNum, x, Direction.Down, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (MapNpc[mapNum].Npc[x].X > targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Left))
                                        {
                                            NpcMove(mapNum, x, Direction.Left, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (MapNpc[mapNum].Npc[x].X < targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Right))
                                        {
                                            NpcMove(mapNum, x, Direction.Right, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 1:
                                    // Right
                                    if (MapNpc[mapNum].Npc[x].X < targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Right))
                                        {
                                            NpcMove(mapNum, x, Direction.Right, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (MapNpc[mapNum].Npc[x].X > targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Left))
                                        {
                                            NpcMove(mapNum, x, Direction.Left, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (MapNpc[mapNum].Npc[x].Y < targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Down))
                                        {
                                            NpcMove(mapNum, x, Direction.Down, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (MapNpc[mapNum].Npc[x].Y > targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Up))
                                        {
                                            NpcMove(mapNum, x, Direction.Up, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 2:
                                    // Down
                                    if (MapNpc[mapNum].Npc[x].Y < targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Down))
                                        {
                                            NpcMove(mapNum, x, Direction.Down, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (MapNpc[mapNum].Npc[x].Y > targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Up))
                                        {
                                            NpcMove(mapNum, x, Direction.Up, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (MapNpc[mapNum].Npc[x].X < targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Right))
                                        {
                                            NpcMove(mapNum, x, Direction.Right, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Left
                                    if (MapNpc[mapNum].Npc[x].X > targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Left))
                                        {
                                            NpcMove(mapNum, x, Direction.Left, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }
                                    break;

                                case 3:
                                    // Left
                                    if (MapNpc[mapNum].Npc[x].X > targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Left))
                                        {
                                            NpcMove(mapNum, x, Direction.Left, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Right
                                    if (MapNpc[mapNum].Npc[x].X < targetX && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Right))
                                        {
                                            NpcMove(mapNum, x, Direction.Right, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Up
                                    if (MapNpc[mapNum].Npc[x].Y > targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Up))
                                        {
                                            NpcMove(mapNum, x, Direction.Up, MovementType.Walking);
                                            didWalk = true;
                                        }
                                    }

                                    // Down
                                    if (MapNpc[mapNum].Npc[x].Y < targetY && !didWalk)
                                    {
                                        if (CanNpcMove(mapNum, x, Direction.Down))
                                        {
                                            NpcMove(mapNum, x, Direction.Down, MovementType.Walking);
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
