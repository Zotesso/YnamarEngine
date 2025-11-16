using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Interfaces;
using YnamarEditors.Models;

namespace YnamarEditors.Commands
{
    internal class MapMultipleTilesClick : ICommand
    {
        private readonly Tile _selectedTile;
        private readonly List<Point> _selectedTilesPoints;
        private readonly int _selectedTileset;
        private readonly int _mapStartTileX;
        private readonly int _mapStartTileY;

        private List<Point> _oldTilesPoints;
        private int _oldTilesetNumber;
        private int _oldMapStartTileX;
        private int _oldMapStartTileY;

        public MapMultipleTilesClick(int mapStartTileX, int mapStartTileY, List<Point> selectedTilesPoints, int selectedTileset)
        {
            _mapStartTileX = mapStartTileX;
            _mapStartTileY = mapStartTileY;
            _selectedTilesPoints = selectedTilesPoints;
            _selectedTileset = selectedTileset;
        }

        public void Execute()
        {
            _oldMapStartTileX = _mapStartTileX;
            _oldMapStartTileY = _mapStartTileY;
            _oldTilesetNumber = _selectedTileset;

            foreach (Point tilePoint in _selectedTilesPoints)
            {
                int targetX = _mapStartTileX + (tilePoint.X - _selectedTilesPoints.Min(t => t.X));
                int targetY = _mapStartTileY + (tilePoint.Y - _selectedTilesPoints.Min(t => t.Y));

                Tile selectedTile = Types.Maps[Globals.SelectedMap].Layer.ElementAt(Globals.SelectedLayer).TileMatrix[targetX, targetY];

                _selectedTile.TileX = _selectedX;
                _selectedTile.TileY = _selectedY;
                _selectedTile.TilesetNumber = _selectedTileset;

            }
        }

        public void Undo()
        {
            _selectedTile.TileX = _oldX;
            _selectedTile.TileY = _oldY;
            _selectedTile.TilesetNumber = _oldTilesetNumber;
        }
    }
}
