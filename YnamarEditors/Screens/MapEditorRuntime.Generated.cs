//Code for MapEditor
using GumRuntime;
using YnamarEditors.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;

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
        public ButtonConfirmRuntime SaveMapButton { get; protected set; }
        public ContainerRuntime LayerContainer { get; protected set; }
        public ButtonIconRuntime LayerUpControl { get; protected set; }
        public ButtonIconRuntime LayerDownControl { get; protected set; }
        public TextRuntime TextLayer { get; protected set; }
        public TextRuntime TilesetNumText { get; protected set; }
        public TextBoxRuntime TilesetNumTextBox { get; protected set; }
        public ButtonStandardRuntime TilesetButton { get; protected set; }
        public ButtonStandardRuntime EventsButton { get; protected set; }
        public ContainerRuntime EventsContainer { get; protected set; }

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
            SaveMapButton = this.GetGraphicalUiElementByName("SaveMapButton") as ButtonConfirmRuntime;
            LayerContainer = this.GetGraphicalUiElementByName("LayerContainer") as ContainerRuntime;
            LayerUpControl = this.GetGraphicalUiElementByName("LayerUpControl") as ButtonIconRuntime;
            LayerDownControl = this.GetGraphicalUiElementByName("LayerDownControl") as ButtonIconRuntime;
            TextLayer = this.GetGraphicalUiElementByName("TextLayer") as TextRuntime;
            TilesetNumText = this.GetGraphicalUiElementByName("TilesetNumText") as TextRuntime;
            TilesetNumTextBox = this.GetGraphicalUiElementByName("TilesetNumTextBox") as TextBoxRuntime;
            TilesetButton = this.GetGraphicalUiElementByName("TilesetButton") as ButtonStandardRuntime;
            EventsButton = this.GetGraphicalUiElementByName("EventsButton") as ButtonStandardRuntime;
            EventsContainer = this.GetGraphicalUiElementByName("EventsContainer") as ContainerRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
