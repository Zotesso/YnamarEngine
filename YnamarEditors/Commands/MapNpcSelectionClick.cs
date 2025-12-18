using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static YnamarEditors.Types;
using YnamarEditors.Models;
using YnamarEditors.Interfaces;
using System.Drawing;
using ProtoBuf;

namespace YnamarEditors.Commands
{
    internal class MapNpcSelectionClick : ICommand
    {
        private readonly MapLayer _selectedLayer;
        private readonly Npc _selectedNpc;
        private readonly Point _selectedPoint;

        private MapNpc _mapNpcToRemove;

        public MapNpcSelectionClick(MapLayer selectedLayer, Npc selectedNpc, Point selectedPoint)
        {
            _selectedLayer = selectedLayer;
            _selectedNpc = selectedNpc;
            _selectedPoint = selectedPoint;
        }

        public void Execute()
        {
            MapNpc newMapNpc = new MapNpc
            {
                Npc = _selectedNpc,
                Hp = _selectedNpc.MaxHp,
                RespawnWait = 0,
                X = _selectedPoint.X,
                Y = _selectedPoint.Y,
                Dir = 0,
            };
            _mapNpcToRemove = newMapNpc;

            _selectedLayer.MapNpc.Add(newMapNpc);
        }

        public void Undo()
        {
            _selectedLayer.MapNpc.Remove(_mapNpcToRemove);
        }
    }
}
