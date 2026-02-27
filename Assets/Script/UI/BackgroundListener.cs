using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.UI
{
    public class BackgroundListener : MonoBehaviour
    {
        [Header("Conexiones")]
        [Tooltip("Arrastra aquí el mismo GlobalColorChannel que usa el GameManager")]
        [SerializeField] private ColorEventChannel colorChannel;
    
        [Tooltip("El componente SpriteRenderer de tu fondo")]
        [SerializeField] private SpriteRenderer[] backgroundSprite;
        [SerializeField] private Tilemap[] tilemap;

        [Header("Paleta de Colores (Visuales)")]
        [SerializeField] private Color colorBase = Color.gray;
        [SerializeField] private Color visualColorA = new(1f, 0.5f, 0.5f); 
        [SerializeField] private Color visualColorB = new(0.5f, 0.5f, 1f); 
        [SerializeField] private Color visualColorC = new(0.5f, 1f, 0.5f); 

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

            // if (backgroundSprite.Length !=null)
            // {
                foreach (SpriteRenderer sprite in backgroundSprite)
                {
                    sprite.color = targetColor;
                    foreach (Tilemap tile in tilemap)
                    {
                        tile.color = targetColor;
                        
                    }
                    
                }
            // }
        }
    }
}