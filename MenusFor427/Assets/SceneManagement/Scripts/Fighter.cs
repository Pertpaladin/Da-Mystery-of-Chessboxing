using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter {
    public int menuX;
    public int menuY;
    public int team;
    public float health;
    public int xPos;
    public int zPos;
    public int range;
    public float attackStat;
    public float defenseStat;
    public string fighterName;
    public int model;

	public Fighter(int t,int menuSelectX,int menuSelectY)
    {
        menuX = menuSelectX;
        menuY = menuSelectY;

        team = t;
        switch (menuSelectX)
        {
            case 0:
                switch (menuSelectY)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        //model = GameManager.fighterGameObjects[menuSelectX * 4 + menuSelectY];
                        model = menuSelectX * 4 + menuSelectY;
                        range = 5;
                        xPos = menuSelectX;
                        zPos = menuSelectY;
                        attackStat = 5;
                        defenseStat = 5;
                        health = 5;
                        fighterName = t + " Team " + menuSelectX.ToString() + menuSelectY.ToString();
                        break;
                }
                break;
            case 1:
                switch (menuSelectY)
                {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                        //model = GameManager.fighterGameObjects[menuSelectX * 4 + menuSelectY];
                        model = menuSelectX * 4 + menuSelectY;
                        range = 5;
                        xPos = menuSelectX;
                        zPos = menuSelectY;
                        attackStat = 5;
                        defenseStat = 5;
                        health = 5;
                        fighterName = t + " Team " + menuSelectX.ToString() + menuSelectY.ToString();
                        break;
                }
                break;
                // attributes instantiated on character select
        }
        //Pos set to start square
        //
    }
    public void Formation(int augX, int augY)
    {
        xPos += augX;
        zPos += augY;
        //alter gamemanager objects too
    }
}
