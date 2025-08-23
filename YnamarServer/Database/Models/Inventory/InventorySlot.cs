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
    internal class InventorySlot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; } = null!;

        [Required]
        public int SlotId { get; set; }

        public int? ItemId { get; set; }

        public Item? Item { get; set; } = null;
        
        public int Quantity { get; set; }
    }
}
