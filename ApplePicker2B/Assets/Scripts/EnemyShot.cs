using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    [SerializeField]
    GameObject deathExplosion;

    float speed = 0;

    // Update is called once per frame
    void Update()
    {
        if (SettingsMenu.settingsOn)
            return;

        transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));

        if (transform.position.y < -6)
        {
            FindObjectOfType<PointTracker>().takeHealth();
            Destroy(gameObject);
        }
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Picker" || other.gameObject.tag == "PlayerShot")
        {
            Destroy(gameObject);
            Instantiate(deathExplosion, transform.position, Quaternion.identity);
            FindObjectOfType<PointTracker>().addPoints(5);
        }
    }
}
