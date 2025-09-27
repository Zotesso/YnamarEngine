using Cyotek.Drawing.BitmapFont;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Database.Models;
using static YnamarClient.Types;

namespace YnamarClient.Services
{
    internal class MapService
    {
        public void convertMapPayloadToClientMap(Map mapPayload)
        {
            Types.Map[mapPayload.Id].MaxMapX = mapPayload.MaxMapX;
            Types.Map[mapPayload.Id].MaxMapY = mapPayload.MaxMapY;
            Types.Map[mapPayload.Id].Name = mapPayload.Name;
            Types.Map[mapPayload.Id].Layer = new MapLayerStruct[mapPayload.Layer.Count];
            foreach (MapLayer layer in mapPayload.Layer)
            {
                Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Index = layer.LayerLevel;

                if (layer.Tile.Count == 0) continue;

                int minX = layer.Tile.Min(t => t.X);
                int minY = layer.Tile.Min(t => t.Y);
                int maxX = layer.Tile.Max(t => t.X);
                int maxY = layer.Tile.Max(t => t.Y);
                Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile = new TileStruct[maxX + 1, maxY + 1];
                if (layer.MapNpc.ToArray().Length > 0)
                {
                    Types.Map[mapPayload.Id].Layer[layer.LayerLevel].MapNpc = layer.MapNpc.ToArray();
                } else
                {
                    Types.Map[mapPayload.Id].Layer[layer.LayerLevel].MapNpc = new MapNpc[Constants.MAX_MAP_NPCS];
                }

                int actualTile = 0;
                for (int i = minX; i <= maxX; i++)
                {
                    for (int j = minY; j <= maxY; j++)
                    {
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].TileX = ((List<Tile>)layer.Tile)[actualTile].TileX;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].TileY = ((List<Tile>)layer.Tile)[actualTile].TileY;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].TilesetNumber = ((List<Tile>)layer.Tile)[actualTile].TilesetNumber;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].Type = ((List<Tile>)layer.Tile)[actualTile].Type;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].Moral = ((List<Tile>)layer.Tile)[actualTile].Moral;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].Data1 = ((List<Tile>)layer.Tile)[actualTile].Data1;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].Data2 = ((List<Tile>)layer.Tile)[actualTile].Data2;
                        Types.Map[mapPayload.Id].Layer[layer.LayerLevel].Tile[i, j].Data3 = ((List<Tile>)layer.Tile)[actualTile].Data3;

                        actualTile++;
                    }
                }
            }

            Globals.PlayerMap = Types.Map[mapPayload.Id];
        }
    }
}
