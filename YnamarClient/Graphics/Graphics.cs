using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RenderingLibrary.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YnamarClient.Database.Models;
using YnamarClient.Network;
using static System.Net.Mime.MediaTypeNames;

namespace YnamarClient.Graphics
{
    internal class Graphics
    {

        public static Texture2D[] Characters = new Texture2D[3];
        public static Texture2D[] Tilesets = new Texture2D[2];
        public static Texture2D[] Spritesheets = new Texture2D[Globals.MAX_SPRITE_SHEET];
        public static Texture2D[] Items = new Texture2D[1];
        private static SpriteFont font;

        public static Texture2D healthbarFull;
        public static Texture2D healthbar;

        public static List<ChatMessage> ChatMessages = new();

        public static void InitializeGraphics(ContentManager manager)
        {
            LoadFonts(manager);
            LoadCharacters(manager);
            LoadTilesets(manager);
            LoadSpritesheets(manager);
            LoadItems(manager);
            LoadGFX(manager);
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

        private static void LoadGFX(ContentManager manager)
        {
            healthbarFull = manager.Load<Texture2D>("GFX/Player/healthbar1");
            healthbar = manager.Load<Texture2D>("GFX/Player/healthbar2");
        }

        private static void LoadItems(ContentManager manager)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = manager.Load<Texture2D>("Items/item" + i.ToString());
            }
        }

        public static void RenderGraphics(GameTime gameTime, ChunkManager chunkManager)
        {
            Game1.spriteBatch.Begin();
            // DrawPlayerName();
            foreach (var chunk in chunkManager.GetVisibleChunks())
            {
                DrawChunk(chunk, gameTime);
            }
            // DrawMapGrid(gameTime);
            DrawPlayerHealthBar(Globals.playerIndex);
            DrawChat();

            Game1.spriteBatch.End();
        }
        private static void DrawPlayer(int index, GameTime gameTime)
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = 0;
            spriteLeft = 0;

            anim = 1;
            int attackSpeed = 1000;
            int animOffsetX = 0, animOffsetY = 0;

            switch (Types.Players[index].Dir)
            {
                case Constants.DIR_UP:
                    spriteLeft = 3;
                    animOffsetY = -32;
                    if (Types.Players[index].YOffset > 8)
                        anim = Types.Players[index].Steps;
                    break;
                case Constants.DIR_DOWN:
                    spriteLeft = 0;
                    animOffsetY = 32;
                    if (Types.Players[index].YOffset < -8)
                        anim = Types.Players[index].Steps;
                    break;
                case Constants.DIR_LEFT:
                    spriteLeft = 1;
                    animOffsetX = -32;
                    if (Types.Players[index].XOffset > 8)
                        anim = Types.Players[index].Steps;
                    break;
                case Constants.DIR_RIGHT:
                    spriteLeft = 2;
                    animOffsetX = 32;
                    if (Types.Players[index].XOffset < -8)
                        anim = Types.Players[index].Steps;
                    break;
            }


            if ((Types.Players[index].AttackCooldown + (attackSpeed / 2) > (int)gameTime.TotalGameTime.TotalMilliseconds) && Types.Players[index].Attacking) 
            {
                anim = 3;
            }

