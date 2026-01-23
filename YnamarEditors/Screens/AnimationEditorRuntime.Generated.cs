//Code for AnimationEditor
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

partial class AnimationEditorRuntime : Gum.Wireframe.BindableGue
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("AnimationEditor", typeof(AnimationEditorRuntime));
    }
    public ColoredRectangleRuntime Background { get; protected set; }
    public ContainerRuntime SelectorSection { get; protected set; }
    public SpriteRuntime AnimationPlayerSprite { get; protected set; }
    public ButtonIconRuntime RemoveFrameButton { get; protected set; }
    public ButtonIconRuntime AddFrameButton { get; protected set; }
    public TextBoxRuntime AnimationNameTextBox { get; protected set; }
    public ScrollViewerRuntime AnimationFrameList { get; protected set; }
    public ScrollViewerRuntime ResourcePanel { get; protected set; }
    public ButtonIconRuntime StartPlayerButton { get; protected set; }
    public TextBoxRuntime TextureTextBox { get; protected set; }
    public CheckBoxRuntime IsLoopCheckBox { get; protected set; }
    public ButtonIconRuntime StopPlayerButton { get; protected set; }
    public RectangleRuntime PlayerRectangle { get; protected set; }
    public RectangleRuntime SpritesheetRectangle { get; protected set; }
    public ButtonIconRuntime ButtonBackScreen { get; protected set; }
    public ButtonStandardRuntime SaveButton { get; protected set; }
    public ContainerRuntime EditorSection { get; protected set; }
    public ColoredRectangleRuntime BackgroundSelector { get; protected set; }
    public ButtonIconRuntime ButtonBackScreenSelector { get; protected set; }
    public ListBoxRuntime ItemListBox { get; protected set; }
    public TextBoxRuntime SearchTextBox { get; protected set; }
    public ButtonStandardRuntime NewButton { get; protected set; }
    public ButtonStandardRuntime SelectButton { get; protected set; }

    public AnimationEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
    {
        if (fullInstantiation)
        {
            var element = ObjectFinder.Self.GetElementSave("AnimationEditor");
            element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
        }



    }
    public override void AfterFullCreation()
    {
        Background = this.GetGraphicalUiElementByName("Background") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        SelectorSection = this.GetGraphicalUiElementByName("SelectorSection") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        AnimationPlayerSprite = this.GetGraphicalUiElementByName("AnimationPlayerSprite") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        RemoveFrameButton = this.GetGraphicalUiElementByName("RemoveFrameButton") as YnamarEditors.Components.ButtonIconRuntime;
        AddFrameButton = this.GetGraphicalUiElementByName("AddFrameButton") as YnamarEditors.Components.ButtonIconRuntime;
        AnimationNameTextBox = this.GetGraphicalUiElementByName("AnimationNameTextBox") as YnamarEditors.Components.TextBoxRuntime;
        AnimationFrameList = this.GetGraphicalUiElementByName("AnimationFrameList") as YnamarEditors.Components.ScrollViewerRuntime;
        ResourcePanel = this.GetGraphicalUiElementByName("ResourcePanel") as YnamarEditors.Components.ScrollViewerRuntime;
        StartPlayerButton = this.GetGraphicalUiElementByName("StartPlayerButton") as YnamarEditors.Components.ButtonIconRuntime;
        TextureTextBox = this.GetGraphicalUiElementByName("TextureTextBox") as YnamarEditors.Components.TextBoxRuntime;
        IsLoopCheckBox = this.GetGraphicalUiElementByName("IsLoopCheckBox") as YnamarEditors.Components.CheckBoxRuntime;
        StopPlayerButton = this.GetGraphicalUiElementByName("StopPlayerButton") as YnamarEditors.Components.ButtonIconRuntime;
        PlayerRectangle = this.GetGraphicalUiElementByName("PlayerRectangle") as global::MonoGameGum.GueDeriving.RectangleRuntime;
        SpritesheetRectangle = this.GetGraphicalUiElementByName("SpritesheetRectangle") as global::MonoGameGum.GueDeriving.RectangleRuntime;
        ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as YnamarEditors.Components.ButtonIconRuntime;
        SaveButton = this.GetGraphicalUiElementByName("SaveButton") as YnamarEditors.Components.ButtonStandardRuntime;
        EditorSection = this.GetGraphicalUiElementByName("EditorSection") as global::MonoGameGum.GueDeriving.ContainerRuntime;
        BackgroundSelector = this.GetGraphicalUiElementByName("BackgroundSelector") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        ButtonBackScreenSelector = this.GetGraphicalUiElementByName("ButtonBackScreenSelector") as YnamarEditors.Components.ButtonIconRuntime;
        ItemListBox = this.GetGraphicalUiElementByName("ItemListBox") as YnamarEditors.Components.ListBoxRuntime;
        SearchTextBox = this.GetGraphicalUiElementByName("SearchTextBox") as YnamarEditors.Components.TextBoxRuntime;
        NewButton = this.GetGraphicalUiElementByName("NewButton") as YnamarEditors.Components.ButtonStandardRuntime;
        SelectButton = this.GetGraphicalUiElementByName("SelectButton") as YnamarEditors.Components.ButtonStandardRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
