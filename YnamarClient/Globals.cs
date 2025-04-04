using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient
{
    internal class Globals
    {
        public static bool InGame;

        public static int playerIndex;
        public static Types.MapStruct PlayerMap;

        public static string loginUsername = "";
        public static string loginPassword = "";

        public static string regUsername = "";
        public static string regPassword = "";
        public static string regRepeatPassword = "";

        public static bool DirUp;
        public static bool DirDown;
        public static bool DirLeft;
        public static bool DirRight;

    }
}
