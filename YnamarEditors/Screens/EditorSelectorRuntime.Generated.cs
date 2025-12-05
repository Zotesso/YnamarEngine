//Code for EditorSelector
using GumRuntime;
using YnamarEditors.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;
using MonoGameGum.GueDeriving;

namespace YnamarEditors.Screens
{
    partial class EditorSelectorRuntime:Gum.Wireframe.BindableGue
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("EditorSelector", typeof(EditorSelectorRuntime));
        }
        public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
        public TextRuntime TextInstance { get; protected set; }
        public ButtonStandardRuntime ButtonStandardInstance { get; protected set; }
        public ButtonStandardRuntime NpcEditorButton { get; protected set; }

        public EditorSelectorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("EditorSelector");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as ColoredRectangleRuntime;
            TextInstance = this.GetGraphicalUiElementByName("TextInstance") as TextRuntime;
            ButtonStandardInstance = this.GetGraphicalUiElementByName("ButtonStandardInstance") as ButtonStandardRuntime;
            NpcEditorButton = this.GetGraphicalUiElementByName("NpcEditorButton") as ButtonStandardRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