            if (Types.Players[index].AttackCooldown + attackSpeed < (int)gameTime.TotalGameTime.TotalMilliseconds)
            {
                Types.Players[index].Attacking = false;
                Types.Players[index].AttackCooldown = 0;
            }

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 4), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 4, Characters[SpriteNum].Height / 4);
            X = Types.Players[index].X * 32 + Types.Players[index].XOffset - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = Types.Players[index].Y * 32 + Types.Players[index].YOffset;

            if (Types.Players[index].WeaponAnim is not null)
            {
                Types.Players[index].WeaponAnim.Update(gameTime);

                if (Types.Players[index].WeaponAnim.IsPlaying)
                {
                    //Acabar aqui o srcrec
                    var frameRectangle = Types.Players[index].WeaponAnim.CurrentFrame.SourceRect;
                    Rectangle rectAnimation = new Rectangle(frameRectangle.X, frameRectangle.Y, frameRectangle.Width, frameRectangle.Height);
                    int animX = (X + animOffsetX) - Camera.X;
                    int animY = (Y + animOffsetY) - Camera.Y;

                    //var animX = ConvertMapX(X + animOffsetX);
                    //var animY = ConvertMapY(Y + animOffsetY);

                    DrawAttackAnimation(animX, animY, Types.Players[index].WeaponAnim.CurrentTexture, rectAnimation);
                }

            }

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawPlayerName(int index)
        {

            int xoffset = Types.Players[index].X * 32 + Types.Players[index].XOffset;
            int yoffset = Types.Players[index].Y * 32 + Types.Players[index].YOffset;
            double logPlayerNameLength = Math.Log(Types.Players[index].Name.Length, 10);
            int lengthOffset = 0;//Convert.ToInt32(Math.Round(logPlayerNameLength)) * 3;
            int x = (xoffset - Camera.X) - 6 - lengthOffset;
            int y = (yoffset - Camera.Y) - 32;

            //int x = ConvertMapX(xoffset) - 6 - lengthOffset;
            //int y = ConvertMapY(yoffset) - 20;

            Game1.spriteBatch.DrawString(font, Types.Players[index].Name, new Vector2(x, y), Color.Blue);
        }

        private static void DrawPlayerHealthBar(int index)
        {
            int healthBarWidht = Types.Players[index].MaxHP == 0 ? 0 : (Types.Players[index].HP * healthbar.Width) / Types.Players[index].MaxHP;
            Rectangle rectanglHealthRight = new Rectangle(0, 0, healthBarWidht, healthbar.Height);
            Rectangle rectangleForBar = new Rectangle(0, 0, healthbarFull.Width, healthbarFull.Height);

            var originRight = new Vector2(rectanglHealthRight.Left, rectanglHealthRight.Top);

            var originBar = new Vector2(rectangleForBar.Left, rectangleForBar.Top);

            Game1.spriteBatch.Draw(healthbarFull, new Vector2(0,0), rectangleForBar, Color.White, 0.0f, originBar, 0.5f, SpriteEffects.None, 0.0f);
            Game1.spriteBatch.Draw(healthbar, new Vector2(0,0), rectanglHealthRight, Color.White, 0.0f, originRight, 0.5f, SpriteEffects.None, 0.0f);
        }

        private static void DrawNpcHealthBar(MapNpc mapNpc)
        {
            int healthBarWidht = mapNpc.Npc.MaxHp == 0 ? 0 : (mapNpc.Hp * 72) / mapNpc.Npc.MaxHp;
            Rectangle rectanglHealthRight = new Rectangle(0, 0, healthBarWidht, 12);
            Rectangle rectangleForBar = new Rectangle(0, 0, 72, 12);

            int xoffset = mapNpc.X * 32 + mapNpc.XOffset;
            int yoffset = mapNpc.Y * 32 + mapNpc.YOffset;
            int x = (xoffset - Camera.X);
            int y = (yoffset - Camera.Y) + 40;

            //int x = ConvertMapX(xoffset);
            //int y = ConvertMapY(yoffset) + 40;

            var originRight = new Vector2(rectanglHealthRight.Left, rectanglHealthRight.Top);

            var originBar = new Vector2(rectangleForBar.Left, rectangleForBar.Top);

            Game1.spriteBatch.Draw(healthbarFull, new Vector2(x, y), rectangleForBar, Color.White, 0.0f, originBar, 0.5f, SpriteEffects.None, 0.0f);
            Game1.spriteBatch.Draw(healthbar, new Vector2(x, y), rectanglHealthRight, Color.White, 0.0f, originRight, 0.5f, SpriteEffects.None, 0.0f);
        }

        private static void DrawNpcName(MapNpc mapNpc)
        {

            int xoffset = mapNpc.X * 32 + mapNpc.XOffset;
            int yoffset = mapNpc.Y * 32 + mapNpc.YOffset;
            // double logPlayerNameLength = Math.Log(Types.Player[index].Name.Length, 10);
            int lengthOffset = 0;//Convert.ToInt32(Math.Round(logPlayerNameLength)) * 3;
            int x = (xoffset - Camera.X) - 6 - lengthOffset;
            int y = (yoffset - Camera.Y) - 20;

            //int x = ConvertMapX(xoffset) - 6 - lengthOffset;
            //int y = ConvertMapY(yoffset) - 20;

            Game1.spriteBatch.DrawString(font, mapNpc.Npc.Name, new Vector2(x, y), Color.Blue);
        }

        private static void DrawChat()
        {
            int baseX = 200;
            int baseY = 20;

            for (int i = 0; i < ChatMessages.Count; i++)
            {
                Game1.spriteBatch.DrawString(
                    font,
                    ChatMessages[i].Text,
                    new Vector2(baseX + 1, (baseY + i * 20) + 1),
                    Color.Black,
                    0f,
                    Vector2.Zero,
                    2f,
                    SpriteEffects.None,
                    0f);

                Game1.spriteBatch.DrawString(
                    font,
                    ChatMessages[i].Text,
                    new Vector2(baseX, baseY + i * 20),
                    ChatMessages[i].Color,
                    0f,                 
                    Vector2.Zero,      
                    2f,
                    SpriteEffects.None,
                    0f
                );
            }
        }

        public static void DrawAttackAnimation(int x, int y, Texture2D sprite, Rectangle srcrec)
        {
            Game1.spriteBatch.Draw(sprite, new Vector2(x, y), srcrec, Color.White);
        }

        private static void DrawSprite(int sprite, int x2, int y2, Rectangle srcrec)
        {
            int X, Y;

            X = x2 - Camera.X;
            Y = y2 - Camera.Y;

            Game1.spriteBatch.Draw(Characters[sprite], new Vector2(X, Y), srcrec, Color.White);
        }

        public static void DrawChunk(Chunk chunk, GameTime gameTime)
        {
            foreach (var layerEntry in chunk.Layers)
            {
                int layerId = layerEntry.Key;

                ushort[] tiles = layerEntry.Value;

                for (int x = 0; x < chunk.Size; x++)
                {
                    for (int y = 0; y < chunk.Size; y++)
                    {
                        int index = y * chunk.Size + x;
                        ushort tileId = tiles[index];

                        if (tileId == 0)
                            continue;

                        if (!Globals.tileDefinitionsLookup.TryGetValue(tileId, out var def))
                            continue;

                        int worldX = chunk.X * chunk.Size + x;
                        int worldY = chunk.Y * chunk.Size + y;

                        DrawTile(worldX, worldY, def);
                    }
                }

                for (int i = 0; i < Constants.MAX_PLAYERS; i++)
                {
                    if (ClientTCP.IsPlaying(i))
                    {
                        if ((Types.Players[i].Map == Types.Players[Globals.playerIndex].Map) && layerId == 0)
                        {
                            DrawPlayerName(i);
                            DrawPlayer(i, gameTime);
                        }
                    }
                }
            }
        }
        private static void DrawMapGrid(GameTime gameTime)
        {
            int maxMapLayer = Globals.PlayerMap.Layer.Length;

            int mapWidth = Globals.PlayerMap.MaxMapX;
            int mapHeight = Globals.PlayerMap.MaxMapY;

            int screenWidth = GraphicsDeviceManager.DefaultBackBufferWidth;
            int screenHeight = GraphicsDeviceManager.DefaultBackBufferHeight;

            int startX = Math.Max(0, Camera.X / Constants.TILE_SIZE);
            int startY = Math.Max(0, Camera.Y / Constants.TILE_SIZE);

            int endX = Math.Min(mapWidth, startX + (screenWidth / Constants.TILE_SIZE) + 2);
            int endY = Math.Min(mapHeight, startY + (screenHeight / Constants.TILE_SIZE) + 2);

            for (int layer = 0; layer < maxMapLayer; layer++)
            {
                var layerData = Globals.PlayerMap.Layer[layer];
                if (layerData.Tile == null) continue;

                for (int x = startX; x < endX; x++)
                {
                    for (int y = startY; y < endY; y++)
                    {
                       // DrawTile(x, y, layer);
                    }
                }


                for (int i = 0; i < Constants.MAX_PLAYERS; i++)
                {
                    if (ClientTCP.IsPlaying(i))
                    {
                        if ((Types.Players[i].Map == Types.Players[Globals.playerIndex].Map) && layer == 0)
                        {
                            DrawPlayerName(i);
                            DrawPlayer(i, gameTime);
                        }
                    }
                }

                if (Globals.PlayerMap.Layer[layer].MapNpc == null) continue;

                for (int x = 0; x < Globals.PlayerMap.Layer[layer].MapNpc.Length; x++)
                {

                    var npc = Globals.PlayerMap.Layer[layer].MapNpc[x];
                    if (npc != null)
                    {
                        if (npc.RespawnWait > 0)
                            continue;

                        DrawMapNpc(npc);
                        DrawNpcName(npc);
                        DrawNpcHealthBar(npc);
                    }
                }
            }
        }

        private static void DrawMapNpc(MapNpc mapNpc)
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = mapNpc.Npc.Sprite;
            spriteLeft = 0;

            anim = 1;

           switch (mapNpc.Dir)
            {
                case Constants.DIR_UP:
                    spriteLeft = 3;
                    if (mapNpc.YOffset > 8)
                        anim = mapNpc.Steps;
                    break;
                case Constants.DIR_DOWN:
                    spriteLeft = 0;
                    if (mapNpc.YOffset < -8)
                        anim = mapNpc.Steps;
                    break;
                case Constants.DIR_LEFT:
                    spriteLeft = 1;
                    if (mapNpc.XOffset > 8)
                        anim = mapNpc.Steps;
                    break;
                case Constants.DIR_RIGHT:
                    spriteLeft = 2;
                    if (mapNpc.XOffset < -8)
                        anim = mapNpc.Steps;
                    break;
            }

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 3), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 3, Characters[SpriteNum].Height / 4);
            X = mapNpc.X * 32 + mapNpc.XOffset - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = mapNpc.Y * 32 + mapNpc.YOffset;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawTile(int x, int y, TileDefinition def)
        {
            if (def.TileX == 0 && def.TileY == 0)
                return;

            //var tile = Globals.PlayerMap.Layer[layerNum].Tile[x, y];

            //if (tile.TileX == 0 && tile.TileY == 0)
            //    return;

            int worldX = x * Constants.TILE_SIZE;
            int worldY = y * Constants.TILE_SIZE;

            int screenX = worldX - Camera.X;
            int screenY = worldY - Camera.Y;

            Rectangle srcrec;

            srcrec = new Rectangle(def.TileX, def.TileY, Constants.TILE_SIZE, Constants.TILE_SIZE);
            Game1.spriteBatch.Draw(Tilesets[def.Tileset], new Vector2(screenX, screenY), srcrec, Color.White);
        }
    }
}
