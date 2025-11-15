using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Interfaces;
using YnamarEditors.Models;
using static YnamarEditors.Types;

namespace YnamarEditors.Commands
{
    internal class MapTileEventClick : ICommand
    {
        private readonly Tile _selectedTile;
        private readonly TileEventStruct _selectedTileEvent;

        private byte _oldType;
        private byte _oldMoral;
        private int _oldData1;
        private int _oldData2;
        private int _oldData3;

        public MapTileEventClick(Tile selectedTile, TileEventStruct selectedEvent)
        {
            _selectedTile = selectedTile;
            _selectedTileEvent = selectedEvent;
        }

        public void Execute()
        {
            _oldType = _selectedTile.Type;
            _oldMoral = _selectedTile.Moral;
            _oldData1 = _selectedTile.Data1;
            _oldData2 = _selectedTile.Data2;
            _oldData3 = _selectedTile.Data3;

            _selectedTile.Type = _selectedTileEvent.Type;
            _selectedTile.Moral = _selectedTileEvent.Moral;
            _selectedTile.Data1 = _selectedTileEvent.Data1;
            _selectedTile.Data2 = _selectedTileEvent.Data2;
            _selectedTile.Data3 = _selectedTileEvent.Data3;
        }

        public void Undo()
        {
            _selectedTile.Type = _oldType;
            _selectedTile.Moral = _oldMoral;
            _selectedTile.Data1 = _oldData1;
            _selectedTile.Data2 = _oldData2;
            _selectedTile.Data3 = _oldData3;
        }
    }
}
