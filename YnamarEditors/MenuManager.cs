using Gum.DataTypes;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RenderingLibrary.Graphics;
using MonoGameGum;
using YnamarEditors.Screens;
using YnamarEditors.Services;
using YnamarEditors.Components.Behavior;
using MonoGameGum.GueDeriving;

namespace YnamarEditors
{
    internal class MenuManager
    {
        private GumProjectSave _gumProject;
        private GraphicalUiElement _currentScreen;

        public MenuManager(GumProjectSave gumProject)
        {
            _gumProject = gumProject;
        }

        /// <summary>
        /// Loads a Gum screen by name and makes it active.
        /// </summary>
        public GraphicalUiElement LoadScreen(string screenName)
        {
            // Remove old screen if exists
            _currentScreen?.RemoveFromRoot();

            // Create new screen
            var screenRuntime = _gumProject.Screens
                .First(s => s.Name == screenName)
                .ToGraphicalUiElement();

            screenRuntime.WidthUnits = DimensionUnitType.PercentageOfParent;
            screenRuntime.HeightUnits = DimensionUnitType.PercentageOfParent;
            screenRuntime.Width = 100;
            screenRuntime.Height = 100;

            screenRuntime.AddToRoot();
            _currentScreen = screenRuntime;

            WireScreenEvents(screenName, screenRuntime);

            return _currentScreen;
        }

        private void WireScreenEvents(string screenName, GraphicalUiElement screenRuntime)
        {
            switch (screenName)
            {
                case "EditorSelector":
                    var selector = (EditorSelectorRuntime)screenRuntime;
                    selector.ButtonStandardInstance.Click += (_, __) =>
                    {
                        LoadScreen("MapEditor");
                    };
                    break;

                case "MapEditor":
                    var editor = (MapEditorRuntime)screenRuntime;
                    editor.TextLayer.Text = $"Layer: {Globals.SelectedLayer}";

                    editor.MapNumTextBox.FormsControl.TextChanged += async(textObject, _) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;
                        if (textBox.Text == "") return;
                        if (NumericTextBoxBehavior.IsValidNumeric(textBox.Text)) 
                        {
                            Globals.isLoadingMap = true;
                            int.TryParse(textBox.Text, out var numericTextBoxValue);
                            Globals.SelectedMap = numericTextBoxValue;
                            await MapEditorService.GetMap();
                            Globals.isLoadingMap = false;
                            return;
                        } else
                        {
                            textBox.Text = Globals.SelectedMap.ToString();
                        }
                    };

                    editor.TilesetNumTextBox.FormsControl.TextChanged += async (textObject, _) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;
                        if (textBox.Text == "") return;
                        if (NumericTextBoxBehavior.IsValidNumeric(textBox.Text))
                        {
                            int.TryParse(textBox.Text, out var numericTextBoxValue);

                            if (numericTextBoxValue < Graphics.Tilesets.Length && numericTextBoxValue >= 0)
                            {
                                Globals.SelectedEventIndex = null;
                                Globals.SelectedTileset = numericTextBoxValue;
                                Graphics.UpdateTilesetPanel((SpriteRuntime)editor.ResourcePanel.InnerPanelInstance.GetChildByName("TilesetSprite"));
                                return;
                            }
                        }
                        
                        textBox.Text = Globals.SelectedTileset.ToString();
                    };

                    editor.SaveMapButton.Click += (_, __) =>
                    {
                        MapEditorService.SaveMap();
                    };

                    editor.LayerUpControl.Click += (_, __) =>
                    {
                        int nextLayer = Globals.SelectedLayer + 1;

                        if (!(nextLayer > Globals.MAX_LAYERS))
                        {
                            Globals.SelectedLayer = nextLayer;
                            editor.TextLayer.Text = $"Layer: {Globals.SelectedLayer}";
                        }
                    };

                    editor.LayerDownControl.Click += (_, __) =>
                    {
                        int downLayer = Globals.SelectedLayer - 1;

                        if (!(downLayer < Globals.MIN_LAYERS))
                        {
                            Globals.SelectedLayer = downLayer;
                            editor.TextLayer.Text = $"Layer: {Globals.SelectedLayer}";
                        }
                    };

                    editor.EventsButton.Click += (_, _) =>
                    {
                        editor.ResourcePanel.Visible = false;
                        editor.EventsContainer.Visible = true;
                    };

                    editor.TilesetButton.Click += (_, _) =>
                    {
                        Globals.SelectedEventIndex = null;
                        editor.ResourcePanel.Visible = true;
                        editor.EventsContainer.Visible = false;
                    };

                    Graphics.LoadGumTilesetResourcePanel(this);
                    break;
            }
        }

        /// <summary>
        /// Gets the currently active Gum screen.
        /// </summary>
        public GraphicalUiElement GetCurrentScreen() => _currentScreen;
    }
}
