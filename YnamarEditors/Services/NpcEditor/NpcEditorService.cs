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
using System.IO;

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

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/npceditor/npcs/list");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

                await using var responseStream = await response.Content.ReadAsStreamAsync();
                NpcList npcList = Serializer.Deserialize<NpcList>(responseStream);
                
                return npcList;
        }
        public static async Task<List<NpcBehavior>> ListNpcBehaviors()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/npceditor/npcs/behavior/list");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return [];
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            List<NpcBehavior> npcBehaviorList = Serializer.Deserialize<List<NpcBehavior>>(responseStream);

            return npcBehaviorList;
        }

        public static async Task<Npc> GetNpcSummary(int npcId)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:8080/api/npceditor/npcs/summary/{npcId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            Npc npc = Serializer.Deserialize<Npc>(responseStream);

            return npc;
        }

        public static async Task SaveNpc(Npc npc)
        {
            using HttpClient httpClient = new HttpClient();
            using MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, npc);

            ms.Position = 0;

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await httpClient.PostAsync("http://localhost:8080/api/npceditor/npcs/save", content);
            MenuManager.StopLoadingAsync(response.IsSuccessStatusCode);
            response.EnsureSuccessStatusCode();
        }
    }
}
