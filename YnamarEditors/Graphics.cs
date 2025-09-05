using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace YnamarEditors
{
    internal class Graphics
    {
        public static Texture2D[] Tilesets = new Texture2D[1];
        private static Rectangle _resourcesPanel = new Rectangle(0, 0, 300, 720);
        private static Rectangle _mapPanel = new Rectangle(300, 0, 980, 720);

        private static Texture2D _pixel;

        public static void InitializeGraphics(ContentManager manager)
        {
            LoadTilesets(manager);
        }

        private static void LoadTilesets(ContentManager manager)
        {
            for (int i = 0; i < Tilesets.Length; i++)
            {
                Tilesets[i] = manager.Load<Texture2D>("Tilesets/" + i.ToString());
            }
        }

        public static void RenderGraphics(GraphicsDevice graphicsDevice)
        {
            Game1._spriteBatch.Begin();
            _pixel = new Texture2D(graphicsDevice, width: 1, height: 1);
            _pixel.SetData(new[] { Color.White });

            //DrawMapGrid();
            Game1._spriteBatch.Draw(_pixel, _resourcesPanel, Color.DarkGray);

            // draw main map panel (blue-ish background)
            Game1._spriteBatch.Draw(_pixel, _mapPanel, Color.LightBlue);

            // Example: draw some placeholder textures in the resources panel
            int y = 10;
            foreach (var tex in Tilesets)
            {
                Game1._spriteBatch.Draw(tex, new Rectangle(10, y, 64, 64), Color.White);
                y += 74;
            }

            // Example: draw grid in map panel
            int tileSize = 32;
            for (int x = _mapPanel.X; x < _mapPanel.Right; x += tileSize)
            {
                for (int y2 = _mapPanel.Y; y2 < _mapPanel.Bottom; y2 += tileSize)
                {
                    var rect = new Rectangle(x, y2, tileSize, tileSize);
                    Game1._spriteBatch.Draw(_pixel, rect, Color.Black * 0.05f);
                }
            }

            Game1._spriteBatch.End();
        }

        public static int ConvertMapX(int x)
        {
            int cameraLeft = 0;
            int tileViewLeft = 0;

            cameraLeft = 0;//Types.Player[Globals.playerIndex].X + Types.Player[Globals.playerIndex].XOffset;
            tileViewLeft = 0;// Types.Player[Globals.playerIndex].X - 5;

            return x - (tileViewLeft * 32) - cameraLeft;
        }

        public static int ConvertMapY(int y)
        {
            int cameraTop = 0;
            int tileViewTop = 0;

            cameraTop = 0;// Types.Player[Globals.playerIndex].Y + Types.Player[Globals.playerIndex].YOffset;
            tileViewTop = 0;// Types.Player[Globals.playerIndex].Y - 5;
            return y - (tileViewTop * 32) - cameraTop;
        }

        /*
        private static void DrawMapGrid()
        {
            int maxMapLayer = Globals.PlayerMap.Layer.Length;


            for (int layer = 0; layer < maxMapLayer; layer++)
            {
                for (int x = 0; x < Globals.PlayerMap.Layer[layer].Tile.GetLength(0); x++)
                {
                    for (int y = 0; y < Globals.PlayerMap.Layer[layer].Tile.GetLength(1); y++)
                    {
                        DrawTile(x * 32, y * 32, x, y, layer);
                    }
                }
            }
        }

        private static void DrawTile(int mapX, int mapY, int x, int y, int layerNum)
        {
            Rectangle srcrec;
            int tilesetnum = Globals.PlayerMap.Layer[layerNum].Tile[x,y].TilesetNumber;

            int MapX, MapY;
            MapX = ConvertMapX(mapX);
            MapY = ConvertMapY(mapY);

            int TilesetX = Globals.PlayerMap.Layer[layerNum].Tile[x, y].TileX;
            int TilesetY = Globals.PlayerMap.Layer[layerNum].Tile[x, y].TileY;

            srcrec = new Rectangle(TilesetX, TilesetY, 32,32);
            Game1.spriteBatch.Draw(Tilesets[tilesetnum], new Vector2(MapX, MapY), srcrec, Color.White);
        }
    }
        */
    }
}
