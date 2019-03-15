using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarMovement : MonoBehaviour
{
    Vector3 axis;
    PointTracker pt;

    [SerializeField]
    float timeRate = 1f;
    [SerializeField]
    float radius = 0.6f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {

        axis = transform.position + (Vector3.up * radius);
        pt = FindObjectOfType<PointTracker>();
        timeRate = (Mathf.PI * 2) / timeRate;
        
        //Sin = Opposite (y) / Hypotenuse (radius)
        //y = sin() * radius
        float y = Mathf.Sin(timer) * radius;

        //Cos = adjacent (x) / Hypotenuse (radius)
        //x = cos() * radius
        float x = Mathf.Cos(timer) * radius;

        transform.position = axis + (Vector3.up * y) + (Vector3.right * x);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pt.gameIsStarted || SettingsMenu.settingsOn)
            return;

        timer += (Time.deltaTime * timeRate);
        timer = timer % (Mathf.PI * 2);

        //Sin = Opposite (y) / Hypotenuse (radius)
        //y = sin() * radius
        float y = Mathf.Sin(timer) * radius;

        //Cos = adjacent (x) / Hypotenuse (radius)
        //x = cos() * radius
        float x = Mathf.Cos(timer) * radius;

        transform.position = axis + (Vector3.up * y) + (Vector3.right * x); //move in a circle
    }
}
