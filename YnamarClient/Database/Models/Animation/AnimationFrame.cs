using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.Database.Models.Animation
{
    [ProtoContract]
    public class AnimationFrame
    {
        [Key]
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public int AnimationClipId { get; set; }

        [ProtoIgnore]
        [ForeignKey(nameof(AnimationClipId))]
        public AnimationClip? AnimationClip { get; set; }

        [ProtoMember(3)]
        [Required]
        public int TextureId { get; init; }

        [ProtoMember(4)]
        public int SourceX { get; set; }

        [ProtoMember(5)]
        public int SourceY { get; set; }

        [ProtoMember(6)]
        public int SourceWidth { get; set; }

        [ProtoMember(7)]
        public int SourceHeight { get; set; }

        [ProtoMember(8)]
        public float Duration { get; set; }

        [NotMapped]
        public Rectangle SourceRect =>
        new(SourceX, SourceY, SourceWidth, SourceHeight);
    }
}
