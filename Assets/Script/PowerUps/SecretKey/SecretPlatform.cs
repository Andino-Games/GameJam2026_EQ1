using Script.UI;
using UnityEngine;

namespace Script.PowerUps.SecretKey
{
    public class SecretPlatform : MonoBehaviour
    {
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameCapabilityState capabilityState;
        [SerializeField] private GameColor revealColor = GameColor.ColorC;
       [SerializeField] private GameObject appearObjects;
        //private Collider2D _collider;
        //private Renderer _renderer;
        
        private bool _isVisible;
        private bool _initialized;
        

        private void Awake()
        {
            //_collider = GetComponent<Collider2D>();
            //_renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            if (colorChannel) colorChannel.OnColorChanged += OnStateChanged;
            if (capabilityState) capabilityState.OnKeyAcquired += OnSecretUnlocked;
            CheckVisibility();
        }

        private void OnDisable()
        {
            if (colorChannel) colorChannel.OnColorChanged -= OnStateChanged;
            if (capabilityState) capabilityState.OnKeyAcquired -= OnSecretUnlocked;
        }

        private void OnStateChanged(GameColor color) => CheckVisibility();
        private void OnSecretUnlocked() => CheckVisibility();

        private void CheckVisibility()
        {
            // Solo aparece si: Tienes la llave Y es el color correcto
            bool canShow = capabilityState.HasSecretKey && 
                           (colorChannel.CurrentColor == revealColor);
        
            // Inicialización silenciosa (primera vez)
            if (!_initialized)
            {
                _isVisible = canShow;
                UpdateVisuals(canShow);
                _initialized = true;
                return;
            }

            // Cambio de estado con audio
            if (canShow != _isVisible)
            {
                _isVisible = canShow;
                UpdateVisuals(canShow);

                if (AudioManager.Instance != null)
                {
                    if (canShow)
                        AudioManager.Instance.Play("PlatformAppear");   //PILAS CAMBIAR NOMBRE
                    else
                        AudioManager.Instance.Play("PlatformVanish");    //PILAS CAMBIAR NOMBRE
                }
            }
        }

        private void UpdateVisuals(bool show)
        {
            //    if (_collider) _collider.enabled = show;
            //    if (_renderer) _renderer.enabled = show;

            if (appearObjects) appearObjects.SetActive(show);

        }
    }
}