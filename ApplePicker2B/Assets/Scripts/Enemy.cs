using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //enemy types, half out then half back (one bomb), full across (one bomb),
    //full across then back (two bombs), v attack (no bomb),
    public enum EnemyType { Half, OneFull, TwoFull, VAttack };

    [SerializeField]
    GameObject bomb;
    [SerializeField]
    GameObject deathExplosion;

    Transform player;

    EnemyType enemyType = EnemyType.OneFull;
    PointTracker pt;

    bool bombDropSelected = false;
    bool hasDroppedBomb = false;
    bool hasTurnedAround = false;
    bool isFromLeft = true;

    float speed;
    float shotSpeed; //shotSpeed is proportional to speed

    float bombLocation;

    int lane = 0;

    // Start is called before the first frame update
    void Start()
    {
        pt = FindObjectOfType<PointTracker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!pt.gameIsStarted || SettingsMenu.settingsOn)
            return;

        switch (enemyType)
        {
            case EnemyType.Half:
                MoveAsHalf();
                break;
            case EnemyType.OneFull:
                MoveAsOneFull();
                break;
            case EnemyType.TwoFull:
                MoveAsTwoFull();
                break;
            case EnemyType.VAttack:
                MoveAsVAttack();
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }

    void MoveAsHalf()
    {
        transform.Translate(new Vector3(
            (isFromLeft ? 1 : -1) * (hasTurnedAround ? -1 : 1) * speed * Time.fixedDeltaTime, 0, 0), Space.Self);

        if (!bombDropSelected)
        {
            bombLocation = Camera.main.ViewportToWorldPoint(
                new Vector3(isFromLeft ? Random.Range(0.1f, 0.4f) : Random.Range(0.6f, 0.9f), 0, 0)).x;
            bombDropSelected = true;
        }
        if (!hasDroppedBomb
            && ((isFromLeft && bombLocation < transform.position.x)
                || (!isFromLeft && bombLocation > transform.position.x))) //dropping bombs
        {
            dropBomb();
            hasDroppedBomb = true;
        }
        if (!hasTurnedAround
            && ((isFromLeft && transform.position.x > Camera.main.transform.position.x)
            || (!isFromLeft && transform.position.x < Camera.main.transform.position.x)))
        {   hasTurnedAround = true; }

        if (hasTurnedAround 
            && ((isFromLeft && transform.position.x < transform.parent.position.x) 
            || (!isFromLeft && transform.position.x > transform.parent.position.x)))
        {
            FindObjectOfType<EnemySpawner>().freeLane(lane);
            Destroy(gameObject);
        }
    }

    void MoveAsOneFull()
    {
        transform.Translate(new Vector3((isFromLeft ? 1 : -1) *  speed * Time.fixedDeltaTime, 0, 0), Space.Self);

        if (!bombDropSelected)
        {
            bombLocation = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 0, 0)).x;
            bombDropSelected = true;
        }
        if (!hasDroppedBomb 
            && ((isFromLeft && bombLocation < transform.position.x) 
                || (!isFromLeft && bombLocation > transform.position.x)))  //dropping bombs
        {
            dropBomb();
            hasDroppedBomb = true;
        }

        if ((isFromLeft && Camera.main.WorldToViewportPoint(transform.position).x > 1.2f) 
            || (!isFromLeft && Camera.main.WorldToViewportPoint(transform.position).x < -0.2f))
        {
            FindObjectOfType<EnemySpawner>().freeLane(lane);
            Destroy(gameObject);
        }
    }

    void MoveAsTwoFull()
    {
        transform.Translate(new Vector3(
            (isFromLeft ? 1 : -1) * (hasTurnedAround ? -1.6f : 1.6f) * speed * Time.fixedDeltaTime, 0, 0), Space.Self);

        if (!bombDropSelected)
        {
            bombLocation = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 0, 0)).x;
            bombDropSelected = true;
        }
        if (!hasDroppedBomb && 
            ((!hasTurnedAround && isFromLeft && bombLocation < transform.position.x) 
            || (hasTurnedAround && isFromLeft && bombLocation > transform.position.x)
            || (!hasTurnedAround && !isFromLeft && bombLocation > transform.position.x)
            || (hasTurnedAround && !isFromLeft && bombLocation < transform.position.x))) //figure out when to drop bomb
        {
            dropBomb();
            hasDroppedBomb = true;
        }

        if (!hasTurnedAround 
            && ((isFromLeft && Camera.main.WorldToViewportPoint(transform.position).x > 1.04f) 
                || (!isFromLeft && Camera.main.WorldToViewportPoint(transform.position).x < -0.04f)))
        {
            hasTurnedAround = true;
            bombDropSelected = false;   //two full drops two bombs
            hasDroppedBomb = false;
        }

        if (hasTurnedAround 
            && ((isFromLeft && transform.position.x < transform.parent.position.x)
                || (!isFromLeft && transform.position.x > transform.parent.position.x)))
        {
            FindObjectOfType<EnemySpawner>().freeLane(lane);
            Destroy(gameObject);
        }
    }

    void MoveAsVAttack()
    {
        if (!bombDropSelected)
        {
            player = GameObject.FindWithTag("Picker").transform;
            bombLocation = player.position.x;
            bombDropSelected = true;  //VATTACK does not drop bombs
        }

        if (!hasTurnedAround)
            transform.Translate(
                (new Vector3(bombLocation, player.position.y, 0) - transform.position).normalized * speed * Time.fixedDeltaTime);
        else
            transform.Translate((Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 0)) - transform.position).normalized * speed * Time.fixedDeltaTime);

        if (transform.position.x >= bombLocation)
            hasTurnedAround = true;

        else if (transform.position.x >= Camera.main.ViewportToWorldPoint(new Vector3(1f,1f,0)).x)
        {
            Destroy(gameObject);
        }
    }


    void dropBomb()
    {
        EnemyShot b = Instantiate(bomb, transform.GetChild(0).position, Quaternion.identity).GetComponent<EnemyShot>();
        b.setSpeed(shotSpeed);
    }

    public void setEnemyType(EnemyType e)
    {
        enemyType = e;
        if (e == EnemyType.VAttack)
        {
            GetComponent<SpriteRenderer>().color = Color.blue; //change color for v attacker
        }
    }

    public void setSpeed(float s)
    {
        speed = s;
        shotSpeed = s * 0.6f; 
    }

    public void setLane(int l)
    {
        lane = l;
        isFromLeft = lane % 2 == 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "PlayerShot")
        {
            if (enemyType != EnemyType.VAttack)
                FindObjectOfType<EnemySpawner>().freeLane(lane);
            pt.addPoints(10);
            Destroy(gameObject);
            GameObject explosion = Instantiate(deathExplosion, transform.position, Quaternion.identity);

            if (enemyType == EnemyType.VAttack)
                explosion.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (other.gameObject.tag == "Picker")
        {
            if (enemyType != EnemyType.VAttack)
                FindObjectOfType<EnemySpawner>().freeLane(lane);
            Destroy(gameObject);
            GameObject explosion = Instantiate(deathExplosion, transform.position, Quaternion.identity);

            if (enemyType == EnemyType.VAttack)
                explosion.GetComponent<SpriteRenderer>().color = Color.blue;
        }

    }
}
