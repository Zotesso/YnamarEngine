//Code for Inventory (Container)
using GumRuntime;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using MonoGameGum.GueDeriving;

using RenderingLibrary.Graphics;

using System.Linq;

namespace YnamarClient.Components
{
    partial class InventoryRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Inventory", typeof(InventoryRuntime));
        }
        public ColoredRectangleRuntime Gray { get; protected set; }
        public TextRuntime Title { get; protected set; }

        public InventoryRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("Inventory");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            Gray = this.GetGraphicalUiElementByName("Gray") as ColoredRectangleRuntime;
            Title = this.GetGraphicalUiElementByName("Title") as TextRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
