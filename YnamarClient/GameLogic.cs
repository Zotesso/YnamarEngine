using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using MonoGameGum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Network;
using YnamarClient.Database.Models;
using YnamarClient.GUI;
using static YnamarClient.Network.NetworkPackets;

namespace YnamarClient
{
    internal class GameLogic
    {

        private static ClientTCP clienttcp = new ClientTCP();

        public static bool IsTryingToMove()
        {
            return (Globals.DirUp || Globals.DirRight || Globals.DirLeft || Globals.DirDown);
        }

        public static bool IsTryingToAttack()
        {
            return Globals.ZKeyPressed;
        }

        public static bool CanMove()
        {
            if (Types.Player[Globals.playerIndex].Moving != 0)
            {
                return false;
            }

            int dir = Types.Player[Globals.playerIndex].Dir;

            if (Globals.DirUp && Types.Player[Globals.playerIndex].Y > 0)
            {
                Types.Player[Globals.playerIndex].Dir = Constants.DIR_UP;
                return true;

            }
            if (Globals.DirDown && Types.Player[Globals.playerIndex].Y < Constants.MAX_MAP_Y)
            {
                Types.Player[Globals.playerIndex].Dir = Constants.DIR_DOWN;
                return true;

            }
            if (Globals.DirRight && Types.Player[Globals.playerIndex].X < Constants.MAX_MAP_X)
            {
                Types.Player[Globals.playerIndex].Dir = Constants.DIR_RIGHT;
                return true;

            }
            if (Globals.DirLeft && Types.Player[Globals.playerIndex].X > 0)
            {
                Types.Player[Globals.playerIndex].Dir = Constants.DIR_LEFT;
                return true;

            }

            return false;
        }

        public static bool CheckDirections(byte direction)
        {
            int X, Y;

            switch (direction)
            {
                case Constants.DIR_UP:
                    X = Types.Player[Globals.playerIndex].X;
                    Y = Types.Player[Globals.playerIndex].Y - 1;
                    break;
                case Constants.DIR_DOWN:
                    X = Types.Player[Globals.playerIndex].X;
                    Y = Types.Player[Globals.playerIndex].Y + 1;
                    break;
                case Constants.DIR_LEFT:
                    X = Types.Player[Globals.playerIndex].X - 1;
                    Y = Types.Player[Globals.playerIndex].Y;
                    break;
                case Constants.DIR_RIGHT:
                    X = Types.Player[Globals.playerIndex].X + 1;
                    Y = Types.Player[Globals.playerIndex].Y;
                    break;
            }

            return false;
        }

        public static void CheckMovement()
        {
            if (IsTryingToMove())
            {
                if (CanMove())
                {
                    switch (Types.Player[Globals.playerIndex].Dir)
                    {
                        case Constants.DIR_UP:
                            clienttcp.SendPlayerMove();
                            break;
                        case Constants.DIR_DOWN:
                            clienttcp.SendPlayerMove();
                            break;
                        case Constants.DIR_LEFT:
                            clienttcp.SendPlayerMove();
                            break;
                        case Constants.DIR_RIGHT:
                            clienttcp.SendPlayerMove();
                            break;
                    }
                }
            }
        }
        public static void CheckAttack(int Tick)
        {
            if (IsTryingToAttack())
            {
                int attackSpeed = 1000;
                if (Types.Player[Globals.playerIndex].AttackCooldown + attackSpeed < Tick)
                {
                    if (!Types.Player[Globals.playerIndex].Attacking)
                    {
                        Types.Player[Globals.playerIndex].Attacking = true;
                        Types.Player[Globals.playerIndex].AttackCooldown = Tick;
                        NetworkManager.Client.SendPlayerAttack();
                    }
                }
            }
        }

