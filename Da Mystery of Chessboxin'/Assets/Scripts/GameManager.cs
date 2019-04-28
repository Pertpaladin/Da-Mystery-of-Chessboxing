using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager{
    //sets chessboxing or boxing, true or false respectively
    public static bool FightType;
    
    //Fighter 0 & 4 are team leaders
    //Fighter data array
    public static Fighter[] fighters= new Fighter[8];
    //Fighter Object Array
    public static GameObject[] fighterGameObjects = new GameObject[8];
    //public static GameObject model; 
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
    public static GameObject[,] gridSpaces;

    //keeps track of turn
    public static int turn = -1;

    //fighters to load into arena
    public static int team2fighterIndex;
    public static int team1fighterIndex;

    //Camera location
    public static int cameraLocation;
    public static int bonus;
}
