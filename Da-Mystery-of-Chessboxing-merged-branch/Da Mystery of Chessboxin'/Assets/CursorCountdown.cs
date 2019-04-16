using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorCountdown : MonoBehaviour {
    Text text;
    int range;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        range = GameObject.Find("SceneManager").GetComponent<GridSceneManager>().range;
	}
	
	// Update is called once per frame
	void Update () {
        range = GameObject.Find("SceneManager").GetComponent<GridSceneManager>().range;

        if (range > 0)
        {
            text.text = (GameManager.fighters[GameManager.turn].range - range).ToString(); 
        } else
        {
            text.text = "";
        }
	}
}
