//Code for Styles (Container)
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
    partial class StylesRuntime:ContainerRuntime
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("Styles", typeof(StylesRuntime));
        }
        public ContainerRuntime Colors { get; protected set; }
        public ColoredRectangleRuntime Black { get; protected set; }
        public ColoredRectangleRuntime DarkGray { get; protected set; }
        public ColoredRectangleRuntime Gray { get; protected set; }
        public ColoredRectangleRuntime LightGray { get; protected set; }
        public ColoredRectangleRuntime White { get; protected set; }
        public ColoredRectangleRuntime PrimaryDark { get; protected set; }
        public ColoredRectangleRuntime Primary { get; protected set; }
        public ColoredRectangleRuntime PrimaryLight { get; protected set; }
        public ColoredRectangleRuntime Success { get; protected set; }
        public ColoredRectangleRuntime Warning { get; protected set; }
        public ColoredRectangleRuntime Danger { get; protected set; }
        public ColoredRectangleRuntime Accent { get; protected set; }
        public TextRuntime Tiny { get; protected set; }
        public TextRuntime Small { get; protected set; }
        public TextRuntime Normal { get; protected set; }
        public TextRuntime Emphasis { get; protected set; }
        public TextRuntime Strong { get; protected set; }
        public TextRuntime H3 { get; protected set; }
        public TextRuntime H2 { get; protected set; }
        public TextRuntime H1 { get; protected set; }
        public ContainerRuntime TextStyles { get; protected set; }
        public TextRuntime Title { get; protected set; }

        public StylesRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("Styles");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            Colors = this.GetGraphicalUiElementByName("Colors") as ContainerRuntime;
            Black = this.GetGraphicalUiElementByName("Black") as ColoredRectangleRuntime;
            DarkGray = this.GetGraphicalUiElementByName("DarkGray") as ColoredRectangleRuntime;
            Gray = this.GetGraphicalUiElementByName("Gray") as ColoredRectangleRuntime;
            LightGray = this.GetGraphicalUiElementByName("LightGray") as ColoredRectangleRuntime;
            White = this.GetGraphicalUiElementByName("White") as ColoredRectangleRuntime;
            PrimaryDark = this.GetGraphicalUiElementByName("PrimaryDark") as ColoredRectangleRuntime;
            Primary = this.GetGraphicalUiElementByName("Primary") as ColoredRectangleRuntime;
            PrimaryLight = this.GetGraphicalUiElementByName("PrimaryLight") as ColoredRectangleRuntime;
            Success = this.GetGraphicalUiElementByName("Success") as ColoredRectangleRuntime;
            Warning = this.GetGraphicalUiElementByName("Warning") as ColoredRectangleRuntime;
            Danger = this.GetGraphicalUiElementByName("Danger") as ColoredRectangleRuntime;
            Accent = this.GetGraphicalUiElementByName("Accent") as ColoredRectangleRuntime;
            Tiny = this.GetGraphicalUiElementByName("Tiny") as TextRuntime;
            Small = this.GetGraphicalUiElementByName("Small") as TextRuntime;
            Normal = this.GetGraphicalUiElementByName("Normal") as TextRuntime;
            Emphasis = this.GetGraphicalUiElementByName("Emphasis") as TextRuntime;
            Strong = this.GetGraphicalUiElementByName("Strong") as TextRuntime;
            H3 = this.GetGraphicalUiElementByName("H3") as TextRuntime;
            H2 = this.GetGraphicalUiElementByName("H2") as TextRuntime;
            H1 = this.GetGraphicalUiElementByName("H1") as TextRuntime;
            TextStyles = this.GetGraphicalUiElementByName("TextStyles") as ContainerRuntime;
            Title = this.GetGraphicalUiElementByName("Title") as TextRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
