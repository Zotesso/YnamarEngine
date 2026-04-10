using ProtoBuf;

namespace YnamarServer.Database.Protos
{
    [ProtoContract]
    public class ChunkDto
    {
        [ProtoMember(1)]
        public int X { get; set; }

        [ProtoMember(2)]
        public int Y { get; set; }

        [ProtoMember(3)]
        public int Size { get; set; }

        [ProtoMember(4)]
        public List<ChunkLayerDto> Layers { get; set; } = new();
    }
}
