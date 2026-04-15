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
            if (Types.Players[Globals.playerIndex].Moving != 0)
            {
                return false;
            }

            int dir = Types.Players[Globals.playerIndex].Dir;

            if (Globals.DirUp && Types.Players[Globals.playerIndex].Y > 0)
            {
                Types.Players[Globals.playerIndex].Dir = Constants.DIR_UP;
                return true;

            }
            if (Globals.DirDown && Types.Players[Globals.playerIndex].Y < Constants.MAX_MAP_Y)
            {
                Types.Players[Globals.playerIndex].Dir = Constants.DIR_DOWN;
                return true;

            }
            if (Globals.DirRight && Types.Players[Globals.playerIndex].X < Constants.MAX_MAP_X)
            {
                Types.Players[Globals.playerIndex].Dir = Constants.DIR_RIGHT;
                return true;

            }
            if (Globals.DirLeft && Types.Players[Globals.playerIndex].X > 0)
            {
                Types.Players[Globals.playerIndex].Dir = Constants.DIR_LEFT;
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
                    X = Types.Players[Globals.playerIndex].X;
                    Y = Types.Players[Globals.playerIndex].Y - 1;
                    break;
                case Constants.DIR_DOWN:
                    X = Types.Players[Globals.playerIndex].X;
                    Y = Types.Players[Globals.playerIndex].Y + 1;
                    break;
                case Constants.DIR_LEFT:
                    X = Types.Players[Globals.playerIndex].X - 1;
                    Y = Types.Players[Globals.playerIndex].Y;
                    break;
                case Constants.DIR_RIGHT:
                    X = Types.Players[Globals.playerIndex].X + 1;
                    Y = Types.Players[Globals.playerIndex].Y;
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
                    switch (Types.Players[Globals.playerIndex].Dir)
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
                if (Types.Players[Globals.playerIndex].AttackCooldown + attackSpeed < Tick)
                {
                    if (!Types.Players[Globals.playerIndex].Attacking)
                    {
                        if (Types.Players[Globals.playerIndex].EquippedItems is not null && Types.Players[Globals.playerIndex].EquippedItems.ElementAt(0).Item.AnimationClip is not null)
                        {
                            Types.Players[Globals.playerIndex].WeaponAnim.Play(Types.Players[Globals.playerIndex].EquippedItems.ElementAt(0).Item.AnimationClip);
                        }

                        Types.Players[Globals.playerIndex].Attacking = true;
                        Types.Players[Globals.playerIndex].AttackCooldown = Tick;
                        NetworkManager.Client.SendPlayerAttack();
                    }
                }
            }
        }

        public static void ProcessMovement(Types.Player player)
        {
            int movementSpeed = 6;//(Types.Player[index].Moving * 6);

            switch (player.Dir)
            {
                case Constants.DIR_UP:
                    player.YOffset -= movementSpeed;
                    if (player.YOffset < 0)
                    {
                        player.YOffset = 0;
                    }
                    break;
                case Constants.DIR_DOWN:
                    player.YOffset += movementSpeed;
                    if (player.YOffset > 0)
                    {
                        player.YOffset = 0;
                    }
                    break;
                case Constants.DIR_LEFT:
                    player.XOffset -= movementSpeed;
                    if (player.XOffset < 0)
                    {
                        player.XOffset = 0;
                    }
                    break;
                case Constants.DIR_RIGHT:
                    player.XOffset += movementSpeed;
                    if (player.XOffset > 0)
                    {
                        player.XOffset = 0;
                    }
                    break;
            }

            if (player.Moving > 0)
            {
                if (player.Dir == Constants.DIR_RIGHT || player.Dir == Constants.DIR_DOWN)
                {
                    if (player.XOffset >= 0 && player.YOffset >= 0)
                    {
                        player.Moving = 0;
                        if (player.Steps == 0)
                        {
                            player.Steps = 2;
                        }
                        else
                        {
                            player.Steps = 0;
                        }
                    }
                }
                else
                {
                    if (player.XOffset <= 0 && player.YOffset <= 0)
                    {
                        player.Moving = 0;
                        if (player.Steps == 0)
                        {
                            player.Steps = 2;
                        }
                        else
                        {
                            player.Steps = 0;
                        }
                    }
                }
            }
        }

        public static void ProcessMapNpcsMovement()
        {
            foreach (MapNpc mapNpc in Game1.chunkManager.GetVisibleMapNpcs())
            {
                if (mapNpc.RespawnWait > 0)
                    continue;

                ProcessNpcMovement(mapNpc);

            }

            //int maxMapLayer = Globals.PlayerMap.Layer.Length;

            //for (int layer = 0; layer < maxMapLayer; layer++)
            //{
            //    if (Globals.PlayerMap.Layer[layer].MapNpc == null) continue;

            //    for (int x = 0; x < Globals.PlayerMap.Layer[layer].MapNpc.Length; x++)
            //    {
            //        var npc = Globals.PlayerMap.Layer[layer].MapNpc[x];
            //        if (npc != null)
            //        {
            //            if (npc.RespawnWait > 0)
            //                continue;

            //            ProcessNpcMovement(npc);
            //        }
            //    }
            //}
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

        public static void ToggleInventory(MenuManager menuManager)
        {
            if (InterfaceGUI.PlayerInventory is not null)
            {
                InterfaceGUI.PlayerInventory.RemoveFromRoot();
                InterfaceGUI.PlayerInventory = null;
                return;
            }

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
