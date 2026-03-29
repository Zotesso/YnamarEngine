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
            public int Id;

            [ProtoMember(2)]
            public string Name;

            [ProtoMember(3)]
            public int Sprite;

            [ProtoMember(4)]
            public int Level;

            [ProtoMember(5)]
            public int EXP;

            [ProtoMember(6)]
            public int Map;

            [ProtoMember(7)]
            public int X;

            [ProtoMember(8)]
            public int Y;

            [ProtoMember(9)]
            public byte Dir;

            [ProtoMember(10)]
            public int XOffset;

            [ProtoMember(11)]
            public int YOffset;

            public int Moving;
            public byte Steps;

            [ProtoMember(12)]
            public byte Access;

            [ProtoMember(13)]
            public int MaxHP;

            [ProtoMember(14)]
            public int HP;

            [ProtoMember(15)]
            public Inventory Inventory;

            [ProtoMember(16)]
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
