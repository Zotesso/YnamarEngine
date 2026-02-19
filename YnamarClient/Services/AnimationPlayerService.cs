using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarClient.Database.Models.Animation;

namespace YnamarClient.Services
{
    public class AnimationPlayerService
    {
        private AnimationClip _clip;
        private int _currentFrameIndex;
        private float _timer;
        private bool _isPlaying;
        private bool _loop;

        public Texture2D CurrentTexture =>
            Graphics.Spritesheets[_clip.Frames[_currentFrameIndex].TextureId];
        public AnimationFrame CurrentFrame =>
            _clip.Frames[_currentFrameIndex];

        public bool IsPlaying => _isPlaying;

        public void Play(AnimationClip clip, bool loop = false)
        {
            _clip = clip;
            _loop = loop;
            _currentFrameIndex = 0;
            _timer = 0f;
            _isPlaying = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!_isPlaying || _clip == null)
                return;

            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            var currentFrame = _clip.Frames[_currentFrameIndex];

            if (_timer >= currentFrame.Duration)
            {
                _timer = 0f;
                _currentFrameIndex++;

                if (_currentFrameIndex >= _clip.Frames.Count)
                {
                    if (_loop)
                        _currentFrameIndex = 0;
                    else
                    {
                        _isPlaying = false;
                        _currentFrameIndex = 0;
                    }
                }
            }
        }
    }
}
