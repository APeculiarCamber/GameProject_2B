using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music = GetComponent<AudioSource>();  //get the music source
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("MusicMuted", 0) == 0)
            music.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);  //if not muted continue playing the music

        music.mute = PlayerPrefs.GetInt("MusicMuted") != 0;
    }
}
