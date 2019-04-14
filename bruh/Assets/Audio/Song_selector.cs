using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Song_selector : MonoBehaviour {
    public AudioSource audioManager;
    public AudioClip first;
    public AudioClip second;
    public AudioClip third;
    public AudioClip fourth;
    private bool canPlay = true;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canPlay)
        {
            switch (Random.Range(1, 4))
            {
                case 1:
                    audioManager.clip = first;
                    audioManager.Play();
                    canPlay = false;
                    StartCoroutine("PlayMoosic");
                    break;
                case 2:
                    audioManager.clip = second;
                    audioManager.Play();
                    canPlay = false;
                    StartCoroutine("PlayMoosic");
                    break;
                case 3:
                    audioManager.clip = third;
                    audioManager.Play();
                    canPlay = false;
                    StartCoroutine("PlayMoosic");
                    break;
                case 4:
                    audioManager.clip = fourth;
                    audioManager.Play();
                    canPlay = false;
                    StartCoroutine("PlayMoosic");
                    break;
                default:
                    break;
            }
        }
	}

    IEnumerator PlayMoosic()
    {
        yield return new WaitForSecondsRealtime(360);
        canPlay = true;
    }

}
