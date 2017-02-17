﻿
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerMovement : MonoBehaviour
{
    public float moveForce = 1.5f;
    public float jumpPower = 5;
    public float boostMultiplier = 2;
    Rigidbody2D myBody;
    bool isGrounded = false;
    public Transform top_left;
    public Transform bottom_right;
    //What layer is consider a ground
    public LayerMask WhatIsGround;
    public GameObject platformPref;
    public GameObject shieldPref;

    float lockPos = 0;

    [SerializeField]
    GameObject firebreath;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;

    void Start()
    {
        platformPref = GameObject.FindGameObjectWithTag("Platform");
        shieldPref = GameObject.FindGameObjectWithTag("Shield");
        Debug.Log(shieldPref.name + " " + platformPref.name);

#if UNITY_ANDROID
                      Debug.Log("Android");
#endif

#if UNITY_STANDALONE
        Debug.Log("PC");
#endif

        myBody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapArea(top_left.position, bottom_right.position, WhatIsGround);
        //Debug.Log(isGrounded);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, lockPos, lockPos);

#if UNITY_STANDALONE
        //Shooting
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Fire();
        }
        //Checking for jumping
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            Attack(transform.position);
        }
        //moving

        if (Input.GetKey(KeyCode.A))
        {
            myBody.AddForce(Vector2.left * moveForce);
        }
        if (Input.GetKey(KeyCode.D))
        {
             myBody.AddForce(Vector2.right * moveForce);
        }
#endif

#if UNITY_ANDROID

        //Shooting
        if (Input.touchCount>0)
                     {
                         if (Input.GetTouch(0).phase == TouchPhase.Ended)
                         {
                             Fire();
                         }
                     }
        //Jumping and moving
         Vector2 moveVec = new Vector2(CrossPlatformInputManager.GetAxis("Horizontal"),
		CrossPlatformInputManager.GetAxis("Vertical")) * moveForce;
		bool isBoosting = CrossPlatformInputManager.GetButtonDown("Boost");
        bool isJumping = CrossPlatformInputManager.GetButtonDown("Jump");

        Debug.Log(isGrounded);

        if (isGrounded)
        {
            if (isJumping)
            {
                Jump();
            }
        }
        
		//Debug.Log(isBoosting ? boostMultiplier : 1); //returns boostMultiplier if true, 1 if false
		myBody.AddForce(moveVec * (isBoosting ? boostMultiplier : 1));


#endif
    }

    void Jump()
    {
        myBody.AddForce(Vector2.up * jumpPower);
    }

    void Attack(Vector2 playerPos)
    {
        GameObject go;
        go = Instantiate(firebreath, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(0, -90, 0)) as GameObject;
    }


    void Fire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.forward * 6;

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }


}
