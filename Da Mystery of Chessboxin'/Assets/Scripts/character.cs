using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour {
    public Animator animator;
    private bool canAnim;
    public float speed;
    private Rigidbody rb;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        canAnim = true;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        rb.AddForce(movement * speed);
        Debug.Log(Input.GetAxis("Horizontal"));
        if (Input.GetAxis("Horizontal") > 0.1)
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        if (Input.GetAxis("Horizontal") < -0.1)
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        if(Input.GetAxis("Horizontal") == 0)
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", true);
        }
    }

    // Update is called once per frame
    void Update () {
        buttonPress();
	}

    void buttonPress()
    {
        if (Input.GetButtonDown("light punch") == true && canAnim)
        {
            animator.SetBool("X", true);
            canAnim = false;
            StartCoroutine(WaitLP());
        }
        else if (Input.GetButtonDown("heavy punch") == true && canAnim)
        {
            animator.SetBool("Y", true);
            canAnim = false;
            StartCoroutine(WaitHP());
        }
        else if (Input.GetButtonDown("light kick") == true && canAnim)
        {
            animator.SetBool("A", true);
            canAnim = false;
            StartCoroutine(WaitLK());
        }
        else if (Input.GetButtonDown("heavy kick") == true && canAnim)
        {
            animator.SetBool("B", true);
            canAnim = false;
            StartCoroutine(WaitHK());
        }
    }
    IEnumerator WaitLP()
    {
        yield return new WaitForSeconds(1.2f);
        animator.SetBool("X", false);
        canAnim = true;
    }
    IEnumerator WaitHP()
    {
        yield return new WaitForSeconds(1.8f);
        animator.SetBool("Y", false);
        canAnim = true;
    }
    IEnumerator WaitLK()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("A", false);
        canAnim = true;
    }
    IEnumerator WaitHK()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("B", false);
        canAnim = true;
    }
}
