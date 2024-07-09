using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;

namespace YnamarClient
{
    internal class Graphics
    {

        public static Texture2D[] Characters = new Texture2D[3];
        private static SpriteFont font;
        public static void InitializeGraphics(ContentManager manager)
        {
            LoadFonts(manager);
            LoadCharacters(manager);
        }

        private static void LoadCharacters(ContentManager manager)
        {
            for (int i = 1; i < Characters.Length; i++)
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
                        DrawPlayerName();
                        DrawPlayer();
            Game1.spriteBatch.End();
        }

        private static void DrawPlayer()
        {
            byte anim;
            int X, Y;
            Rectangle srcrec;
            int SpriteNum;
            int spriteLeft;

            SpriteNum = 1;
            spriteLeft = 0;

            anim = 1;

            srcrec = new Rectangle((anim) * (Characters[SpriteNum].Width / 4), spriteLeft * (Characters[SpriteNum].Height / 4), Characters[SpriteNum].Width / 4, Characters[SpriteNum].Height / 4);
            X = 1 * 32 + 1 - ((Characters[SpriteNum].Width / 4 - 32) / 2);
            Y = 1 * 32 + 1;

            DrawSprite(SpriteNum, X, Y, srcrec);
        }

        private static void DrawPlayerName()
        {

            int xoffset = 1 * 32 + 1;
            int yoffset = 1 * 32 + 1;

            int x = 0;
            int y = 0;

            Game1.spriteBatch.DrawString(font, "TESTE", new Vector2(x, y), Color.Yellow);
        }

        private static void DrawSprite(int sprite, int x2, int y2, Rectangle srcrec)
        {
            int X, Y;
            X = 0;
            Y = 0;

            Game1.spriteBatch.Draw(Characters[sprite], new Vector2(X, Y), srcrec, Color.White);
        }
    }
}
