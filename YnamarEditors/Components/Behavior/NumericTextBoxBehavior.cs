using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Components.Behavior
{
    public class NumericTextBoxBehavior
    {
        public static bool IsValidNumeric(string value,int min = 0, int max = 999)
        {
            if (string.IsNullOrEmpty(value)) return false;

            if (!int.TryParse(value, out var number)) return false;

            if (number < min) return false;

            if (number > max) return false;

            return true;
        }
    }
}
