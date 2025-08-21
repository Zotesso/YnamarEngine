using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YnamarServer.Network.NetworkPackets;
using YnamarServer.Network;
using YnamarServer.Database;

namespace YnamarServer.GameLogic
{
    internal class GameLogicHandler
    {
        private static ServerTCP stcp = new ServerTCP();

        public static void PlayerMove(int index, byte dir, int movement)
        {
            InMemoryDatabase.Player[index].Dir = dir;

            int x = DirToX(InMemoryDatabase.Player[index].X, dir);
            int y = DirToY(InMemoryDatabase.Player[index].Y, dir);

            if (IsValidPosition(x, y))
            {
                InMemoryDatabase.Player[index].X = x;
                InMemoryDatabase.Player[index].Y = y;

                PacketBuffer buffer = new PacketBuffer();
                buffer.AddInteger((int)ServerPackets.SPlayerMove);
                buffer.AddInteger(index);
                buffer.AddInteger(InMemoryDatabase.Player[index].X);
                buffer.AddInteger(InMemoryDatabase.Player[index].Y);
                buffer.AddByte(dir);
                buffer.AddInteger(movement);

                stcp.SendDataToMapBut(index, InMemoryDatabase.Player[index].Map, buffer.ToArray());
                buffer.Dispose();
            }
        }

        public static void PlayerAttack(int index, byte dir)
        {
            int targetX = DirToX(InMemoryDatabase.Player[index].X, dir);
            int targetY = DirToY(InMemoryDatabase.Player[index].Y, dir);
            int playerMapNum = InMemoryDatabase.Player[index].Map;
            int? mapNpcIndex = MapLogicHandler.CheckForNpcInRange(playerMapNum, targetX, targetY);

            if (mapNpcIndex)
            {
                InMemoryDatabase.Maps[playerMapNum].Layer[0].MapNpc[mapNpcIndex].Hp -= 10;
                NpcService npcService = Program.npcService;
                npcService.SendNpcAttackedtoMap(mapNum, layerNum, mapNpc);
            }
        }
        public static int DirToX(int x, byte dir)
        {
            if (dir == Constants.DIR_UP || dir == Constants.DIR_DOWN)
            {
                return x;
            }

            return x + ((dir * 2) - 5);
        }

        public static int DirToY(int y, byte dir)
        {
            if (dir == Constants.DIR_LEFT || dir == Constants.DIR_RIGHT)
            {
                return y;
            }

            return y + ((dir * 2) - 1);
        }

        public static bool IsValidPosition(int x, int y)
        {
            if (x < 0 || x > Constants.MAX_MAP_X || y < 0 || y > Constants.MAX_MAP_Y)
            {
                return false;
            }
            return true;
        }
    }
}
