using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using YnamarEditors.Screens;
using System.Linq;
using YnamarEditors.Components;
using System.Collections.Generic;
using System.Reflection.Metadata;
using RenderingLibrary.Graphics;
using MonoGameGum.GueDeriving;

namespace YnamarEditors
{
    internal class Graphics
    {
        public static Texture2D[] Tilesets = new Texture2D[1];
        public static ScrollBarRuntime verticalScrollbar;
        public static ScrollBarRuntime horizontalScrollbar;

        private static Texture2D pixel;
        private static int resourcePanelBoundariesX = 0;
        public static void InitializeGraphics(ContentManager manager)
        {
            LoadTilesets(manager);
        }
        public static void LoadGumTilesetResourcePanel(MenuManager menuManager)
        {
            MapEditorRuntime mapEditorScreen = (MapEditorRuntime)menuManager.GetCurrentScreen();
            verticalScrollbar = mapEditorScreen.VerticalScrollbar;
            horizontalScrollbar = mapEditorScreen.HorizontalScrollbar;

            int tileSize = 32;
            float viewportHeight = 30;
            float maxScroll = (55) - viewportHeight;

            verticalScrollbar.FormsControl.Minimum = 0;
            verticalScrollbar.FormsControl.Maximum = maxScroll;
            verticalScrollbar.FormsControl.ViewportSize = viewportHeight;

            horizontalScrollbar.FormsControl.Minimum = 0;
            horizontalScrollbar.FormsControl.Maximum = maxScroll;
            horizontalScrollbar.FormsControl.ViewportSize = viewportHeight;

            var resourcePanel = mapEditorScreen.ResourcePanel;
            resourcePanelBoundariesX = (int)resourcePanel.Width;
            var tilesetSprite = new SpriteRuntime
            {
                Texture = Tilesets[0],
                Width = Tilesets[0].Width,
                Height = Tilesets[0].Height,
                WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                X = 0,
                Y = 0
            };

            var selectionBox = new RectangleRuntime
            {
                Name = "SelectionBox",
                Width = tileSize,
                Height = tileSize,
                Color = Microsoft.Xna.Framework.Color.White, // no fill
                //OutlineColor = Microsoft.Xna.Framework.Color.Yellow,
                OutlineThickness = 2,
                WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                Visible = false
            };

            // Add to Gum screen
            resourcePanel.InnerPanelInstance.ChildrenLayout = Gum.Managers.ChildrenLayout.Regular;
            resourcePanel.InnerPanelInstance.Children.Add(tilesetSprite);
            resourcePanel.InnerPanelInstance.Children.Add(selectionBox);
            selectionBox.Z = tilesetSprite.Z + 1;

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
            pixel = new Texture2D(graphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            Game1._spriteBatch.Begin();
            DrawMapGrid();

            Game1._spriteBatch.End();
        }

        public static int ConvertMapX(int x)
        {
            int cameraLeft = 0;
            int tileViewLeft = 0;

            cameraLeft = (int)(horizontalScrollbar?.FormsControl?.Value ?? 0);
            tileViewLeft = (int)(horizontalScrollbar?.FormsControl?.Value ?? 0);
            
            return x - (tileViewLeft * 32) - cameraLeft;
        }

        public static int ConvertMapY(int y)
        {
            int cameraTop = 0;
            int tileViewTop = 0;

            cameraTop = (int)(verticalScrollbar?.FormsControl?.Value ?? 0);
            tileViewTop = (int)(verticalScrollbar?.FormsControl?.Value ?? 0);
            return y - (tileViewTop * 32) - cameraTop;
        }

        
        private static void DrawMapGrid()
        {
            int maxMapLayer = 2;
            Types.Maps[0].MaxMapX = 50;
            Types.Maps[0].MaxMapY = 50;

            for (int layer = 0; layer < maxMapLayer; layer++)
            {
                for (int x = 0; x < Types.Maps[0].MaxMapX; x++)
                {
                    for (int y = 0; y < Types.Maps[0].MaxMapY; y++)
                    {
                        DrawTileGrid((resourcePanelBoundariesX + 30) + (x * 32), y * 32, x, y, layer);
                    }
                }
            }
        }

        private static void DrawTileGrid(int mapX, int mapY, int x, int y, int layerNum)
        {
            int TilesetX = Types.Maps[0].Layer.ElementAt(layerNum).TileMatrix[x, y].TileX;
            int TilesetY = Types.Maps[0].Layer.ElementAt(layerNum).TileMatrix[x, y].TileY;


            Rectangle srcrec;
            //int tilesetnum = 0;
            int tileSize = 32;
            int thickness = 1;

            int MapX, MapY;
            MapX = ConvertMapX(mapX);
            MapY = ConvertMapY(mapY);



            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY, tileSize, thickness), Color.White);
            // Left line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY, thickness, tileSize), Color.White);
            // Right line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX + tileSize - thickness, MapY, thickness, tileSize), Color.White);
            // Bottom line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY + tileSize - thickness, tileSize, thickness), Color.White);

            if (TilesetX == 0 && TilesetY == 0) return;

            srcrec = new Rectangle(TilesetX, TilesetY, 32, 32);
            Game1._spriteBatch.Draw(Tilesets[0], new Vector2(MapX, MapY), srcrec, Color.White);

        }
    }
    
}
