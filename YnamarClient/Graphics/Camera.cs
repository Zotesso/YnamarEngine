using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Graphics
{
    public static class Camera
    {
        public static int X;
        public static int Y;

        public static void UpdateCamera()
        {
            var player = Types.Players[Globals.playerIndex];

            Camera.X = (player.X * 32) + player.XOffset - 350;
            Camera.Y = (player.Y * 32) + player.YOffset - 250;
        }
    }
}
