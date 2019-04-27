using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour {
    public Animator animator;
    private bool canAnim;
    public float speed;
    private Rigidbody rb;
    private int health = 100;
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        canAnim = true;
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveHorizontal2 = Input.GetAxis("Horizontal 2");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, 0.0f);
        Vector3 movement2 = new Vector3(moveHorizontal2, 0.0f, 0.0f);
        if (this.CompareTag("Player"))
        {
            rb.AddForce(movement * speed);
        }
        else if(this.CompareTag("Player 2"))
        {
            rb.AddForce(movement2 * speed);
        }
        if (Input.GetAxis("Horizontal") == 0 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", true);
        }
        else if(Input.GetAxis("Horizontal") < -0.1 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        else if(Input.GetAxis("Horizontal") > 0.1 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        if (Input.GetAxis("Horizontal 2") == 0 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", true);
        }
        else if(Input.GetAxis("Horizontal 2") < -0.1 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", false);
        }
        else if(Input.GetAxis("Horizontal 2") > 0.1 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", false);
        }
    }

    // Update is called once per frame
    void Update () {
        buttonPress();
        Debug.Log(health);
	}

    private void OnCollisionEnter(Collision collision)
    {
        health -= 10;
    }

    void buttonPress()
    {
        if (Input.GetButtonDown("light punch") == true && canAnim && this.CompareTag("Player"))
        {
            animator.SetBool("X", true);
            canAnim = false;
            StartCoroutine(WaitLP());
        }
        else if (Input.GetButtonDown("heavy punch") == true && canAnim && this.CompareTag("Player"))
        {
            animator.SetBool("Y", true);
            canAnim = false;
            StartCoroutine(WaitHP());
        }
        else if (Input.GetButtonDown("light kick") == true && canAnim && this.CompareTag("Player"))
        {
            animator.SetBool("A", true);
            canAnim = false;
            StartCoroutine(WaitLK());
        }
        else if (Input.GetButtonDown("heavy kick") == true && canAnim && this.CompareTag("Player"))
        {
            animator.SetBool("B", true);
            canAnim = false;
            StartCoroutine(WaitHK());
        }
        if (Input.GetButtonDown("light punch 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            animator.SetBool("X", true);
            canAnim = false;
            StartCoroutine(WaitLP());
        }
        else if (Input.GetButtonDown("heavy punch 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            animator.SetBool("Y", true);
            canAnim = false;
            StartCoroutine(WaitHP());
        }
        else if (Input.GetButtonDown("light kick 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            animator.SetBool("A", true);
            canAnim = false;
            StartCoroutine(WaitLK());
        }
        else if (Input.GetButtonDown("heavy kick 2") == true && canAnim && this.CompareTag("Player 2"))
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
