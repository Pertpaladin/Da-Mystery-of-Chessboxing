using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FightSceneManager : MonoBehaviour {
    private Text newclock;
    private bool rightTime;
    private int thing = 20;
    public GameObject CHAR00;
    public GameObject CHAR01;
    public GameObject CHAR02;
    public GameObject CHAR03;
    public GameObject CHAR10;
    public GameObject CHAR11;
    public GameObject CHAR12;
    public GameObject CHAR13;

    public GameObject Tile1Location0;
    public GameObject Tile2Location0;
    public GameObject Tile1Location1;
    public GameObject Tile2Location1;
    public GameObject Tile1Location2;
    public GameObject Tile2Location2;
    public GameObject Tile1Location3;
    public GameObject Tile2Location3;
    public GameObject Tile1Location4;
    public GameObject Tile2Location4;

    public GameObject cameraLocation0;
    public GameObject cameraLocation1;
    public GameObject cameraLocation2;
    public GameObject cameraLocation3;
    public GameObject cameraLocation4;


    GameObject fighter1;
    GameObject fighter2;
    GameObject cam;
    int fightLocation = GameManager.cameraLocation;
    // Use this for initialization
    void Start () {
        newclock = GameObject.Find("Timer").GetComponent<Text>();
        rightTime = true;
        cam = GameObject.Find("Main Camera");
        GameManager.fighterGameObjects[0] = CHAR00;
        GameManager.fighterGameObjects[1] = CHAR01;
        GameManager.fighterGameObjects[2] = CHAR02;
        GameManager.fighterGameObjects[3] = CHAR03;
        GameManager.fighterGameObjects[4] = CHAR10;
        GameManager.fighterGameObjects[5] = CHAR11;
        GameManager.fighterGameObjects[6] = CHAR12;
        GameManager.fighterGameObjects[7] = CHAR13;
        if (GameManager.FightType)
        {
            fighter1 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[GameManager.team1fighterIndex].model]);
            fighter2 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[GameManager.team2fighterIndex].model]);
        }
        else
        {
            fighter1 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[0].model]);
            fighter2 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[4].model]);
        }
        fighter1.tag = "Player";
        fighter2.tag = "Player 2";

        switch (fightLocation)
        {
            case 0:
                fighter1.transform.position = Tile1Location0.transform.position;
                fighter2.transform.position = Tile2Location0.transform.position;
                cam.transform.position = cameraLocation0.transform.position;
                break;
            case 1:
                fighter1.transform.position = Tile1Location1.transform.position;
                fighter2.transform.position = Tile2Location1.transform.position;
                cam.transform.position = cameraLocation1.transform.position;
                break;
            case 2:
                fighter1.transform.position = Tile1Location2.transform.position;
                fighter2.transform.position = Tile2Location2.transform.position;
                cam.transform.position = cameraLocation2.transform.position;
                break;

            case 3:
                fighter1.transform.position = Tile1Location3.transform.position;
                fighter2.transform.position = Tile2Location3.transform.position;
                cam.transform.position = cameraLocation3.transform.position;
                break;

            case 4:
                fighter1.transform.position = Tile1Location4.transform.position;
                fighter2.transform.position = Tile2Location4.transform.position;
                cam.transform.position = cameraLocation4.transform.position;
                break;
        }
        cam.transform.LookAt((fighter1.transform.position + fighter2.transform.position) / 2);
        fighter1.transform.LookAt(fighter2.transform);
        fighter2.transform.LookAt(fighter1.transform);
    }
	
	// Update is called once per frame
	void Update () {
        if (rightTime)
        {
            rightTime = false;
            newclock.text = thing.ToString();
            thing--;
            StartCoroutine("Clock");
            if(thing <= 0)
            {
                SceneManager.LoadScene("Startegy_Scene");
            }
        }
	}
    IEnumerator Clock()
    {
        yield return new WaitForSecondsRealtime(1);
        rightTime = true;
    }

}
