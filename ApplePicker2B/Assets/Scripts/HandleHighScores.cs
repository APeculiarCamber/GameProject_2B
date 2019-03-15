using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HandleHighScores : MonoBehaviour
{
    Text hsText;
    Text playerScore;
    InputField inputName;
    GameObject SubmitButton;

    // Start is called before the first frame update
    void Start()
    {
        hsText = transform.GetChild(0).gameObject.GetComponent<Text>();
        hsText.text = "";
        playerScore = transform.GetChild(1).gameObject.GetComponent<Text>();
        playerScore.text = $"YOUR SCORE IS\n{PlayerPrefs.GetInt("PlayerScore", 0)}"
            + "\nENTER YOUR NAME:";
        inputName = transform.GetChild(2).gameObject.GetComponent<InputField>();
        SubmitButton = transform.GetChild(3).gameObject;
    }
    
    public void submitName()  //submit name and score to high scores
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        if (inputName.text == "")
        {
            return;
        }
        adjustHighScoresBoard(inputName.text, PlayerPrefs.GetInt("PlayerScore", 0));

    }

    void adjustHighScoresBoard(string newName, int newScore)  //adjust highscores board to new score
    {
        if (newScore >= PlayerPrefs.GetInt("HighScore10", 0))
        {
            int pos;
            for (pos = 10; pos > 0; pos--)
            {
                if (newScore < PlayerPrefs.GetInt("HighScore" + pos.ToString(), (10 - pos) * 20))
                {
                    pos++;
                    break;
                }
            }
            pos = pos == 0 ? 1 : pos;

            //change high scores
            int lastScore = PlayerPrefs.GetInt("HighScore" + pos.ToString(), (10 - pos) * 20);
            string lastName = PlayerPrefs.GetString("Name" + pos.ToString(), "TMP");
            PlayerPrefs.SetInt("HighScore" + pos.ToString(), newScore);
            PlayerPrefs.SetString("Name" + pos.ToString(), newName);
            pos++;

            while (pos != 11)
            {
                int nextScore = PlayerPrefs.GetInt("HighScore" + pos.ToString(), (10 - pos) * 20);
                string nextName = PlayerPrefs.GetString("Name" + pos.ToString(), "TMP");
                PlayerPrefs.SetInt("HighScore" + pos.ToString(), lastScore);
                PlayerPrefs.SetString("Name" + pos.ToString(), lastName);
                lastScore = nextScore;
                lastName = nextName;
                pos++;
            }
        }
        playerScore.gameObject.SetActive(false);
        inputName.gameObject.SetActive(false);
        SubmitButton.gameObject.SetActive(false);

        string highScores = "HIGH SCORES!!\n";
        for (int i = 1; i <= 10; i++)
        {
            highScores += i.ToString() + ")  ";
            highScores += PlayerPrefs.GetString("Name" + i.ToString(), "TMP");
            highScores += "   ";
            highScores += PlayerPrefs.GetInt("HighScore" + i.ToString(), (10 - i) * 20);
            highScores += "\n";
        }
        hsText.text = highScores;
    }

    public void goBackToMenu() //go to title
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        SceneManager.LoadScene(0);
    }

    public void goBackToPlay()  //go to play
    {
        if (SettingsMenu.settingsOn)
            SettingsMenu.instance.toggleDropdown();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
