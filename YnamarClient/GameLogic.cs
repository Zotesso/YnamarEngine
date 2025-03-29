using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient
{
    internal class GameLogic
    {

        //private static ClientTCP clienttcp = new ClientTCP();

        public static bool IsTryingToMove()
        {
            if (Globals.DirUp || Globals.DirRight || Globals.DirLeft || Globals.DirDown)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    Types.Player[Globals.playerIndex].Moving = 1;
                    switch (Types.Player[Globals.playerIndex].Dir)
                    {
                        case Constants.DIR_UP:
                            //clienttcp.SendPlayerMove();
                            Types.Player[Globals.playerIndex].YOffset = 32;
                            Types.Player[Globals.playerIndex].Y -= 1;
                            break;
                        case Constants.DIR_DOWN:
                            //clienttcp.SendPlayerMove();

                            Types.Player[Globals.playerIndex].YOffset = (32 * -1);
                            Types.Player[Globals.playerIndex].Y += 1;
                            break;
                        case Constants.DIR_LEFT:
                            //clienttcp.SendPlayerMove();

                            Types.Player[Globals.playerIndex].XOffset = 32;
                            Types.Player[Globals.playerIndex].X -= 1;
                            break;
                        case Constants.DIR_RIGHT:
                            //clienttcp.SendPlayerMove();
                            Types.Player[Globals.playerIndex].XOffset = (32 * -1);
                            Types.Player[Globals.playerIndex].X += 1;
                            break;
                    }
                }
            }
        }

        public static void ProcessMovement(int index)
        {
            //int movementSpeed = (Types.Player[index].Moving * 6);
            int movementSpeed = (2 * 6);

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

        public static void InGame()
        {
            Globals.InGame = true;
        }
    }
}
