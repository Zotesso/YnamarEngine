using ENet;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Threading;
using YnamarClient.Network;

namespace YnamarClient
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch spriteBatch;
        ClientUDP clientUdp;
        ClientHandleData clientDataHandle;

        private static Thread udpThread;

        float WalkTimer;
        public static new int Tick;
        public static int ElapsedTime;
        public static int FrameTime;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            clientUdp = new ClientUDP();
            clientDataHandle = new ClientHandleData();
            clientDataHandle.InitializeMessages();

            udpThread = new Thread(new ThreadStart(clientUdp.ConnectToServer));
            udpThread.Start();

            Types.Player[0].X = 0;
            Types.Player[0].Y = 0;
            Types.Player[0].Dir = 0;

            Types.Player[0].Name = "teste";
            Types.Player[0].Map = 0;
            Types.Player[0].Level = 1;
            Types.Player[0].EXP = 1;
            Types.Player[0].Access = 0;

            Graphics.InitializeGraphics(Content);
           
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Tick = (int)gameTime.TotalGameTime.TotalMilliseconds;
            ElapsedTime = (Tick - FrameTime);
            FrameTime = Tick;

            if (WalkTimer < Tick)
            {
              GameLogic.ProcessMovement(Globals.playerIndex);
              WalkTimer = Tick + 30;
            }

            CheckKeys();
            GameLogic.CheckMovement();
            Graphics.RenderGraphics();
            
            // TODO: Add your drawing code here
            Graphics.RenderGraphics();
            base.Draw(gameTime);
        }

        private void CheckKeys()
        {
            Globals.DirUp = Keyboard.GetState().IsKeyDown(Keys.Up);
            Globals.DirDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            Globals.DirRight = Keyboard.GetState().IsKeyDown(Keys.Right);
            Globals.DirLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
        }
    }
}
