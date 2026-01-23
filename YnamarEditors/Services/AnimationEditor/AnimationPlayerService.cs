using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models.Animation;

namespace YnamarEditors.Services.AnimationEditor
{
    public class AnimationPlayerService
    {
        private AnimationClip _clip;
        private int _frameIndex;
        private float _timer;
        private bool isPlaying = false;
        public event Action<AnimationFrame>? FrameChanged;

        public AnimationFrame? CurrentFrame
        {
            get
            {
                if (_clip == null || _clip.Frames.Count == 0)
                    return null;

                return _clip.Frames[_frameIndex];
            }
        }

        public bool IsPlaying { get => isPlaying; set => isPlaying = value; }

        public void Play(AnimationClip clip)
        {
            _clip = clip;
            _frameIndex = 0;
            _timer = 0f;
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            _clip = null;
            _frameIndex = 0;
            _timer = 0f;
        }

        public void Update(float deltaTime)
        {
            if (_clip == null || _clip.Frames.Count == 0)
                return;

            _timer += deltaTime;

            while (_timer >= CurrentFrame.Duration)
            {
                _timer -= CurrentFrame.Duration;
                _frameIndex++;

                if (_frameIndex >= _clip.Frames.Count)
                {
                    if (_clip.Loop)
                        _frameIndex = 0;
                    else
                        _frameIndex = _clip.Frames.Count - 1;
                }
                
                FrameChanged?.Invoke(CurrentFrame);
            }
        }
    }
}
