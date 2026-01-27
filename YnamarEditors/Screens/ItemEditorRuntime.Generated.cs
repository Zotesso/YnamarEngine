//Code for ItemEditor
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

partial class ItemEditorRuntime : Gum.Wireframe.BindableGue
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("ItemEditor", typeof(ItemEditorRuntime));
    }
    public ColoredRectangleRuntime Background { get; protected set; }
    public ButtonIconRuntime ButtonBackScreen { get; protected set; }
    public SpriteRuntime ItemSprite { get; protected set; }
    public TextBoxRuntime ItemSpriteTextBox { get; protected set; }
    public TextRuntime ItemSpriteText { get; protected set; }
    public TextRuntime NameText { get; protected set; }
    public TextBoxRuntime NameTextBox { get; protected set; }
    public TextRuntime DescriptionText { get; protected set; }
    public TextBoxRuntime DescriptionTextBox { get; protected set; }
    public ListBoxRuntime ItemListBox { get; protected set; }
    public TextBoxRuntime SearchTextBox { get; protected set; }
    public ButtonStandardRuntime SaveButton { get; protected set; }
    public ButtonStandardRuntime NewButton { get; protected set; }
    public TextRuntime ItemTypeText { get; protected set; }
    public ComboBoxRuntime ItemTypeComboBox { get; protected set; }
    public CheckBoxRuntime CheckBoxInstance { get; protected set; }
    public ContainerRuntime AnimationContainer { get; protected set; }
    public ComboBoxRuntime AnimationComboBox { get; protected set; }
    public SpriteRuntime AnimationSprite { get; protected set; }

    public ItemEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
    {
        if (fullInstantiation)
        {
            var element = ObjectFinder.Self.GetElementSave("ItemEditor");
            element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
        }



    }
    public override void AfterFullCreation()
    {
        Background = this.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as YnamarEditors.Components.ButtonIconRuntime;
        ItemSprite = this.GetGraphicalUiElementByName("ItemSprite") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        ItemSpriteTextBox = this.GetGraphicalUiElementByName("ItemSpriteTextBox") as YnamarEditors.Components.TextBoxRuntime;
        ItemSpriteText = this.GetGraphicalUiElementByName("ItemSpriteText") as global::MonoGameGum.GueDeriving.TextRuntime;
        NameText = this.GetGraphicalUiElementByName("NameText") as global::MonoGameGum.GueDeriving.TextRuntime;
        NameTextBox = this.GetGraphicalUiElementByName("NameTextBox") as YnamarEditors.Components.TextBoxRuntime;
        DescriptionText = this.GetGraphicalUiElementByName("DescriptionText") as global::MonoGameGum.GueDeriving.TextRuntime;
        DescriptionTextBox = this.GetGraphicalUiElementByName("DescriptionTextBox") as YnamarEditors.Components.TextBoxRuntime;
        ItemListBox = this.GetGraphicalUiElementByName("ItemListBox") as YnamarEditors.Components.ListBoxRuntime;
        SearchTextBox = this.GetGraphicalUiElementByName("SearchTextBox") as YnamarEditors.Components.TextBoxRuntime;
        SaveButton = this.GetGraphicalUiElementByName("SaveButton") as YnamarEditors.Components.ButtonStandardRuntime;
        NewButton = this.GetGraphicalUiElementByName("NewButton") as YnamarEditors.Components.ButtonStandardRuntime;
        ItemTypeText = this.GetGraphicalUiElementByName("ItemTypeText") as global::MonoGameGum.GueDeriving.TextRuntime;
        ItemTypeComboBox = this.GetGraphicalUiElementByName("ItemTypeComboBox") as YnamarEditors.Components.ComboBoxRuntime;
        CheckBoxInstance = this.GetGraphicalUiElementByName("CheckBoxInstance") as YnamarEditors.Components.CheckBoxRuntime;
        AnimationContainer = this.GetGraphicalUiElementByName("AnimationContainer") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        AnimationComboBox = this.GetGraphicalUiElementByName("AnimationComboBox") as YnamarEditors.Components.ComboBoxRuntime;
        AnimationSprite = this.GetGraphicalUiElementByName("AnimationSprite") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
