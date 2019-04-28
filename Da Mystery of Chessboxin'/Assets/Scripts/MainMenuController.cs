using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{


    Image[] buttons;
    int index;
    int previousInput = 0;
    public GameObject canvas1;
    public GameObject canvas2;
    public GameObject canvas3;
    // Use this for initialization
    void Start()
    {

        GameManager.turn = -1;
        index = 0;
        buttons = new Image[5];
        buttons[0] = GameObject.Find("Chessboxing").GetComponent<Image>();
        buttons[1] = GameObject.Find("Boxing").GetComponent<Image>();
        buttons[2] = GameObject.Find("Quit").GetComponent<Image>();
        buttons[3] = GameObject.Find("Game Information").GetComponent<Image>();
        buttons[4] = GameObject.Find("Credits").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("A1"))
        {
            Debug.Log("A Pressed");
            if (index == 0)
            {
                GameManager.FightType = true;
                SceneManager.LoadScene("CharacterSelect");
            }
            else if (index == 1)
            {
                GameManager.FightType = false;
                GameManager.team1fighterIndex = 0;
                GameManager.team2fighterIndex = 4;
                SceneManager.LoadScene("CharacterSelect");
            }
            else if (index == 2)
            {
                Application.Quit();
            }
            else if (index == 3)
            {
                canvas1.SetActive(false);
                canvas2.SetActive(true);
                canvas3.SetActive(false);
            }
            else if (index == 4)
            {
                canvas1.SetActive(false);
                canvas2.SetActive(false);
                canvas3.SetActive(true);
            }
        }
        if (Input.GetButtonDown("B1"))
        {
            canvas1.SetActive(true);
            canvas2.SetActive(false);
            canvas3.SetActive(false);
        }
        if (Input.GetAxisRaw("DY1") >= 1 && previousInput == 0)
        {
            buttons[index].enabled = false;
            if (index > 0)
                index = (index - 1) % 5;
            else
                index = 4;
            buttons[index].enabled = true;
            previousInput = 1;
        }
        else if (Input.GetAxisRaw("DY1") <= -1 && previousInput == 0)
        {
            buttons[index].enabled = false;
            index = (index + 1) % 5;
            buttons[index].enabled = true;
            previousInput = -1;
        }
        else if (Input.GetAxisRaw("DY1") == 0)
        {
            previousInput = 0;
        }
    }
}
