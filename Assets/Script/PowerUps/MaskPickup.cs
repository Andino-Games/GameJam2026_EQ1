using UnityEngine;
using Script.UI;
using UnityEngine.UI; // Importante: Necesario para ver GameColor

namespace Script.PowerUps
{
    public class MaskPickup : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private ColorCapabilityState colorCapabilities;
        [SerializeField] private GameColor colorToUnlock;
        [SerializeField] private Image colorWheel;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            // Desbloquear el color
            colorCapabilities.UnlockColor(colorToUnlock);
            colorWheel.color = Color.white;
            // Feedback de Audio (Usamos el sistema existente)
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("KeyPickup"); // Reutilizamos el sonido de pickup o puedes crear "MaskPickup"
            }

            // Destruir el objeto
            Destroy(gameObject);
        }
    }
}