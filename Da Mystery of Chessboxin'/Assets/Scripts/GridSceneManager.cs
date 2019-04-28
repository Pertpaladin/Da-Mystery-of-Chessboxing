using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSceneManager : MonoBehaviour
{

    //grid
    public Animator[] anim;
    public GameObject CHAR00;
    public GameObject CHAR01;
    public GameObject CHAR02;
    public GameObject CHAR03;
    public GameObject CHAR10;
    public GameObject CHAR11;
    public GameObject CHAR12;
    public GameObject CHAR13;
    public GameObject HealthBarBlue;
    public GameObject LeaderHealthBarBlue;
    public GameObject HealthBarRed;
    public GameObject LeaderHealthBarRed;
    public GameObject CursorTail;
    List<GameObject> tail;

    GameObject[,] tiles;
    int[,] whereThePlayers;
    GameObject SpeculateSheet;
    GameObject SpeculateTile;
    GameObject DeadTile;
    public static GameObject[] CharacterModels;
    public int sheetxWide;
    public int sheetzWide;
    public int xWide;
    public int zWide;


    //UI
    public GameObject Cursor;
    int cursorX;
    int cursorY;
    Material cursorMaterial;

    //movement
    Stack<GameObject> movementStack;
    public int range;
    public int bonusRange;

    //controller
    Fighter currentCharacter;
    bool takeInput;
    int previousDX1;
    int previousDX2;
    int previousDY1;
    int previousDY2;

    List<int> CharactersToMove;

    // Use this for initialization
    void Start()
    {

        DeadTile = GameObject.Find("Plane");
        Cursor = GameObject.Find("Cursor");
        tail = new List<GameObject>();
        cursorMaterial = Cursor.GetComponent<Material>();
        takeInput = false;
        whereThePlayers = new int[8, 2];
        movementStack = new Stack<GameObject>();
        CharacterModels = new GameObject[8];
        CharactersToMove = new List<int>();
        anim = new Animator[8];
        //GameManager.turn = -1;
        //Map SceneGrid to GameManagerGrid
        //Only at start of Game
        if (true)//GameManager.turn == -1)
        {
            tiles = new GameObject[xWide * sheetxWide, zWide * sheetzWide];
            for (int i = 0; i < sheetxWide; i++)
            {
                for (int j = 0; j < sheetzWide; j++)
                {
                    SpeculateSheet = GameObject.Find("Tile Sheet (" + i.ToString() + "," + j.ToString() + ")");
                    if (SpeculateSheet != null)
                    {
                        for (int x = 0; x < xWide; x++)
                        {
                            for (int z = 0; z < zWide; z++)
                            {
                                SpeculateTile = DeadTile;
                                for (int k = 0; k < SpeculateSheet.transform.childCount; k++)
                                {
                                    if (SpeculateSheet.transform.GetChild(k).name.Equals("Tile (" + x + "," + z + ")") && SpeculateSheet.transform.GetChild(k).gameObject.activeSelf)
                                    {
                                        SpeculateTile = SpeculateSheet.transform.GetChild(k).gameObject;
                                    }
                                }
                                tiles[i * xWide + x, j * zWide + z] = SpeculateTile;
                                Debug.Log(SpeculateTile.name + " from Sheet(" + i + "," + j + " put in tiles(" + (i * xWide + x) + "," + (j * zWide + z));
                            }
                        }
                    }
                }
            }
            GameManager.gridSpaces = tiles;
        }
        GameManager.fighterGameObjects[0] = CHAR00;
        GameManager.fighterGameObjects[1] = CHAR01;
        GameManager.fighterGameObjects[2] = CHAR02;
        GameManager.fighterGameObjects[3] = CHAR03;
        GameManager.fighterGameObjects[4] = CHAR10;
        GameManager.fighterGameObjects[5] = CHAR11;
        GameManager.fighterGameObjects[6] = CHAR12;
        GameManager.fighterGameObjects[7] = CHAR13;

        anim[0] = CHAR00.GetComponent<Animator>();
        anim[1] = CHAR01.GetComponent<Animator>();
        anim[2] = CHAR02.GetComponent<Animator>();
        anim[3] = CHAR03.GetComponent<Animator>();
        anim[4] = CHAR10.GetComponent<Animator>();
        anim[5] = CHAR11.GetComponent<Animator>();
        anim[6] = CHAR12.GetComponent<Animator>();
        anim[7] = CHAR13.GetComponent<Animator>();

        /*GameManager.fighters[0] = new Fighter(1, 0, 0);
        GameManager.fighters[1] = new Fighter(1, 1, 0);
        GameManager.fighters[2] = new Fighter(1, 2, 0);
        GameManager.fighters[3] = new Fighter(1, 3, 0);
        GameManager.fighters[4] = new Fighter(2, 0, 1);
        GameManager.fighters[5] = new Fighter(2, 1, 1);
        GameManager.fighters[6] = new Fighter(2, 2, 1);
        GameManager.fighters[7] = new Fighter(2, 3, 1);*/
        
        //place characters
        for (int i = 0; i < 8; i++)
        {
            if (GameManager.turn < 0)
            {
                if (i < 4)
                {
                    GameManager.fighters[i].xPos = i;
                    GameManager.fighters[i].zPos = 4;
                }
                else
                {
                    GameManager.fighters[i].xPos = 34;
                    GameManager.fighters[i].zPos = 10-i;
                }
            }
            CharacterModels[i] =
                Instantiate(GameManager.fighterGameObjects[GameManager.fighters[i].model],
                GameManager.gridSpaces[GameManager.fighters[i].xPos, GameManager.fighters[i].zPos].transform.position, new Quaternion());
            CharacterModels[i].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            CharacterModels[i].GetComponent<character>().enabled = false;
            CharacterModels[i].GetComponent<CapsuleCollider>().radius = 0.1f;
            for (int j = 0; j < 8; j++)
            {
                if (i == j)
                {
                    continue;
                }
                else if (GameManager.fighters[i].xPos == whereThePlayers[j, 0] && GameManager.fighters[i].zPos /*- (int)Input.GetAxisRaw("DY2")*/ == whereThePlayers[j, 1])
                {
                    CharacterModels[i].transform.position += new Vector3(3, 0, 0);
                    CharacterModels[j].transform.position -= new Vector3(3, 0, 0);
                    CharacterModels[i].transform.LookAt(CharacterModels[j].transform);
                    CharacterModels[j].transform.LookAt(CharacterModels[i].transform);
                }
            }
            whereThePlayers[i, 0] = GameManager.fighters[i].xPos;
            whereThePlayers[i, 1] = GameManager.fighters[i].zPos;
            if(i == 0)
            {
                Instantiate(LeaderHealthBarRed).GetComponent<HealthImage>().SetPlayer(i);
            } else if(i < 4)
            {
                Instantiate(HealthBarRed).GetComponent<HealthImage>().SetPlayer(i);
            }
            else if (i == 4)
            {
                Instantiate(LeaderHealthBarBlue).GetComponent<HealthImage>().SetPlayer(i);
            } else if (i <= 7 && i > 4)
            {
                Instantiate(HealthBarBlue).GetComponent<HealthImage>().SetPlayer(i);
            }
        }
        if (GameManager.turn > -1)
        {
            Debug.Log("DisplayResults");
            //Display Fight Results
        }
        NextTurn();

    }


    // Update is called once per frame
    void Update()
    {
        if (takeInput)
        {
            switch (GameManager.turn)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    if (Input.GetButtonDown("Y1"))
                    {
                        GameObject.Find("Main Camera").GetComponent<CameraFollow>().CameraRotate();
                    }
                    else if (Input.GetButtonDown("X1"))
                    {
                        //Switch Character
                        SwitchCharacter();
                        //A - Select Tile
                    }
                    else if (Input.GetButtonDown("A1"))
                    {
                        //stay in place
                        if (cursorX == currentCharacter.xPos && cursorY == currentCharacter.zPos)
                        {
                            takeInput = false;
                            bonusRange += 1;
                            CheckForEnemy();
                        }
                        //move
                        else
                        {
                            /*
                            for (int i = 0; i < SpeculatePaths.Count; i++)// (Stack<int[]> x in SpeculatePaths)
                            {
                                if (SpeculatePaths[i].Peek()[0] == cursorX && SpeculatePaths[i].Peek()[1] == cursorY)
                                {
                                    takeInput = false;
                                    moveCharacter(movementStack.ToArray());
                                    whereThePlayers[GameManager.turn, 0] = cursorX;
                                    whereThePlayers[GameManager.turn, 1] = cursorY;
                                    currentCharacter.xPos = cursorX;
                                    currentCharacter.zPos = cursorY;
                                    NextTurn();
                                    break;
                                }
                            }
                            */
                            takeInput = false;
                            StartCoroutine(MoveCharacter(movementStack.ToArray()));
                            whereThePlayers[GameManager.turn, 0] = cursorX;
                            whereThePlayers[GameManager.turn, 1] = cursorY;
                            currentCharacter.xPos = cursorX;
                            currentCharacter.zPos = cursorY;
                            

                            break;
                        }
                    }

                    // B - clear path
                    else if (Input.GetButtonDown("B1"))
                    {
                        //move cursor to current character
                        range = 0;
                        cursorX = currentCharacter.xPos;
                        cursorY = currentCharacter.zPos;
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        //clear movementStack and add current character location
                        while (movementStack.Count > 0)
                        {
                            movementStack.Pop();//.GetComponent<Material>().color = Color.gray;
                        }
                        DestroyTail();
                        movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                    }

                    //Directional Input
                    else if ((Input.GetAxisRaw("DX1") >= 1 || Input.GetAxisRaw("DX1") <= -1) && previousDX1 == 0)
                    {
                        previousDX1 = 1;
                        cursorX += (int)Input.GetAxisRaw("DX1");
                        //if cursor move invalid, reposition cursor
                        if ((cursorX >= xWide * sheetxWide) || cursorX < 0)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX1");
                        }
                        else if (GameManager.gridSpaces[cursorX, cursorY] == DeadTile)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX1");
                        }
                        bool enemyOccupied = false;
                        for (int i = 4; i < 8; i++)
                        {
                            if (i == GameManager.turn)
                            {
                                continue;
                            }
                            else if (cursorX - (int)Input.GetAxisRaw("DX1") == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                            {
                                enemyOccupied = true;
                            }
                        }

                        //if movementStack contains cursor position, pop until peek = cursor position
                        if (movementStack.Contains(GameManager.gridSpaces[cursorX, cursorY]))
                        {
                            while (movementStack.Peek() != GameManager.gridSpaces[cursorX, cursorY])
                            {
                                if(tail.Count > 0)
                                {
                                    Destroy(tail[tail.Count - 1]);
                                    tail.RemoveAt(tail.Count - 1);
                                }
                                movementStack.Pop();
                                range--;
                            }
                        }
                        else if (enemyOccupied && range != 0)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX1");
                        }
                        //else add cursor position to stack
                        else
                        {
                            //check to see if in range
                            if (range >= currentCharacter.range + bonusRange)
                            {
                                cursorX -= (int)Input.GetAxisRaw("DX1");
                            }
                            else
                            {
                                bool occupied = false;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i == GameManager.turn)
                                    {
                                        continue;
                                    }
                                    else if (cursorX == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                                    {
                                        occupied = true;
                                    }
                                }
                                if (!occupied)
                                {
                                    if(movementStack.Count > 1)
                                    {
                                        tail.Add(Instantiate(CursorTail, Cursor.transform.position, new Quaternion()));
                                    }
                                    movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                                    range++;
                                }
                                else
                                {
                                    cursorX -= (int)Input.GetAxisRaw("DX1");
                                }
                            }
                        }
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        Debug.Log("Range: " + range);
                    }
                    else if ((Input.GetAxisRaw("DY1") >= 1 || Input.GetAxisRaw("DY1") <= -1) && previousDY1 == 0)
                    {
                        previousDY1 = 1;
                        cursorY += (int)Input.GetAxisRaw("DY1");
                        //if cursor move invalid, reposition cursor
                        if ((cursorY >= zWide * sheetzWide) || cursorY < 0)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY1");
                        }
                        else if (GameManager.gridSpaces[cursorX, cursorY] == DeadTile)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY1");
                        }
                        bool enemyOccupied = false;
                        for (int i = 4; i < 8; i++)
                        {
                            if (i == GameManager.turn)
                            {
                                continue;
                            }
                            else if (cursorX == whereThePlayers[i, 0] && cursorY - (int)Input.GetAxisRaw("DY1") == whereThePlayers[i, 1])
                            {
                                enemyOccupied = true;
                            }
                        }


                        //if movementStack contains cursor position, pop until peek = cursor position
                        if (movementStack.Contains(GameManager.gridSpaces[cursorX, cursorY]))
                        {
                            while (movementStack.Peek() != GameManager.gridSpaces[cursorX, cursorY])
                            {
                                if (tail.Count > 0)
                                {
                                    Destroy(tail[tail.Count - 1]);
                                    tail.RemoveAt(tail.Count - 1);
                                }
                                movementStack.Pop();
                                range--;
                            }
                        }
                        else if (enemyOccupied && range > 0)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY1");
                        }
                        //else add cursor position to stack
                        else
                        {
                            //check to see if range maxed
                            if (range >= currentCharacter.range + bonusRange)
                            {
                                cursorY -= (int)Input.GetAxisRaw("DY1");
                            }
                            //check to see if occupied
                            else
                            {
                                bool occupied = false;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i == GameManager.turn)
                                    {
                                        continue;
                                    }
                                    else if (cursorX == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                                    {
                                        occupied = true;
                                    }
                                }
                                if (!occupied)
                                {
                                    if (movementStack.Count > 1)
                                    {
                                        tail.Add(Instantiate(CursorTail, Cursor.transform.position, new Quaternion()));                                        
                                    }
                                    movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                                    range++;
                                }
                                else
                                {
                                    cursorY -= (int)Input.GetAxisRaw("DY1");
                                }
                            }
                        }
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        Debug.Log("Range: " + range);

                    }
                    if (Input.GetAxisRaw("DX1") == 0)
                    {
                        previousDX1 = 0;
                    }
                    if (Input.GetAxisRaw("DY1") == 0)
                    {
                        previousDY1 = 0;
                    }
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    if (Input.GetButtonDown("Y2"))
                    {
                        GameObject.Find("Main Camera").GetComponent<CameraFollow>().CameraRotate();
                    }
                    else if (Input.GetButtonDown("X2"))
                    {
                        //MapView
                        SwitchCharacter();
                        //A - Select Tile
                    }
                    else if (Input.GetButtonDown("A2"))
                    {
                        //stay in place
                        if (cursorX == currentCharacter.xPos && cursorY == currentCharacter.zPos)
                        {
                            takeInput = false;
                            bonusRange += 1;
                            CheckForEnemy();

                        }
                        //move
                        else
                        {
                            /*
                            for (int i = 0; i < SpeculatePaths.Count; i++)// (Stack<int[]> x in SpeculatePaths)
                            {
                                if (SpeculatePaths[i].Peek()[0] == cursorX && SpeculatePaths[i].Peek()[1] == cursorY)
                                {
                                    takeInput = false;
                                    moveCharacter(movementStack.ToArray());
                                    whereThePlayers[GameManager.turn, 0] = cursorX;
                                    whereThePlayers[GameManager.turn, 1] = cursorY;
                                    NextTurn();
                                    break;
                                }
                            }
                            */
                            takeInput = false;
                            StartCoroutine(MoveCharacter(movementStack.ToArray()));
                            whereThePlayers[GameManager.turn, 0] = cursorX;
                            whereThePlayers[GameManager.turn, 1] = cursorY;
                            currentCharacter.xPos = cursorX;
                            currentCharacter.zPos = cursorY;
                            
                            break;
                        }
                    }

                    // B - clear path
                    else if (Input.GetButtonDown("B2"))
                    {
                        //move cursor to current character
                        range = 0;
                        cursorX = currentCharacter.xPos;
                        cursorY = currentCharacter.zPos;
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        //clear movementStack and add current character location
                        while (movementStack.Count > 0)
                        {
                            movementStack.Pop();//.GetComponent<Material>().color = Color.gray;
                        }
                        DestroyTail();
                        movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                    }

                    //Directional Input
                    else if ((Input.GetAxisRaw("DX2") >= 1 || Input.GetAxisRaw("DX2") <= -1) && previousDX2 == 0)
                    {
                        previousDX2 = 1;
                        cursorX += (int)Input.GetAxisRaw("DX2");
                        //if cursor move invalid, reposition cursor
                        if ((cursorX >= xWide * sheetxWide) || cursorX < 0)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX2");
                        }
                        else if (GameManager.gridSpaces[cursorX, cursorY] == DeadTile)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX2");
                        }
                        bool enemyOccupied = false;
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == GameManager.turn)
                            {
                                continue;
                            }
                            else if (cursorX - (int)Input.GetAxisRaw("DX2") == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                            {
                                enemyOccupied = true;
                            }
                        }


                        //if movementStack contains cursor position, pop until peek = cursor position
                        if (movementStack.Contains(GameManager.gridSpaces[cursorX, cursorY]))
                        {
                            while (movementStack.Peek() != GameManager.gridSpaces[cursorX, cursorY])
                            {
                                if (tail.Count > 0)
                                {
                                    Destroy(tail[tail.Count - 1]);
                                    tail.RemoveAt(tail.Count - 1);
                                }
                                movementStack.Pop();//.GetComponent<Material>().color = Color.gray;
                                range--;
                            }
                        }
                        else if (enemyOccupied && range > 0)
                        {
                            cursorX -= (int)Input.GetAxisRaw("DX2");
                        }


                        //else add cursor position to stack
                        else
                        {
                            //check to see if in range
                            if (range >= currentCharacter.range + bonusRange)
                            {
                                cursorX -= (int)Input.GetAxisRaw("DX2");
                            }
                            else
                            {
                                bool occupied = false;
                                for (int i = 4; i < 8; i++)
                                {
                                    if (i == GameManager.turn)
                                    {
                                        continue;
                                    }
                                    else if (cursorX == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                                    {
                                        occupied = true;
                                    }
                                }
                                if (!occupied)
                                {
                                    if (movementStack.Count > 1)
                                    {
                                        tail.Add(Instantiate(CursorTail, Cursor.transform.position, new Quaternion()));
                                    }
                                    movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                                    //GameManager.gridSpaces[cursorX, cursorY].GetComponent<Material>().color = Color.green;
                                    range++;
                                }
                                else
                                {
                                    cursorX -= (int)Input.GetAxisRaw("DX2");
                                }
                            }
                        }
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        Debug.Log("Range: " + range);

                    }
                    else if ((Input.GetAxisRaw("DY2") >= 1 || Input.GetAxisRaw("DY2") <= -1) && previousDY2 == 0)
                    {
                        previousDY2 = 1;
                        cursorY += (int)Input.GetAxisRaw("DY2");
                        //if cursor move invalid, reposition cursor
                        if ((cursorY >= zWide * sheetzWide) || cursorY < 0)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY2");
                        }
                        else if (GameManager.gridSpaces[cursorX, cursorY] == DeadTile)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY2");
                        }
                        bool enemyOccupied = false;
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == GameManager.turn)
                            {
                                continue;
                            }
                            else if (cursorX == whereThePlayers[i, 0] && cursorY - (int)Input.GetAxisRaw("DY2") == whereThePlayers[i, 1])
                            {
                                enemyOccupied = true;
                            }
                        }

                        //if movementStack contains cursor position, pop until peek = cursor position
                        if (movementStack.Contains(GameManager.gridSpaces[cursorX, cursorY]))
                        {
                            while (movementStack.Peek() != GameManager.gridSpaces[cursorX, cursorY])
                            {
                                if (tail.Count > 0)
                                {
                                    Destroy(tail[tail.Count - 1]);
                                    tail.RemoveAt(tail.Count - 1);
                                }
                                movementStack.Pop();//.GetComponent<Material>().color = Color.gray;
                                range--;
                            }
                        }
                        else if (enemyOccupied && range > 0)
                        {
                            cursorY -= (int)Input.GetAxisRaw("DY2");
                        }


                        //else add cursor position to stack
                        else
                        {
                            //check to see if range maxed
                            if (range >= currentCharacter.range + bonusRange)
                            {
                                cursorY -= (int)Input.GetAxisRaw("DY2");
                            }
                            //check to see if occupied
                            else
                            {
                                bool occupied = false;
                                for (int i = 4; i < 8; i++)
                                {
                                    if (i == GameManager.turn)
                                    {
                                        continue;
                                    }
                                    else if (cursorX == whereThePlayers[i, 0] && cursorY == whereThePlayers[i, 1])
                                    {
                                        occupied = true;
                                    }
                                }
                                if (!occupied)
                                {
                                    if(movementStack.Count > 1)
                                    {
                                        tail.Add(Instantiate(CursorTail, Cursor.transform.position, new Quaternion()));
                                    }
                                    movementStack.Push(GameManager.gridSpaces[cursorX, cursorY]);
                                    //GameManager.gridSpaces[cursorX, cursorY].GetComponent<Material>().color = Color.green;
                                    range++;
                                }
                                else
                                {
                                    cursorY -= (int)Input.GetAxisRaw("DY2");
                                }
                            }
                        }
                        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
                        Debug.Log("Range: " + range);

                    }
                    if (Input.GetAxisRaw("DX2") == 0)
                    {
                        previousDX2 = 0;
                    }
                    if (Input.GetAxisRaw("DY2") == 0)
                    {
                        previousDY2 = 0;
                    }
                    break;

                default:
                    break;
            }
        }
    }
    void SwitchCharacter()
    {
        GameManager.turn = CharactersToMove[(CharactersToMove.IndexOf(GameManager.turn) + 1) % CharactersToMove.Count];
        currentCharacter = GameManager.fighters[GameManager.turn];
        //send cursor to currentCharacter
        cursorX = currentCharacter.xPos;
        cursorY = currentCharacter.zPos;
        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
        //clear EVERYTHING
        takeInput = false;
        movementStack.Clear();
        movementStack.Push(GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos]);
        range = 0;
        takeInput = true;
        DestroyTail();
    }

    //start of turn
    void NextTurn()
    {
        if (CharactersToMove.Count > 1)
        {
            int lastIndex = CharactersToMove.IndexOf(GameManager.turn);
            CharactersToMove.Remove(GameManager.turn);
            GameManager.turn = CharactersToMove[lastIndex % CharactersToMove.Count];
        }
        else
        {
            CharactersToMove.Clear();
            //GameManager.turn = (GameManager.turn + 1) % 8;
            GameManager.turn = ((GameManager.turn / 4) * 4 + 4) % 8;
            CharactersToMove.Add(GameManager.turn);
            CharactersToMove.Add(GameManager.turn + 1);
            CharactersToMove.Add(GameManager.turn + 2);
            CharactersToMove.Add(GameManager.turn + 3);
            bonusRange = 0;
        }
        currentCharacter = GameManager.fighters[GameManager.turn];
        //send cursor to currentCharacter
        cursorX = currentCharacter.xPos;
        cursorY = currentCharacter.zPos;
        Cursor.transform.position = GameManager.gridSpaces[cursorX, cursorY].transform.position;
        //clear EVERYTHING
        takeInput = false;
        movementStack.Clear();
        movementStack.Push(GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos]);
        range = 0;
        takeInput = true;
        DestroyTail();
    
    }




