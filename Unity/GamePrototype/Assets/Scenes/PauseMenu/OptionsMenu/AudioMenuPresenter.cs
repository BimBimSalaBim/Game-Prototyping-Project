using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioMenuPresenter
{
    private Slider masterSlider;
    private Label masterLabel;

    private Slider effectSlider;
    private Label effectLabel;

    private Slider musicSlider;
    private Label musicLabel;
    public AudioMenuPresenter(VisualElement root) {
        masterSlider = root.Q<Slider>("MasterSlider");
        masterLabel = root.Q<Label>("MasterLabel");
        masterSlider.value = AudioListener.volume*100;
        masterLabel.text = (AudioListener.volume * 100).ToString();
        masterSlider.RegisterValueChangedCallback(v => {
            updateLabel(masterLabel, v.newValue);
            MasterVolume(v.newValue);
        });

        effectSlider = root.Q<Slider>("EffectsSlider");
        effectLabel = root.Q<Label>("EffectLabel");
        //effectSlider.value = AudioListener.volume * 100;
        //effectLabel.text = (AudioListener.volume * 100).ToString();
        effectSlider.RegisterValueChangedCallback(v => {
            updateLabel(effectLabel, v.newValue);
            EffectVolume();
        });

        musicSlider = root.Q<Slider>("MusicSlider");
        musicLabel = root.Q<Label>("MusicLabel");
        //musicSlider.value = AudioListener.volume * 100;
        //musicLabel.text = (AudioListener.volume * 100).ToString();
        musicSlider.RegisterValueChangedCallback(v => {
            updateLabel(musicLabel, v.newValue);
            MusicVolume();
        });
    }

    void MasterVolume(float volume) {
        AudioListener.volume = (float)Decimal.Round((decimal)volume/100, 3);
    }

    void EffectVolume() {
        //todo
    }

    void MusicVolume() {
        //todo
    }

    void updateLabel(Label label, float volume) {
        label.text = volume.ToString();
    }
}
