using Gum.DataTypes;
using Gum.Wireframe;
using GumRuntime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameGum;
using Myra;
using Myra.Graphics2D;
using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Components;
using YnamarClient.Database.Models;
using YnamarClient.Network;
using YnamarClient.Screens;
using YnamarClient.Services;
using static YnamarClient.Constants;

namespace YnamarClient.GUI
{
    internal class InterfaceGUI
    {
        public static List<Panel> Windows = new List<Panel>();
        private ClientTCP clienttcp = new ClientTCP();
        private static InventoryRuntime? _playerInventory = null;

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

        public static InventoryRuntime? PlayerInventory { get => _playerInventory; set => _playerInventory = value; }

        public void CreateWindow_Inventory()
        {
            PlayerInventory = new InventoryRuntime();

            foreach (int equipSlot in Enum.GetValues<EquipmentsEnum>())
            {
                InventoryItemRuntime invSlot = new InventoryItemRuntime();

                Item? equippedItem = Types.Players[Globals.playerIndex].EquippedItems is not null ? 
                    Types.Players[Globals.playerIndex].EquippedItems.FirstOrDefault(e => e.Slot == equipSlot)?.Item
                    : null;
                int spriteNum = equippedItem is not null ? equippedItem.Sprite : 0;
                Texture2D texture = Graphics.Graphics.Items[spriteNum];

                invSlot.SpriteInstance.Texture = texture;
                int column = equipSlot % 5;
                int row = equipSlot / 5;

                invSlot.X = 50 + (column * 32) + ((column + 1) * 5);
                invSlot.Y = 50 + (row * 32) + ((row + 1) * 15);

                invSlot.Click += (_, _) =>
                {
                    if (equippedItem is null) return;

                    ItemService.handleItemUsed(equippedItem.Id);
                };

                PlayerInventory.Children.Add(invSlot);
            }

            foreach (var slot in Types.Players[Globals.playerIndex].Inventory.Slots.Select((value, i) => new { i, value }))
            {
                Texture2D texture = Graphics.Graphics.Items[slot.value.Item.Sprite];

                InventoryItemRuntime invSlot = new InventoryItemRuntime();

                invSlot.SpriteInstance.Texture = texture;
                int column = slot.i % 5;
                int row = slot.i / 5;

                invSlot.X = 50 + (column * 32) + ((column + 1) * 5);
                invSlot.Y = 200 + (row * 32) + ((row + 1) * 15);

                PlayerInventory.Name = "InventoryRuntime";

                invSlot.Click += (_, _) =>
                {
                    ItemService.handleItemUsed((int)slot.value.ItemId);
                };

                PlayerInventory.Children.Add(invSlot);
            }

            PlayerInventory.AddToRoot();
        }

        //public void CloseWindow_Inventory(MenuManager menuManager)
        //{
        //    var inventory = menuManager.GetCurrentScreen().GetGraphicalUiElementByName("InventoryRuntime");

        //    if (inventory is not null) 
        //    {
        //        inventory.RemoveFromRoot();
        //    }
        //    OpenedInventory = false;
        //}

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
