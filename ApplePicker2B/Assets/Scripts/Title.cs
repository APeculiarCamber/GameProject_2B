using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [Header("Different Screens of Title")]
    [SerializeField]
    GameObject title;
    [SerializeField]
    GameObject credits;
    [SerializeField]
    GameObject controls;
    [SerializeField]
    GameObject highScores;

    private void Start() //set title menu on
    {
        credits.SetActive(false);
        controls.SetActive(false);
        highScores.SetActive(false);
        title.SetActive(true);
    }

    public void startGame()  //go to game
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        SceneManager.LoadScene(1);
    }

    public void goBack()  //go back to title
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        title.SetActive(true);

        credits.SetActive(false);
        controls.SetActive(false);
        highScores.SetActive(false);
    }

    public void goToCredits()
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        credits.SetActive(true);

        controls.SetActive(false);
        highScores.SetActive(false);
        title.SetActive(false);
    }

    public void goToControls()
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        controls.SetActive(true);

        credits.SetActive(false);
        highScores.SetActive(false);
        title.SetActive(false);
    }

    public void goToHighScores()
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        print("GOING");

        controls.SetActive(false);
        highScores.SetActive(false);
        title.SetActive(false);

        highScores.SetActive(true);
    }
}
