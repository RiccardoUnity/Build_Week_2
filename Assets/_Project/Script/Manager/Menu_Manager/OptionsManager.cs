using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

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
            _BrightnessSlider.onValueChanged.AddListener(ChangeBrightness);

        if (_BrightnessOverlay != null || _MainLight != null)
            ChangeBrightness(_BrightnessSlider.value);

        if (_MusicSlider != null)
            _MusicSlider.onValueChanged.AddListener(ChangeMusicVolume);

        if (_EffectsSlider != null)
            _EffectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);

    }

    public void ChangeBrightness(float Value)
    {
        Color c = _BrightnessOverlay.color;
        c.a = Mathf.Clamp01(Value);
        _BrightnessOverlay.color = c;
        _MainLight.intensity = Value;
    }
    public void ChangeMusicVolume(float Value)
    {
        float dB = Mathf.Lerp(0, -80f, Value);
        _AudioMixer.SetFloat("MusicVolume", dB);
    }
    public void ChangeEffectsVolume(float Value)
    {
        float dB = Mathf.Lerp(0f, -80f, Value);
        _AudioMixer.SetFloat("EffectsVolume", dB);
    }
}
