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
