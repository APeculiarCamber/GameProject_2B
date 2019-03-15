using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleHighScores : MonoBehaviour
{
    Text highScores;

    private void OnEnable()  //when enabled, create high scores
    {
        highScores = transform.GetChild(0).gameObject.GetComponent<Text>();
        highScores.text = "HIGH SCORES!!\n";
        
        for (int i = 1; i <= 10; i++)
        {
            highScores.text += i.ToString() + ")  ";
            highScores.text += PlayerPrefs.GetString("Name" + i.ToString(), "TMP") + "    ";
            highScores.text += PlayerPrefs.GetInt("HighScore" + i.ToString(), (10 - i) * 20);
            highScores.text += "\n";
        }
    }
}
