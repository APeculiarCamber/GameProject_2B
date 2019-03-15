using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PointTracker : MonoBehaviour
{
    public bool gameIsStarted = false;
    public bool gameOver = false;

    [SerializeField]
    int maxHealth = 3;
    int health;

    Text ammoText;
    Text pointsText;
    Text healthText;
    Text gameOverText;

    int ammo = 0;
    int points = 0;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;

        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
        pointsText = GameObject.Find("PointsText").GetComponent<Text>();
        healthText = GameObject.Find("HealthText").GetComponent<Text>();
        gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
        updateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameIsStarted && !gameOver && Input.GetMouseButtonDown(0))
        {
            gameIsStarted = true;
            gameOverText.text = "";
        }
        if (gameOver && !IsInvoking())
        {
            Invoke("goToHighScores", 2);
        }
    }

    //MODIFIERS
    public void addAmmo()
    {
        ammo++;
        updateUI();
    }
    public void takeAmmo()
    {
        ammo--;
        updateUI();
    }
    public void addPoints(int p)
    {
        points += p;
        updateUI();
    }
    public void takeHealth()
    {
        if (health != 0)
            health--;

        if (health <= 0)
        {
            gameOver = true;
            gameIsStarted = false;
        }
        updateUI();
    }

    //ACCESSORS
    public int getAmmo()
    {
        return ammo;
    }
    public int getHealth()
    {
        return health;
    }

    void updateUI() //update the ui for the new vars
    {
        ammoText.text = ammo + "  :Ammo";
        pointsText.text = " Points:  " + points;
        healthText.text = "Health: " + health;
        if (gameIsStarted)
        {
            gameOverText.text = "";
        }
        else if (gameOver)
        {
            gameOverText.text = "GAME OVER!";
        }
    }

    void goToHighScores()  //game over, save score, go to game over scene
    {
        PlayerPrefs.SetInt("PlayerScore", points);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
