using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models;

namespace YnamarEditors
{
    public class Globals
    {
        // Constants
        public const int MAX_LAYERS = 3;
        public const int MIN_LAYERS = 0;
        public const int MAX_SPRITES = 3;
        public const int MAX_SPRITE_SHEET = 1;
        public const int MAX_ITEM_SPRITES = 3;

        // Variables
        public static bool isLoadingMap = false;
        private static int? selectedEventIndex = null;
        private static int selectedTileset = 0;
        private static int selectedSpritesheet = 0;
        private static int selectedLayer = 0;
        private static int selectedMap = 0;
        private static Npc? selectedNpc = null;

        public static int? SelectedEventIndex { get => selectedEventIndex; set => selectedEventIndex = value; }
        public static int SelectedTileset { get => selectedTileset; set => selectedTileset = value; }
        public static int SelectedSpritesheet { get => selectedSpritesheet; set => selectedSpritesheet = value; }
        public static int SelectedLayer { get => selectedLayer; set => selectedLayer = value; }
        public static int SelectedMap { get => selectedMap; set => selectedMap = value; }
        public static Npc? SelectedNpc { get => selectedNpc; set => selectedNpc = value; }


        public enum TileEventsTypes {
            Block = 1,
            Npc
        }

    }
}
