//Code for NpcEditor
using GumRuntime;
using System.Linq;
using MonoGameGum;
using MonoGameGum.GueDeriving;
using YnamarEditors.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

namespace YnamarEditors.Screens;

partial class NpcEditorRuntime : Gum.Wireframe.BindableGue
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("NpcEditor", typeof(NpcEditorRuntime));
    }
    public ColoredRectangleRuntime EditorSection { get; protected set; }
    public SpriteRuntime NpcSprite { get; protected set; }
    public TextRuntime NameText { get; protected set; }
    public TextBoxRuntime NameTextBox { get; protected set; }
    public TextRuntime LevelText { get; protected set; }
    public TextRuntime MaxHpText { get; protected set; }
    public TextRuntime AtkText { get; protected set; }
    public TextRuntime DefText { get; protected set; }
    public TextRuntime BehaviorText { get; protected set; }
    public TextRuntime RespawnTimeText { get; protected set; }
    public TextBoxRuntime LevelTextBox { get; protected set; }
    public TextBoxRuntime MaxHpTextBox { get; protected set; }
    public TextBoxRuntime AtkTextBox { get; protected set; }
    public TextBoxRuntime DefTextBox { get; protected set; }
    public TextBoxRuntime RespawnTimeTextBox { get; protected set; }
    public ListBoxRuntime BehaviorListBox { get; protected set; }
    public ListBoxRuntime NpcListBox { get; protected set; }
    public TextBoxRuntime SearchTextBox { get; protected set; }
    public ButtonStandardRuntime SaveButton { get; protected set; }
    public ButtonStandardRuntime NewButton { get; protected set; }
    public TextBoxRuntime NpcSpriteTextBox { get; protected set; }
    public TextRuntime NpcSpriteText { get; protected set; }
    public ButtonIconRuntime ButtonBackScreen { get; protected set; }
    public TextRuntime DropListText { get; protected set; }
    public ButtonIconRuntime ButtonAddItemDropList { get; protected set; }
    public ButtonIconRuntime ButtonRemoveItemDropList { get; protected set; }
    public ListBoxRuntime DropListBox { get; protected set; }

    public NpcEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
    {
        if (fullInstantiation)
        {
            var element = ObjectFinder.Self.GetElementSave("NpcEditor");
            element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
        }



    }
    public override void AfterFullCreation()
    {
        EditorSection = this.GetGraphicalUiElementByName("EditorSection") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        NpcSprite = this.GetGraphicalUiElementByName("NpcSprite") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        NameText = this.GetGraphicalUiElementByName("NameText") as global::MonoGameGum.GueDeriving.TextRuntime;
        NameTextBox = this.GetGraphicalUiElementByName("NameTextBox") as YnamarEditors.Components.TextBoxRuntime;
        LevelText = this.GetGraphicalUiElementByName("LevelText") as global::MonoGameGum.GueDeriving.TextRuntime;
        MaxHpText = this.GetGraphicalUiElementByName("MaxHpText") as global::MonoGameGum.GueDeriving.TextRuntime;
        AtkText = this.GetGraphicalUiElementByName("AtkText") as global::MonoGameGum.GueDeriving.TextRuntime;
        DefText = this.GetGraphicalUiElementByName("DefText") as global::MonoGameGum.GueDeriving.TextRuntime;
        BehaviorText = this.GetGraphicalUiElementByName("BehaviorText") as global::MonoGameGum.GueDeriving.TextRuntime;
        RespawnTimeText = this.GetGraphicalUiElementByName("RespawnTimeText") as global::MonoGameGum.GueDeriving.TextRuntime;
        LevelTextBox = this.GetGraphicalUiElementByName("LevelTextBox") as YnamarEditors.Components.TextBoxRuntime;
        MaxHpTextBox = this.GetGraphicalUiElementByName("MaxHpTextBox") as YnamarEditors.Components.TextBoxRuntime;
        AtkTextBox = this.GetGraphicalUiElementByName("AtkTextBox") as YnamarEditors.Components.TextBoxRuntime;
        DefTextBox = this.GetGraphicalUiElementByName("DefTextBox") as YnamarEditors.Components.TextBoxRuntime;
        RespawnTimeTextBox = this.GetGraphicalUiElementByName("RespawnTimeTextBox") as YnamarEditors.Components.TextBoxRuntime;
        BehaviorListBox = this.GetGraphicalUiElementByName("BehaviorListBox") as YnamarEditors.Components.ListBoxRuntime;
        NpcListBox = this.GetGraphicalUiElementByName("NpcListBox") as YnamarEditors.Components.ListBoxRuntime;
        SearchTextBox = this.GetGraphicalUiElementByName("SearchTextBox") as YnamarEditors.Components.TextBoxRuntime;
        SaveButton = this.GetGraphicalUiElementByName("SaveButton") as YnamarEditors.Components.ButtonStandardRuntime;
        NewButton = this.GetGraphicalUiElementByName("NewButton") as YnamarEditors.Components.ButtonStandardRuntime;
        NpcSpriteTextBox = this.GetGraphicalUiElementByName("NpcSpriteTextBox") as YnamarEditors.Components.TextBoxRuntime;
        NpcSpriteText = this.GetGraphicalUiElementByName("NpcSpriteText") as global::MonoGameGum.GueDeriving.TextRuntime;
        ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as YnamarEditors.Components.ButtonIconRuntime;
        DropListText = this.GetGraphicalUiElementByName("DropListText") as global::MonoGameGum.GueDeriving.TextRuntime;
        ButtonAddItemDropList = this.GetGraphicalUiElementByName("ButtonAddItemDropList") as YnamarEditors.Components.ButtonIconRuntime;
        ButtonRemoveItemDropList = this.GetGraphicalUiElementByName("ButtonRemoveItemDropList") as YnamarEditors.Components.ButtonIconRuntime;
        DropListBox = this.GetGraphicalUiElementByName("DropListBox") as YnamarEditors.Components.ListBoxRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
