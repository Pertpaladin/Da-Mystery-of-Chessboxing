using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridSceneManager : MonoBehaviour
{

    //grid
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

    //movement
    Stack<GameObject> movementStack;
    int range;

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
        takeInput = false;
        whereThePlayers = new int[8, 2];
        movementStack = new Stack<GameObject>();
        CharacterModels = new GameObject[8];
        CharactersToMove = new List<int>();

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
                                    if (SpeculateSheet.transform.GetChild(k).name.Equals("Tile (" + x + "," + z + ")"))
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
        
        GameManager.fighters[0] = new Fighter(1, 0, 0);
        GameManager.fighters[1] = new Fighter(1, 1, 0);
        GameManager.fighters[2] = new Fighter(1, 2, 0);
        GameManager.fighters[3] = new Fighter(1, 3, 0);
        GameManager.fighters[4] = new Fighter(2, 0, 1);
        GameManager.fighters[5] = new Fighter(2, 1, 1);
        GameManager.fighters[6] = new Fighter(2, 2, 1);
        GameManager.fighters[7] = new Fighter(2, 3, 1);
        
        //place characters
        for (int i = 0; i < 8; i++)
        {
            Debug.Log(GameManager.fighters[i].model);
            CharacterModels[i] =
                Instantiate(GameManager.fighterGameObjects[GameManager.fighters[i].model],
                GameManager.gridSpaces[GameManager.fighters[i].xPos, GameManager.fighters[i].zPos].transform.position, new Quaternion());
            whereThePlayers[i, 0] = GameManager.fighters[i].xPos;
            whereThePlayers[i, 1] = GameManager.fighters[i].zPos;
            Debug.Log("Char " + i + " at " + GameManager.gridSpaces[GameManager.fighters[i].xPos, GameManager.fighters[i].zPos].transform.name);
            //CharacterModels[i].transform.Find("Canvas").Find("Slider").GetComponent<HealthbarScript>().SetPlayer(i);
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
                            if (range >= currentCharacter.range)
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
                            if (range >= currentCharacter.range)
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
                            if (range >= currentCharacter.range)
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
                            if (range >= currentCharacter.range)
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
    
    }




IEnumerator MoveCharacter(GameObject[] Path)
    {
        GameObject guy = CharacterModels[GameManager.turn];
        //turn on walk animation

        Debug.Log("MoveCharacterCalled");
        for (int i = Path.Length - 1; i >= 0; i--)
        {
            Debug.Log("Going to " + Path[i].name);
            guy.transform.LookAt(new Vector3(Path[i].transform.position.x, guy.transform.position.y, Path[i].transform.position.z));
            while ((guy.transform.position - Path[i].transform.position).magnitude > 0)
            {
                guy.transform.position = Vector3.Lerp(guy.transform.position, Path[i].transform.position, 1);
                yield return new WaitForSeconds(0.5f);
            }
            //guy.transform.position = Path[i].transform.position;
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
            else if (cursorX == whereThePlayers[i, 0] && cursorY - (int)Input.GetAxisRaw("DY2") == whereThePlayers[i, 1])
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
                SceneManager.LoadScene("DummyScene");
            }
        }
        if (!newScene)
        {
            NextTurn();
        }
    }
}
