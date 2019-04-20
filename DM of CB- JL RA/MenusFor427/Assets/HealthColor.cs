using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthColor : MonoBehaviour {
    Graphic image;
    Color team1 = Color.red;
    Color team2 = Color.blue;
	// Use this for initialization
	void Start () {
        image = GetComponent<Graphic>();
        
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void SetPlayer(int index)
    {
        image.color = Color.cyan;
        if(index < 4)
        {
            //image.color = team1;
            //image.SetColor("_Color", team1);
        } else
        {
            //image.color = team2;
        }
    }
}
