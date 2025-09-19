using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors
{
    internal class Types
    {
        public static MapStruct[] Maps = new MapStruct[100];

        [Serializable]
        public struct MapStruct
        {
            public string Name;
            public int MaxMapX;
            public int MaxMapY;

            public MapLayerStruct[] Layer;
        }

        [Serializable]
        public struct MapLayerStruct
        {
            public byte Index;
            public TileStruct[,] Tile;
            //public MapNpc[] MapNpc;
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
