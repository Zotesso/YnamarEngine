using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace YnamarServer.Database.Models
{
    internal class Character
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int Sprite { get; set; }
        public int Level { get; set; }
        public int EXP { get; set; }

        public int Map { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public byte Dir { get; set; }

        public int XOffset { get; set; }
        public int YOffset { get; set; }

        public byte Access { get; set; }
        public Account Account { get; set; } = null!;
    }
}
