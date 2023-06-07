using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini
{
    /// <summary>
    /// Helpful extensions for <see cref="UIDocument"/>.
    /// </summary>
    public static class UIDocumentExtensions
    {

        public static Sprite GetSpriteForVisualElement(this UIDocument uiDocument, string name)
        {
            return uiDocument.GetVisualElement(name).style.backgroundImage.value.sprite;
        }
        
        public static VisualElement GetVisualElement(this UIDocument uiDocument, string name)
        {
            return uiDocument.rootVisualElement.Q<VisualElement>(name);
        }        
        
        public static void SetSpriteForVisualElement(this UIDocument uiDocument, string name, Sprite sprite)
        {
            uiDocument.GetVisualElement(name).style.backgroundImage =
                new StyleBackground(sprite);
        }

        public static async void SetSpriteForVisualElementAsync(this UIDocument uiDocument, string name, 
            Sprite sprite, string offClassName, string onClassName, float durationSeconds)
        {

            var visualElement = uiDocument.GetVisualElement(name);
            
            if (visualElement.ClassListContains(onClassName))
            {
                visualElement.RemoveFromClassList(onClassName);
            }

            visualElement.AddToClassList(offClassName);

            //TODO: load css duration, x.resolvedStyle.transitionDuration.First().value
            // Wait For: Fade out
            await Task.Delay((int)(durationSeconds*1000/2));

            //change
            uiDocument.GetVisualElement(name).style.backgroundImage =
                new StyleBackground(sprite);
            
            // Wait For: 1 frame
            await Task.Delay(100);

            if (visualElement.ClassListContains(offClassName))
            {
                visualElement.RemoveFromClassList(offClassName);
            }

            visualElement.AddToClassList(onClassName);

            // Wait For: Fade in
            await Task.Delay((int)(durationSeconds*1000/2));
        }

    }
}