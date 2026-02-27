using UnityEngine;

namespace Script.UI
{
    public class ColorReactiveObject : MonoBehaviour
    {
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameColor objectColor;

        private Collider2D _collider;
        private Renderer _renderer;
        private SpriteRenderer sp;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<Renderer>();
            sp = GetComponent<SpriteRenderer>();
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
            Color colorTra = sp.color;
            if (_collider != null) _collider.enabled = isPhysical;
            // if (_renderer != null) _renderer.enabled = isPhysical;
            if (sp != null && isPhysical)
            {
                
                colorTra.a = 1f;
                sp.color = colorTra;
            }
            else
            {
                colorTra.a = 0.3f;
                sp.color = colorTra;
                
            }

        }
    }
}