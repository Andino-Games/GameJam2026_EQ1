using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement; 
using Script.UI;
using Script.PowerUps.SecretKey;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuración General")]
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;

    [Header("Librería de Sonidos")]
    public Sound[] sounds;

    [Header("Conexión con Eventos (Opcional)")]
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameCapabilityState _capabilityState;

    private void Awake()
    {
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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            if (s.outputGroup != null) 
                s.source.outputAudioMixerGroup = s.outputGroup;
            else
                s.source.outputAudioMixerGroup = s.loop ? _musicGroup : _sfxGroup;
        }
    }

    private void OnEnable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged += OnColorChangedAudio;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired += OnKeyAcquiredAudio;
        
        // Suscribirse al evento de cambio de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged -= OnColorChangedAudio;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired -= OnKeyAcquiredAudio;

        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Este método se llamará cada vez que una escena nueva termine de cargar
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        Play("BGM_Main");
    }

    private void OnColorChangedAudio(GameColor newColor)
    {
        if (newColor != GameColor.None)
            Play("ColorChange");
        else
            Play("ColorReset");
    }

    private void OnKeyAcquiredAudio()
    {
        Play("KeyPickup");
    }

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