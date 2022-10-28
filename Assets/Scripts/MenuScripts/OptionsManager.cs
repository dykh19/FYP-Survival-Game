using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsManager : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullScreen;

    Resolution[] Resolutions;

    private void Start()
    {
        Resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        int CurrentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < Resolutions.Length; i++)
        {
            string option = Resolutions[i].width + " x " + Resolutions[i].height + " @ " + Resolutions[i].refreshRate + "hz";
            options.Add(option);

            if (Resolutions[i].width == Screen.width && Resolutions[i].height == Screen.height)
            {
                CurrentResolutionIndex = i;
            }
        }
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = CurrentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        Resolution resolution = Resolutions[ResolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, FullScreen.isOn, resolution.refreshRate);
    }

    public void SetFullScreen(bool FullScreen)
    {
        Screen.fullScreen = FullScreen;
        Debug.Log(FullScreen.ToString());
    }


    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("Music Volume", volume);
    }

   public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat("SFX Volume", volume);
    }
}
