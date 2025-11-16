using Gum.DataTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using RenderingLibrary;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using YnamarEditors.Commands;
using YnamarEditors.Components;
using YnamarEditors.Models;
using YnamarEditors.Screens;
using YnamarEditors.Services;
using YnamarEditors.Services.MapEditor;
using static YnamarEditors.Types;

namespace YnamarEditors;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    public static SpriteBatch _spriteBatch;
    GumService Gum => GumService.Default;
    public static GumProjectSave gumProject;
    private MenuManager _menuManager;
    private CommandService _commandService;
    private ResourcePanelService _resourcePanelService;

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
        gumProject.DefaultCanvasWidth = 1280;
        gumProject.DefaultCanvasHeight = 720;
        _menuManager = new MenuManager(gumProject);
        _menuManager.LoadScreen("EditorSelector");
        _commandService = new CommandService();
        _resourcePanelService = new ResourcePanelService();

        base.Initialize();

    }

    protected override async void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        await MapEditorService.GetMap();

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.Z))
        {
            _commandService.UndoCommand();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && Keyboard.GetState().IsKeyDown(Keys.Y))
        {
            _commandService.RedoCommand();
        }

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
                    localX < Graphics.Tilesets[Globals.SelectedTileset].Width &&
                    localY < Graphics.Tilesets[Globals.SelectedTileset].Height)
                {
                    bool shift = Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift);

                    if (!shift)
                    {
                        _resourcePanelService.SelectedTileOnResourcePanel((MapEditorRuntime)currentScreen, (int)localX, (int)localY);
                    } 
                    else
                    {
                        _resourcePanelService.SelectedMultipleTilesOnResourcePanel((MapEditorRuntime)currentScreen, (int)localX, (int)localY);
                    }
                }
                else if (
                    (screenX < (_graphics.PreferredBackBufferWidth - ((MapEditorRuntime)currentScreen).VerticalScrollbarSection.GetAbsoluteWidth() - 15))
                    && (screenY < (_graphics.PreferredBackBufferHeight - ((MapEditorRuntime)currentScreen).HorizontalScrollbarSection.GetAbsoluteHeight() - 15))
                    )
                {
                    //RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
                    List<System.Drawing.Point> selectedTiles = _resourcePanelService.GetSelectedTiles();
                    float mapLocalX = screenX -( ((MapEditorRuntime)currentScreen).EditorSection.GetAbsoluteWidth());
                        
                    int mapTileX = ((int)mapLocalX / 32) + (int)Graphics.horizontalScrollbar.FormsControl.Value;
                    int mapTileY = ((int)screenY / 32) + (int)Graphics.verticalScrollbar.FormsControl.Value;

                    if (mapTileX >= 0 && mapTileY >= 0 && mapTileX < 50 && mapTileY < 50)
                    {
                        Tile selectedTile = Types.Maps[Globals.SelectedMap].Layer.ElementAt(Globals.SelectedLayer).TileMatrix[mapTileX, mapTileY];
                        
                        if (Globals.SelectedEventIndex is not null)
                        {
                            TileEventStruct selectedEvent = Types.TileEvents[(int)Globals.SelectedEventIndex];
                            _commandService.ExecuteCommand(new MapTileEventClick(selectedTile, selectedEvent));
                        }

                        if (selectedTiles.Count > 0)
                        {
                            if (selectedTiles.Count > 1)
                            {
                                int selectedTileset = Globals.SelectedTileset;
                                int selectedMap = Globals.SelectedMap;
                                int selectedLayer = Globals.SelectedLayer;
                                _commandService.ExecuteCommand(new MapMultipleTilesClick(mapTileX, mapTileY, selectedTiles, selectedTileset, selectedMap, selectedLayer));
                            }
                            else
                            {
                                int selectedTileset = Globals.SelectedTileset;
                                _commandService.ExecuteCommand(new MapTileClick(selectedTile, selectedTiles.ElementAt(0).Y, selectedTiles.ElementAt(0).X, selectedTileset));
                            }
                        }
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
