using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public static SettingsMenu instance;

    [SerializeField]
    GameObject settings;
    
    Slider musicSlider;
    Toggle musicToggle;
    Slider sfxSlider;
    Toggle sfxToggle;

    public static bool settingsOn = false;

    float musicVolume;
    float sfxVolume;
    int musicMuted;
    int sfxMuted;  

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0);  //0 = false, 1 = true
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0);  //0 = false, 1 = true

        musicSlider = settings.transform.GetChild(1).gameObject.GetComponent<Slider>();
        musicToggle = settings.transform.GetChild(2).gameObject.GetComponent<Toggle>();
        sfxSlider = settings.transform.GetChild(3).gameObject.GetComponent<Slider>();
        sfxToggle = settings.transform.GetChild(4).gameObject.GetComponent<Toggle>();

        musicSlider.value = musicVolume;
        musicToggle.isOn = musicMuted > 0;
        sfxSlider.value = sfxVolume;
        sfxToggle.isOn = sfxMuted > 0;

        //set delegates
        musicSlider.onValueChanged.AddListener(
            delegate { PlayerPrefs.SetFloat("MusicVolume", musicSlider.value); });
        musicToggle.onValueChanged.AddListener(
            delegate { PlayerPrefs.SetInt("MusicMuted", musicToggle.isOn ? 1 : 0); });
        sfxSlider.onValueChanged.AddListener(
            delegate { PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value); });
        sfxToggle.onValueChanged.AddListener(
            delegate { PlayerPrefs.SetInt("SFXMuted", sfxToggle.isOn ? 1 : 0); });
    }

    public void toggleDropdown()  //toggle
    {
        settings.SetActive(!settings.activeSelf);
        settingsOn = settings.activeSelf;
    }
}
