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
using Microsoft.Xna.Framework.Graphics;
using Gum.DataTypes;
using GumRuntime;
using Gum.Wireframe;
using MonoGameGum;
using YnamarClient.Screens;
using YnamarClient.Database.Models;
using YnamarClient.Components;

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

        public void CreateWindow_Inventory()
        {
            InventoryRuntime inventoryRuntime = new InventoryRuntime();

            foreach (var slot in Types.Player[Globals.playerIndex].Inventory.Slots.Select((value, i) => new { i, value }))
            {
                Texture2D texture = Graphics.Items[0]; //Atualizar pra pegar o spriteNum do item.

                InventoryItemRuntime invSlot = new InventoryItemRuntime();

                invSlot.SpriteInstance.Texture = texture;
                int column = slot.i % 5;
                int row = slot.i / 5;

                invSlot.X = 50 + (column * 32) + ((column + 1) * 5);
                invSlot.Y = 50 + (row * 32) + ((row + 1) * 15);

                inventoryRuntime.Children.Add(invSlot);
            }
            
            inventoryRuntime.AddToRoot();
        }

        public void CreateWindow_Login(Desktop desktop)
        {
            var screeen = new LoginScreenRuntime();
            screeen.ButtonStandardIconInstance.Click += (_, _) =>
            {
                if (screeen.TextBoxInstance.Text == string.Empty || screeen.TextBoxInstance1.Text == string.Empty)
                {
                    var messageBox = Dialog.CreateMessageBox("Sem dados", "Por favor, insira seu Login e sua Senha!");
                    messageBox.ShowModal(desktop);

                }
                else
                {
                    var messageBox = Dialog.CreateMessageBox("Boa", "Foi pro server!");
                    messageBox.ShowModal(desktop);
                    clienttcp.SendLogin(screeen.TextBoxInstance.Text, screeen.TextBoxInstance1.Text);
                }
            };

            screeen.AddToRoot();

            Panel panel = new Panel();
            
            CreateWindow(panel);
            
        }

        public void CreateWindow_Register(Desktop desktop)
        {
           Panel panel = new Panel();
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
                HorizontalAlignment = Myra.Graphics2D.UI.HorizontalAlignment.Left,
                VerticalAlignment = Myra.Graphics2D.UI.VerticalAlignment.Bottom,
            };
           
            panel.Widgets.Add(txtGameChatBox);
        }
    }
}
