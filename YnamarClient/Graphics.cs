using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using YnamarClient.Network;

namespace YnamarClient
{
    internal class Graphics
    {

        public static Texture2D[] Characters = new Texture2D[2];
        public static Texture2D[] Tilesets = new Texture2D[1];
        private static SpriteFont font;
        public static void InitializeGraphics(ContentManager manager)
        {
            LoadFonts(manager);
            LoadCharacters(manager);
            LoadTilesets(manager);
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

        public static void RenderGraphics()
        {
            Game1.spriteBatch.Begin();
            // DrawPlayerName();
            DrawMapGrid();

            for (int i = 0; i < Constants.MAX_PLAYERS; i++)
            {
                if (ClientTCP.IsPlaying(i))
                {
                    if (Types.Player[i].Map == Types.Player[Globals.playerIndex].Map)
                    {
                        DrawPlayerName(i);
                        DrawPlayer(i);
                    }
                }
            }

            Game1.spriteBatch.End();
        }
        private static void DrawPlayer(int index)
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = 0;
            spriteLeft = 0;

            anim = 1;

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

            Game1.spriteBatch.DrawString(font, Types.Player[index].Name, new Vector2(x, y), Color.Black);
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
            byte maxMapLayer = 3;

            for (byte layer = 0; layer < maxMapLayer; layer++)
            {
                for (int x = 0; x < Constants.MAX_MAP_X; x++)
                {
                    for (int y = 0; y < Constants.MAX_MAP_Y; y++)
                    {
                        DrawTile(x * 32, y * 32);
                    }
                }
            }
        }

        private static void DrawTile(int x, int y)
        {
            Rectangle srcrec;
            int tilesetnum = 0;

            int X, Y;
            X = ConvertMapX(x);
            Y = ConvertMapY(y);

            srcrec = new Rectangle(0,0,32,32);
            Game1.spriteBatch.Draw(Tilesets[tilesetnum], new Vector2(X, Y), srcrec, Color.White);
        }
    }
}
