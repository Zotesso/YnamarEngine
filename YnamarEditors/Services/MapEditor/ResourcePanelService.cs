using Gum.Wireframe;
using MonoGameGum.GueDeriving;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models;
using YnamarEditors.Screens;

namespace YnamarEditors.Services.MapEditor
{
    internal class ResourcePanelService
    {
        private List<Point> _selectedTiles { get; set; } = new();
        private Point? _selectionStart {  get; set; }

        public void SelectedTileOnResourcePanel(MapEditorRuntime mapEditorScreen, int mouseX, int mouseY)
        {
            ClearSelection();
            int tileX = mouseX / 32;
            int tileY = mouseY / 32;

            _selectionStart = new Point(tileX, tileY);

            int columns = Graphics.Tilesets[Globals.SelectedTileset].Width / 32;
            var contentPanel = mapEditorScreen.ResourcePanel.InnerPanelInstance;

            _selectedTiles.Add(new Point(tileX, tileY));

            RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
            selectionBox.X = tileX * 32;
            selectionBox.Y = tileY * 32;
            selectionBox.Width = 32;
            selectionBox.Height = 32;
            selectionBox.Z = 3;
            selectionBox.Visible = true;
        }

        public void SelectedMultipleTilesOnResourcePanel(MapEditorRuntime mapEditorScreen, int mouseX, int mouseY)
        {
            int tileX = mouseX / 32;
            int tileY = mouseY / 32;

            if (_selectionStart is not null)
            {
                var contentPanel = mapEditorScreen.ResourcePanel.InnerPanelInstance;
                RectangleRuntime selectionBox = (RectangleRuntime)contentPanel.GetGraphicalUiElementByName("SelectionBox");
                int startX = Math.Min(_selectionStart.Value.X, tileX);
                int endX = Math.Max(_selectionStart.Value.X, tileX);

                int startY = Math.Min(_selectionStart.Value.Y, tileY);
                int endY = Math.Max(_selectionStart.Value.Y, tileY);

                _selectedTiles.Clear();

                for (int x = startX; x <= endX; x++)
                    for (int y = startY; y <= endY; y++)
                        _selectedTiles.Add(new Point(x, y));

                selectionBox.X = _selectionStart.Value.X * 32;
                selectionBox.Y = _selectionStart.Value.Y * 32;
                selectionBox.Width = (endX - startX + 1) * 32;
                selectionBox.Height = (endY - startY + 1) * 32;
                selectionBox.Visible = true;
            }
            else
            {
                SelectedTileOnResourcePanel(mapEditorScreen, mouseX, mouseY);
            }
        }

        public void ClearSelection()
        {
            _selectionStart = null;
            _selectedTiles?.Clear();
        }

        public List<Point> GetSelectedTiles()
        {
            return _selectedTiles;
        }
    }
}
