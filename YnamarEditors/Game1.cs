using Gum.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary;
using System.Diagnostics;
using System.Linq;
using YnamarEditors.Components;
using YnamarEditors.Models;
using YnamarEditors.Screens;
using YnamarEditors.Services;

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
        int mapIndex = 0;
        int maxLayers = 2;
        int maxMapX = 50;
        int maxMapY = 50;

        if (Types.Maps[mapIndex] == null)
        {
            Types.Maps[mapIndex] = new Map
            {
                Id = mapIndex,
                Name = $"Map {mapIndex}",
                MaxMapX = maxMapX,
                MaxMapY = maxMapY
            };
        }


        for (int l = 0; l < maxLayers; l++)
        {
            Types.Maps[mapIndex].Layer.Add(new MapLayer
            {
                MapId = mapIndex,
                LayerLevel = (byte)l,
                TileMatrix = new Tile[maxMapX, maxMapY],
            });

            // optional: initialize each tile’s fields
            for (int x = 0; x < maxMapX; x++)
            {
                for (int y = 0; y < maxMapY; y++)
                {

                    Tile tile = new Tile { 
                        TilesetNumber = 0,
                        Type = 0,
                        Moral = 0,
                        Data1 = 0,
                        Data2 = 0,
                        Data3 = 0,
                        X = x,
                        Y = y,
                        TileX = 0,
                        TileY = 0,
                    };

                    Types.Maps[mapIndex].Layer.ElementAt(l).Tile.Add(tile);
                    Types.Maps[mapIndex].Layer.ElementAt(l).TileMatrix[x, y] = tile;
                }
            }

        }
        Graphics.InitializeGraphics(Content);

        gumProject = Gum.Initialize(this, "GumUI/ynamarEditorsProject.gumx");
        gumProject.DefaultCanvasWidth = 1280;
        gumProject.DefaultCanvasHeight = 720;
        _menuManager = new MenuManager(gumProject);

        EditorSelectorRuntime mainMenu = (EditorSelectorRuntime)_menuManager.LoadScreen("EditorSelector");   
        ButtonStandardRuntime startButton = mainMenu.ButtonStandardInstance;

        // Subscribe to click
        startButton.Click += (not, used) =>
        {
            // Switch to Map Editor screen
            _menuManager.LoadScreen("MapEditor");
            ((MapEditorRuntime)_menuManager.GetCurrentScreen()).SaveMapButton.Click += (_, _) =>
            {
                MapEditorService.SaveMap();
            };
            Graphics.LoadGumTilesetResourcePanel(_menuManager);
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

        var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();

        if (mouse.LeftButton == ButtonState.Pressed)
        {
            var currentScreen = _menuManager.GetCurrentScreen();

            if (currentScreen.Name == "MapEditor")
            {
                var contentPanel = ((MapEditorRuntime)currentScreen).ResourcePanel.InnerPanelInstance;
                float screenX = mouse.X;
                float screenY = mouse.Y;

                // 2. Convert to local coordinates inside the ScrollViewer’s content
                float localX = screenX - (float)contentPanel.GetAbsoluteX();
                float localY = screenY - (float)contentPanel.GetAbsoluteY();

                if (localX >= 0 && localY >= 0 &&
                    localX < Graphics.Tilesets[0].Width &&
                    localY < Graphics.Tilesets[0].Height)
                {
                    int tileX = (int)localX / 32;
                    int tileY = (int)localY / 32;
                    int columns = Graphics.Tilesets[0].Width / 32;

                    //CurrentSelectedTileIndex = tileY * columns + tileX;
                    RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
                    // Move selection box
                    selectionBox.X = tileX * 32;
                    selectionBox.Y = tileY * 32;
                    selectionBox.Z = 3;
                    selectionBox.Visible = true;
                }
                else if (
                    (screenX < (_graphics.PreferredBackBufferWidth - ((MapEditorRuntime)currentScreen).VerticalScrollbarSection.GetAbsoluteWidth() - 15))
                    && (screenY < (_graphics.PreferredBackBufferHeight - ((MapEditorRuntime)currentScreen).HorizontalScrollbarSection.GetAbsoluteHeight() - 15))
                    )
                {
                    RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");

                    float mapLocalX = screenX -( ((MapEditorRuntime)currentScreen).ResourcePanel.Width + 30);
                        
                    int mapTileX = ((int)mapLocalX / 32) + (int)Graphics.horizontalScrollbar.FormsControl.Value;
                    int mapTileY = ((int)screenY / 32) + (int)Graphics.verticalScrollbar.FormsControl.Value;

                    if (mapTileX >= 0 && mapTileY >= 0 && mapTileX < 50 && mapTileY < 50)
                    {
                        // SetTile Tileset
                        Types.Maps[0].Layer.ElementAt(0).TileMatrix[mapTileX, mapTileY].TileY = (int)selectionBox.Y;
                        Types.Maps[0].Layer.ElementAt(0).TileMatrix[mapTileX, mapTileY].TileX = (int)selectionBox.X;

                    }

                }
            }

        }
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
