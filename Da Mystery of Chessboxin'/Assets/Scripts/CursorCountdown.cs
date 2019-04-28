using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorCountdown : MonoBehaviour {
    Text text;
    int range;
    GameObject SceneMan;
	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
        SceneMan = GameObject.Find("SceneManager");
        range = SceneMan.GetComponent<GridSceneManager>().range;
	}
	
	// Update is called once per frame
	void Update () {
        range = SceneMan.GetComponent<GridSceneManager>().range;

        if (range > 0)
        {
            text.text = (GameManager.fighters[GameManager.turn].range + SceneMan.GetComponent<GridSceneManager>().bonusRange - range).ToString(); 
        } else
        {
            text.text = "";
        }
	}
}
