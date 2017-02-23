﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FadeInOrOut : MonoBehaviour {

    CanvasGroup cg;
    public List<GameObject> myCanvases = new List<GameObject>();

    // Use this for initialization
    void Start () {
        for (int i = 0; i < myCanvases.Count; i++)
            myCanvases[i].GetComponent<CanvasGroup>().alpha = 0;
        //cg.alpha = 0F;
	}
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < myCanvases.Count; i++)
        {
            if (myCanvases[i].GetComponent<CanvasGroup>().alpha < 1F)
                myCanvases[i].GetComponent<CanvasGroup>().alpha += Time.deltaTime * 0.5F;
        }
	}
}