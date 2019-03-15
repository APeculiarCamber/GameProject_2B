using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    PointTracker pt;

    AudioSource hitSound;

    [SerializeField]
    GameObject apple;
    
    [Space]
    [SerializeField]
    float speed = 0;

    [Space]
    [SerializeField]
    [Range(0,1)]
    float turnChance = 0;
    
    int direction = 1;
    float posOnScreen;


    float timer = 0;
    [Space]
    [Header("Drops")]
    [SerializeField]
    float rateChangeFrequency;
    [Space]
    [SerializeField]
    float dropRate = 0;
    [SerializeField]
    float dropRateDelta = 0;
    [SerializeField]
    float dropRateMin = 0;
    [Space]
    [SerializeField]
    float dropSpeed = 0;
    [SerializeField]
    float dropSpeedDelta = 0;
    [SerializeField]
    float dropSpeedMax;


    private void Start()
    {
        hitSound = GetComponent<AudioSource>();
        pt = FindObjectOfType<PointTracker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pt.gameIsStarted || SettingsMenu.settingsOn)
        {
            CancelInvoke();
            return;
        }

        Move();

        handleDropRateChange();

        if (!IsInvoking()) //can't InvokeRepeating because the dropRate changes, instead check when invoke is done
        {
            Invoke("Drop", Mathf.Abs(Random.Range(dropRate - 0.2f, dropRate + 0.2f)));
        }
    }

    void Move()
    {
        if (Random.value < turnChance)
        {
            direction *= -1;
        }
        posOnScreen = Camera.main.WorldToViewportPoint(transform.position).x; //get the viewport position of dropper on the screen

        if (posOnScreen > 0.95f)
        {
            direction = -1;
        }
        else if (posOnScreen < 0.05f)
        {
            direction = 1;
        }
        
        transform.Translate(new Vector3(speed * direction * Time.fixedDeltaTime, 0, 0));
    }

    void Drop()
    {
        Drop d = Instantiate(apple, transform.position, Quaternion.identity).GetComponent<Drop>();
        d.setSpeed(Random.Range(dropSpeed - 0.5f, dropSpeed + 0.5f)); //set d's drop speed
    }

    void handleDropRateChange()
    {
        if (timer > rateChangeFrequency)
        {
            dropRate -= (dropRate - dropRateDelta > dropRateMin ? dropRateDelta : 0f);
            dropSpeed += (dropSpeed + dropSpeedDelta < dropSpeedMax ? dropSpeedDelta : 0f);
            timer = 0;
        }

        timer += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerShot")
        {
            pt.addPoints(15);
            if (PlayerPrefs.GetInt("SFXMuted", 0) == 0)
                hitSound.Play();
        }
    }
}
