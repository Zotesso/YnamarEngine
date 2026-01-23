using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Protos
{
    [ProtoContract]

    internal class AnimationClipList
    {
        [ProtoMember(1)]
        public List<AnimationClipSummary> AnimationClipSummaryList { get; set; } = new();
    }

    [ProtoContract]
    internal class AnimationClipSummary
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
