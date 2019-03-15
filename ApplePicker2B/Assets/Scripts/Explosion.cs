using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    float lingerTime = 0.66f;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("SFXMuted", 0) == 0)
        {
            GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip,
                PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        }

        Invoke("Explode", lingerTime);
    }

    void Explode()
    {
        Destroy(gameObject);
    }
}
