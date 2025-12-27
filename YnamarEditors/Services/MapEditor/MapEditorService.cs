using ProtoBuf;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using YnamarEditors.Models;
using YnamarEditors.Utils;

namespace YnamarEditors.Services.MapEditor
{
    internal class MapEditorService
    {
        public static async Task SaveMap()
        {
            Types.Maps[Globals.SelectedMap].LastUpdate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);
            using HttpClient httpClient = new HttpClient();
            using MemoryStream ms = new MemoryStream();

            Map mapToSave = Types.Maps[Globals.SelectedMap].DeepClone();

            foreach (MapLayer layer in mapToSave.Layer)
            {
                foreach (MapNpc mapNpc in layer.MapNpc)
                {
                    mapNpc.Npc = null;
                }
            }

            Serializer.Serialize(ms, mapToSave);

            ms.Position = 0;
            byte[] payload = ms.ToArray();

            // Salva o arquivo para debug
            File.WriteAllBytes("mapeditor_request.pb", payload);

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await httpClient.PostAsync("http://localhost:8080/api/mapeditor", content);
            response.EnsureSuccessStatusCode();
        }

        public static async Task GetMap()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:8080/api/mapeditor/{Globals.SelectedMap}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                Globals.SelectedLayer = 0;
                CreateNewLocalMap(Globals.SelectedMap);
                return;
            }
            else
            {
                Globals.SelectedLayer = 0;
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                Map map = Serializer.Deserialize<Map>(responseStream);
                Types.Maps[Globals.SelectedMap] = map;
                int maxLayers = 2;

                for (int l = 0; l < maxLayers; l++)
                {
                    if (Types.Maps[Globals.SelectedMap].Layer.ElementAt(l) == null)
                    {
                        Types.Maps[Globals.SelectedMap].Layer.Add(new MapLayer
                        {
                            MapId = Globals.SelectedMap,
                            LayerLevel = (byte)l,
                            TileMatrix = new Tile[50, 50],
                        });
                    }
                    else
                    {
                        Types.Maps[Globals.SelectedMap].Layer.ElementAt(l).TileMatrix = new Tile[50, 50];
                    }

                    for (int x = 0; x < 50; x++)
                    {
                        for (int y = 0; y < 50; y++)
                        {
                            Types.Maps[Globals.SelectedMap]
                                .Layer.ElementAt(l)
                                    .TileMatrix[x, y] = Types.Maps[Globals.SelectedMap].Layer
                                                                .ElementAt(l).Tile
                                                                    .ElementAt(x + x * 49 + y)
                            ?? new Tile
                            {
                                TilesetNumber = 0,
                                Type = 0,
                                Moral = 0,
                                Data1 = 0,
                                Data2 = 0,
                                Data3 = 0,
                                X = x,
                                Y = y,
                                TileX = 0,
                                TileY = 0,
                            };
                        }
                    }
                }
            }
        }

        private static void CreateNewLocalMap(int mapIndex)
        {
            int maxLayers = 2;
            if (Types.Maps[mapIndex] == null)
            {
                Types.Maps[mapIndex] = new Map
                {
                    Id = mapIndex,
                    Name = $"Map {mapIndex}",
                    MaxMapX = 50,
                    MaxMapY = 50
                };
            }


            for (int l = 0; l < maxLayers; l++)
            {
                Types.Maps[mapIndex].Layer.Add(new MapLayer
                {
                    MapId = mapIndex,
                    LayerLevel = (byte)l,
                    TileMatrix = new Tile[50, 50],
                });


                for (int x = 0; x < 50; x++)
                {
                    for (int y = 0; y < 50; y++)
                    {

                        Tile tile = new Tile
                        {
                            TilesetNumber = 0,
                            Type = 0,
                            Moral = 0,
                            Data1 = 0,
                            Data2 = 0,
                            Data3 = 0,
                            X = x,
                            Y = y,
                            TileX = 0,
                            TileY = 0,
                        };

                        Types.Maps[mapIndex].Layer.ElementAt(l).Tile.Add(tile);
                        Types.Maps[mapIndex].Layer.ElementAt(l).TileMatrix[x, y] = tile;
                    }
                }
            }
        }
    }
}
