using UnityEngine;

using Script.UI;
using UnityEngine.UI; // Importante: Necesario para ver GameColor

namespace Script.PowerUps
{
    [CreateAssetMenu(menuName = "Game Architecture/Color Capability State")]
    public class ColorCapabilityState : ScriptableObject
    {
        [Header("Sprites de Colores")]
        public Sprite spriteA;
        public Sprite spriteB;
        public Sprite spriteC;
        public Sprite spriteDefault;
        // Estado de desbloqueo (Solo memoria RAM, se reinicia al cerrar el juego)
        public bool IsColorAUnlocked { get; private set; }
        public bool IsColorBUnlocked { get; private set; }
        public bool IsColorCUnlocked { get; private set; }
        

        public void UnlockColor(GameColor color)
        {
            switch (color)
            {
                case GameColor.ColorA:
                    IsColorAUnlocked = true;
                   
                    break;
                case GameColor.ColorB:
                    IsColorBUnlocked = true;
                    
                    
                    break;
                case GameColor.ColorC:
                    IsColorCUnlocked = true;
                   
                    
                    break;
            }
        }

        public bool IsUnlocked(GameColor color)
        {
            return color switch
            {
                GameColor.ColorA => IsColorAUnlocked,
                GameColor.ColorB => IsColorBUnlocked,
                GameColor.ColorC => IsColorCUnlocked,
                GameColor.None => true, // El color base (gris) siempre está disponible
                _ => false
            };
        }

        public void UpdateSprite(SpriteRenderer renderer, GameColor color)
        {
            if (renderer == null) return;

            // Solo cambiamos el sprite si el color está desbloqueado
            if (IsUnlocked(color))
            {
                renderer.sprite = color switch
                {
                    GameColor.ColorA => spriteA,
                    GameColor.ColorB => spriteB,
                    GameColor.ColorC => spriteC,
                    _ => spriteDefault
                };
            }
            else
            {
                renderer.sprite = spriteDefault;
                Debug.LogWarning($"El color {color} aún está bloqueado.");
            }
        }

        public void ResetState()
        {
            IsColorAUnlocked = false;
            IsColorBUnlocked = false;
            IsColorCUnlocked = false;
        }
    }
}