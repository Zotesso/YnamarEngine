using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Models.Protos
{
    [ProtoContract]
    internal class NpcList
    {
        [ProtoMember(1)]
        public List<NpcSummary> NpcsSummary { get; set; } = new();
    }

    [ProtoContract]
    internal class NpcSummary
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
