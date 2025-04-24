using ENet;
using Gum.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Myra;
using Myra.Graphics2D.UI;
using System;
using System.Threading;
using YnamarClient.GUI;
using YnamarClient.Network;

namespace YnamarClient
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        GumService Gum => GumService.Default;

        public static SpriteBatch spriteBatch;

        ClientUDP clientUdp;
        ClientHandleData clientDataHandle;
        private static Thread udpThread;

        ClientTCP ctcp;
        ClientHandleDataTCP clientDataHandleTCP;
        private static Thread tcpThread;

        InterfaceGUI IGUI = new InterfaceGUI();
        public static Desktop desktop;
        public static GumProjectSave gumProject;
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

            ctcp = new ClientTCP();
            clientDataHandleTCP = new ClientHandleDataTCP();
            clientDataHandleTCP.InitializeMessages();

            tcpThread = new Thread(new ThreadStart(ctcp.ConnectToServer));
            tcpThread.Start();

            gumProject = Gum.Initialize(this, "GumUI/gumproject.gumx");
            Graphics.InitializeGraphics(Content);

            var rectangle = new ColoredRectangleRuntime();
            rectangle.Width = 100;
            rectangle.Height = 100;
            rectangle.Color = Color.White;
            rectangle.AddToRoot();

            base.Initialize();

            IGUI.InitializeGUI(this, desktop);
            MenuManager.ChangeMenu(MenuManager.Menu.Login, desktop);
        }

        protected override void LoadContent()
        {
            MyraEnvironment.Game = this;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            desktop = new Desktop();

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Gum.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            if (Globals.InGame)
            {
                Tick = (int)gameTime.TotalGameTime.TotalMilliseconds;
                ElapsedTime = (Tick - FrameTime);
                FrameTime = Tick;

                if (WalkTimer < Tick)
                {
                    GameLogic.ProcessMovement(Globals.playerIndex);
                    GameLogic.ProcessNpcMovement(Globals.PlayerMap.Layer[0].MapNpc[0]);
                    WalkTimer = Tick + 30;
                }

                CheckKeys();
                GameLogic.CheckMovement();
                Graphics.RenderGraphics();
            }

            desktop.Render();

            Gum.Draw();
            base.Draw(gameTime);
        }

        private void CheckKeys()
        {
            Globals.DirUp = Keyboard.GetState().IsKeyDown(Keys.Up);
            Globals.DirDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            Globals.DirRight = Keyboard.GetState().IsKeyDown(Keys.Right);
            Globals.DirLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
        }

        public static void ClearScreenGum()
        {
            //Gum.Root.Children.Clear();
        }
    }
}
