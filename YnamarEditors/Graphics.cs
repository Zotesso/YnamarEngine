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
using YnamarEditors.Models;
using System.Reflection;
using static YnamarEditors.Types;
using static YnamarEditors.Globals;
using Microsoft.VisualBasic;

namespace YnamarEditors
{
    internal class Graphics
    {
        public static Texture2D[] Tilesets = new Texture2D[2];
        public static Texture2D[] Spritesheets = new Texture2D[Globals.MAX_SPRITE_SHEET];
        public static Texture2D[] Characters = new Texture2D[Globals.MAX_SPRITES];

        public static ScrollBarRuntime verticalScrollbar;
        public static ScrollBarRuntime horizontalScrollbar;
        private static SpriteFont font;

        private static Texture2D pixel;
        private static int resourcePanelBoundariesX = 0;
        public static void InitializeGraphics(ContentManager manager)
        {
            LoadFonts(manager);
            LoadTilesets(manager);
            LoadSpritesheets(manager);
            LoadCharacters(manager);
        }
        public static void LoadGumTilesetResourcePanel(MenuManager menuManager)
        {
            MapEditorRuntime mapEditorScreen = (MapEditorRuntime)menuManager.GetCurrentScreen();
            verticalScrollbar = mapEditorScreen.VerticalScrollbar;
            horizontalScrollbar = mapEditorScreen.HorizontalScrollbar;

            int tileSize = 32;
            float viewportHeight = 30;
            float maxScroll = (65) - viewportHeight;

            verticalScrollbar.FormsControl.Minimum = 0;
            verticalScrollbar.FormsControl.Maximum = maxScroll;
            verticalScrollbar.FormsControl.ViewportSize = viewportHeight;

            horizontalScrollbar.FormsControl.Minimum = 0;
            horizontalScrollbar.FormsControl.Maximum = maxScroll;
            horizontalScrollbar.FormsControl.ViewportSize = viewportHeight;

            var resourcePanel = mapEditorScreen.ResourcePanel;
            resourcePanelBoundariesX = (int)mapEditorScreen.EditorSection.GetAbsoluteWidth();
            var tilesetSprite = new SpriteRuntime
            {
                Name = "TilesetSprite",
                Texture = Tilesets[Globals.SelectedTileset],
                Width = Tilesets[Globals.SelectedTileset].Width,
                Height = Tilesets[Globals.SelectedTileset].Height,
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

            InitializeTileEvents(menuManager);
        }

        public static void LoadGumSpriteSheetResourcePanel(MenuManager menuManager)
        {
            AnimationEditorRuntime animationEditorScreen = (AnimationEditorRuntime)menuManager.GetCurrentScreen();

            var resourcePanel = animationEditorScreen.ResourcePanel;

            var tilesetSprite = new SpriteRuntime
            {
                Name = "SpriteSheetSprite",
                Texture = Spritesheets[Globals.SelectedSpritesheet],
                Width = Spritesheets[Globals.SelectedSpritesheet].Width,
                Height = Spritesheets[Globals.SelectedSpritesheet].Height,
                WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                X = 0,
                Y = 0
            };

            var selectionBox = new RectangleRuntime
            {
                Name = "SelectionBox",
                Width = 0,
                Height = 0,
                Color = Microsoft.Xna.Framework.Color.White, // no fill
                //OutlineColor = Microsoft.Xna.Framework.Color.Yellow,
                OutlineThickness = 2,
                XUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
                YUnits = Gum.Converters.GeneralUnitType.PixelsFromSmall,
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

        private static void InitializeTileEvents(MenuManager menuManager)
        {
            MapEditorRuntime mapEditorScreen = (MapEditorRuntime)menuManager.GetCurrentScreen();
            ContainerRuntime eventsContainer = mapEditorScreen.EventsContainer;

            Types.TileEvents[0].Name = "Block";
            Types.TileEvents[0].Moral = 0;
            Types.TileEvents[0].Type = (byte)TileEventsTypes.Block;
            Types.TileEvents[0].Data1 = 0;
            Types.TileEvents[0].Data2 = 0;
            Types.TileEvents[0].Data3 = 0;
            Types.TileEvents[0].mapAcronym = "B";
            Types.TileEvents[0].mapAcronymColor = Color.Red;

            Types.TileEvents[1].Name = "Npc";
            Types.TileEvents[1].Moral = 0;
            Types.TileEvents[1].Type = (byte)TileEventsTypes.Npc;
            Types.TileEvents[1].Data1 = 0;
            Types.TileEvents[1].Data2 = 0;
            Types.TileEvents[1].Data3 = 0;
            Types.TileEvents[1].mapAcronym = "Npc";
            Types.TileEvents[1].mapAcronymColor = Color.White;

            for (int i = 0; i < Types.TileEvents.Length; i++)
            {
                if (Types.TileEvents[i].Name is not null)
                {
                    int index = i;

                    ButtonStandardRuntime eventButton = new ButtonStandardRuntime
                    {
                        Name = Types.TileEvents[i].Name,
                        Width = 120,
                        Height = 60,                 
                        WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                        HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,

                    };

                    eventButton.Click += (_, _) =>
                    {
                        Color selectedButtonColor = eventButton.Background.Color;
                        selectedButtonColor.A = 180;
                        eventButton.Background.Color = selectedButtonColor;

                        Globals.SelectedEventIndex = index;

                        if (Types.TileEvents[index].Type == (byte)TileEventsTypes.Npc)
                        {
                            menuManager.openNpcSelection();
                        }
                    };

                    eventButton.TextInstance.Text = Types.TileEvents[i].Name;
                    eventsContainer.Children.Add(eventButton);
                }
            }

        }

        public static void UpdateTilesetPanel(SpriteRuntime tilesetToUpdate, Texture2D textureToUpdate)
        {
            tilesetToUpdate.Texture = textureToUpdate;
            tilesetToUpdate.Width = textureToUpdate.Width;
            tilesetToUpdate.Height = textureToUpdate.Height;
        }

        private static void LoadTilesets(ContentManager manager)
        {
            for (int i = 0; i < Tilesets.Length; i++)
            {
                Tilesets[i] = manager.Load<Texture2D>("Tilesets/" + i.ToString());
            }
        }
        private static void LoadSpritesheets(ContentManager manager)
        {
            for (int i = 0; i < Spritesheets.Length; i++)
            {
                Spritesheets[i] = manager.Load<Texture2D>("Spritesheets/" + i.ToString());
            }
        }

        private static void LoadCharacters(ContentManager manager)
        {
            for (int i = 0; i < Characters.Length; i++)
            {
                Characters[i] = manager.Load<Texture2D>("Characters/" + i.ToString());
            }
        }

        private static void LoadFonts(ContentManager manager)
        {
            font = manager.Load<SpriteFont>("Font");
        }

        public static void RenderGraphics(GraphicsDevice graphicsDevice)
        {
            if (Types.Maps[Globals.SelectedMap] != null && !Globals.isLoadingMap)
            {
                pixel = new Texture2D(graphicsDevice, 1, 1);
                pixel.SetData(new[] { Color.White });

                Game1._spriteBatch.Begin();
                DrawMapGrid();

                Game1._spriteBatch.End();
            }
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

            for (int layer = 0; layer < maxMapLayer; layer++)
            {
                for (int x = 0; x < Types.Maps[Globals.SelectedMap].MaxMapX; x++)
                {
                    for (int y = 0; y < Types.Maps[Globals.SelectedMap].MaxMapY; y++)
                    {
                        DrawTileGrid(x * 32, y * 32, x, y, layer);
                    }
                }
                if (Globals.SelectedEventIndex is null) continue;

                if (Types.Maps[Globals.SelectedMap].Layer.ElementAt(layer).MapNpc == null || Types.TileEvents[(int)Globals.SelectedEventIndex].Type != (byte)TileEventsTypes.Npc) continue;

                for (int x = 0; x < Types.Maps[Globals.SelectedMap].Layer.ElementAt(layer).MapNpc.Count; x++)
                {

                    var npc = Types.Maps[Globals.SelectedMap].Layer.ElementAt(layer).MapNpc.ElementAt(x);
                    if (npc != null)
                    {
                        DrawMapNpc(npc);
                    }
                }
            }
        }

        private static void DrawTileGrid(int mapX, int mapY, int x, int y, int layerNum)
        {
            Tile actualTile = Types.Maps[Globals.SelectedMap].Layer.ElementAt(layerNum).TileMatrix[x, y];
            int TilesetX = actualTile.TileX;
            int TilesetY = actualTile.TileY;

            Rectangle srcrec;
            int tileSize = 32;
            int thickness = 1;

            int MapX, MapY;
            MapX = (resourcePanelBoundariesX) + ConvertMapX(mapX);
            MapY = ConvertMapY(mapY);

            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY, tileSize, thickness), Color.White);
            // Left line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY, thickness, tileSize), Color.White);
            // Right line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX + tileSize - thickness, MapY, thickness, tileSize), Color.White);
            // Bottom line
            Game1._spriteBatch.Draw(pixel, new Rectangle(MapX, MapY + tileSize - thickness, tileSize, thickness), Color.White);

            if (!(TilesetX == 0 && TilesetY == 0))
            {
                srcrec = new Rectangle(TilesetX, TilesetY, 32, 32);
                Game1._spriteBatch.Draw(Tilesets[actualTile.TilesetNumber], new Vector2(MapX, MapY), srcrec, Color.White);
            }

            if (Globals.SelectedEventIndex is not null && Types.TileEvents[(int)Globals.SelectedEventIndex].Type != (byte)TileEventsTypes.Npc && actualTile.Type != 0)
            {
                DrawTileEventAcronym(MapX, MapY, Types.TileEvents[(int)Globals.SelectedEventIndex]);
            }
        }

        private static void DrawTileEventAcronym(int mapX, int mapY, TileEventStruct tileEvent)
        {
            int x = mapX + 5;
            int y = mapY + 5;

            Game1._spriteBatch.DrawString(font, tileEvent.mapAcronym, new Vector2(x, y), tileEvent.mapAcronymColor);
        }

        private static void DrawMapNpc(MapNpc mapNpc)
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft = 0;

            SpriteNum = mapNpc.Npc.Sprite;

            anim = 1;

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 3), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 3, Characters[SpriteNum].Height / 4);
            X = mapNpc.X * 32 - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = mapNpc.Y * 32;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawSprite(int sprite, int x2, int y2, Rectangle srcrec)
        {
            int X, Y;
            X = (resourcePanelBoundariesX) + ConvertMapX(x2);
            Y = ConvertMapY(y2);

            Game1._spriteBatch.Draw(Characters[sprite], new Vector2(X, Y), srcrec, Color.White);
        }
    }
}
