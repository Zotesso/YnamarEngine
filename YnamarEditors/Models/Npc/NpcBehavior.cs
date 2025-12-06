using ProtoBuf;
using System.ComponentModel.DataAnnotations;

namespace YnamarEditors.Models
{
    [ProtoContract]
    public class NpcBehavior
    {
        [Key]
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }
}
