using ProtoBuf;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Models
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
        public int Sprite { get; set; }
    }
}
