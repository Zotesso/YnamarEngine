using MonoGameGum.GueDeriving;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Screens;

namespace YnamarEditors.Services.AnimationEditor
{
    internal class AnimationResourcePanelService
    {
        public void SelectedTileOnResourcePanel(ContainerRuntime contentPanel, int startX, int startY)
        {
            RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
            selectionBox.X = startX;
            selectionBox.Y = startY;
            selectionBox.Width = 0;
            selectionBox.Height = 0;
            selectionBox.Z = 3;
            selectionBox.Visible = true;
        }

        public void UpdateSelectionBox(ContainerRuntime contentPanel, Point position, Point size)
        {
            RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
            selectionBox.X = position.X;
            selectionBox.Y = position.Y;
            selectionBox.Width = Math.Max(1, size.X);
            selectionBox.Height = Math.Max(1, size.Y);
        }
    }
}
