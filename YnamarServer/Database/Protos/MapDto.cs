using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarServer.Database.Models;
using YnamarServer.Database.Models.Map;

namespace YnamarServer.Database.Protos
{
    [ProtoContract]
    public class MapLoadDto
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public int Width { get; set; }

        [ProtoMember(4)]
        public int Height { get; set; }

        [ProtoMember(5)]
        public int ChunkSize { get; set; }

        [ProtoMember(6)]
        public List<TileDefinition> TileDefinitions { get; set; }

        [ProtoMember(7)]
        public List<MapNpc> Npcs { get; set; }
    }
}
