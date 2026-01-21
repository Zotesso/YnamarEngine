using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YnamarEditors.Models.Animation
{
    public class AnimationFrame
    {
        public int TextureId { get; init; }
        public Rectangle SourceRect { get; init; }
        public float Duration { get; init; }

    }
}
