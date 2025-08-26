using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Database.Models
{
    [ProtoContract]
    internal class Inventory
    {
        [Key]
        [ProtoMember(1)]
        public int InventoryId { get; set; }

        public Character Character { get; set; } = null!;

        [ProtoMember(2)]
        public ICollection<InventorySlot> Slots { get; set; } = new List<InventorySlot>();
    }
}
