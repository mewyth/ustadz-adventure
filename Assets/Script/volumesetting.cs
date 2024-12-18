using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class volumesetting : MonoBehaviour
{
    [SerializeField] private AudioMixer mymixer;
    [SerializeField] private Slider musicSlider;

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mymixer.SetFloat("music", Mathf.Log10(volume)*20);
    }
}
