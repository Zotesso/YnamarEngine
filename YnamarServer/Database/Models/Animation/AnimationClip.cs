using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models.Animation
{
    [ProtoContract]
    public class AnimationClip
    {
        [Key]
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; } = string.Empty;

        [ProtoMember(3)]
        public bool Loop { get; set; }

        [ProtoMember(4)]
        public List<AnimationFrame> Frames { get; set; } = new();
    }
}
