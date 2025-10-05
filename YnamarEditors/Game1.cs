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
using static YnamarEditors.Types;

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
        gumProject.DefaultCanvasWidth = 1280;
        gumProject.DefaultCanvasHeight = 720;
        _menuManager = new MenuManager(gumProject);
        _menuManager.LoadScreen("EditorSelector");

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
                    int tileX = (int)localX / 32;
                    int tileY = (int)localY / 32;
                    int columns = Graphics.Tilesets[Globals.SelectedTileset].Width / 32;

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

                    float mapLocalX = screenX -( ((MapEditorRuntime)currentScreen).EditorSection.GetAbsoluteWidth());
                        
                    int mapTileX = ((int)mapLocalX / 32) + (int)Graphics.horizontalScrollbar.FormsControl.Value;
                    int mapTileY = ((int)screenY / 32) + (int)Graphics.verticalScrollbar.FormsControl.Value;

                    if (mapTileX >= 0 && mapTileY >= 0 && mapTileX < 50 && mapTileY < 50)
                    {
                        Tile selectedTile = Types.Maps[Globals.SelectedMap].Layer.ElementAt(Globals.SelectedLayer).TileMatrix[mapTileX, mapTileY];
                        
                        if (Globals.SelectedEventIndex is not null)
                        {
                            TileEventStruct selectedEvent = Types.TileEvents[(int)Globals.SelectedEventIndex];
                            selectedTile.Type = selectedEvent.Type;
                            selectedTile.Moral = selectedEvent.Moral;
                            selectedTile.Data1 = selectedEvent.Data1;
                            selectedTile.Data2 = selectedEvent.Data2;
                            selectedTile.Data3 = selectedEvent.Data3;
                            return;
                        }

                        selectedTile.TileY = (int)selectionBox.Y;
                        selectedTile.TileX = (int)selectionBox.X;
                        selectedTile.TilesetNumber = Globals.SelectedTileset;
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
