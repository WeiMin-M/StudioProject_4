﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenerateEnemy : MonoBehaviour {
    [SerializeField]
    GameObject enemyType;
    int enemyCount = 0;
    public static int spawnPointIndex;
    public List<GameObject> myPlatforms;

    // Use this for initialization
    void Start () {
        myPlatforms = new List<GameObject>();
        spawnPointIndex = 0;
        myPlatforms.Clear();
        foreach (GameObject platforms in GameObject.FindGameObjectsWithTag("Platform"))
        {
            myPlatforms.Add(platforms);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalScript.enemyCount < 6)
        {
            CreateEnemy();
            GlobalScript.enemyCount++;
        }
    }
    void CreateEnemy()
    {
        spawnPointIndex = Random.Range(0, myPlatforms.Count);

        float width = myPlatforms[spawnPointIndex].GetComponent<Collider2D>().bounds.min.x + 2;
        float width2 = myPlatforms[spawnPointIndex].GetComponent<Collider2D>().bounds.max.x - 2;
        float spawnX = 0;
       
        spawnX = Random.Range(width, width2);
        Vector2 a = new Vector2(spawnX, myPlatforms[spawnPointIndex].transform.position.y + 2);
        enemyCount++;
        GameObject go = Instantiate(enemyType, new Vector2(spawnX, myPlatforms[spawnPointIndex].transform.position.y + 0.5F), Quaternion.identity) as GameObject;
    }
}
