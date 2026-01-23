using Microsoft.Xna.Framework;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using YnamarEditors.Models;
using YnamarEditors.Models.Animation;
using YnamarEditors.Models.Protos;

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
                Id = 0,
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
                SourceHeight = sourceRect.Height,
                SourceWidth = sourceRect.Width,
                SourceX = sourceRect.X,
                SourceY = sourceRect.Y,
                AnimationClipId = CurrentAnimationClip.Id,
                Duration = 0.1f
            };
        }

        public static async Task<AnimationClipList> ListAnimationClips()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync("http://localhost:8080/api/animationeditor/animation/list");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            AnimationClipList animationClipsList = Serializer.Deserialize<AnimationClipList>(responseStream);
            MenuManager.StopLoadingRemoveFeedbackPanelAsync();

            return animationClipsList;
        }

        public static async Task<AnimationClip> GetAnimationClip(int animationClipId)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-protobuf")
            );

            HttpResponseMessage response = await httpClient.GetAsync($"http://localhost:8080/api/animationeditor/animation/{animationClipId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            await using var responseStream = await response.Content.ReadAsStreamAsync();
            AnimationClip animationClip = Serializer.Deserialize<AnimationClip>(responseStream);
            MenuManager.StopLoadingRemoveFeedbackPanelAsync();

            return animationClip;
        }

        public static async Task SaveAnimationClip(AnimationClip animationClip)
        {
            using HttpClient httpClient = new HttpClient();
            using MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, animationClip);
            string filePath = Path.Combine(
                    Environment.CurrentDirectory,
                    "animationClip.bin"
                );

                using FileStream fs = File.Create(filePath);
                Serializer.Serialize(fs, animationClip);
            ms.Position = 0;

            var content = new ByteArrayContent(ms.ToArray());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-protobuf");
            var response = await httpClient.PostAsync("http://localhost:8080/api/animationeditor/animation/save", content);
            MenuManager.StopLoadingAsync(response.IsSuccessStatusCode);
            response.EnsureSuccessStatusCode();
        }
    }
}