        public static void ProcessMovement(int index)
        {
            int movementSpeed = 6;//(Types.Player[index].Moving * 6);

            switch (Types.Player[index].Dir)
            {
                case Constants.DIR_UP:
                    Types.Player[index].YOffset -= movementSpeed;
                    if (Types.Player[index].YOffset < 0)
                    {
                        Types.Player[index].YOffset = 0;
                    }
                    break;
                case Constants.DIR_DOWN:
                    Types.Player[index].YOffset += movementSpeed;
                    if (Types.Player[index].YOffset > 0)
                    {
                        Types.Player[index].YOffset = 0;
                    }
                    break;
                case Constants.DIR_LEFT:
                    Types.Player[index].XOffset -= movementSpeed;
                    if (Types.Player[index].XOffset < 0)
                    {
                        Types.Player[index].XOffset = 0;
                    }
                    break;
                case Constants.DIR_RIGHT:
                    Types.Player[index].XOffset += movementSpeed;
                    if (Types.Player[index].XOffset > 0)
                    {
                        Types.Player[index].XOffset = 0;
                    }
                    break;
            }

            if (Types.Player[index].Moving > 0)
            {
                if (Types.Player[index].Dir == Constants.DIR_RIGHT || Types.Player[index].Dir == Constants.DIR_DOWN)
                {
                    if (Types.Player[index].XOffset >= 0 && Types.Player[index].YOffset >= 0)
                    {
                        Types.Player[index].Moving = 0;
                        if (Types.Player[index].Steps == 0)
                        {
                            Types.Player[index].Steps = 2;
                        }
                        else
                        {
                            Types.Player[index].Steps = 0;
                        }
                    }
                }
                else
                {
                    if (Types.Player[index].XOffset <= 0 && Types.Player[index].YOffset <= 0)
                    {
                        Types.Player[index].Moving = 0;
                        if (Types.Player[index].Steps == 0)
                        {
                            Types.Player[index].Steps = 2;
                        }
                        else
                        {
                            Types.Player[index].Steps = 0;
                        }
                    }
                }
            }
        }

        public static void ProcessMapNpcsMovement()
        {
            int maxMapLayer = Globals.PlayerMap.Layer.Length;

            for (int layer = 0; layer < maxMapLayer; layer++)
            {
                if (Globals.PlayerMap.Layer[layer].MapNpc == null) continue;

                for (int x = 0; x < Globals.PlayerMap.Layer[layer].MapNpc.Length; x++)
                {
                    var npc = Globals.PlayerMap.Layer[layer].MapNpc[x];
                    if (npc != null)
                    {
                        if (npc.RespawnWait > 0)
                            continue;

                        ProcessNpcMovement(npc);
                    }
                }
            }
        }

        public static void ProcessNpcMovement(MapNpc mapNpc)
        {
            int movementSpeed = 6;//(Types.Player[index].Moving * 6);

            switch (mapNpc.Dir)
            {
                case Constants.DIR_UP:
                    mapNpc.YOffset -= movementSpeed;
                    if (mapNpc.YOffset < 0)
                    {
                        mapNpc.YOffset = 0;
                    }
                    break;
                case Constants.DIR_DOWN:
                    mapNpc.YOffset += movementSpeed;
                    if (mapNpc.YOffset > 0)
                    {
                        mapNpc.YOffset = 0;
                    }
                    break;
                case Constants.DIR_LEFT:
                    mapNpc.XOffset -= movementSpeed;
                    if (mapNpc.XOffset < 0)
                    {
                        mapNpc.XOffset = 0;
                    }
                    break;
                case Constants.DIR_RIGHT:
                    mapNpc.XOffset += movementSpeed;
                    if (mapNpc.XOffset > 0)
                    {
                        mapNpc.XOffset = 0;
                    }
                    break;
            }

            if (mapNpc.Moving > 0)
            {
                if (mapNpc.Dir == Constants.DIR_RIGHT || mapNpc.Dir == Constants.DIR_DOWN)
                {
                    if (mapNpc.XOffset >= 0 && mapNpc.YOffset >= 0)
                    {
                        mapNpc.Moving = 0;
                        if (mapNpc.Steps == 0)
                        {
                            mapNpc.Steps = 2;
                        }
                        else
                        {
                            mapNpc.Steps = 0;
                        }
                    }
                }
                else
                {
                    if (mapNpc.XOffset <= 0 && mapNpc.YOffset <= 0)
                    {
                        mapNpc.Moving = 0;
                        if (mapNpc.Steps == 0)
                        {
                            mapNpc.Steps = 2;
                        }
                        else
                        {
                            mapNpc.Steps = 0;
                        }
                    }
                }
            }
        }

        public static void OpenInventory()
        {
            MenuManager.IGUI.CreateWindow_Inventory();
        }

        public static void InGame()
        {
            Globals.InGame = true;
            //Game1.ClearScreenGum();
            GumService.Default.Root.Children.Clear();
        }
    }
}
