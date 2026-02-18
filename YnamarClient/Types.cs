using Microsoft.VisualBasic;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Database.Models;
using YnamarClient.Services;

namespace YnamarClient
{
    internal class Types
    {
        public static Player[] Players = new Player[100];
        public static MapStruct[] Map = new MapStruct[100];

        [ProtoContract]
        public class Player
        {
            public string Login;
            public string Password;

            [ProtoMember(1)]
            public string Name;

            [ProtoMember(2)]
            public int Sprite;

            [ProtoMember(3)]
            public int Level;

            [ProtoMember(4)]
            public int EXP;

            [ProtoMember(5)]
            public int Map;

            [ProtoMember(6)]
            public int X;

            [ProtoMember(7)]
            public int Y;

            [ProtoMember(8)]
            public byte Dir;

            [ProtoMember(9)]
            public int XOffset;

            [ProtoMember(10)]
            public int YOffset;

            public int Moving;
            public byte Steps;

            [ProtoMember(11)]
            public byte Access;

            [ProtoMember(12)]
            public int MaxHP;

            [ProtoMember(13)]
            public int HP;

            [ProtoMember(14)]
            public Inventory Inventory;

            [ProtoMember(15)]
            public ICollection<PlayerEquipament> EquippedItems { get; set; }

            public int AttackCooldown;
            public bool Attacking;
            public AnimationPlayerService WeaponAnim = new AnimationPlayerService();
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
            public MapNpc[] MapNpc;
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
