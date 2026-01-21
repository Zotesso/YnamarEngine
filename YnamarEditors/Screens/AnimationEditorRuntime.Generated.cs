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
    public ButtonIconRuntime ButtonBackScreen { get; protected set; }
    public RectangleRuntime SpritesheetRectangle { get; protected set; }
    public RectangleRuntime PlayerRectangle { get; protected set; }
    public ButtonIconRuntime StopPlayerButton { get; protected set; }
    public CheckBoxRuntime IsLoopCheckBox { get; protected set; }
    public TextBoxRuntime TextureTextBox { get; protected set; }
    public ButtonIconRuntime StartPlayerButton { get; protected set; }
    public ScrollViewerRuntime ResourcePanel { get; protected set; }
    public ScrollViewerRuntime AnimationFrameList { get; protected set; }
    public TextBoxRuntime AnimationNameTextBox { get; protected set; }
    public ButtonIconRuntime AddFrameButton { get; protected set; }
    public ButtonIconRuntime RemoveFrameButton { get; protected set; }
    public SpriteRuntime AnimationPlayerSprite { get; protected set; }

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
        ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as YnamarEditors.Components.ButtonIconRuntime;
        SpritesheetRectangle = this.GetGraphicalUiElementByName("SpritesheetRectangle") as global::MonoGameGum.GueDeriving.RectangleRuntime;
        PlayerRectangle = this.GetGraphicalUiElementByName("PlayerRectangle") as global::MonoGameGum.GueDeriving.RectangleRuntime;
        StopPlayerButton = this.GetGraphicalUiElementByName("StopPlayerButton") as YnamarEditors.Components.ButtonIconRuntime;
        IsLoopCheckBox = this.GetGraphicalUiElementByName("IsLoopCheckBox") as YnamarEditors.Components.CheckBoxRuntime;
        TextureTextBox = this.GetGraphicalUiElementByName("TextureTextBox") as YnamarEditors.Components.TextBoxRuntime;
        StartPlayerButton = this.GetGraphicalUiElementByName("StartPlayerButton") as YnamarEditors.Components.ButtonIconRuntime;
        ResourcePanel = this.GetGraphicalUiElementByName("ResourcePanel") as YnamarEditors.Components.ScrollViewerRuntime;
        AnimationFrameList = this.GetGraphicalUiElementByName("AnimationFrameList") as YnamarEditors.Components.ScrollViewerRuntime;
        AnimationNameTextBox = this.GetGraphicalUiElementByName("AnimationNameTextBox") as YnamarEditors.Components.TextBoxRuntime;
        AddFrameButton = this.GetGraphicalUiElementByName("AddFrameButton") as YnamarEditors.Components.ButtonIconRuntime;
        RemoveFrameButton = this.GetGraphicalUiElementByName("RemoveFrameButton") as YnamarEditors.Components.ButtonIconRuntime;
        AnimationPlayerSprite = this.GetGraphicalUiElementByName("AnimationPlayerSprite") as global::MonoGameGum.GueDeriving.SpriteRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
