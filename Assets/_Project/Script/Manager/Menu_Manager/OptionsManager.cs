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
    [SerializeField] Image _BrightnessOverlay;

    void Start()
    {

        if (_BrightnessSlider != null)
        {
            _BrightnessSlider.onValueChanged.AddListener(ChangeBrightness);
            _BrightnessSlider.value = SGM.S_SaveManager.GetBrightness();

        }

        if (_BrightnessOverlay != null)
        {
            ChangeBrightness(_BrightnessSlider.value);
        }

        if (_MusicSlider != null)
        {
            _MusicSlider.onValueChanged.AddListener(ChangeMusicVolume);
            _MusicSlider.value = SGM.S_SaveManager.GetMusic();

        }

        if (_EffectsSlider != null)
        {
            _EffectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
            _EffectsSlider.value = SGM.S_SaveManager.GetEffects();

        }
    }

    public void ChangeBrightness(float Value)
    {
        Color c = _BrightnessOverlay.color;
        float minBrightness = 0.2f;
        float maxBrightness = 0.8f;
        c.a = Mathf.Clamp(Value, minBrightness, maxBrightness);
        _BrightnessOverlay.color = c;
        SGM.S_SaveManager.SaveBrightness(_BrightnessSlider.value);
    }
    public void ChangeMusicVolume(float Value)
    {
        float dB = Mathf.Lerp(0, -24f, Value);
        _AudioMixer.SetFloat("MusicVolume", dB);
        SGM.S_SaveManager.SaveMusic(_MusicSlider.value);
    }
    public void ChangeEffectsVolume(float Value)
    {
        float dB = Mathf.Lerp(0f, -24f, Value);
        _AudioMixer.SetFloat("EffectsVolume", dB);
        SGM.S_SaveManager.SaveEffects(_EffectsSlider.value);
    }

}
