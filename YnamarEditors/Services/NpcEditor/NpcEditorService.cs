using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using YnamarEditors.Models;
using YnamarEditors.Models.Protos;

namespace YnamarEditors.Services.NpcEditor
{
    internal class NpcEditorService
    {
        public static async Task<NpcList> ListNpcs()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/npceditor/listnpcs");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                NpcList npcList = Serializer.Deserialize<NpcList>(responseStream);
                
                return npcList;
        }
    }
}
