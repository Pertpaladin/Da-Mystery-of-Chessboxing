using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightSceneManager : MonoBehaviour {
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
    int fightLocation;


    // Use this for initialization
    void Start () {
        cam = GameObject.Find("Main Camera");
        GameManager.fighterGameObjects[0] = CHAR00;
        GameManager.fighterGameObjects[1] = CHAR01;
        GameManager.fighterGameObjects[2] = CHAR02;
        GameManager.fighterGameObjects[3] = CHAR03;
        GameManager.fighterGameObjects[4] = CHAR10;
        GameManager.fighterGameObjects[5] = CHAR11;
        GameManager.fighterGameObjects[6] = CHAR12;
        GameManager.fighterGameObjects[7] = CHAR13;
        fighter1 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[GameManager.team1fighterIndex].model]);
        fighter2 = Instantiate(GameManager.fighterGameObjects[GameManager.fighters[GameManager.team2fighterIndex].model]);
        fighter1.tag = "player";
        fighter2.tag = "player 2";
        //set to Game.Manager variable
        fightLocation = 0;

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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
