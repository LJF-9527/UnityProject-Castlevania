using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider_UI : MonoBehaviour
{
    public Slider slider;

    [SerializeField] private AudioMixer audioMixer;
    public string parametr;
    [SerializeField] private float multiplier;
    public void SliderValue(float _value) => audioMixer.SetFloat(parametr, Mathf.Log10(_value) * multiplier);
    public void LoadSlider(float _value)
    {
        if(_value>=0.0001f)
        {
            slider.value = _value;
        }
    }
}
