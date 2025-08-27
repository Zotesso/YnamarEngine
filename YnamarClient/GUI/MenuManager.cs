using Myra.Graphics2D.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarClient.GUI
{
    internal class MenuManager
    {
        public static Menu menu;
        public static InterfaceGUI IGUI { get; } = new InterfaceGUI();

        public enum Menu
        {
            Login,
            Register,
            InGame,
            adminPanel
        }

        public static void ChangeMenu(Menu menu, Desktop desktop)
        {
            foreach (Panel window in InterfaceGUI.Windows)
            {
                window.Visible = false;
            }

            desktop.Root = InterfaceGUI.Windows[(int)menu];

            InterfaceGUI.Windows[(int)menu].Visible = true;
        }

        public static void ShowAdminPanel()
        {
            var messageBox = Dialog.CreateMessageBox("Muito Curto", "Sua senha e Username devem ter pelomenos 6 digitos.");
            messageBox.ShowModal(Game1.desktop);
        }
    }
}
