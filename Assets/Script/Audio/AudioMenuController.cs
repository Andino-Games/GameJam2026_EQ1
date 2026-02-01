using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioMenuController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private AudioMixer _myMixer;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _masterSlider;

    private void Start()
    {
        // Cargar volúmenes guardados o ponerlos al máximo
        float masterVol = PlayerPrefs.GetFloat("Master", 1f);
        float musicVol = PlayerPrefs.GetFloat("Music", 1f);
        float sfxVol = PlayerPrefs.GetFloat("SFX", 1f);

        if (_masterSlider) { _masterSlider.value = masterVol; SetMasterVolume(); }
        if (_musicSlider) { _musicSlider.value = musicVol; SetMusicVolume(); }
        if (_sfxSlider) { _sfxSlider.value = sfxVol; SetSFXVolume(); }
    }

    public void SetMasterVolume()
    {
        SetVolume("Master", _masterSlider.value);
    }

    public void SetMusicVolume()
    {
        SetVolume("Music", _musicSlider.value);
    }

    public void SetSFXVolume()
    {
        SetVolume("SFX", _sfxSlider.value);
    }

    private void SetVolume(string paramName, float sliderValue)
    {
        // Convertimos el valor lineal del slider (0.001 a 1) a Decibelios logarítmicos (-80 a 0)
        // Usamos 0.001 en lugar de 0 para evitar log(0) que es error matemático (-infinito)
        float dbVolume = Mathf.Log10(Mathf.Max(sliderValue, 0.001f)) * 20;
        
        _myMixer.SetFloat(paramName, dbVolume);
        PlayerPrefs.SetFloat(paramName, sliderValue);
    }
}