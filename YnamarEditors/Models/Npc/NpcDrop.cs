using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace YnamarEditors.Models
{
	[ProtoContract]
	public class NpcDrop
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public int NpcId { get; set; }
		public Npc Npc { get; set; } = null!;

		[Required]
		public int ItemId { get; set; }

		public Item Item { get; set; } = null!;

		[Required]
		public float DropRate { get; set; } // e.g., 0.25 = 25% chance

	}
}
