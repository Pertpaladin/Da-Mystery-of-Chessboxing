﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    Image[] buttons;
    int index;
    int previousInput = 0;
	// Use this for initialization
	void Start () {
        index = 0;
        buttons = new Image[3];
        buttons[0] = GameObject.Find("Chessboxing").GetComponent<Image>();
        buttons[1] = GameObject.Find("Boxing").GetComponent<Image>();
        buttons[2] = GameObject.Find("Quit").GetComponent<Image>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("A1"))
        {
            Debug.Log("A Pressed");
            if(index == 0)
            {
                GameManager.IncludeGameGrid = true;
                SceneManager.LoadScene("CharacterSelect");
            }
            else if(index == 1)
            {
                GameManager.IncludeGameGrid = false;
                SceneManager.LoadScene("CharacterSelect");
            } else if(index == 2)
            {
                Application.Quit();
            }
        }
        
            if (Input.GetAxisRaw("DY1") >= 1 && previousInput == 0)
            {
                buttons[index].enabled = false;
                if (index > 0)
                    index = (index - 1) % 3;
                else
                    index = 2;
                buttons[index].enabled = true;
                previousInput = 1;
            }
            else if (Input.GetAxisRaw("DY1") <= -1 && previousInput == 0)
            {
                buttons[index].enabled = false;
                index = (index + 1) % 3;
                buttons[index].enabled = true;
                previousInput = -1;
            }
            else if(Input.GetAxisRaw("DY1") == 0)
            {
                previousInput = 0;
            }
    }
}
