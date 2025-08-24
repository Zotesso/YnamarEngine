using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using YnamarClient.Network;
using YnamarClient.Database.Models;

namespace YnamarClient
{
    internal class Graphics
    {

        public static Texture2D[] Characters = new Texture2D[3];
        public static Texture2D[] Tilesets = new Texture2D[1];
        private static SpriteFont font;

        public static Texture2D healthbarFull;
        public static Texture2D healthbar;

        public static void InitializeGraphics(ContentManager manager)
        {
            LoadFonts(manager);
            LoadCharacters(manager);
            LoadTilesets(manager);
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
        private static void LoadGFX(ContentManager manager)
        {
            healthbarFull = manager.Load<Texture2D>("GFX/Player/healthbar1");
            healthbar = manager.Load<Texture2D>("GFX/Player/healthbar2");
        }

        public static void RenderGraphics(GameTime gameTime)
        {
            Game1.spriteBatch.Begin();
            // DrawPlayerName();
            DrawMapGrid();
            DrawPlayerHealthBar(Globals.playerIndex);

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (ClientTCP.IsPlaying(i))
                {
                    if (Types.Player[i].Map == Types.Player[Globals.playerIndex].Map)
                    {
                        DrawPlayerName(i);
                        DrawPlayer(i, gameTime);
                    }
                }
            }

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

            switch (Types.Player[index].Dir)
            {
                case Constants.DIR_UP:
                    spriteLeft = 3;
                    if (Types.Player[index].YOffset > 8)
                        anim = Types.Player[index].Steps;
                    break;
                case Constants.DIR_DOWN:
                    spriteLeft = 0;
                    if (Types.Player[index].YOffset < -8)
                        anim = Types.Player[index].Steps;
                    break;
                case Constants.DIR_LEFT:
                    spriteLeft = 1;
                    if (Types.Player[index].XOffset > 8)
                        anim = Types.Player[index].Steps;
                    break;
                case Constants.DIR_RIGHT:
                    spriteLeft = 2;
                    if (Types.Player[index].XOffset < -8)
                        anim = Types.Player[index].Steps;
                    break;
            }

            if ((Types.Player[index].AttackCooldown + (attackSpeed / 2) > (int)gameTime.TotalGameTime.TotalMilliseconds) && Types.Player[index].Attacking) 
            {
                anim = 3;
            }

            if (Types.Player[index].AttackCooldown + attackSpeed < (int)gameTime.TotalGameTime.TotalMilliseconds)
            {
                Types.Player[index].Attacking = false;
                Types.Player[index].AttackCooldown = 0;
            }

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 4), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 4, Characters[SpriteNum].Height / 4);
            X = Types.Player[index].X * 32 + Types.Player[index].XOffset - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = Types.Player[index].Y * 47 + Types.Player[index].YOffset;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawPlayerName(int index)
        {

            int xoffset = Types.Player[index].X * 32 + Types.Player[index].XOffset;
            int yoffset = Types.Player[index].Y * 47 + Types.Player[index].YOffset;
            double logPlayerNameLength = Math.Log(Types.Player[index].Name.Length, 10);
            int lengthOffset = 0;//Convert.ToInt32(Math.Round(logPlayerNameLength)) * 3;
            int x = ConvertMapX(xoffset) - 6 - lengthOffset;
            int y = ConvertMapY(yoffset) - 20;

            Game1.spriteBatch.DrawString(font, Types.Player[index].Name, new Vector2(x, y), Color.Blue);
        }

        private static void DrawPlayerHealthBar(int index)
        {
            int healthBarWidht = Types.Player[index].MaxHP == 0 ? 0 : (Types.Player[index].HP * healthbar.Width) / Types.Player[index].MaxHP;
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
            int x = ConvertMapX(xoffset);
            int y = ConvertMapY(yoffset) + 40;

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
            int x = ConvertMapX(xoffset) - 6 - lengthOffset;
            int y = ConvertMapY(yoffset) - 20;

            Game1.spriteBatch.DrawString(font, mapNpc.Npc.Name, new Vector2(x, y), Color.Blue);
        }

        public static int ConvertMapX(int x)
        {
            int cameraLeft = 0;
            int tileViewLeft = 0;

            cameraLeft = Types.Player[Globals.playerIndex].X + Types.Player[Globals.playerIndex].XOffset;
            tileViewLeft = Types.Player[Globals.playerIndex].X - 5;

            return x - (tileViewLeft * 32) - cameraLeft;
        }

        public static int ConvertMapY(int y)
        {
            int cameraTop = 0;
            int tileViewTop = 0;

            cameraTop = Types.Player[Globals.playerIndex].Y + Types.Player[Globals.playerIndex].YOffset;
            tileViewTop = Types.Player[Globals.playerIndex].Y - 5;
            return y - (tileViewTop * 32) - cameraTop;
        }

        private static void DrawSprite(int sprite, int x2, int y2, Rectangle srcrec)
        {
            int X, Y;
            X = ConvertMapX(x2);
            Y = ConvertMapY(y2);

            Game1.spriteBatch.Draw(Characters[sprite], new Vector2(X, Y), srcrec, Color.White);
        }

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

                for (int x = 0; x < Globals.PlayerMap.Layer[layer].MapNpc.Length; x++)
                {

                    var npc = Globals.PlayerMap.Layer[layer].MapNpc[x];
                    if (npc.RespawnWait > 0)
                        continue;

                    DrawMapNpc(npc);
                    DrawNpcName(npc);
                    DrawNpcHealthBar(npc);
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
}
