using Gum.Forms.Controls;
using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models.Protos;

namespace YnamarEditors.Components
{
    internal class NpcsDisplayingListBoxItem : ListBoxItem
    {
        public NpcsDisplayingListBoxItem(InteractiveGue gue) : base(gue) { }
        public override void UpdateToObject(object o)
        {
            var idAsInt = (int)o;
            // assuming this has access to Weapons:
            //var weapon = NpcList[idAsInt];
            coreText.RawText = "teste";//weapon.Name;
        }
    }
}
