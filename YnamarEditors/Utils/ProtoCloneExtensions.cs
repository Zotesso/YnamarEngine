using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Utils
{
    public static class ProtoCloneExtensions
    {
        public static T DeepClone<T>(this T obj)
        {
            if (obj == null) return default;

            using var ms = new MemoryStream();
            Serializer.Serialize(ms, obj);
            ms.Position = 0;
            return Serializer.Deserialize<T>(ms);
        }
    }
}
