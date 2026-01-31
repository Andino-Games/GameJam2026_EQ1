using UnityEngine;

public class SecretKeyPickup : MonoBehaviour
{
    [SerializeField] private GameCapabilityState _capabilityState;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _capabilityState.UnlockSecret();
            Destroy(gameObject);
        }
    }
}