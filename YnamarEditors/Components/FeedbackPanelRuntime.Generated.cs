//Code for FeedbackPanel (Container)
using GumRuntime;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;
using MonoGameGum.GueDeriving;

using RenderingLibrary.Graphics;

using System.Linq;

namespace YnamarEditors.Components
{
    partial class FeedbackPanelRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("FeedbackPanel", typeof(FeedbackPanelRuntime));
        }
        public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
        public TextRuntime TextInstance { get; protected set; }
        public SpriteRuntime SuccessIcon { get; protected set; }
        public SpriteRuntime ErrorIcon { get; protected set; }

        public FeedbackPanelRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("FeedbackPanel");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as ColoredRectangleRuntime;
            TextInstance = this.GetGraphicalUiElementByName("TextInstance") as TextRuntime;
            SuccessIcon = this.GetGraphicalUiElementByName("SuccessIcon") as SpriteRuntime;
            ErrorIcon = this.GetGraphicalUiElementByName("ErrorIcon") as SpriteRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
