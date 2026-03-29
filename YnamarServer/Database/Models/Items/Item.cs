using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models.Animation;
using static YnamarServer.Constants;

namespace YnamarServer.Database.Models
{
    [ProtoContract]
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [ProtoMember(1)]
        public string Name { get; set; }

        [ProtoMember(2)]
        public string Description { get; set; }

        [ProtoMember(3)]
        public bool Stackable { get; set; }

        [ProtoMember(4)]
        public int Type { get; set; }

        [ProtoMember(5)]
        public UsageItemType UsageType { get; set; }

        [ProtoMember(6)]
        public int Sprite { get; set; }

        [ProtoMember(7)]
        public int? AnimationClipId { get; set; }

        [ProtoMember(8)]
        [ForeignKey(nameof(AnimationClipId))]
        public AnimationClip? AnimationClip { get; set; }
    }
}
