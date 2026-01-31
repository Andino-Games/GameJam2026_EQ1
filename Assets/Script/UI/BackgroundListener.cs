using UnityEngine;
using UnityEngine.Serialization;

namespace Script.UI
{
    public class BackgroundListener : MonoBehaviour
    {
        [FormerlySerializedAs("_colorChannel")]
        [Header("Conexiones")]
        [Tooltip("Arrastra aquí el mismo GlobalColorChannel que usa el GameManager")]
        [SerializeField] private ColorEventChannel colorChannel;
    
        [FormerlySerializedAs("_backgroundSprite")]
        [Tooltip("El componente SpriteRenderer de tu fondo")]
        [SerializeField] private SpriteRenderer backgroundSprite;

        [FormerlySerializedAs("_colorBase")]
        [Header("Paleta de Colores (Visuales)")]
        [SerializeField] private Color colorBase = Color.gray;
        [FormerlySerializedAs("_visualColorA")] [SerializeField] private Color visualColorA = new(1f, 0.5f, 0.5f); 
        [FormerlySerializedAs("_visualColorB")] [SerializeField] private Color visualColorB = new(0.5f, 0.5f, 1f); 
        [FormerlySerializedAs("_visualColorC")] [SerializeField] private Color visualColorC = new(0.5f, 1f, 0.5f); 

        private void OnEnable()
        {
            if (colorChannel)
            {
                colorChannel.OnColorChanged += UpdateBackgroundColor;
            }
        }

        private void OnDisable()
        {
            if (colorChannel)
            {
                colorChannel.OnColorChanged -= UpdateBackgroundColor;
            }
        }

        private void UpdateBackgroundColor(GameColor newColor)
        {
            Color targetColor = newColor switch
            {
                GameColor.ColorA => visualColorA,
                GameColor.ColorB => visualColorB,
                GameColor.ColorC => visualColorC,
                _ => colorBase
            };

            if (backgroundSprite)
            {
                backgroundSprite.color = targetColor;
            }
        }
    }
}