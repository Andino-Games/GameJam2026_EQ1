using System;
using UnityEngine;
using UnityEngine.Audio;
using Script.UI; // Para ver los Event Channels
using Script.PowerUps.SecretKey; // Para ver la Llave

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración General")]
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;

    [Header("Librería de Sonidos")]
    public Sound[] sounds; // Unifiqué Music y SFX en una sola lista para simplificar búsqueda

    [Header("Conexión con Eventos (Opcional)")]
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameCapabilityState _capabilityState;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Generar AudioSources automáticamente
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            // Asignar grupo del Mixer (Si no tiene uno específico en el Sound, usa los generales)
            if (s.outputGroup != null) 
                s.source.outputAudioMixerGroup = s.outputGroup;
            else
                s.source.outputAudioMixerGroup = s.loop ? _musicGroup : _sfxGroup;
        }
    }

    private void OnEnable()
    {
        // Suscripción automática a eventos del juego
        if (_colorChannel != null) _colorChannel.OnColorChanged += OnColorChangedAudio;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired += OnKeyAcquiredAudio;
    }

    private void OnDisable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged -= OnColorChangedAudio;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired -= OnKeyAcquiredAudio;
    }

    private void Start()
    {
        // Ejemplo: Iniciar música de fondo
        Play("BGM_Main");   //PILAS COLOCAR NOMBRE
    }

    // --- REACCIONES AUTOMÁTICAS ---
    private void OnColorChangedAudio(GameColor newColor)
    {
        if (newColor != GameColor.None)
            Play("ColorChange"); //PILAS COLOCAR NOMBRE
        else
            Play("ColorReset");  //PILAS COLOCAR NOMBRE
    }

    private void OnKeyAcquiredAudio()
    {
        Play("KeyPickup");
    }

    // --- SISTEMA LIBRE POR STRINGS ---
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) return;
        s.source.Stop();
    }
}