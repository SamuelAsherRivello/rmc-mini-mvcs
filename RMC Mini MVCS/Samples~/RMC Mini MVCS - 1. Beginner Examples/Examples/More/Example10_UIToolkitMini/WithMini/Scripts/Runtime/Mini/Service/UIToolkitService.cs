using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RMC.Core.Architectures.Mini.Context;
using RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Model;
using RMC.Core.Architectures.Mini.Service;
using UnityEngine;
using UnityEngine.Events;
using TextAsset = UnityEngine.TextAsset;

namespace RMC.Core.Architectures.Mini.Samples.Login.WithMini.Mini.Service
{
    //  Namespace Properties ------------------------------
    public class OnLoadCompletedUnityEvent : UnityEvent<CharacterData> {}

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Service handles external data 
    /// </summary>
    public class UIToolkitService : BaseService  // Extending 'base' is optional
    {
        //  Events ----------------------------------------
        public readonly OnLoadCompletedUnityEvent OnLoaded = new OnLoadCompletedUnityEvent();
        
        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------

        //  Initialization  -------------------------------
        public override void Initialize(IContext context)
        {
            if (!IsInitialized)
            {
                base.Initialize(context);
            }
        }

        //  Methods ---------------------------------------

        public async void Load()
        {
            RequireIsInitialized();

            //For simplicity we check a local file to validate.
            //In production, call a server to validate instead
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/UIToolkitWithMiniText"); //txt file

            //Add cosmetic delay to simulate latency
            await Task.Delay(500);

            CharacterData characterData = new CharacterData();
            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                // Load format like "1,2,0,1" and parse into separate values
                string data = textAsset.ToString();
                List<string> datas = data.Split(",").ToList();

                if (string.IsNullOrEmpty(data) || datas.Count != 4)
                {
                    Debug.LogError("Error: LoadAsync failed.");
                }
                else
                {
                    characterData.HairIndex = Int32.Parse(datas[0]);
                    characterData.FaceIndex = Int32.Parse(datas[1]);
                    characterData.ShirtIndex = Int32.Parse(datas[2]);
                    characterData.BodyIndex = Int32.Parse(datas[3]);
                }
            }
            OnLoaded.Invoke(characterData);
        }
        
        
        //  Event Handlers --------------------------------

    }
}