using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Vector3 cursorPos;
    Vector3 nodePos;
    GameObject cursor;
    public Vector3 cameraOffset;
    Vector3[] offsetRotation;
    Quaternion[] rotationRotation;
    int camPosition;
	// Use this for initialization
	void Start () {
        camPosition = 0;
        offsetRotation = new Vector3[4];
        rotationRotation = new Quaternion[4];
        cameraOffset = new Vector3(0, 80, -65);
        offsetRotation[0] = cameraOffset;
        offsetRotation[1] = new Vector3(32, 120, -48);
        offsetRotation[2] = new Vector3(-32, 120, -48);
        offsetRotation[3] = new Vector3(0, 240, -15);
        cursor = GameObject.Find("Cursor");
        cursorPos = cursor.transform.position;

        transform.position = cursorPos + offsetRotation[3];
        transform.LookAt(cursorPos);
        rotationRotation[3] = transform.rotation;

        transform.position = cursorPos + offsetRotation[2];
        transform.LookAt(cursorPos);
        rotationRotation[2] = transform.rotation;

        transform.position = cursorPos + offsetRotation[1];
        transform.LookAt(cursorPos);
        rotationRotation[1] = transform.rotation;

        transform.position = cursorPos + offsetRotation[0];
        transform.LookAt(cursorPos);
        rotationRotation[0] = transform.rotation;


    }

    // Update is called once per frame
    void Update () {
        cursorPos = cursor.transform.position;
        transform.position = Vector3.Lerp(transform.position, cursorPos + cameraOffset, 0.05f);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationRotation[camPosition], 0.05f);
    }

    public void CameraRotate()
    {

        camPosition = (camPosition + 1) % 4;
        cameraOffset = offsetRotation[camPosition];
        
    }
}
