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

namespace YnamarEditors.Services.ItemEditor
{
    internal class ItemEditorService
    {
        public static async Task<ItemList> ListItems()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/itemeditor/item/list");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            ItemList itemList = Serializer.Deserialize<ItemList>(responseStream);
                
            return itemList;
        }

        public static async Task<List<ItemType>> ListItemType()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/itemeditor/item/type/list");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return [];
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            List<ItemType> itemTypeList = Serializer.Deserialize<List<ItemType>>(responseStream);

            return itemTypeList;
        }

        public static async Task<Item> GetItemSummary(int itemId)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:8080/api/itemeditor/item/summary/{itemId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            Item item = Serializer.Deserialize<Item>(responseStream);

            return item;
        }

        public static async Task SaveItem(Item item)
        {
            using HttpClient httpClient = new HttpClient();
            using MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, item);

            ms.Position = 0;

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await httpClient.PostAsync("http://localhost:8080/api/itemeditor/item/save", content);
            MenuManager.StopLoadingAsync(response.IsSuccessStatusCode);
            response.EnsureSuccessStatusCode();
        }
    }
}
