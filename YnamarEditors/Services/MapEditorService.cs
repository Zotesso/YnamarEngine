using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Services
{
    internal class MapEditorService
    {
        public static async Task SaveMap()
        {
            Types.Maps[0].LastUpdate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            using HttpClient httpClient = new HttpClient();
            using MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, Types.Maps[0]);
            using (var file = File.Create("mapdata.bin"))
            {
                Serializer.Serialize(file, Types.Maps[0]);
            }

            ms.Position = 0;

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await httpClient.PostAsync("http://localhost:8080/api/mapeditor", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
