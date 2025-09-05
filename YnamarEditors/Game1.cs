using Gum.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using System.Diagnostics;
using System.Linq;
using YnamarEditors.Components;
using YnamarEditors.Screens;

namespace YnamarEditors;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch _spriteBatch;
    GumService Gum => GumService.Default;
    public static GumProjectSave gumProject;
    private MenuManager _menuManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        _graphics.PreferredBackBufferWidth = 1280;

        _graphics.PreferredBackBufferHeight = 720;

        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        Graphics.InitializeGraphics(Content);

        gumProject = Gum.Initialize(this, "GumUI/ynamarEditorsProject.gumx");
        _menuManager = new MenuManager(gumProject);

        EditorSelectorRuntime mainMenu = (EditorSelectorRuntime)_menuManager.LoadScreen("EditorSelector");   
        ButtonStandardRuntime startButton = mainMenu.ButtonStandardInstance;

        // Subscribe to click
        startButton.Click += (not, used) =>
        {
            // Switch to Map Editor screen
            _menuManager.LoadScreen("MapEditor");
        };

        //var screenRuntime = gumProject.Screens.First().ToGraphicalUiElement();
        //screenRuntime.AddToRoot();

        base.Initialize();

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
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
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Graphics.RenderGraphics(GraphicsDevice);
        Gum.Draw();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
