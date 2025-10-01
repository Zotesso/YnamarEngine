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
        private static int selectedLayer = 0;

        public static int SelectedLayer { get => selectedLayer; set => selectedLayer = value; }
    }
}
