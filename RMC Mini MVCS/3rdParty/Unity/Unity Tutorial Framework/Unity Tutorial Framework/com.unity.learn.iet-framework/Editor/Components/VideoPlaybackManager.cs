using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Unity.Tutorials.Core.Editor
{
    internal class VideoPlaybackManager
    {
        struct CacheEntry
        {
            public Texture2D Texture2D;
            public VideoPlayer VideoPlayer;
            public RenderTexture RenderTexture;
        }

        // NOTE Static reference fixes a peculiar NRE issue when a tutorial which has Window Layout set
        // is exited by Tutorials > Show Tutorials instead of exiting the tutorial regularly.
        static GameObject m_GameObject;
        Dictionary<VideoClip, CacheEntry> m_Cache = new Dictionary<VideoClip, CacheEntry>();

        public void OnEnable()
        {
            if (!m_GameObject)
            {
                m_GameObject = new GameObject() { hideFlags = HideFlags.HideAndDontSave };
            }
        }

        public void OnDisable()
        {
            ClearCache();
            Object.DestroyImmediate(m_GameObject);
            m_GameObject = null;
        }

        public Texture2D GetTextureForVideoClip(VideoClip videoClip)
        {
            CacheEntry cacheEntry;
            if (!m_Cache.TryGetValue(videoClip, out cacheEntry))
            {
                var renderTexture = new RenderTexture((int)videoClip.width, (int)videoClip.height, 32);
                renderTexture.hideFlags = HideFlags.HideAndDontSave;
                renderTexture.Create();

                var videoPlayer = m_GameObject.AddComponent<VideoPlayer>();
                videoPlayer.clip = videoClip;
                videoPlayer.isLooping = true;
                videoPlayer.renderMode = VideoRenderMode.RenderTexture;
                videoPlayer.targetTexture = renderTexture;
                videoPlayer.Play();

                cacheEntry.RenderTexture = renderTexture;
                cacheEntry.VideoPlayer = videoPlayer;
                m_Cache.Add(videoClip, cacheEntry);
            }

            if (cacheEntry.Texture2D == null)
                cacheEntry.Texture2D = new Texture2D(cacheEntry.RenderTexture.width, cacheEntry.RenderTexture.height, TextureFormat.RGBA32, false);

            TextureToTexture2D(cacheEntry.RenderTexture, ref cacheEntry.Texture2D);
            m_Cache[videoClip] = cacheEntry;

            return cacheEntry.Texture2D;
        }

        static void TextureToTexture2D(Texture texture, ref Texture2D texture2D)
        {
            RenderTexture currentRT = RenderTexture.active;
            RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
            Graphics.Blit(texture, renderTexture);

            RenderTexture.active = renderTexture;
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();

            RenderTexture.active = currentRT;
            RenderTexture.ReleaseTemporary(renderTexture);
        }

        public void ClearCache()
        {
            foreach (var cacheEntry in m_Cache.Values)
            {
                Object.DestroyImmediate(cacheEntry.Texture2D);
                Object.DestroyImmediate(cacheEntry.VideoPlayer);
                Object.DestroyImmediate(cacheEntry.RenderTexture);
            }

            m_Cache.Clear();
        }
    }
}
