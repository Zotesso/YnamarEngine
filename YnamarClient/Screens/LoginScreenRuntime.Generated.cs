//Code for LoginScreen
using GumRuntime;
using YnamarClient.Components;
using Gum.Converters;
using Gum.DataTypes;
using Gum.Managers;
using Gum.Wireframe;

using RenderingLibrary.Graphics;

using System.Linq;
using MonoGameGum.GueDeriving;

namespace YnamarClient.Screens
{
    partial class LoginScreenRuntime:Gum.Wireframe.BindableGue
    {
        [System.Runtime.CompilerServices.ModuleInitializer]
        public static void RegisterRuntimeType()
        {
            GumRuntime.ElementSaveExtensions.RegisterGueInstantiationType("LoginScreen", typeof(LoginScreenRuntime));
        }
        public SpriteRuntime SpriteInstance { get; protected set; }
        public ColoredRectangleRuntime ColoredRectangleInstance { get; protected set; }
        public TextRuntime TextInstance { get; protected set; }
        public TextRuntime TextInstance1 { get; protected set; }
        public TextRuntime TextInstance2 { get; protected set; }
        public TextBoxRuntime TextBoxInstance1 { get; protected set; }
        public TextBoxRuntime TextBoxInstance { get; protected set; }
        public ButtonStandardIconRuntime ButtonStandardIconInstance { get; protected set; }

        public LoginScreenRuntime(bool fullInstantiation = true, bool tryCreateFormsObject = true)
        {
            if(fullInstantiation)
            {
                var element = ObjectFinder.Self.GetElementSave("LoginScreen");
                element?.SetGraphicalUiElement(this, global::RenderingLibrary.SystemManagers.Default);
            }



        }
        public override void AfterFullCreation()
        {
            SpriteInstance = this.GetGraphicalUiElementByName("SpriteInstance") as SpriteRuntime;
            ColoredRectangleInstance = this.GetGraphicalUiElementByName("ColoredRectangleInstance") as ColoredRectangleRuntime;
            TextInstance = this.GetGraphicalUiElementByName("TextInstance") as TextRuntime;
            TextInstance1 = this.GetGraphicalUiElementByName("TextInstance1") as TextRuntime;
            TextInstance2 = this.GetGraphicalUiElementByName("TextInstance2") as TextRuntime;
            TextBoxInstance1 = this.GetGraphicalUiElementByName("TextBoxInstance1") as TextBoxRuntime;
            TextBoxInstance = this.GetGraphicalUiElementByName("TextBoxInstance") as TextBoxRuntime;
            ButtonStandardIconInstance = this.GetGraphicalUiElementByName("ButtonStandardIconInstance") as ButtonStandardIconRuntime;
            CustomInitialize();
        }
        //Not assigning variables because Object Instantiation Type is set to By Name rather than Fully In Code
        partial void CustomInitialize();
    }
}
