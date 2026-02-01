using UnityEngine;
using UnityEngine.Audio; // Necesario para el Mixer

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    
    [Range(0f, 1f)]
    public float volume = 1f;
    
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    
    public bool loop;

    [HideInInspector]
    public AudioSource source;
    
    // Nuevo: Para asignar si es Musica o SFX desde el inspector
    public AudioMixerGroup outputGroup; 
}