IEnumerator MoveCharacter(GameObject[] Path)
    {
        GameObject guy = CharacterModels[GameManager.turn];
        Vector3 difference;
        int divisions = 10;
        //turn on walk animation
        guy.GetComponent<Animator>().SetBool("walk", true);
        
        

        Debug.Log("MoveCharacterCalled");
        for (int i = Path.Length - 2; i >= 0; i--)
        {
            Debug.Log("Going to " + Path[i].name);
            guy.transform.LookAt(new Vector3(Path[i].transform.position.x, guy.transform.position.y, Path[i].transform.position.z));
            difference = Path[i].transform.position - guy.transform.position;
            for(int j = 0; j < divisions; j++) {
                guy.transform.position += difference / divisions;
                yield return new WaitForSeconds(0.00001f);
            }
            if (tail.Count > 0)
            {
                if (tail[tail.Count - 1] != null)
                {
                    Destroy(tail[Path.Length - i - 2]);
                    //tail.RemoveAt(0);
                }
            }
            
            //guy.transform.position = Path[i].transform.position;
        }
        guy.GetComponent<Animator>().SetBool("walk", false);
        if (range > currentCharacter.range)
        {
            bonusRange -= range - currentCharacter.range;
        }
        CheckForEnemy();
    }

    void CheckForEnemy()
    {
        bool newScene = false;
        for (int i = 0; i < 8; i++)
        {
            if (i == GameManager.turn)
            {
                continue;
            }
            else if (cursorX == whereThePlayers[i, 0] && cursorY /*- (int)Input.GetAxisRaw("DY2")*/ == whereThePlayers[i, 1])
            {
                if (currentCharacter.team == 1)
                {
                    GameManager.team1fighterIndex = GameManager.turn;
                    GameManager.team2fighterIndex = i;
                }
                else
                {
                    GameManager.team2fighterIndex = GameManager.turn;
                    GameManager.team1fighterIndex = i;
                }
                newScene = true;
                if (GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (0,0)") || GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (0,1)") || GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (1,0)"))
                {
                    GameManager.cameraLocation = 0;
                    Debug.Log("Location 0");
                }
                else if (GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (1,1)"))
                {
                    GameManager.cameraLocation = 1;
                    Debug.Log("Location 1");

                }
                else if (GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (3,1)") || GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (4,1)"))
                {
                    GameManager.cameraLocation = 2;
                    Debug.Log("Location 2");

                }
                else if (GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (5,1)") || GameManager.gridSpaces[currentCharacter.xPos, currentCharacter.zPos].transform.parent.name.Equals("Tile Sheet (5,2)"))
                {
                    GameManager.cameraLocation = 4;
                    Debug.Log("Location 4");

                }
                else
                {
                    GameManager.cameraLocation = 3;
                    Debug.Log("Location 3");

                }
                SceneManager.LoadScene("Fighting_Scene");
            }
        }
        if (!newScene)
        {
            NextTurn();
        }
    }
    void DestroyTail()
    {
        foreach(GameObject x in tail)
        {
            Destroy(x);
        }
        tail.Clear();
    }
}
