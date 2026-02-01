using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script.Canvas
{
    public class MenuPrincipal : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private string playSceneName;
        
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        }
        
        private void OnPlayButtonClicked()
        {
            SceneManager.LoadScene(playSceneName);
        }
        
        private void OnSettingsButtonClicked()
        {
            
        }
    }
}