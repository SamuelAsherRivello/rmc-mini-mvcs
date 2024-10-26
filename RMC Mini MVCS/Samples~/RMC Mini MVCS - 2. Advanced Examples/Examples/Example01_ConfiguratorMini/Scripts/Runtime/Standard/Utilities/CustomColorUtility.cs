using System.Collections.Generic;
using System.Threading.Tasks;
using RMC.Mini.Samples.Configurator.Standard.Gameplay;
using UnityEngine;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
namespace RMC.Mini.Samples.Configurator.Standard
{
    public static class CustomColorUtility
    {
        //  Fields ----------------------------------------
        public static float DefaultDuration = 0.25f;
        
        //  Methods ---------------------------------------
        public static List<Color> GetRandomColorsWithoutRepeat(int count)
        {
            var randomColors = new List<Color>();
            do
            {
                var randomColor = GetRandomColor();
                if (!randomColors.Contains(randomColor))
                {
                    randomColors.Add(randomColor);
                }
            } while (randomColors.Count < count);
            return randomColors;
        }   
        
        public static List<Color> GetFullColorList()
        {
            List<Color> colors = new List<Color>();
            
            // // #1 with 6 Colors: Harmonious Colors
            // colors.Add(new Color(0.941f, 0.502f, 0.502f)); // Light Coral
            // colors.Add(new Color(0.502f, 0.800f, 0.800f)); // Medium Turquoise
            // colors.Add(new Color(0.600f, 0.800f, 0.196f)); // Yellow Green
            // colors.Add(new Color(0.392f, 0.584f, 0.929f)); // Cornflower Blue
            // colors.Add(new Color(0.933f, 0.910f, 0.667f)); // Pale Goldenrod
            // colors.Add(new Color(0.839f, 0.502f, 0.502f)); // Indian Red
            
            
            // #2 with 6 Colors: Warm and Earthy
            // colors.Add(new Color(0.855f, 0.647f, 0.125f)); // Goldenrod
            // colors.Add(new Color(0.824f, 0.412f, 0.118f)); // Peru
            // colors.Add(new Color(0.545f, 0.271f, 0.075f)); // Saddle Brown
            // colors.Add(new Color(0.718f, 0.525f, 0.043f)); // Dark Goldenrod
            // colors.Add(new Color(0.804f, 0.361f, 0.361f)); // Indian Red
            // colors.Add(new Color(0.722f, 0.525f, 0.043f)); // Dark Khaki
            
            // #3 with 6 Colors: Cool and Calm
            colors.Add(new Color(0.678f, 0.847f, 0.902f)); // Light Sky Blue
            colors.Add(new Color(0.529f, 0.808f, 0.922f)); // Deep Sky Blue
            colors.Add(new Color(0.686f, 0.933f, 0.933f)); // Pale Turquoise
            colors.Add(new Color(0.282f, 0.820f, 0.800f)); // Medium Aquamarine
            colors.Add(new Color(0.196f, 0.804f, 0.196f)); // Lime Green
            colors.Add(new Color(0.196f, 0.196f, 0.804f)); // Royal Blue
            
            return colors;
        }
        
        public static List<Color> GetDefaultColorList()
        {
            List<Color> colors = new List<Color>();
            
            //make 6 shades of very light grey monochromatic colors
            colors.Add(new Color(0.6f, 0.6f, 0.6f)); // Light grey
            colors.Add(new Color(0.85f, 0.85f, 0.85f)); // Light grey
            colors.Add(new Color(0.8f, 0.8f, 0.8f)); // Light grey
            colors.Add(new Color(0.75f, 0.75f, 0.75f)); // Light grey
            colors.Add(new Color(0.7f, 0.7f, 0.7f)); // Light grey
            colors.Add(new Color(0.65f, 0.65f, 0.65f)); // Light grey
            
            return colors;
        }
        
        /// <summary>
        /// 0 to 100 to tint, 0 to -100 to shade
        /// </summary>
        /// <param name="color"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static Color TintOrShadeColor(Color color, float amount)
        {
            amount = Mathf.Clamp(amount, -100f, 100f);

            if (amount < 0)
            {
                float factor = 1 + (amount / 100f);
                return new Color(color.r * factor, color.g * factor, color.b * factor, color.a);
            }
            else
            {
                float factor = amount / 100f;
                return new Color(color.r + (1 - color.r) * factor, color.g + (1 - color.g) * factor, color.b + (1 - color.b) * factor, color.a);
            }
        }
        
        public static Color GetRandomColor()
        {
            var colors = GetFullColorList();
            return colors[Random.Range(0, colors.Count)];
        }

        public static void SetColor(Material material, Color toColor)
        {
            material.SetColor("_Color", toColor);
        }
        
        public static Color GetColor(Material material)
        {
            return material.GetColor("_Color");
        }
        
        public static async Task SetColorAsync(Material material, Color toColor, float durationInSeconds)
        {
            float startTime = Time.time;
            float endTime = startTime + durationInSeconds;
            Color fromColor = GetColor(material);

            while (Time.time < endTime)
            {
                float elapsed = Time.time - startTime;
                float percent = Mathf.Clamp01(elapsed / durationInSeconds);
                SetColor(material, Color.Lerp(fromColor, toColor, percent));

                await Task.Yield();
            }
            
            // Ensure the final color is set to the target color
            SetColor(material, toColor);
        }
        

        public static async Task SetColorAsync(RendererSet rendererSet, Color toColor, float durationInSeconds)
        {
            foreach (Renderer renderer in rendererSet.Renderers)
            {
                //Allow color change simultaneously
                //So, don't await
                SetColorAsync(renderer.material, toColor, durationInSeconds);
            }
        }
    }
}
