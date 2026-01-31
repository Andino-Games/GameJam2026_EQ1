using UnityEngine;

public class ColorReactiveObject : MonoBehaviour
{
    [Header("Arquitectura")]
    [SerializeField] private ColorEventChannel _colorChannel;

    [Header("Configuración")]
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
        
        
        // 1. Si el mundo está en estado "Base" (None), TODO debe ser visible/sólido.
        // Esto arregla que las cosas desaparezcan al inicio.
        if (newWorldColor == GameColor.None)
        {
            SetPhysical(true);
            return;
        }

        // 2. Si este objeto es "Neutro" (None), nunca debe verse afectado por colores.
        // Siempre será sólido (como una pared normal).
        if (_objectColor == GameColor.None)
        {
            SetPhysical(true);
            return;
        }

        // 3. Lógica Normal: Solo desaparece si los colores coinciden explícitamente.
        bool shouldBePhysical = (newWorldColor != _objectColor);
        SetPhysical(shouldBePhysical);
    }

    // Función auxiliar para no repetir código
    private void SetPhysical(bool isPhysical)
    {
        if (_collider != null) _collider.enabled = isPhysical;
        if (_renderer != null) _renderer.enabled = isPhysical;
    }
}