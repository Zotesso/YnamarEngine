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
using static YnamarEditors.Globals;
using System.Reflection;
using YnamarEditors.Services.ItemEditor;
using YnamarEditors.Services.AnimationEditor;
using Microsoft.Xna.Framework;
using Gum.Graphics.Animation;
using YnamarEditors.Models.Animation;

namespace YnamarEditors
{
    internal class MenuManager
    {
        private GumProjectSave _gumProject;
        private static GraphicalUiElement _currentScreen;
        private static FeedbackPanelRuntime _feedbackPanel;
        private AnimationEditorService _animationEditorService = new AnimationEditorService();

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

                    selector.ItemEditorButton.Click += (_, __) =>
                    {
                        LoadScreen("ItemEditor");
                    };
                    
                    selector.AnimationEditorButton.Click += (_, __) =>
                    {
                        LoadScreen("AnimationEditor");
                    };

                    break;

                case "MapEditor":
                    var editor = (MapEditorRuntime)screenRuntime;
                    editor.ButtonBackScreen.Click += (_, __) =>
                    {
                        LoadScreen("EditorSelector");
                    };


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
                                Graphics.UpdateTilesetPanel((SpriteRuntime)editor.ResourcePanel.InnerPanelInstance.GetChildByName("TilesetSprite"), Graphics.Tilesets[Globals.SelectedTileset]);
                                return;
                            }
                        }
                        
                        textBox.Text = Globals.SelectedTileset.ToString();
                    };

                    editor.SaveMapButton.Click += (_, __) =>
                    {
                        StartLoading();
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
                    npcEditor.ButtonBackScreen.Click += (_, __) =>
                    {
                        LoadScreen("EditorSelector");
                    };


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

                        StartLoading();
                        await NpcEditorService.SaveNpc(NpcToSave);
                    };

                    break;

                case "ItemEditor":
                    ItemEditorRuntime itemEditor = (ItemEditorRuntime)screenRuntime;
                    itemEditor.ButtonBackScreen.Click += (_, __) =>
                    {
                        LoadScreen("EditorSelector");
                    };


                    ItemList itemList = await ItemEditorService.ListItems();
                    List<ItemType> itemTypeList = await ItemEditorService.ListItemType();
                    AnimationClipList animationItemList = await AnimationEditorService.ListAnimationClips();

                    foreach (ItemSummary itemSummary in itemList.ItemsSummary)
                    {
                        var item = $"Name: {itemSummary.Name} Id: {itemSummary.Id}";
                        itemEditor.ItemListBox.FormsControl.Items.Add(item);
                    }

                    foreach (ItemType itemType in itemTypeList)
                    {
                        var type = $"Name: {itemType.Name}";
                        itemEditor.ItemTypeComboBox.FormsControl.Items.Add(type);
                    }

                    itemEditor.AnimationComboBox.FormsControl.Items.Clear();


                    foreach (var animationSummary in animationItemList.AnimationClipSummaryList)
                    {
                        itemEditor.AnimationComboBox.FormsControl.Items.Add(
                            $"Animation: {animationSummary.Name} Id: {animationSummary.Id}"
                        );
                    }

                    itemEditor.ItemListBox.FormsControl.SelectionChanged += (sender, args) =>
                    {
                        handleItemSelected(itemEditor.ItemListBox.FormsControl.SelectedIndex, screenRuntime);
                    };

                    itemEditor.ItemTypeComboBox.FormsControl.SelectionChanged += (sender, args) =>
                    {
                        itemEditor.AnimationContainer.Visible = itemEditor.ItemTypeComboBox.FormsControl.SelectedIndex == 1;
                    };

                    itemEditor.AnimationComboBox.FormsControl.SelectionChanged += (sender, args) =>
                    {
                        handleItemAnimationSelected(itemEditor.AnimationComboBox.FormsControl.SelectedIndex);
                    };

                    itemEditor.ItemSpriteTextBox.FormsControl.TextChanged += async (textObject, textInput) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;

                        textBox.Text = new string(textBox.Text.Where(char.IsDigit).ToArray());
                        if (textBox.Text == "") return;

                        int itemSpriteNum = int.Parse(textBox.Text);

                        if (itemSpriteNum < 0 || itemSpriteNum > Globals.MAX_ITEM_SPRITES)
                        {
                            textBox.Text = "0";
                            itemSpriteNum = 0;
                        }

                        Texture2D itemSprite = Graphics.Items[itemSpriteNum];
                        itemEditor.ItemSprite.Texture = itemSprite;
                        itemEditor.ItemSprite.TextureAddress = Gum.Managers.TextureAddress.Custom;
                        itemEditor.ItemSprite.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 32, 32, 32);
                    };

                   itemEditor.NewButton.Click += (_, _) =>
                    {
                        var item = $"Name: ";
                        itemEditor.ItemListBox.FormsControl.Items.Add(item);
                        Item newItem = new Item
                        {
                            Name = "",
                            Description = "",
                            Stackable = false,
                            Type = 0,
                            Sprite = 0,
                        };
                        fillItemSummary(newItem);
                    };
                   

                    itemEditor.SaveButton.Click += async (_, _) =>
                    {
                        Item ItemToSave = new Item
                        {
                            Name = itemEditor.NameTextBox.Text,
                            Description = itemEditor.DescriptionText.Text,
                            Type = (byte)itemEditor.ItemTypeComboBox.FormsControl.SelectedIndex,
                            Stackable = itemEditor.CheckBoxInstance.FormsControl.IsChecked ?? false,
                            Sprite = int.Parse(itemEditor.ItemSpriteTextBox.Text),
                            AnimationClipId = itemEditor.AnimationContainer.Visible ? (int?)itemEditor.AnimationComboBox.FormsControl.SelectedIndex : null,
                        };

                        StartLoading();
                        await ItemEditorService.SaveItem(ItemToSave);
                    };
                   
                    break;

                case "AnimationEditor":
                    AnimationEditorRuntime animationEditor = (AnimationEditorRuntime)screenRuntime;

                    AnimationClipList animationClipList = await AnimationEditorService.ListAnimationClips();

                    if (animationClipList is not null && animationClipList.AnimationClipSummaryList.Count > 0)
                    {
                        foreach (AnimationClipSummary clipSummary in animationClipList.AnimationClipSummaryList)
                        {
                            var clip = $"Clip Name: {clipSummary.Name} Id: {clipSummary.Id}";
                            animationEditor.ItemListBox.FormsControl.Items.Add(clip);
                        }

                    }
                    animationEditor.ButtonBackScreen.Click += (_, __) =>
                    {
                        Game1.animationPlayerService.Stop();
                        animationEditor.EditorSection.Visible = false;
                        animationEditor.SelectorSection.Visible = true;
                    };

                    animationEditor.ButtonBackScreenSelector.Click += (_, __) =>
                    {
                        LoadScreen("EditorSelector");
                    };

                    animationEditor.ItemListBox.FormsControl.SelectionChanged += (sender, args) =>
                    {
                        handleAnimationSelected(animationEditor.ItemListBox.FormsControl.SelectedIndex, screenRuntime);
                    };

                    animationEditor.TextureTextBox.FormsControl.TextChanged += async (textObject, _) =>
                    {
                        MonoGameGum.Forms.Controls.TextBox textBox = (MonoGameGum.Forms.Controls.TextBox)textObject;
                        if (textBox.Text == "") return;
                        if (NumericTextBoxBehavior.IsValidNumeric(textBox.Text))
                        {
                            int.TryParse(textBox.Text, out var numericTextBoxValue);

                            if (numericTextBoxValue < Graphics.Spritesheets.Length && numericTextBoxValue >= 0)
                            {
                                Globals.SelectedSpritesheet = numericTextBoxValue;
                                Graphics.UpdateTilesetPanel((SpriteRuntime)animationEditor.ResourcePanel.InnerPanelInstance.GetChildByName("SpriteSheetSprite"), Graphics.Spritesheets[Globals.SelectedSpritesheet]);
                                return;
                            }
                        }

                        textBox.Text = Globals.SelectedSpritesheet.ToString();
                    };

                    animationEditor.AddFrameButton.Click += (_, __) =>
                    {
                        RectangleRuntime selectionBox = (RectangleRuntime)animationEditor.ResourcePanel.InnerPanelInstance.GetGraphicalUiElementByName("SelectionBox");
                        if (selectionBox is not null && selectionBox.Width > 1)
                        {
                            Rectangle selectedRectangle = new Rectangle
                            {
                                X = (int)selectionBox.X,
                                Y = (int)selectionBox.Y,
                                Width = (int)selectionBox.Width,
                                Height = (int)selectionBox.Height,
                            };

                            Models.Animation.AnimationFrame animationFrame = _animationEditorService.CreateNewFrame(Globals.SelectedSpritesheet, selectedRectangle);
                            _animationEditorService.AddFrame(animationFrame);
                            addAnimationFrameToClipList(animationEditor);
                        }
                    };
                    
                    animationEditor.StartPlayerButton.Click += (_, __) =>
                    {
                        Game1.animationPlayerService.Play(_animationEditorService.CurrentAnimationClip);
                    };

                    animationEditor.StopPlayerButton.Click += (_, __) =>
                    {
                        Game1.animationPlayerService.Stop();
                    };

                    Game1.animationPlayerService.FrameChanged += frame =>
                    {
                        Texture2D texture =
                            Graphics.Spritesheets[frame.TextureId];

                        animationEditor.AnimationPlayerSprite.Texture = texture;
                        animationEditor.AnimationPlayerSprite.TextureAddress =
                            Gum.Managers.TextureAddress.Custom;

                        animationEditor.AnimationPlayerSprite.SourceRectangle =
                            frame.SourceRect;
                    };

                    animationEditor.NewButton.Click += (_, _) =>
                    {
                        var item = $"Name: ";
                        animationEditor.ItemListBox.FormsControl.Items.Add(item);
                        AnimationClip newAnimationClip = new AnimationClip
                        {
                            Name = "",
                            Id = 0,
                            Loop = false,
                            Frames = new List<Models.Animation.AnimationFrame>(),
                        };

                        fillAnimationClip(newAnimationClip);
                    };


                    animationEditor.SaveButton.Click += async (_, _) =>
                    {
                        AnimationClip AnimationClipToSave = new AnimationClip
                        {
                            Name = animationEditor.AnimationNameTextBox.Text,
                            Loop = animationEditor.IsLoopCheckBox.FormsControl.IsChecked ?? false,
                            Frames = _animationEditorService.CurrentAnimationClip.Frames,
                        };

                        StartLoading();
                        await AnimationEditorService.SaveAnimationClip(AnimationClipToSave);
                    };

                    Graphics.LoadGumSpriteSheetResourcePanel(this);
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

        private async Task handleAnimationSelected(int clipId, GraphicalUiElement screenRuntime)
        {
            StartLoading();
            AnimationClip animationClip = await AnimationEditorService.GetAnimationClip(clipId);
            fillAnimationClip(animationClip);
        }

        private void fillAnimationClip(AnimationClip animationClip)
        {
            AnimationEditorRuntime animationEditor = (AnimationEditorRuntime)_currentScreen;
            animationEditor.EditorSection.Visible = true;
            animationEditor.AnimationNameTextBox.Text = animationClip.Name;
            animationEditor.TextureTextBox.Text = animationClip.Frames.Count > 0 ? animationClip.Frames[0].TextureId.ToString() : "0";
            animationEditor.IsLoopCheckBox.FormsControl.IsChecked = animationClip.Loop;

            animationClip.Frames.ForEach(frame =>
            {
                _animationEditorService.AddFrame(frame);
                addAnimationFrameToClipList(animationEditor);
            });
        }

        private async Task handleItemAnimationSelected(int animationId)
        {
            AnimationClip animationClip = await AnimationEditorService.GetAnimationClip(animationId);
            ItemEditorRuntime itemEditor = (ItemEditorRuntime)_currentScreen;
            itemEditor.AnimationSprite.Texture = Graphics.Spritesheets[animationClip.Frames[0].TextureId];
            itemEditor.AnimationSprite.TextureAddress = Gum.Managers.TextureAddress.Custom;
            itemEditor.AnimationSprite.SourceRectangle = new Microsoft.Xna.Framework.Rectangle(0, 0, 32, 32);
        }

        private async Task handleItemSelected(int itemId, GraphicalUiElement screenRuntime)
        {
            Item itemSummary = await ItemEditorService.GetItemSummary(itemId);
            fillItemSummary(itemSummary);
        }

        private void fillItemSummary(Item itemSummary)
        {
            ItemEditorRuntime itemEditor = (ItemEditorRuntime)_currentScreen;
            itemEditor.NameTextBox.Text = itemSummary.Name;
            itemEditor.DescriptionTextBox.Text = itemSummary.Description;
            itemEditor.ItemSpriteTextBox.Text = itemSummary.Sprite.ToString();
            itemEditor.CheckBoxInstance.FormsControl.IsChecked = itemSummary.Stackable;

            itemEditor.ItemTypeComboBox.FormsControl.SelectedIndex = itemSummary.Type;
        }

        public async Task openNpcSelection()
        {
            MapNpcSelectPanelRuntime mapNpcSelectPanel = new MapNpcSelectPanelRuntime();
            mapNpcSelectPanel.Z = 10;
            mapNpcSelectPanel.HasEvents = true;
            mapNpcSelectPanel.ColoredRectangleInstance.Z = 11;
            mapNpcSelectPanel.ButtonCloseNpcSelection.HasEvents = true;
            mapNpcSelectPanel.ButtonSelectNpc.HasEvents = true;
            mapNpcSelectPanel.ButtonSelectNpc.Z = 12;
            mapNpcSelectPanel.ButtonCloseNpcSelection.Z = 12;

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

            ButtonStandardRuntime eventButton = new ButtonStandardRuntime
            {
                Name = "Teste",
                Width = 120,
                Height = 60,
                WidthUnits = Gum.DataTypes.DimensionUnitType.Absolute,
                HeightUnits = Gum.DataTypes.DimensionUnitType.Absolute,

            };

            eventButton.Click += async (_, _) =>
            {
                Globals.SelectedNpc = await NpcEditorService.GetNpcSummary(mapNpcSelectPanel.ListBoxInstance.FormsControl.SelectedIndex);
                mapNpcSelectPanel.RemoveFromManagers();
                _currentScreen.Children.Remove(mapNpcSelectPanel);
            };

            eventButton.TextInstance.Text = "Teste";
            mapNpcSelectPanel.Children.Add(eventButton);

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
    
        private void addAnimationFrameToClipList(AnimationEditorRuntime animationEditor)
        {
            AnimationFrameListItemRuntime animationFrameListItem = new AnimationFrameListItemRuntime();
            animationEditor.AnimationFrameList.InnerPanelInstance.Children.Add(animationFrameListItem);
            animationFrameListItem.FrameNumberText.Text = $"Frame: {animationEditor.AnimationFrameList.InnerPanelInstance.Children.Count}";
            animationFrameListItem.DurationTextBox.Text = "1000";
            animationFrameListItem.frameNum = animationEditor.AnimationFrameList.InnerPanelInstance.Children.Count - 1;
            animationFrameListItem.Y = (animationEditor.AnimationFrameList.InnerPanelInstance.Children.Count - 1) * 50;
            animationFrameListItem.RemoveFrameButton.Click += (_, __) =>
            {
                _animationEditorService.RemoveFrame(animationFrameListItem.frameNum);
                animationEditor.AnimationFrameList.InnerPanelInstance.Children.Remove(animationFrameListItem);
                for (int i = 0; i < animationEditor.AnimationFrameList.InnerPanelInstance.Children.Count; i++)
                {
                    var child = animationEditor.AnimationFrameList.InnerPanelInstance.Children[i] as AnimationFrameListItemRuntime;
                    child.FrameNumberText.Text = $"Frame: {i}";
                    child.frameNum = i - 1;
                    child.Y = i * 50;
                }
            };
        }

        public static void StartLoading()
        {
            _feedbackPanel = new FeedbackPanelRuntime();
            _feedbackPanel.Name = "FeedbackPanel";
            _feedbackPanel.Z = 999;
            _currentScreen.Children.Add(_feedbackPanel);
        }

        public static async Task StopLoadingAsync(Boolean sucess)
        {
            if (_feedbackPanel != null)
            {
                if (sucess)
                {
                    _feedbackPanel.SuccessIcon.Visible = true;
                    _feedbackPanel.TextInstance.Text = "Salvo com Sucesso - Successfully Saved";
                }
                else
                {
                    _feedbackPanel.ErrorIcon.Visible = true;
                    _feedbackPanel.TextInstance.Text = "Erro ao Salvar - Error while saving";
                }

                await Task.Delay(1500);
                _currentScreen.Children.Remove(_feedbackPanel);
                _feedbackPanel = null;
            }
        }

        public static async Task StopLoadingRemoveFeedbackPanelAsync()
        {
            if (_feedbackPanel != null)
            {
                _currentScreen.Children.Remove(_feedbackPanel);
                _feedbackPanel = null;
            }
        }
    }
}
