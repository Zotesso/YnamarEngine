using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarServer.Database.Models
{
    internal class Map
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MaxMapX { get; set; }
        public int MaxMapY { get; set; }

        [Timestamp]
        public Byte[] LastUpdate { get; set; }

        public ICollection<MapLayer> Layer { get; } = new List<MapLayer>();
    }
}
