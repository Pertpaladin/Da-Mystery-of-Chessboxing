﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager{
    //sets gamemode
    public static bool IncludeGameGrid;
    
    //Fighter 0 & 4 are team leaders
    //Fighter data array
    public static Fighter[] fighters= new Fighter[8];
    //Fighter Object Array
    public static GameObject[] fighterGameObjects = new GameObject[8];

    //decide which controller is P1 and P2
    //whoever first controller to give input is P1, second is P2, set for whole game

    //Specific to grid mode
    //-----------------------------------------------
    //add starting formations later

    //occupied spaces
    //-1 = obstacle
    //0 = unoccupied
    //1 = team one occupied
    //2 = team two occupied
    //grid size subject to change
    public static int[,] gridSpaces = new int[20,20];

    //keeps track of turn
    public static int turn;

    //fighters to load into arena
    public static int team2fighterIndex;
    public static int team1fighterIndex;
}
