using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RMC.Core.Architectures.Mini.Samples.RollABall.WithoutMini
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// The Example is the main entry point to the demo
    /// </summary>
    public class RollABallWithoutMiniExample : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        
        //  Fields ----------------------------------------
        [SerializeField] 
        private Player _player;

        [SerializeField] 
        private UI _ui;

        [SerializeField] 
        private List<Pickup> _pickups = new List<Pickup>();

        private bool _isGamePaused = true;
        private bool _isGameOver = false;
        private int _score = 0;
        private int _scoreMax = 0;
        private string _statusText = "";
        
        //  Unity Methods  --------------------------------
        protected void Start()
        {
            _ui.OnRestart.AddListener(UI_OnRestart);
            _player.OnPickup.AddListener(Player_OnPickup);
            _statusText = "Loading...";
            UpdateUI();
            Load();
        }

        
        protected void Update()
        {
            if (_isGameOver || _isGamePaused)
            {
                return;
            }
            
            foreach (Pickup pickup in _pickups)
            {
                pickup.Rotate();
            }
            
            float moveHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
            float moveVertical = Input.GetAxis ("Vertical") * Time.deltaTime;
            Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
            _player.Move(movement);
        }


        //  Methods ---------------------------------------
        private void Load()
        {
            TextAsset textAsset = Resources.Load<TextAsset>("Texts/RollABallWithoutMiniText"); //txt file

            if (textAsset == null)
            {
                Debug.LogError("Error: LoadAsync failed.");
            }
            else
            {
                _isGamePaused = false;
                _scoreMax = Int32.Parse(textAsset.ToString());
                _score = 0;
                _statusText = "Use Arrow Keys";
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            _ui.ScoreText.text = $"Score: {_score}/{_scoreMax}";
            _ui.StatusText.text = $"{_statusText}";
        }

        //  Event Handlers --------------------------------
        private void UI_OnRestart(Dialog dialogPrefab)
        {
            //Pause
            _isGamePaused = true;
            
            // Show Prompt
            Dialog dialog = GameObject.Instantiate(dialogPrefab);
            dialog.OnCancel.AddListener(() =>
            {
                //Unpause
                _isGamePaused = false;
                GameObject.Destroy(dialog.gameObject);
            });
            
            dialog.OnConfirm.AddListener(() =>
            {
                //Unpause
                _isGamePaused = false;
                GameObject.Destroy(dialog.gameObject);
                
                //Reload game
                SceneManager.LoadScene("RollABallWithoutMiniExample");
            });
        }
        
        private void Player_OnPickup(Pickup pickup)
        {
            if (_isGameOver)
            {
                return;
            }
            
            pickup.gameObject.SetActive (false);
            AudioClip audioClip = Resources.Load<AudioClip>("AudioClips/Bounce01");
            AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, 1);
            
            if (++_score >= _scoreMax)
            {
                _isGameOver = true;
                _statusText = "You Win!";
                UpdateUI();
            }
        }
    }
}