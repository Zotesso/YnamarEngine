using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    public class InventorySlot
    {
        [Required]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; } = null!;

        [Required]
        [ProtoMember(1)]
        public int SlotId { get; set; }

        [ProtoMember(2)]
        public int? ItemId { get; set; }

        [ProtoMember(3)]
        public Item? Item { get; set; } = null;

        [ProtoMember(4)]
        public int Quantity { get; set; }
    }
}
