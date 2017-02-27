﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrimsonBoss : MonoBehaviour 
{
    int CrimsonAnimation;
    Animator anim;
    Vector2 bossDirection;
    Vector2 randomDirection;
    CrimsonState crimsonState;
    private int frame;

    private int attack1Counter;
    private int attack1Interval;
    private int attack2Counter;
    private int attack2Interval;
    private int directionCounter;
    private int changeDirectionInterval;

    private int DamageTaken;

    private Vector2 directionToPlayer;
    public GameObject player;

    public static int spawnPointIndex;
    public List<GameObject> myPlatforms;

    public float speed = 10.0f;

    private int randState;

    SpriteRenderer Sr;

    enum CrimsonState
    {
        IDLE,
        FLY,
        HURT,
        Attack1,
        Attack2
    }

	// Use this for initialization
	void Start () 
    {
        anim = GetComponent<Animator>();
        CrimsonAnimation = 0;
        GlobalScript.CrimsonHealth = 300;
        attack1Counter = 0;
        attack1Interval = 60;
        attack2Counter = 0;
        attack2Interval = 30;
        directionCounter = 0;
        changeDirectionInterval = Random.Range(1, 10) * 60;

        randomDirection = new Vector2(Random.Range(-100, -100),Random.Range(-100, 100));
        bossDirection = randomDirection.normalized;

        myPlatforms = new List<GameObject>();
        spawnPointIndex = 0;
        myPlatforms.Clear();
        foreach (GameObject platforms in GameObject.FindGameObjectsWithTag("Platform"))
        {
            myPlatforms.Add(platforms);
        }
        Sr = this.GetComponent<SpriteRenderer>();

        Debug.Log("CrimsonBossScript");
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GlobalScript.CrimsonHealth > 0 && collision.transform.tag == "Bullet")
        {
            ouch();
            GlobalScript.CrimsonHealth -= 5;
            DamageTaken += 5;
        }
        if (collision.transform.tag == "Platform" || collision.transform.tag == "Boundary")
        {
            bossDirection *= -1;
        }
    }
	// Update is called once per frame
	void Update()
    {
        StateChange();
        CrimsonStateInfo();
        Death();
        ConditionToChaneState();

        if(player==null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        FacingPlayer();

        anim.SetInteger("CrimsonAnimationState", CrimsonAnimation);
	}

    void FacingPlayer()
    {
        if (transform.position.x > player.transform.position.x)
        {
            Sr.flipX = false;
        }
        else
        {
            Sr.flipX = true;
        }
    }

    void Death()
    {
        if (GlobalScript.CrimsonHealth == 0)
        {
            CrimsonAnimation = 5;
            if (frame >= 60)
            {
                gameObject.SetActive(false);
            }
        }
        if (frame < 60 && GlobalScript.CrimsonHealth == 0)
        {
            frame++;
        }
    }

    void ConditionToChaneState()
    {
        if (DamageTaken > 25)
        {
            if(GlobalScript.CrimsonHealth > 200)
            {
                randState = Random.Range(0, 4);
            }
            else
            {
                randState = Random.Range(2, 4);
            }
            Debug.Log("DamageTaken " + DamageTaken + "   RandState " + randState);

            DamageTaken = 0;
        }

        if (randState == 1)
        {
            crimsonState = CrimsonState.FLY;
            CrimsonAnimation = 1;
        }
        if (randState == 2)
        {
            crimsonState = CrimsonState.Attack1;
        }
        if (randState == 3)
        {
            crimsonState = CrimsonState.Attack2;
        }

    }

    void CrimsonStateInfo()
    {
        if (crimsonState == CrimsonState.IDLE)
        {
            CrimsonAnimation = 0;
        }
        if (crimsonState == CrimsonState.HURT)
        {
            CrimsonAnimation = 2;
        }
        if (crimsonState == CrimsonState.FLY)
        {
            bossMove();
        }
        if (crimsonState == CrimsonState.Attack1)
        {
            bossAttack1();
            bossAttack1IntervalScale();
            if (GlobalScript.CrimsonHealth < 200)
            {
                bossMove();
            }

        }
        if (crimsonState == CrimsonState.Attack2)
        {
            bossAttack2();
        }
    }

    void StateChange()
    {

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            crimsonState = CrimsonState.IDLE;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            crimsonState = CrimsonState.FLY;
            CrimsonAnimation = 1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            ouch();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            crimsonState = CrimsonState.Attack1;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            crimsonState = CrimsonState.Attack2;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            CrimsonAnimation = 5;
        }
    }

    void ouch()
    {
        CrimsonAnimation = 2;
        Debug.Log("OUCH");
    }

    void bossAttack1()
    {
        CrimsonAnimation = 4;
        attack1Counter++;
        if(attack1Counter > attack1Interval)
        {
            Instantiate(Resources.Load("CrimsonAttack_1"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            attack1Counter = 0;
        }
    }

    void bossAttack1IntervalScale()
    {
        if (GlobalScript.CrimsonHealth > 200)
        {
            attack1Interval = 60;
        }
        else if (GlobalScript.CrimsonHealth > 75 && GlobalScript.CrimsonHealth < 200)
        {
            attack1Interval = 40;
        }
        else if (GlobalScript.CrimsonHealth < 75)
        {
            attack1Interval = 20;
        }
    }

    void bossAttack2()
    {
        CrimsonAnimation = 3;
        if (GlobalScript.CrimsonLightingEffect < 10)
        CreateAttack2();
    }

    void CreateAttack2()
    {
        spawnPointIndex = Random.Range(0, myPlatforms.Count);

        float width = myPlatforms[spawnPointIndex].GetComponent<Collider2D>().bounds.min.x + 2;
        float width2 = myPlatforms[spawnPointIndex].GetComponent<Collider2D>().bounds.max.x - 2;
        float spawnX = 0;

        spawnX = Random.Range(width, width2);
        Vector2 a = new Vector2(spawnX, myPlatforms[spawnPointIndex].transform.position.y + 2);
        GlobalScript.CrimsonLightingEffect++;
        GameObject go = Instantiate(Resources.Load("AttackEffects"), new Vector2(spawnX, myPlatforms[spawnPointIndex].transform.position.y -12), Quaternion.identity) as GameObject;
    }

    void bossMove()
    {
        directionCounter++;
        if (directionCounter > changeDirectionInterval)
        {
            int randNum = Random.Range(1, 5);
            //Debug.Log(randNum);
            if (randNum < 3)
            {
                Vector2 position = transform.position;
                Vector2 playerPosition;
                playerPosition = player.transform.position;
                bossDirection = (playerPosition - position).normalized;
                changeDirectionInterval = Random.Range(1, 10)    * 60;
                directionCounter = 0;
            }
            else
            {
                randomDirection = new Vector2(Random.Range(-10,10), Random.Range(-10, 10));
                bossDirection = randomDirection.normalized;
                changeDirectionInterval = Random.Range(1, 10) * 60;
                directionCounter = 0;
            }
            
        }
        transform.Translate(bossDirection * speed * Time.deltaTime);
        //gameObject.transform.position += new Vector3(1, 0   ,0);
    }
    

}
