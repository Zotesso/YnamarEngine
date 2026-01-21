using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models.Animation;

namespace YnamarEditors.Services.AnimationEditor
{
    internal class AnimationEditorService
    {
        private AnimationClip _currentAnimationClip;

        public AnimationClip CurrentAnimationClip 
        { 
            get { return _currentAnimationClip; } 
            set { _currentAnimationClip = value; }
        }

        public AnimationEditorService()
        {
            initAnimationClip();
        }

        public void initAnimationClip()
        {
            _currentAnimationClip = new AnimationClip 
            { 
                Id = null,
                Name = "",
                Loop = false,
                Frames = new List<AnimationFrame>()
            };
        }

        public void AddFrame(AnimationFrame frame)
        {
            _currentAnimationClip.Frames.Add(frame);
        }

        public void RemoveFrame(int index)
        {
            if (index >= 0 && index < _currentAnimationClip.Frames.Count)
            {
                _currentAnimationClip.Frames.RemoveAt(index);
            }
        }

        public AnimationFrame CreateNewFrame(int textureId, Rectangle sourceRect)
        {
            return new AnimationFrame
            {
                TextureId = textureId,
                SourceRect = sourceRect,
                Duration = 0.1f
            };
        }
    }
}
