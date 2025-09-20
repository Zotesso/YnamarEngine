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
        public ColoredRectangleRuntime HorizontalScrollbarSection { get; protected set; }
        public ColoredRectangleRuntime VerticalScrollbarSection { get; protected set; }
        public ScrollBarRuntime VerticalScrollbar { get; protected set; }
        public ScrollBarRuntime HorizontalScrollbar { get; protected set; }
        public TextRuntime MapNumText { get; protected set; }
        public TextRuntime MaxMapYText { get; protected set; }
        public TextRuntime MapMaxXText { get; protected set; }
        public TextBoxRuntime MapNumTextBox { get; protected set; }
        public TextBoxRuntime MapMaxYTextBox { get; protected set; }
        public TextBoxRuntime MapMaxXTextBox { get; protected set; }

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
            HorizontalScrollbarSection = this.GetGraphicalUiElementByName("HorizontalScrollbarSection") as ColoredRectangleRuntime;
            VerticalScrollbarSection = this.GetGraphicalUiElementByName("VerticalScrollbarSection") as ColoredRectangleRuntime;
            VerticalScrollbar = this.GetGraphicalUiElementByName("VerticalScrollbar") as ScrollBarRuntime;
            HorizontalScrollbar = this.GetGraphicalUiElementByName("HorizontalScrollbar") as ScrollBarRuntime;
            MapNumText = this.GetGraphicalUiElementByName("MapNumText") as TextRuntime;
            MaxMapYText = this.GetGraphicalUiElementByName("MaxMapYText") as TextRuntime;
            MapMaxXText = this.GetGraphicalUiElementByName("MapMaxXText") as TextRuntime;
            MapNumTextBox = this.GetGraphicalUiElementByName("MapNumTextBox") as TextBoxRuntime;
            MapMaxYTextBox = this.GetGraphicalUiElementByName("MapMaxYTextBox") as TextBoxRuntime;
            MapMaxXTextBox = this.GetGraphicalUiElementByName("MapMaxXTextBox") as TextBoxRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
