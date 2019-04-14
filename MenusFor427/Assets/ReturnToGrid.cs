using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToGrid : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Return());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Return()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("GridDemo");
    }
}
