using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class OptionsController : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public TMP_Dropdown resolutionDropdown;
    public Toggle FullScreen;

    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int CurrentResolutionIndex = 0;

        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                CurrentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = CurrentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
