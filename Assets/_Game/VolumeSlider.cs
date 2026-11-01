using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public sealed class VolumeSlider : MonoBehaviour
{
    private Slider _slider;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        GetComponent<UIDocument>().enabled = false;
    }

    private IEnumerator Start()
    {
        yield return null;
        GetComponent<UIDocument>().enabled = true;
        var ui = GetComponent<UIDocument>();
        _slider = ui.rootVisualElement.Q<Slider>("VolumeSlider");
        
        _slider.value = Meta.Instance.Volume;
        UpgradeVolume(_slider.value);
        
        _slider.RegisterValueChangedCallback(evt =>
        {
            Meta.Instance.Volume = evt.newValue;
            UpgradeVolume(evt.newValue);
        });
    }

    private void UpgradeVolume(float value)
    {
        var label = GetComponent<UIDocument>().rootVisualElement.Q<Label>("VolumeLabel");
        var fill = _slider.Q("Fill");
        float percent = (value - _slider.lowValue) / (_slider.highValue - _slider.lowValue);
        fill.style.width = new Length(percent * 312, LengthUnit.Pixel);
        label.text = $"{(int)(percent * 100)}%";
        AudioListener.volume = value / 100;
    }
}
