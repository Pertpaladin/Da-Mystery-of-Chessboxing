using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    Vector3 cursorPos;
    Vector3 nodePos;
    Vector3 cameraOffset;
	// Use this for initialization
	void Start () {
        
        cameraOffset = new Vector3(0, 80, -45);
        cursorPos = GridSceneManager.Cursor.transform.position;
        transform.position = cursorPos + cameraOffset;
        transform.LookAt(cursorPos);
    }

    // Update is called once per frame
    void Update () {
        cursorPos = GridSceneManager.Cursor.transform.position;
        transform.position = Vector3.Lerp(transform.position, cursorPos + cameraOffset, 0.05f);
    }
}
