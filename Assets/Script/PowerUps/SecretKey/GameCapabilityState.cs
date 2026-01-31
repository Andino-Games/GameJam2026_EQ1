using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Game Architecture/Capability State")]
public class GameCapabilityState : ScriptableObject
{
    // ¿El jugador ya tiene la mascara?
    public bool HasSecretKey { get; private set; }

    // Evento para avisar que acabamos de conseguir la llave
    public event Action OnKeyAcquired;

    // Método para resetear al iniciar el juego (importante para testing)
    public void ResetState()
    {
        HasSecretKey = false;
    }

    // Método que llama la llave al ser recogida
    public void UnlockSecret()
    {
        if (!HasSecretKey)
        {
            HasSecretKey = true;
            OnKeyAcquired?.Invoke();
            Debug.Log("¡Poder desbloqueado! Estado guardado.");
        }
    }
}