using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors
{
    public class Globals
    {
        // Constants
        public const int MAX_LAYERS = 3;
        public const int MIN_LAYERS = 0;

        // Variables
        public static bool isLoadingMap = false;
        private static int selectedTileset = 0;
        private static int selectedLayer = 0;
        private static int selectedMap = 0;

        public static int SelectedTileset { get => selectedTileset; set => selectedTileset = value; }
        public static int SelectedLayer { get => selectedLayer; set => selectedLayer = value; }
        public static int SelectedMap { get => selectedMap; set => selectedMap = value; }

    }
}
