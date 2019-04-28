using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthImage : MonoBehaviour
{
    Image image;
    int playerIndex;
    GameObject cam;
    Vector3 offset;
    Vector3 bounceVector;
    public int bounceScale = 1;


    // Use this for initialization
    void Start()
    {
        image = transform.GetChild(1).GetComponent<Image>();
        Debug.Log(image.name);
        cam = GameObject.Find("Main Camera");
        offset = new Vector3(0, 40f, 0);
        bounceVector = new Vector3(0, 0.125f, 0);
        transform.position = GridSceneManager.CharacterModels[playerIndex].transform.position + offset;
        transform.LookAt(transform.position + cam.GetComponent<CameraFollow>().cameraOffset);
        StartCoroutine(Bounce());
    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = GameManager.fighters[playerIndex].health;
        transform.LookAt(transform.position + cam.GetComponent<CameraFollow>().cameraOffset);
        //transform.LookAt(cam.transform);
        //transform.rotation = Quaternion.Lerp(transform.rotation, cam.transform.rotation, 0.05f);
        //transform.LookAt(Vector3.Lerp(transform.position + cam.GetComponent<CameraFollow>().previousOffset, transform.position + cam.GetComponent<CameraFollow>().cameraOffset, 0.05f));
    }
    public void SetPlayer(int index)
    {
        playerIndex = index;
    }

    IEnumerator Bounce()
    {
        while (true)
        {
            if (GameManager.turn != playerIndex)
            {
                bounceVector = new Vector3(0, 0.125f, 0);

                //transform.position = Vector3.Lerp(transform.position, GridSceneManager.CharacterModels[playerIndex].transform.position + offset, 0.1f);
                transform.position = GridSceneManager.CharacterModels[playerIndex].transform.position + offset;
                yield return new WaitForSeconds(0.01f);

            }
            else
            {
                //transform.position = transform.position + bounceOffset;
                while (GameManager.turn == playerIndex)
                {
                    switch ((bounceScale * (int)(transform.position.y - (GridSceneManager.CharacterModels[playerIndex].transform.position + offset).y) + 2))
                    {
                        case 0:
                            bounceVector = Vector3.zero - bounceVector;
                            break;
                        case 1:
                            if (bounceVector.y > 0)
                            {
                                bounceVector = bounceVector * 1.1f;
                            }
                            else
                            {
                                bounceVector = bounceVector / 1.1f;
                            }
                            break;
                        case 2:
                            if (bounceVector.y < 0)
                            {
                                bounceVector = bounceVector * 1.1f;
                            }
                            else
                            {
                                bounceVector = bounceVector / 1.1f;
                            }
                            break;
                        case 3:
                            bounceVector = Vector3.zero - bounceVector;
                            break;
                    }

                    transform.position = new Vector3(GridSceneManager.CharacterModels[playerIndex].transform.position.x, transform.position.y + bounceVector.y, GridSceneManager.CharacterModels[playerIndex].transform.position.z);
                    yield return new WaitForSeconds(0.01f);
                }
            }
        }
    }
}
