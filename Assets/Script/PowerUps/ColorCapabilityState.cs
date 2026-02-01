using UnityEngine;

using Script.UI; // Importante: Necesario para ver GameColor

namespace Script.PowerUps
{
    [CreateAssetMenu(menuName = "Game Architecture/Color Capability State")]
    public class ColorCapabilityState : ScriptableObject
    {
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

        public void ResetState()
        {
            IsColorAUnlocked = false;
            IsColorBUnlocked = false;
            IsColorCUnlocked = false;
        }
    }
}