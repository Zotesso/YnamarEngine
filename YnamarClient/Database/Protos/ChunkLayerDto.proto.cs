using ProtoBuf;

namespace YnamarClient.Database.Protos
{
    [ProtoContract]
    public class ChunkLayerDto
    {
        [ProtoMember(1)]
        public int LayerId { get; set; }

        [ProtoMember(2)]
        public byte[] Tiles { get; set; }
    }
}
