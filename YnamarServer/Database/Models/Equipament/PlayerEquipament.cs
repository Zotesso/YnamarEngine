using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models.Animation;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    internal class PlayerEquipament
    {
        [ProtoMember(1)]
        public int CharacterId { get; set; }

        [ProtoIgnore]
        public Character Character { get; set; }

        [ProtoMember(2)]
        public int ItemId { get; set; }

        [ProtoMember(3)]
        [ForeignKey(nameof(ItemId))]
        public Item? Item { get; set; }

        [ProtoMember(4)]
        public int Slot { get; set; }

    }
}
