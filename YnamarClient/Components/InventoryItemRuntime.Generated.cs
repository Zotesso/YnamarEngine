//Code for InventoryItem (Container)
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
    partial class InventoryItemRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("InventoryItem", typeof(InventoryItemRuntime));
        }
        public ColoredRectangleRuntime Black { get; protected set; }
        public SpriteRuntime SpriteInstance { get; protected set; }

        public InventoryItemRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("InventoryItem");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            Black = this.GetGraphicalUiElementByName("Black") as ColoredRectangleRuntime;
            SpriteInstance = this.GetGraphicalUiElementByName("SpriteInstance") as SpriteRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
