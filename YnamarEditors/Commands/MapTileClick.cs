using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Interfaces;
using YnamarEditors.Models;

namespace YnamarEditors.Commands
{
    internal class MapTileClick : ICommand
    {
        private readonly Tile _selectedTile;
        private readonly int _selectedX;
        private readonly int _selectedY;
        private readonly int _selectedTileset;

        private int _oldX;
        private int _oldY;
        private int _oldTilesetNumber;

        public MapTileClick(Tile selectedTile, int selectionBoxY, int selectionBoxX, int selectedTileset) 
        {
            _selectedTile = selectedTile;
            _selectedX = selectionBoxX;
            _selectedY = selectionBoxY;
            _selectedTileset = selectedTileset;
        }

        public void Execute()
        {
            _oldX = _selectedTile.TileX;
            _oldY = _selectedTile.TileY;
            _oldTilesetNumber = _selectedTile.TilesetNumber;
            _selectedTile.TileX = _selectedX;
            _selectedTile.TileY = _selectedY;
            _selectedTile.TilesetNumber = _selectedTileset;
        }

        public void Undo()
        {
            _selectedTile.TileX = _oldX;
            _selectedTile.TileY = _oldY;
            _selectedTile.TilesetNumber = _oldTilesetNumber;
        }
    }
}
