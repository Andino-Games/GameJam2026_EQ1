using UnityEngine;

namespace Script.UI
{
    public class ColorReactiveObject : MonoBehaviour
    {
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameColor objectColor;

        private Collider2D _collider;
        private Renderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            if (colorChannel != null)
            {
                colorChannel.OnColorChanged += HandleColorChanged;
                HandleColorChanged(colorChannel.CurrentColor);
            }
        }

        private void OnDisable()
        {
            if (colorChannel != null) colorChannel.OnColorChanged -= HandleColorChanged;
        }

        private void HandleColorChanged(GameColor newWorldColor)
        {
            // Regla: Si el mundo es gris (None) o el objeto es gris, SIEMPRE visible.
            if (newWorldColor == GameColor.None || objectColor == GameColor.None)
            {
                SetPhysical(true);
                return;
            }

            // Si los colores coinciden, desaparece. Si no, aparece.
            bool shouldBePhysical = (newWorldColor != objectColor);
            SetPhysical(shouldBePhysical);
        }

        private void SetPhysical(bool isPhysical)
        {
            if (_collider != null) _collider.enabled = isPhysical;
            if (_renderer != null) _renderer.enabled = isPhysical;
        }
    }
}