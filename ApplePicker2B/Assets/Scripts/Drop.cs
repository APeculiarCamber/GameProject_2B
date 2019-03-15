using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    float speed = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (SettingsMenu.settingsOn)
            return;

        transform.Translate(new Vector3(0, -speed * Time.fixedDeltaTime, 0)); //move down

        if (transform.position.y < -6) Destroy(gameObject);
    }

    public void setSpeed(float s)
    {
        speed = s;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string oTag = other.gameObject.tag;
        if (oTag == "Picker" || oTag == "PlayerShot")
        {
            Destroy(gameObject);
        }
    }
}
