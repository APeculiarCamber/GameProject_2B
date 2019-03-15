using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    float speed;

    private void Start()
    {
        if (PlayerPrefs.GetInt("SFXMuted") == 0) //if not muted, play laser sound
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip,
                PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SettingsMenu.settingsOn)
            return;

        if (transform.position.y > 6)
        {
            Destroy(gameObject);
            return;
        }

        transform.Translate(new Vector3(0, speed * Time.fixedDeltaTime, 0));
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string oTag = other.gameObject.tag;
        if (oTag == "Dropper" || oTag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
