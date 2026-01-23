//Code for AnimationFrameListItem (Container)
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

namespace YnamarEditors.Components;

partial class AnimationFrameListItemRuntime : ContainerRuntime
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("AnimationFrameListItem", typeof(AnimationFrameListItemRuntime));
    }
    public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
    public TextRuntime FrameNumberText { get; protected set; }
    public TextBoxRuntime DurationTextBox { get; protected set; }
    public ButtonIconRuntime RemoveFrameButton { get; protected set; }

    public int frameNum
    {
        get;
        set;
    }
    public AnimationFrameListItemRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
    {
        if (fullInstantiation)
        {
            var element = ObjectFinder.Self.GetElementSave("AnimationFrameListItem");
            element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
        }



    }
    public override void AfterFullCreation()
    {
        ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        FrameNumberText = this.GetGraphicalUiElementByName("FrameNumberText") as global::MonoGameGum.GueDeriving.TextRuntime;
        DurationTextBox = this.GetGraphicalUiElementByName("DurationTextBox") as YnamarEditors.Components.TextBoxRuntime;
        RemoveFrameButton = this.GetGraphicalUiElementByName("RemoveFrameButton") as YnamarEditors.Components.ButtonIconRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
