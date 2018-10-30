using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown vSyncDropdown;
    public Dropdown textureQualityDropdown;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Slider masterVolumeSlider;
    public Button applyButton;
    public Button backButton;

    public AudioSource musicSource;
    public Resolution[] resolutions;
    public GameSettings gameSettings;

    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;
    public GameObject lobbyMenuHodler;

    void OnEnable()
    {
        gameSettings = new GameSettings();

        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        textureQualityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVSyncChange(); });
        musicVolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { OnSFXVolumeChange(); });
        masterVolumeSlider.onValueChanged.AddListener(delegate { OnMasterVolumeChange(); });
        applyButton.onClick.AddListener(delegate { OnApplyButtonClick(); });
        backButton.onClick.AddListener(delegate { OnBackButtonClick(); });

        resolutions = Screen.resolutions;
        foreach(Resolution resolution in resolutions) //Fills the resolution dropdown with all the resolutions available for the screen
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }
    public void OnFullscreenToggle()
    {
        gameSettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gameSettings.textureQuality = textureQualityDropdown.value;
        
    }
    public void OnVSyncChange()
    {
        QualitySettings.vSyncCount = gameSettings.vSync = vSyncDropdown.value;
    }
    public void OnMusicVolumeChange()
    {
        musicSource.volume = gameSettings.musicVolume = musicVolumeSlider.value;
    }
    public void OnSFXVolumeChange()
    {

    }
    public void OnMasterVolumeChange()
    {

    }
    public void OnApplyButtonClick()
    {
        SaveSettings();
    }
    public void OnBackButtonClick()
    {
        Debug.Log("Back");
        mainMenuHolder.SetActive(true);
        lobbyMenuHodler.SetActive(false);
        optionsMenuHolder.SetActive(false);
    }
    public void SaveSettings()
    {
        string jsonData = JsonUtility.ToJson(gameSettings, true); //Formats settings data so it is actually a readable text file
        File.WriteAllText(Application.persistentDataPath + "/gameSettings.json", jsonData); //Saves settings to appdata on computer
    }
    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gameSettings.json"));
        
        vSyncDropdown.value = gameSettings.vSync;
        textureQualityDropdown.value = gameSettings.textureQuality;
        resolutionDropdown.value = gameSettings.resolutionIndex;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        musicVolumeSlider.value = gameSettings.musicVolume;
        sfxVolumeSlider.value = gameSettings.sfxVolume;
        masterVolumeSlider.value = gameSettings.masterVolume;
        Screen.fullScreen = gameSettings.fullscreen;
    }

}
