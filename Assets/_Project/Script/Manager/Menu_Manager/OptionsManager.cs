using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using SGM;

public class OptionsManager : MonoBehaviour
{
    [Header("Sliders")]

    [SerializeField] Slider _BrightnessSlider;
    [SerializeField] Slider _EffectsSlider;
    [SerializeField] Slider _MusicSlider;

    [Header("Audio")]
    [SerializeField] AudioMixer _AudioMixer;

    [Header("Light")]
    [SerializeField] Light _MainLight;
    [SerializeField] Image _BrightnessOverlay;

    void Start()
    {

        if (_BrightnessSlider != null)
        {
            _BrightnessSlider.value = SGM.S_SaveManager.GetBrightness();
            _BrightnessSlider.onValueChanged.AddListener(ChangeBrightness);
        }

        if (_BrightnessOverlay != null || _MainLight != null)
        {
            ChangeBrightness(_BrightnessSlider.value);
        }

        if (_MusicSlider != null)
        {
            _MusicSlider.value = SGM.S_SaveManager.GetMusic();
            _MusicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        if (_EffectsSlider != null)
        {
            _EffectsSlider.value = SGM.S_SaveManager.GetEffects();
            _EffectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        }
    }

    public void ChangeBrightness(float Value)
    {
        Color c = _BrightnessOverlay.color;
        c.a = Mathf.Clamp01(Value);
        _BrightnessOverlay.color = c;
        _MainLight.intensity = Value;
        SGM.S_SaveManager.SaveBrightness(_BrightnessSlider.value);
    }
    public void ChangeMusicVolume(float Value)
    {
        float dB = Mathf.Lerp(0, -80f, Value);
        _AudioMixer.SetFloat("MusicVolume", dB);
        SGM.S_SaveManager.SaveMusic(_MusicSlider.value);
    }
    public void ChangeEffectsVolume(float Value)
    {
        float dB = Mathf.Lerp(0f, -80f, Value);
        _AudioMixer.SetFloat("EffectsVolume", dB);
        SGM.S_SaveManager.SaveEffects(_EffectsSlider.value);
    }

}
