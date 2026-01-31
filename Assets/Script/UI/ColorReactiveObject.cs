using UnityEngine;

public class ColorReactiveObject : MonoBehaviour
{
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameColor _objectColor;

    private Collider2D _collider;
    private Renderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        if (_colorChannel != null)
        {
            _colorChannel.OnColorChanged += HandleColorChanged;
            HandleColorChanged(_colorChannel.CurrentColor);
        }
    }

    private void OnDisable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged -= HandleColorChanged;
    }

    private void HandleColorChanged(GameColor newWorldColor)
    {
        // Regla: Si el mundo es gris (None) o el objeto es gris, SIEMPRE visible.
        if (newWorldColor == GameColor.None || _objectColor == GameColor.None)
        {
            SetPhysical(true);
            return;
        }

        // Si los colores coinciden, desaparece. Si no, aparece.
        bool shouldBePhysical = (newWorldColor != _objectColor);
        SetPhysical(shouldBePhysical);
    }

    private void SetPhysical(bool isPhysical)
    {
        if (_collider != null) _collider.enabled = isPhysical;
        if (_renderer != null) _renderer.enabled = isPhysical;
    }
}