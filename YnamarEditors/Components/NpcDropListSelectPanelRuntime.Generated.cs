//Code for NpcDropListSelectPanel (Container)
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

partial class NpcDropListSelectPanelRuntime : ContainerRuntime
{
    [System.Runtime.CompilerServices.ModuleInitializer]
    public static void RegisterRuntimeType()
    {
        GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("NpcDropListSelectPanel", typeof(NpcDropListSelectPanelRuntime));
    }
    public TextRuntime DropRateText { get; protected set; }
    public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
    public ListBoxRuntime ListBoxInstance { get; protected set; }
    public TextBoxRuntime DropRateTextBox { get; protected set; }
    public ButtonStandardRuntime ButtonCloseNpcSelection { get; protected set; }

    public NpcDropListSelectPanelRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
    {
        if (fullInstantiation)
        {
            var element = ObjectFinder.Self.GetElementSave("NpcDropListSelectPanel");
            element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
        }



    }
    public override void AfterFullCreation()
    {
        DropRateText = this.GetGraphicalUiElementByName("DropRateText") as global::MonoGameGum.GueDeriving.TextRuntime;
        ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as global::MonoGameGum.GueDeriving.ColoredRectangleRuntime;
        ListBoxInstance = this.GetGraphicalUiElementByName("ListBoxInstance") as YnamarEditors.Components.ListBoxRuntime;
        DropRateTextBox = this.GetGraphicalUiElementByName("DropRateTextBox") as YnamarEditors.Components.TextBoxRuntime;
        ButtonCloseNpcSelection = this.GetGraphicalUiElementByName("ButtonCloseNpcSelection") as YnamarEditors.Components.ButtonStandardRuntime;
        CustomInitialize();
    }
    //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
    partial void CustomInitialize();
}
