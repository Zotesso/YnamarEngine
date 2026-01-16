//Code for NpcEditor
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
    partial class NpcEditorRuntime:Gum.Wireframe.BindableGue
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("NpcEditor", typeof(NpcEditorRuntime));
        }
        public ColoredRectangleRuntime EditorSection { get; protected set; }
        public SpriteRuntime NpcSprite { get; protected set; }
        public TextRuntime NameText { get; protected set; }
        public TextBoxRuntime NameTextBox { get; protected set; }
        public TextRuntime LevelText { get; protected set; }
        public TextRuntime MaxHpText { get; protected set; }
        public TextRuntime AtkText { get; protected set; }
        public TextRuntime DefText { get; protected set; }
        public TextRuntime BehaviorText { get; protected set; }
        public TextRuntime RespawnTimeText { get; protected set; }
        public TextBoxRuntime LevelTextBox { get; protected set; }
        public TextBoxRuntime MaxHpTextBox { get; protected set; }
        public TextBoxRuntime AtkTextBox { get; protected set; }
        public TextBoxRuntime DefTextBox { get; protected set; }
        public TextBoxRuntime RespawnTimeTextBox { get; protected set; }
        public ListBoxRuntime BehaviorListBox { get; protected set; }
        public ListBoxRuntime NpcListBox { get; protected set; }
        public TextBoxRuntime SearchTextBox { get; protected set; }
        public ButtonStandardRuntime SaveButton { get; protected set; }
        public ButtonStandardRuntime NewButton { get; protected set; }
        public TextBoxRuntime NpcSpriteTextBox { get; protected set; }
        public TextRuntime NpcSpriteText { get; protected set; }
        public ButtonIconRuntime ButtonBackScreen { get; protected set; }

        public NpcEditorRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("NpcEditor");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            EditorSection = this.GetGraphicalUiElementByName("EditorSection") as ColoredRectangleRuntime;
            NpcSprite = this.GetGraphicalUiElementByName("NpcSprite") as SpriteRuntime;
            NameText = this.GetGraphicalUiElementByName("NameText") as TextRuntime;
            NameTextBox = this.GetGraphicalUiElementByName("NameTextBox") as TextBoxRuntime;
            LevelText = this.GetGraphicalUiElementByName("LevelText") as TextRuntime;
            MaxHpText = this.GetGraphicalUiElementByName("MaxHpText") as TextRuntime;
            AtkText = this.GetGraphicalUiElementByName("AtkText") as TextRuntime;
            DefText = this.GetGraphicalUiElementByName("DefText") as TextRuntime;
            BehaviorText = this.GetGraphicalUiElementByName("BehaviorText") as TextRuntime;
            RespawnTimeText = this.GetGraphicalUiElementByName("RespawnTimeText") as TextRuntime;
            LevelTextBox = this.GetGraphicalUiElementByName("LevelTextBox") as TextBoxRuntime;
            MaxHpTextBox = this.GetGraphicalUiElementByName("MaxHpTextBox") as TextBoxRuntime;
            AtkTextBox = this.GetGraphicalUiElementByName("AtkTextBox") as TextBoxRuntime;
            DefTextBox = this.GetGraphicalUiElementByName("DefTextBox") as TextBoxRuntime;
            RespawnTimeTextBox = this.GetGraphicalUiElementByName("RespawnTimeTextBox") as TextBoxRuntime;
            BehaviorListBox = this.GetGraphicalUiElementByName("BehaviorListBox") as ListBoxRuntime;
            NpcListBox = this.GetGraphicalUiElementByName("NpcListBox") as ListBoxRuntime;
            SearchTextBox = this.GetGraphicalUiElementByName("SearchTextBox") as TextBoxRuntime;
            SaveButton = this.GetGraphicalUiElementByName("SaveButton") as ButtonStandardRuntime;
            NewButton = this.GetGraphicalUiElementByName("NewButton") as ButtonStandardRuntime;
            NpcSpriteTextBox = this.GetGraphicalUiElementByName("NpcSpriteTextBox") as TextBoxRuntime;
            NpcSpriteText = this.GetGraphicalUiElementByName("NpcSpriteText") as TextRuntime;
            ButtonBackScreen = this.GetGraphicalUiElementByName("ButtonBackScreen") as ButtonIconRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
