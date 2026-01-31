using UnityEngine;

public class BackgroundListener : MonoBehaviour
{
    [Header("Conexiones")]
    [Tooltip("Arrastra aquí el mismo GlobalColorChannel que usa el GameManager")]
    [SerializeField] private ColorEventChannel _colorChannel;
    
    [Tooltip("El componente SpriteRenderer de tu fondo")]
    [SerializeField] private SpriteRenderer _backgroundSprite;

    [Header("Paleta de Colores (Visuales)")]
    // Configuración de oolores
    [SerializeField] private Color _colorBase = Color.gray;   // Color neutro
    [SerializeField] private Color _visualColorA = new Color(1f, 0.5f, 0.5f); 
    [SerializeField] private Color _visualColorB = new Color(0.5f, 0.5f, 1f); 
    [SerializeField] private Color _visualColorC = new Color(0.5f, 1f, 0.5f); 

    private void OnEnable()
    {
        // Verificar nulos para evitar errores en consola
        if (_colorChannel != null)
        {
            _colorChannel.OnColorChanged += UpdateBackgroundColor;
        }
    }

    private void OnDisable()
    {
        if (_colorChannel != null)
        {
            _colorChannel.OnColorChanged -= UpdateBackgroundColor;
        }
    }

    private void UpdateBackgroundColor(GameColor newColor)
    {
        // Asignación directa usando Switch Expression (C# 8.0+)
        // Esto traduce el "Concepto" (ColorA) a la "Realidad" (RGBA)
        Color targetColor = newColor switch
        {
            GameColor.ColorA => _visualColorA,
            GameColor.ColorB => _visualColorB,
            GameColor.ColorC => _visualColorC,
            _ => _colorBase // GameColor.None
        };

        if (_backgroundSprite != null)
        {
            _backgroundSprite.color = targetColor;
        }
    }
}