using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour {
    Slider slider;
    int playerIndex;
    GameObject cam;
    Vector3 offset;
    Vector3 bounceOffset;
    Vector3 bouncing;
    float movespeed;
	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        cam = GameObject.Find("Main Camera");
        offset = new Vector3(0, 10f, 0);
        bounceOffset = new Vector3(0, 3f, 0);
        transform.position = GridSceneManager.CharacterModels[playerIndex].transform.position + offset;
        transform.LookAt(transform.position + cam.GetComponent<CameraFollow>().cameraOffset);
        movespeed = 0.1f;
        bouncing = new Vector3(0, 0.055f, 0);
        StartCoroutine(bounce());

    }

    // Update is called once per frame
    void Update () {
        slider.value = GameManager.fighters[playerIndex].health;
        
        transform.LookAt(transform.position + cam.GetComponent<CameraFollow>().cameraOffset);
        
	}
    public void SetPlayer(int index)
    {
        playerIndex = index;
        if(index < 4)
        {
            //slider.fillRect = (RectTransform)transform.Find("FillArea");
            transform.Find("FillArea").Find("Fill(2)").gameObject.SetActive(false);
            transform.Find("FillArea").Find("Fill").gameObject.SetActive(false);
        }
        else
        {
            //slider.fillRect = (RectTransform)transform.Find("Fill Area").Find("Fill (2)");
            transform.Find("FillArea").Find("Fill(1)").gameObject.SetActive(false);
            transform.Find("FillArea").Find("Fill").gameObject.SetActive(false);
            slider.fillRect = transform.Find("FillArea").Find("Fill(2)").GetComponent<RectTransform>();
        }
    }
    IEnumerator bounce()
    {
        while (true)
        {
            if (GameManager.turn != playerIndex)
            {
                while (GameManager.turn != playerIndex)
                {
                    transform.position = Vector3.Lerp(transform.position, GridSceneManager.CharacterModels[playerIndex].transform.position + offset, 0.1f);
                    yield return new WaitForSeconds(0.01f);
                }
            }
            else
            {
                //transform.position = transform.position + bounceOffset;
                while (GameManager.turn == playerIndex)
                {
                    transform.position += bouncing;
                    if (((bouncing.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.up * 3).y >= -0.01) || (bouncing.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.down * 3).y <= 0.01)))
                    {
                        bouncing = Vector3.zero - bouncing;
                    } else if (((bouncing.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.up * 3).y >= -1) || (bouncing.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.down * 3).y <= 1)))
                    {
                        bouncing.y = Mathf.Clamp(bouncing.y / 1.5f, -0.5f, 0.5f);
                    }
                    else if (((bouncing.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.up * 3).y <= -2) || (bouncing.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + Vector3.down * 3).y >= 2)))
                    {
                        bouncing.y = Mathf.Clamp(bouncing.y * 1.5f, -0.5f, 0.5f);
                    }
                    yield return new WaitForSecondsRealtime(0.01f);

                    /*
                    transform.position = Vector3.Lerp(transform.position, GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset, movespeed);
                    if ((bounceOffset.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y >= -0.01) ||  (bounceOffset.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y <= 0.01))
                    {
                        bounceOffset = Vector3.zero - bounceOffset;
                    } else if ((bounceOffset.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y >= -1) || (bounceOffset.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y <= 1))
                    {
                        movespeed = 0.05f;
                    } else if ((bounceOffset.y > 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y >= -2) || (bounceOffset.y < 0 && transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset + bounceOffset).y <= 2))
                    {
                        movespeed = 0.3f;
                    }
                    */
                }
            }
        }
    }
}
