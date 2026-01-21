using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Models.Animation
{
    public class AnimationClip
    {
        public int? Id { get; init; }
        public string Name { get; init; }
        public bool Loop { get; init; }
        public List<AnimationFrame> Frames { get; init; }
    }
}
