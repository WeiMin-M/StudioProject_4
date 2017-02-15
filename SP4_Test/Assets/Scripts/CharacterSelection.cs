﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {

    [SerializeField]
    Text description;
    [SerializeField]
    Text charName;

    private List<GameObject> sprites;
    //Default index of the model
    private int selectionIndex = 0;
	// Use this for initialization
	void Start () {
        sprites = new List<GameObject>();
        foreach(Transform t in transform)
        {
            sprites.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }

        sprites[selectionIndex].SetActive(true);
	}
    public void SelectCharacter(int index)
    {
        if (index == selectionIndex)
            return;
        if (index < 0 || index >= sprites.Count)
            return;
        sprites[selectionIndex].SetActive(false);
        selectionIndex = index;
        sprites[selectionIndex].SetActive(true);
    }
	public void NextCharacter()
    {
        Debug.Log("AA");
        if (selectionIndex < sprites.Count-1)
        {
            sprites[selectionIndex].SetActive(false);
            selectionIndex++;
            sprites[selectionIndex].SetActive(true);
        }
    }
    public void PreviousCharacter()
    {
        if (selectionIndex > 0)
        {
            sprites[selectionIndex].SetActive(false);
            selectionIndex--;
            sprites[selectionIndex].SetActive(true);
        }
    }
    public void Selected()
    {
        GlobalScript.CharacterType = selectionIndex;
        Debug.Log(GlobalScript.CharacterType);
        Debug.Log(GlobalScript.myCharacters[selectionIndex].shield);

    }
    // Update is called once per frame
    void Update () {
        description.text = GlobalScript.myCharacters[selectionIndex].description 
            + '\n' + "Speed: " + GlobalScript.myCharacters[selectionIndex].speed + 
            '\n' + "Shield Duration: " + GlobalScript.myCharacters[selectionIndex].shield + 
            '\n' + "Buff Duration: " + GlobalScript.myCharacters[selectionIndex].buffDuration
            + '\n' + "Bullet Speed: " + GlobalScript.myCharacters[selectionIndex].bulletSpeed;
        charName.text = GlobalScript.myCharacters[selectionIndex].name;
    }
}