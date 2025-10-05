using Microsoft.Xna.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models;

namespace YnamarEditors
{
    internal class Types
    {
        public static Map[] Maps = new Map[100];

        public static TileEventStruct[] TileEvents = new TileEventStruct[2];

        public struct TileEventStruct
        {
            public string mapAcronym;
            public Color mapAcronymColor;
            public string Name;
            public byte Type;
            public byte Moral;
            public int Data1;
            public int Data2;
            public int Data3;
        }
    }
}
