using ENet;
using Gum.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using Myra;
using Myra.Graphics2D.UI;
using RenderingLibrary;
using System;
using System.Threading;
using YnamarClient.Graphics;
using YnamarClient.GUI;
using YnamarClient.Network;
using static YnamarClient.Types;
using Camera = YnamarClient.Graphics.Camera;

namespace YnamarClient
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        GumService Gum => GumService.Default;

        public static SpriteBatch spriteBatch;

        ClientHandleData clientDataHandle;
        public static ChunkManager chunkManager = new ChunkManager();
        private static Thread udpThread;

        ClientHandleDataTCP clientDataHandleTCP;
        private static Thread tcpThread;
        private MenuManager _menuManager;

        public static Desktop desktop;
        public static GumProjectSave gumProject;
        float WalkTimer;
        public static new int Tick;
        public static int ElapsedTime;
        public static int FrameTime;
        private KeyboardState _previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            clientDataHandle = new ClientHandleData();
            clientDataHandle.InitializeMessages();

            udpThread = new Thread(new ThreadStart(NetworkManager.Client.ConnectToServer));
            udpThread.Start();

            clientDataHandleTCP = new ClientHandleDataTCP();
            clientDataHandleTCP.InitializeMessages();

            tcpThread = new Thread(new ThreadStart(NetworkManager.ClientTcp.ConnectToServer));
            tcpThread.Start();

            gumProject = Gum.Initialize(this, "GumUI/gumproject.gumx");
            _menuManager = new MenuManager(gumProject);
            Graphics.Graphics.InitializeGraphics(Content);

            var rectangle = new ColoredRectangleRuntime();
            rectangle.Width = 100;
            rectangle.Height = 100;
            rectangle.Color = Color.White;
            rectangle.AddToRoot();

            base.Initialize();

            MenuManager.IGUI.InitializeGUI(this, desktop);
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
            var currentKeyboardState = Keyboard.GetState();
            bool isComboPressed =
                currentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                currentKeyboardState.IsKeyDown(Keys.E);

            bool wasComboPressed =
                _previousKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                _previousKeyboardState.IsKeyDown(Keys.E);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Globals.InGame)
            {
                if (isComboPressed && !wasComboPressed)
                {
                    GameLogic.ToggleInventory(_menuManager);
                }
            }

            _previousKeyboardState = currentKeyboardState;

            UpdateChat(gameTime);

                // TODO: Add your update logic here
                Gum.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (Globals.InGame)
            {
                chunkManager.Update(Types.Players[Globals.playerIndex].X, Types.Players[Globals.playerIndex].Y);
                Tick = (int)gameTime.TotalGameTime.TotalMilliseconds;
                ElapsedTime = (Tick - FrameTime);
                FrameTime = Tick;

                if (WalkTimer < Tick)
                {
                    foreach (Types.Player player in Types.Players.Values)
                    {
                        if (player.Moving == 0)
                            continue;

                        GameLogic.ProcessMovement(player);
                    }

                    GameLogic.ProcessMapNpcsMovement();
                    WalkTimer = Tick + 30;
                }


                CheckKeys();
                GameLogic.CheckMovement();
                GameLogic.CheckAttack(Tick);
                Camera.UpdateCamera();
                Graphics.Graphics.RenderGraphics(gameTime, chunkManager);
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
            Globals.ZKeyPressed = Keyboard.GetState().IsKeyDown(Keys.Z);
        }
        public static void UpdateChat(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for (int i = Graphics.Graphics.ChatMessages.Count - 1; i >= 0; i--)
            {
                Graphics.Graphics.ChatMessages[i].TimeLeft -= delta;

                if (Graphics.Graphics.ChatMessages[i].TimeLeft <= 0)
                    Graphics.Graphics.ChatMessages.RemoveAt(i);
            }
        }

        public static void ClearScreenGum()
        {
            //Gum.Root.Children.Clear();
        }
    }
}
