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
    internal class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        public Character Character { get; set; } = null!;

        public ICollection<InventorySlot> Slots { get; set; } = new List<InventorySlot>();
    }
}
