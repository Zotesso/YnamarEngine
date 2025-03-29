using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace YnamarClient
{
    internal class Graphics
    {

        public static Texture2D[] Characters = new Texture2D[2];
        private static SpriteFont font;
        public static void InitializeGraphics(ContentManager manager)
        {
            //LoadFonts(manager);
            LoadCharacters(manager);
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

        public static void RenderGraphics()
        {
            Game1.spriteBatch.Begin();
            // DrawPlayerName();
            DrawPlayer();

            DrawPlayer(0);
            Game1.spriteBatch.End();
        }
        private static void DrawPlayer(int index)
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = 1;
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
            Y = Types.Player[index].Y * 64 + Types.Player[index].YOffset;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }
        private static void DrawPlayer()
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = 0;
            spriteLeft = 0;

            anim = 1;

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 4), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 4, Characters[SpriteNum].Height / 4);
            X = 3 * 32 + 1 - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = 3 * 47 + 1;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawPlayerName()
        {

            int xoffset = 1 * 32 + 1;
            int yoffset = 1 * 32 + 1;
            int x = 0;
            int y = 0;

            Vector2 textMiddlePoint = font.MeasureString("") / 2;
            Game1.spriteBatch.DrawString(font, "", new Vector2(x, y), Color.Yellow, 0, textMiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
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
    }
}
