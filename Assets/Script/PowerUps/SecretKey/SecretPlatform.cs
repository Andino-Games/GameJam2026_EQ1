using Script.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.PowerUps.SecretKey
{
    public class SecretPlatform : MonoBehaviour
    {
        [FormerlySerializedAs("_colorChannel")] [SerializeField] private ColorEventChannel colorChannel;
        [FormerlySerializedAs("_capabilityState")] [SerializeField] private GameCapabilityState capabilityState;
        [FormerlySerializedAs("_revealColor")] [SerializeField] private GameColor revealColor = GameColor.ColorC;

        private Collider2D _collider;
        private Renderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<Renderer>();
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
        
            if (_collider) _collider.enabled = canShow;
            if (_renderer) _renderer.enabled = canShow;
        }
    }
}