using UnityEngine;

namespace Script.PowerUps.SecretKey
{
    public class SecretKeyPickup : MonoBehaviour
    {
        [SerializeField] private GameCapabilityState capabilityState;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            capabilityState.UnlockSecret();
            Destroy(gameObject);
        }
    }
}