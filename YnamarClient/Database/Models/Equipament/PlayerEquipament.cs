using ProtoBuf;
using System.ComponentModel.DataAnnotations.Schema;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    internal class PlayerEquipament
    {
        [ProtoMember(1)]
        public int CharacterId { get; set; }

        [ProtoMember(2)]
        public int ItemId { get; set; }

        [ProtoMember(3)]
        public Item? Item { get; set; }

        [ProtoMember(4)]
        public int Slot { get; set; }
    }
}
