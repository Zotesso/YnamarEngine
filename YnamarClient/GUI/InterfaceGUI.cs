using Myra.Graphics2D.UI;
using Myra.Graphics2D;
using Myra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Network;
using Microsoft.Xna.Framework;
using Gum.DataTypes;
using GumRuntime;
using Gum.Wireframe;
using MonoGameGum;

namespace YnamarClient.GUI
{
    internal class InterfaceGUI
    {
        public static List<Panel> Windows = new List<Panel>();
        private ClientTCP clienttcp = new ClientTCP();

        public void InitializeGUI(Game1 game, Desktop desktop)
        {
            MyraEnvironment.Game = game;

            CreateWindow_Login(desktop);
            CreateWindow_Register(desktop);
            CreateWindow_InGame();
        }

        public void CreateWindow(Panel panel)
        {
            Windows.Add(panel);
        }

        public void CreateWindow_Login(Desktop desktop)
        {
            ScreenSave screen = Game1.gumProject.Screens.Find(item => item.Name == "LoginScreen");
            GraphicalUiElement screenRuntime = screen.ToGraphicalUiElement();
            screenRuntime.AddToRoot();

            Panel panel = new Panel();

            Label lblLogin = new Label
            {
                Id = "lblLogin",
                Text = "Login:",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

           // panel.Widgets.Add(lblLogin);

            TextBox txtLogin = new TextBox
            {
                Margin = new Thickness(0, 40, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            txtLogin.ValueChanging += (s, a) =>
            {
                Globals.loginUsername = a.NewValue;
            };

            //panel.Widgets.Add(txtLogin);

            Label lblPassword = new Label
            {
                Margin = new Thickness(0, 80, 0, 0),
                Id = "lblPassword",
                Text = "Password:",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            //panel.Widgets.Add(lblPassword);

            TextBox txtPassword = new TextBox
            {
                Margin = new Thickness(0, 120, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            txtPassword.ValueChanging += (s, a) =>
            {
                Globals.loginPassword = a.NewValue;

            };
            //panel.Widgets.Add(txtPassword);

            // Button
            TextButton sendLogin = new TextButton
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 50),
                Text = "LOGAR"
            };

            //panel.Widgets.Add(sendLogin);

            TextButton register = new TextButton
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 20),
                Text = "CRIAR CONTA"
            };

            register.Click += (s, a) =>
            {
                MenuManager.ChangeMenu(MenuManager.Menu.Register, desktop);
            };

            sendLogin.Click += (s, a) =>
            {
                if (Globals.loginUsername == string.Empty || Globals.loginPassword == string.Empty)
                {
                    var messageBox = Dialog.CreateMessageBox("Sem dados", "Por favor, insira seu Login e sua Senha!");
                    messageBox.ShowModal(desktop);

                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Boa", "Foi pro server!");
                    messageBox.ShowModal(desktop);
                    clienttcp.SendLogin();
                }
            };

            //panel.Widgets.Add(register);

            CreateWindow(panel);
        }

        public void CreateWindow_Register(Desktop desktop)
        {

            Panel panel = new Panel();

            Label lblRegister = new Label
            {
                Id = "lblRegister",
                Text = "Username:",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.Widgets.Add(lblRegister);

            TextBox txtUsername = new TextBox
            {
                Margin = new Thickness(0, 40, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            txtUsername.ValueChanging += (s, a) =>
            {
                Globals.regUsername = a.NewValue;

            };

            panel.Widgets.Add(txtUsername);

            Label lblPassword = new Label
            {
                Margin = new Thickness(0, 80, 0, 0),
                Id = "lblPassword",
                Text = "Senha:",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.Widgets.Add(lblPassword);

            TextBox txtPassword = new TextBox
            {
                Margin = new Thickness(0, 120, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            txtPassword.ValueChanging += (s, a) =>
            {
                Globals.regPassword = a.NewValue;
            };

            panel.Widgets.Add(txtPassword);

            Label lblPasswordRepeat = new Label
            {
                Margin = new Thickness(0, 160, 0, 0),
                Id = "lblPasswordRepeat",
                Text = "Repita a Senha:",
                TextColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            panel.Widgets.Add(lblPasswordRepeat);

            TextBox txtPasswordRepeat = new TextBox
            {
                Margin = new Thickness(0, 200, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            txtPasswordRepeat.ValueChanging += (s, a) =>
            {
                Globals.regRepeatPassword = a.NewValue;
            };

            panel.Widgets.Add(txtPasswordRepeat);
            // Button
            TextButton sendRegister = new TextButton
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 50),
                Text = "CADASTRAR"
            };

            panel.Widgets.Add(sendRegister);


            TextButton back = new TextButton
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 20),
                Text = "Voltar"
            };

            back.Click += (s, a) =>
            {
                MenuManager.ChangeMenu(MenuManager.Menu.Login, desktop);
            };

            sendRegister.Click += (s, a) =>
            {
                if (Globals.regUsername == string.Empty || Globals.regPassword == string.Empty || Globals.regRepeatPassword == string.Empty)
                {
                    var messageBox = Dialog.CreateMessageBox("Sem dados", "Por favor, insira seu Login e sua Senha!");
                    messageBox.ShowModal(desktop);

                }
                else if (Globals.regPassword != Globals.regRepeatPassword)
                {
                    var messageBox = Dialog.CreateMessageBox("Digitou errado :(", "Repita a senha corretamente.");
                    messageBox.ShowModal(desktop);
                }
                else if (Globals.regPassword.Length < 6 || Globals.regUsername.Length < 6)
                {
                    var messageBox = Dialog.CreateMessageBox("Muito Curto", "Sua senha e Username devem ter pelomenos 6 digitos.");
                    messageBox.ShowModal(desktop);
                }
                else
                {
                    clienttcp.SendRegister();
                    var messageBox = Dialog.CreateMessageBox("foi", " cadastrado.");
                    messageBox.ShowModal(desktop);
                }
            };

            panel.Widgets.Add(back);
            CreateWindow(panel);
        }

        public void CreateWindow_InGame()
        {

            Panel panel = new Panel();
            CreateGameChat(panel);

            CreateWindow(panel);
        }

        private void CreateGameChat(Panel panel)
        {
            TextBox txtGameChatBox = new TextBox
            {
                Margin = new Thickness(0, 40, 0, 0),
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
            };
           
            panel.Widgets.Add(txtGameChatBox);
        }
    }
}
