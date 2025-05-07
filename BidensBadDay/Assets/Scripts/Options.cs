using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System;

public class Options : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioMixer musicMixer;
    public AudioMixer sfxMixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullScreenToggle;
    public GameObject mainMenu;

    public static bool onScreen;


    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        musicSlider.value = PlayerPrefs.GetFloat("Music Volume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        resolutionDropdown.value = PlayerPrefs.GetInt("Quality");
        fullScreenToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("FullScreen"));
        
        resolutionDropdown.RefreshShownValue();
    }

    private void Update()
    {
        if (gameObject.activeSelf)
        {
            onScreen = true;
        }
        else
        {
            onScreen = false;
        }

        if (onScreen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameObject.SetActive(false);
                onScreen = false;
            }
        }

    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("Music Volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("SFX Volume", volume);
    }

    public void SetQuality(int qualIndex)
    {
        QualitySettings.SetQualityLevel(qualIndex);
        PlayerPrefs.SetInt("Quality", qualIndex);
    }

    public void SetFullScreen (bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        int fullBool = Convert.ToInt32(isFullscreen);
        PlayerPrefs.SetInt("FullScreen", fullBool);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }
}
