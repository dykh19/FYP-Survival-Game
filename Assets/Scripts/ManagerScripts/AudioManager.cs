using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Audio Mangager manages the music playing in the background
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioSource audioSource;

    public AudioClip mainMenuBGM;

    public AudioClip openWorldBGM;

    public AudioClip waveBGM;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(instance);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        //audioSource.volume = 0;
        audioSource.clip = mainMenuBGM;
        audioSource.Play();
        //StartCoroutine(FadeIn());
    }

    public void PlayMainMenu()
    {
        SwitchMusic(mainMenuBGM);
    }

    public void PlayOpenWorld()
    {
        SwitchMusic(openWorldBGM);
    }

    public void PlayWave()
    {
        SwitchMusic(waveBGM);
    }

    public void SwitchMusic(AudioClip clip)
    {
        //StartCoroutine(FadeOut());
        audioSource.clip = clip;
        audioSource.Play();
        //StartCoroutine(FadeIn());
    }

/*    IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.2f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / 0.8f;

            yield return null;
        }
    }

    IEnumerator FadeIn()
    {
        float startVolume = 0.2f;

        while (audioSource.volume < 1)
        {
            audioSource.volume += startVolume * Time.deltaTime / 0.8f;

            yield return null;
        }
    }*/
}
