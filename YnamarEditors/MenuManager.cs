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
using YnamarEditors.Components.Behavior;
using MonoGameGum.GueDeriving;
using YnamarEditors.Services.MapEditor;
using YnamarEditors.Services.NpcEditor;
using Gum.Forms.Controls;
using MonoGameGum.Forms.Controls;
using YnamarEditors.Models.Protos;
using YnamarEditors.Models;
using Microsoft.Xna.Framework.Graphics;
using YnamarEditors.Components;

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

            WireScreenEventsAsync(screenName, screenRuntime);

            return _currentScreen;
        }

        private async Task WireScreenEventsAsync(string screenName, GraphicalUiElement screenRuntime)
        {
            switch (screenName)
            {
                case "EditorSelector":
                    var selector = (EditorSelectorRuntime)screenRuntime;
                    selector.ButtonStandardInstance.Click += (_, __) =>
                    {
                        LoadScreen("MapEditor");
                    };

                    selector.NpcEditorButton.Click += (_, __) =>
                    {
                        LoadScreen("NpcEditor");
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
                        Globals.SelectedNpc = null;
                        editor.ResourcePanel.Visible = false;
                        editor.EventsContainer.Visible = true;
                    };

                    editor.TilesetButton.Click += (_, _) =>
                    {
                        Globals.SelectedEventIndex = null;
                        Globals.SelectedNpc = null;
                        editor.ResourcePanel.Visible = true;
                        editor.EventsContainer.Visible = false;
                    };

                    Graphics.LoadGumTilesetResourcePanel(this);
                    break;

                case "NpcEditor":
                    NpcEditorRuntime npcEditor = (NpcEditorRuntime)screenRuntime;
                    NpcList npcList = await NpcEditorService.ListNpcs();
                    List<NpcBehavior> npcBehaviors = await NpcEditorService.ListNpcBehaviors();

                    foreach (NpcSummary npcSummary in npcList.NpcsSummary)
                    {
                        var npc = $"Name: {npcSummary.Name} Id: {npcSummary.Id}";
                        npcEditor.NpcListBox.FormsControl.Items.Add(npc);
                    }

                    foreach (NpcBehavior npcBehavior in npcBehaviors)
                    {
                        var behavior = $"Name: {npcBehavior.Name}";
                        npcEditor.BehaviorListBox.FormsControl.Items.Add(behavior);
                    }

                    npcEditor.NpcListBox.FormsControl.SelectionChanged += (sender, args) =>
                    {
                        handleNpcSelected(npcEditor.NpcListBox.FormsControl.SelectedIndex, screenRuntime);
                    };

                    npcEditor.LevelTextBox.FormsControl.TextChanged += async (textObject, textInput) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;
                        textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray());
                    };

                    npcEditor.NpcSpriteTextBox.FormsControl.TextChanged += async (textObject, textInput) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;

                        textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray());
                        if (textBox.Text == "") return;

                        int npcSpriteNum = int.Parse(textBox.Text);

                        if (npcSpriteNum < 0 || npcSpriteNum > Globals.MAX_SPRITES) {
                            textBox.Text = "0";
                            npcSpriteNum = 0;
                        }

                        Texture2D npcSprite = Graphics.Characters[npcSpriteNum];
                        npcEditor.NpcSprite.Texture = npcSprite;
                        npcEditor.NpcSprite.TextureAddress = Gum.Managers.TextureAddress.Custom;
                        npcEditor.NpcSprite.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32);
                    };

                    npcEditor.NewButton.Click += (_, _) =>
                    {
                        var npc = $"Name: ";
                        npcEditor.NpcListBox.FormsControl.Items.Add(npc);
                        Npc newNpc = new Npc
                        {
                            Name = "",
                            Level = 0,
                            MaxHp = 0,
                            Atk = 0,
                            Def = 0,
                            RespawnTime = 0,
                            Behavior = 0,
                            Sprite = 0,
                        };
                        fillNpcSummary(newNpc);
                    };

                    npcEditor.SaveButton.Click += async (_, _) =>
                    {
                        Npc NpcToSave = new Npc
                        {
                            Name = npcEditor.NameTextBox.Text,
                            Level = int.Parse(npcEditor.LevelTextBox.Text),
                            MaxHp = int.Parse(npcEditor.MaxHpTextBox.Text),
                            Atk = int.Parse(npcEditor.AtkTextBox.Text),
                            Def = int.Parse(npcEditor.DefTextBox.Text),
                            RespawnTime = int.Parse(npcEditor.RespawnTimeTextBox.Text),
                            Behavior = (byte)npcEditor.BehaviorListBox.FormsControl.SelectedIndex,
                            Sprite = int.Parse(npcEditor.NpcSpriteTextBox.Text),
                        };

                        await NpcEditorService.SaveNpc(NpcToSave);
                    };

                    break;
            }
        }

        /// <summary>
        /// Gets the currently active Gum screen.
        /// </summary>
        public GraphicalUiElement GetCurrentScreen() => _currentScreen;

        private async Task handleNpcSelected(int npcId, GraphicalUiElement screenRuntime)
        {
            Npc npcSummary = await NpcEditorService.GetNpcSummary(npcId);
            fillNpcSummary(npcSummary);
        }

        private void fillNpcSummary(Npc npcSummary)
        {
            NpcEditorRuntime npcEditor = (NpcEditorRuntime)_currentScreen;
            npcEditor.NameTextBox.Text = npcSummary.Name;
            npcEditor.LevelTextBox.Text = npcSummary.Level.ToString();
            npcEditor.MaxHpTextBox.Text = npcSummary.MaxHp.ToString();
            npcEditor.AtkTextBox.Text = npcSummary.Atk.ToString();
            npcEditor.DefTextBox.Text = npcSummary.Def.ToString();
            npcEditor.RespawnTimeTextBox.Text = npcSummary.RespawnTime.ToString();
            npcEditor.NpcSpriteTextBox.Text = npcSummary.Sprite.ToString();

            npcEditor.BehaviorListBox.FormsControl.SelectedIndex = npcSummary.Behavior;
        }

        public async Task openNpcSelection()
        {
            MapNpcSelectPanelRuntime mapNpcSelectPanel = new MapNpcSelectPanelRuntime();
            mapNpcSelectPanel.Z = 10;
            mapNpcSelectPanel.ButtonSelectNpc.Z = 11;
            mapNpcSelectPanel.ButtonCloseNpcSelection.Z = 11;

            NpcList npcList = await NpcEditorService.ListNpcs();
            mapNpcSelectPanel.ButtonSelectNpc.IsEnabled = false;

            mapNpcSelectPanel.AddToManagers();
            _currentScreen.Children.Add(mapNpcSelectPanel);

            foreach (NpcSummary npcSummary in npcList.NpcsSummary)
            {
                var npc = $"Name: {npcSummary.Name} Id: {npcSummary.Id}";
                mapNpcSelectPanel.ListBoxInstance.FormsControl.Items.Add(npc);
            }

            mapNpcSelectPanel.ListBoxInstance.FormsControl.SelectionChanged += (sender, args) =>
            {
                mapNpcSelectPanel.ButtonSelectNpc.IsEnabled = true;
            };

            mapNpcSelectPanel.ButtonSelectNpc.Click += async (_, _) =>
            {
                Globals.SelectedNpc = await NpcEditorService.GetNpcSummary(mapNpcSelectPanel.ListBoxInstance.FormsControl.SelectedIndex);
                mapNpcSelectPanel.RemoveFromManagers();
                _currentScreen.Children.Remove(mapNpcSelectPanel);
            };

            mapNpcSelectPanel.ButtonCloseNpcSelection.Click += (_, _) =>
            {
                mapNpcSelectPanel.RemoveFromManagers();
                _currentScreen.Children.Remove(mapNpcSelectPanel);
            };
        }
    }
}
