using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Picker : MonoBehaviour
{
    PointTracker pt;

    AudioSource pickUpNoise;

    [Header("Player Emissions")]
    [SerializeField]
    GameObject shot;
    [SerializeField]
    GameObject explosion;

    [Space]
    [Header("Player Shot Speed")]
    [SerializeField]
    float shotSpeed = 0;
    float mouseXPos = 0;

    private void Start()
    {
        pickUpNoise = GetComponent<AudioSource>();
        pt = FindObjectOfType<PointTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pt.getHealth() <= 0) //if picker was killed
        {
            GameObject ex = Instantiate(explosion, transform.position, Quaternion.identity);
            ex.GetComponent<SpriteRenderer>().color = new Color32(51, 255, 0, 255);  //color of picker
            Destroy(gameObject);
        }

        if (pt.gameIsStarted == false) return;
        if (SettingsMenu.settingsOn) return;

        Move();
        Shoot();
    }

    void Move() //move with the mouse
    {
        //get mouse x pos in world space
        mouseXPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

        //clamp mouseXPos if it is outside camera bounds
        mouseXPos = Mathf.Clamp(mouseXPos, 
            Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 0f)).x,    //min value for pos
            Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, 0f)).x);   //max value for pos

        transform.position = new Vector3(mouseXPos, transform.position.y, transform.position.z);
    }

    void Shoot()  //shoot a laser
    {
        if (Input.GetMouseButtonDown(0) && pt.getAmmo() > 0)
        {
            pt.takeAmmo();
            PlayerShot s = Instantiate(shot, transform.GetChild(0).position, Quaternion.identity).GetComponent<PlayerShot>();
            s.setSpeed(shotSpeed);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Drop")  //if picked up drop, gain ammo and play noise
        {
            pt.addAmmo();
            if (PlayerPrefs.GetInt("SFXMuted") == 0)
                pickUpNoise.PlayOneShot(pickUpNoise.clip, PlayerPrefs.GetFloat("SFXVolume", 0.5f));

        }
        else if (other.gameObject.tag == "EnemyShot" || other.gameObject.tag == "Enemy")  //if enemy of shot, take damage
        {
            pt.takeHealth();
        }
    }
}
