using ProtoBuf;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YnamarEditors.Models
{
	[ProtoContract]
	public class NpcDrop
	{
		[Key]
        [ProtoMember(1)]
        public int Id { get; set; }

		[Required]
        [ProtoMember(2)]
        public int NpcId { get; set; }

        [ProtoIgnore]
		public Npc? Npc { get; set; }

		[Required]
        [ProtoMember(3)]
        public int ItemId { get; set; }

        [ProtoIgnore]
        public Item? Item { get; set; }

		[Required]
        [ProtoMember(5)]
        public float DropRate { get; set; } // e.g., 0.25 = 25% chance

        [NotMapped]
        public string? ItemName { get; set; } = string.Empty;

        public override string ToString() =>
        $"{ItemName} - Id: {ItemId} - Rate: {DropRate}%";
    }
}
