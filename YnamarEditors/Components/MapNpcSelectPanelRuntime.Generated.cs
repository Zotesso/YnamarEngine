//Code for MapNpcSelectPanel (Container)
using GumRuntime;
using YnamarEditors.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;
using MonoGameGum.GueDeriving;

namespace YnamarEditors.Components
{
    partial class MapNpcSelectPanelRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("MapNpcSelectPanel", typeof(MapNpcSelectPanelRuntime));
        }
        public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
        public ListBoxRuntime ListBoxInstance { get; protected set; }
        public ButtonStandardRuntime ButtonCloseNpcSelection { get; protected set; }
        public ButtonStandardRuntime ButtonSelectNpc { get; protected set; }

        public MapNpcSelectPanelRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("MapNpcSelectPanel");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as ColoredRectangleRuntime;
            ListBoxInstance = this.GetGraphicalUiElementByName("ListBoxInstance") as ListBoxRuntime;
            ButtonCloseNpcSelection = this.GetGraphicalUiElementByName("ButtonCloseNpcSelection") as ButtonStandardRuntime;
            ButtonSelectNpc = this.GetGraphicalUiElementByName("ButtonSelectNpc") as ButtonStandardRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
