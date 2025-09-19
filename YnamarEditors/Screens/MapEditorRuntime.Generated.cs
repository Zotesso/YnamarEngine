//Code for MapEditor
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
    partial class MapEditorRuntime:Gum.Wireframe.BindableGue
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("MapEditor", typeof(MapEditorRuntime));
        }
        public ColoredRectangleRuntime EditorSection { get; protected set; }
        public ScrollViewerRuntime ResourcePanel { get; protected set; }

        public MapEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("MapEditor");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            EditorSection = this.GetGraphicalUiElementByName("EditorSection") as ColoredRectangleRuntime;
            ResourcePanel = this.GetGraphicalUiElementByName("ResourcePanel") as ScrollViewerRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
