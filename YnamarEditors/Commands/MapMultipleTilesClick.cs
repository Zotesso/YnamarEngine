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
        private readonly int _selectedMap;
        private readonly int _selectedLayer;

        private List<Point> _oldTilesPoints;
        private int _oldTilesetNumber;
        private int _oldMapStartTileX;
        private int _oldMapStartTileY;
        private int _oldSelectedMap;
        private int _oldSelectedLayer;

        public MapMultipleTilesClick(int mapStartTileX, int mapStartTileY, List<Point> selectedTilesPoints, int selectedTileset, int selectedMap, int selectedLayer)
        {
            _mapStartTileX = mapStartTileX;
            _mapStartTileY = mapStartTileY;
            _selectedTilesPoints = selectedTilesPoints.Select(p => new Point(p.X, p.Y)).ToList();
            _selectedTileset = selectedTileset;
            _selectedMap = selectedMap;
            _selectedLayer = selectedLayer;
        }

        public void Execute()
        {
            _oldMapStartTileX = _mapStartTileX;
            _oldMapStartTileY = _mapStartTileY;
            _oldTilesetNumber = _selectedTileset;
            _oldSelectedMap = _selectedMap;
            _oldSelectedLayer = _selectedLayer;
            _oldTilesPoints = new List<Point>();

            foreach (Point tilePoint in _selectedTilesPoints)
            {
                int targetX = _mapStartTileX + (tilePoint.X - _selectedTilesPoints.Min(t => t.X));
                int targetY = _mapStartTileY + (tilePoint.Y - _selectedTilesPoints.Min(t => t.Y));

                Tile selectedTile = Types.Maps[_selectedMap].Layer.ElementAt(_selectedLayer).TileMatrix[targetX, targetY];

                _oldTilesPoints.Add(new Point(selectedTile.TileX, selectedTile.TileY));
                
                selectedTile.TileX = tilePoint.X * 32;
                selectedTile.TileY = tilePoint.Y * 32;
                selectedTile.TilesetNumber = _selectedTileset;
            }
        }

        public void Undo()
        {
            for (int i = 0; i < _oldTilesPoints.Count; i++)
            {
                int targetX = _oldMapStartTileX + (_selectedTilesPoints[i].X - _selectedTilesPoints.Min(t => t.X));
                int targetY = _oldMapStartTileY + (_selectedTilesPoints[i].Y - _selectedTilesPoints.Min(t => t.Y));

                Tile selectedTile = Types.Maps[_oldSelectedMap].Layer.ElementAt(_oldSelectedLayer).TileMatrix[targetX, targetY];

                selectedTile.TileX = _oldTilesPoints[i].X * 32;
                selectedTile.TileY = _oldTilesPoints[i].Y * 32;
                selectedTile.TilesetNumber = _oldTilesetNumber;
            }
        }
    }
}
