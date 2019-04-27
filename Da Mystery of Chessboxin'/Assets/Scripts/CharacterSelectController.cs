using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterSelectController : MonoBehaviour
{
    //public GameObject playerModel;
    Image[] buttons;
    int P1teamIndex;
    int P2teamIndex;
    int P1X;
    int P1Y;
    int P2X;
    int P2Y;
    int previousInputP1X;
    int previousInputP1Y;
    int previousInputP2X;
    int previousInputP2Y;
    Color P1Color = Color.red;
    Color P2Color = Color.blue;
    Color ComboColor = Color.green;
    bool posChanged;
    // Use this for initialization
    void Start()
    {
        //GameManager.fighterGameObjects[0] = playerModel;
        posChanged = false;
        P1X = 0;
        P1Y = 0;
        P2X = 0;
        P2Y = 0;
        P1teamIndex = 0;
        P2teamIndex = 4;
        previousInputP1X = 0;
        previousInputP1Y = 0;
        previousInputP2X = 0;
        previousInputP2Y = 0;
        buttons = new Image[8];
        buttons[0] = GameObject.Find("CHAR_00").GetComponent<Image>();
        buttons[1] = GameObject.Find("CHAR_01").GetComponent<Image>();
        buttons[2] = GameObject.Find("CHAR_02").GetComponent<Image>();
        buttons[3] = GameObject.Find("CHAR_03").GetComponent<Image>();
        buttons[4] = GameObject.Find("CHAR_10").GetComponent<Image>();
        buttons[5] = GameObject.Find("CHAR_11").GetComponent<Image>();
        buttons[6] = GameObject.Find("CHAR_12").GetComponent<Image>();
        buttons[7] = GameObject.Find("CHAR_13").GetComponent<Image>();
        buttons[0].color = ComboColor;
    }

    // Update is called once per frame
    void Update()
    {

        //P1 INPUT
        //--------------------------------------------------------------------------------------------------------------------
        if (Input.GetButtonDown("A1"))
        {
            if (P1teamIndex < 4)
            {
                bool inTeam = false;
                for (int i = 0; i < P1teamIndex; i++)
                {
                    if (GameManager.fighters[i].menuX == P1X && GameManager.fighters[i].menuY == P1Y)
                    {
                        inTeam = true;
                    }
                }
                if (!inTeam)
                {
                    GameManager.fighters[P1teamIndex] = new Fighter(1, P1X, P1Y);
                    P1teamIndex++;
                    Debug.Log(P1teamIndex.ToString() + " in team 1");
                }
                else
                {
                    //play negative sound
                }
            }
            else
            {
                //play negative sound
            }
        }
        if (Input.GetButtonDown("B1"))
        {
            //if (P1teamIndex >= 0)
            //{
                P1teamIndex--;
                Debug.Log(P1teamIndex + " in team 1");

            if(P1teamIndex < 0)
            {
                Debug.Log("change scene");
                SceneManager.LoadScene("MainMenu");
            }
        }
        //P1 Y AXIS
        if (Input.GetAxisRaw("DY1") >= 1 && previousInputP1Y == 0)
        {
            if (P1Y == 0)
                P1Y = 1;
            else
                P1Y = 0;
            previousInputP1Y = 1;
            posChanged = true;
        }
        else if (Input.GetAxisRaw("DY1") <= -1 && previousInputP1Y == 0)
        {
            if (P1Y == 0)
                P1Y = 1;
            else
                P1Y = 0;
            previousInputP1Y = -1;
            posChanged = true;
        }
        else if (Input.GetAxisRaw("DY1") == 0)
        {
            previousInputP1Y = 0;
        }

        //P1 X AXIS
        if (Input.GetAxisRaw("DX1") >= 1 && previousInputP1X == 0)
        {
            if (P1X == 3)
                P1X = 0;
            else
                P1X = P1X + 1;
            previousInputP1X = 1;
            posChanged = true;
        }
        else if (Input.GetAxisRaw("DX1") <= -1 && previousInputP1X == 0)
        {
            if (P1X == 0)
                P1X = 3;
            else
                P1X = P1X - 1;
            previousInputP1X = -1;
            posChanged = true;
        }
        else if (Input.GetAxisRaw("DX1") == 0)
        {
            previousInputP1X = 0;
        }








        //P2 INPUT
        //--------------------------------------------------------------------------------------------------------------------
        if (Input.GetButtonDown("A2"))
        {
            
            if (P2teamIndex < 8)
            {
                bool inTeam = false;
                for(int i = 4; i < P2teamIndex; i++)
                {
                    if(GameManager.fighters[i].menuX == P2X && GameManager.fighters[i].menuY == P2Y)
                    {
                        inTeam = true;
                    }
                }
                if (!inTeam)
                {
                    GameManager.fighters[P2teamIndex] = new Fighter(2, P2X, P2Y);
                    P2teamIndex++;
                    Debug.Log(P2teamIndex + "in team 2");
                } else
                {
                    //play negative sound
                }
            }
            else
            {
                //play negative sound
            }
        }
        if (Input.GetButtonDown("B2"))
        {
            if (P2teamIndex > 4)
            {
                P2teamIndex--;
                Debug.Log(P2teamIndex + "in team 2");
            }
            else
            {
                Debug.Log("change scene");
                SceneManager.LoadScene("MainMenu");
            }
        }
            //P2 Y AXIS
            if (Input.GetAxisRaw("DY2") >= 1 && previousInputP2Y == 0)
            {
                if (P2Y == 0)
                    P2Y = 1;
                else
                    P2Y = 0;
                previousInputP2Y = 1;
                posChanged = true;
            }
            else if (Input.GetAxisRaw("DY2") <= -1 && previousInputP2Y == 0)
            {
                if (P2Y == 0)
                    P2Y = 1;
                else
                    P2Y = 0;
                previousInputP2Y = -1;
                posChanged = true;
            }
            else if (Input.GetAxisRaw("DY2") == 0)
            {
                previousInputP2Y = 0;
            }

            //P2 X AXIS
            if (Input.GetAxisRaw("DX2") >= 1 && previousInputP2X == 0)
            {
                if (P2X == 3)
                    P2X = 0;
                else
                    P2X = P2X + 1;
                previousInputP2X = 1;
                posChanged = true;
            }
            else if (Input.GetAxisRaw("DX2") <= -1 && previousInputP2X == 0)
            {
                if (P2X == 0)
                    P2X = 3;
                else
                    P2X = P2X - 1;
                previousInputP2X = -1;
                posChanged = true;
            }
            else if (Input.GetAxisRaw("DX2") == 0)
            {
                previousInputP2X = 0;
            }

            if (posChanged)
            {
                if (P1X == P2X && P1Y == P2Y)
                {
                    foreach (Image x in buttons)
                    {
                        x.enabled = false;
                    }
                    buttons[P2X + 4 * P2Y].enabled = true;
                    buttons[P2X + 4 * P2Y].color = ComboColor;
                }
                else
                {
                    foreach (Image x in buttons)
                    {
                        x.enabled = false;
                    }
                    buttons[P1X + 4 * P1Y].enabled = true;
                    buttons[P1X + 4 * P1Y].color = P1Color;
                    buttons[P2X + 4 * P2Y].enabled = true;
                    buttons[P2X + 4 * P2Y].color = P2Color;
                }
                posChanged = false;
            }

            if(P1teamIndex >= 4 && P2teamIndex >= 8)
        {
            if (GameManager.IncludeGameGrid)
            {
                SceneManager.LoadScene("GridDemo");
            } else
            {
                SceneManager.LoadScene("Fighting_Scene");
            }
        }
        }
    }
