using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient
{
    internal class Types
    {
        public static PlayerStruct[] Player = new PlayerStruct[100];

        [Serializable]
        public struct PlayerStruct
        {
            public string Login;
            public string Password;

            public string Name;
            public int Sprite;
            public int Level;
            public int EXP;

            public int Map;
            public int X;
            public int Y;
            public byte Dir;

            public int XOffset;
            public int YOffset;
            public int Moving;
            public byte Steps;

            public byte Access;
        }

        [Serializable]
        public struct MapStruct
        {
            public string Name;
            public int  MaxMapX;
            public int MaxMapY;

            public MapLayerStruct[] Layer;
        }

        [Serializable]
        public struct MapLayerStruct
        {
            public byte Index;
            public TileStruct[,] Tile;
        }

        [Serializable]
        public struct TileStruct
        {
            public int TilesetNumber;
            public int TileX;
            public int TileY;
            public byte Type;
            public byte Moral;
            public int Data1;
            public int Data2;
            public int Data3;
        }
    }
}
