using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    internal class InventorySlot
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int SlotId { get; set; }

        [ProtoMember(3)]
        public int? ItemId { get; set; }

        [ProtoMember(4)]
        public Item? Item { get; set; } = null;

        [ProtoMember(5)]
        public int Quantity { get; set; }
    }
}
