//Code for ItemEditor
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
    partial class ItemEditorRuntime:Gum.Wireframe.BindableGue
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("ItemEditor", typeof(ItemEditorRuntime));
        }
        public ColoredRectangleRuntime Background { get; protected set; }
        public ButtonIconRuntime ButtonBackScreen { get; protected set; }
        public SpriteRuntime ItemSprite { get; protected set; }
        public TextBoxRuntime ItemSpriteTextBox { get; protected set; }
        public TextRuntime ItemSpriteText { get; protected set; }
        public TextRuntime NameText { get; protected set; }
        public TextBoxRuntime NameTextBox { get; protected set; }
        public TextRuntime DescriptionText { get; protected set; }
        public TextBoxRuntime DescriptionTextBox { get; protected set; }
        public ListBoxRuntime ItemListBox { get; protected set; }
        public TextBoxRuntime SearchTextBox { get; protected set; }
        public ButtonStandardRuntime SaveButton { get; protected set; }
        public ButtonStandardRuntime NewButton { get; protected set; }
        public TextRuntime ItemTypeText { get; protected set; }
        public ComboBoxRuntime ItemTypeComboBox { get; protected set; }
        public CheckBoxRuntime CheckBoxInstance { get; protected set; }

        public ItemEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("ItemEditor");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            Background = this.GetGraphicalUiElementByName("Background") as ColoredRectangleRuntime;
            ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as ButtonIconRuntime;
            ItemSprite = this.GetGraphicalUiElementByName("ItemSprite") as SpriteRuntime;
            ItemSpriteTextBox = this.GetGraphicalUiElementByName("ItemSpriteTextBox") as TextBoxRuntime;
            ItemSpriteText = this.GetGraphicalUiElementByName("ItemSpriteText") as TextRuntime;
            NameText = this.GetGraphicalUiElementByName("NameText") as TextRuntime;
            NameTextBox = this.GetGraphicalUiElementByName("NameTextBox") as TextBoxRuntime;
            DescriptionText = this.GetGraphicalUiElementByName("DescriptionText") as TextRuntime;
            DescriptionTextBox = this.GetGraphicalUiElementByName("DescriptionTextBox") as TextBoxRuntime;
            ItemListBox = this.GetGraphicalUiElementByName("ItemListBox") as ListBoxRuntime;
            SearchTextBox = this.GetGraphicalUiElementByName("SearchTextBox") as TextBoxRuntime;
            SaveButton = this.GetGraphicalUiElementByName("SaveButton") as ButtonStandardRuntime;
            NewButton = this.GetGraphicalUiElementByName("NewButton") as ButtonStandardRuntime;
            ItemTypeText = this.GetGraphicalUiElementByName("ItemTypeText") as TextRuntime;
            ItemTypeComboBox = this.GetGraphicalUiElementByName("ItemTypeComboBox") as ComboBoxRuntime;
            CheckBoxInstance = this.GetGraphicalUiElementByName("CheckBoxInstance") as CheckBoxRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
