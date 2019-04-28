using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class character : MonoBehaviour
{
    public Animator animator;
    private bool canAnim;
    public float speed;
    private Rigidbody rb;
    public int health = 100;
    public BoxCollider leftHand;
    public BoxCollider rightHand;
    public CapsuleCollider rightLegUpper;
    public CapsuleCollider leftLeg;
    public CapsuleCollider rightLeg;
    public GameObject enemy;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        canAnim = true;
        rb = GetComponent<Rigidbody>();
        leftHand = leftHand.GetComponent<BoxCollider>();
        rightHand = rightHand.GetComponent<BoxCollider>();
        leftLeg = leftLeg.GetComponent<CapsuleCollider>();
        rightLeg = rightLeg.GetComponent<CapsuleCollider>();
        rightLegUpper = rightLegUpper.GetComponent<CapsuleCollider>();
        if (this.tag == "Player")
        {
            enemy = GameObject.FindGameObjectWithTag("Player 2");
        }
        else
            enemy = GameObject.FindGameObjectWithTag("Player");
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
        else if (this.CompareTag("Player 2"))
        {
            rb.AddForce(movement2 * speed);
        }
        if (Input.GetAxis("Horizontal") == 0 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", true);
        }
        else if (Input.GetAxis("Horizontal") < -0.1 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        else if (Input.GetAxis("Horizontal") > 0.1 && this.CompareTag("Player"))
        {
            animator.SetFloat("movement", Input.GetAxis("Horizontal"));
            animator.SetBool("stopped", false);
        }
        if (Input.GetAxis("Horizontal 2") == 0 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", true);
        }
        else if (Input.GetAxis("Horizontal 2") < -0.1 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", false);
        }
        else if (Input.GetAxis("Horizontal 2") > 0.1 && this.CompareTag("Player 2"))
        {
            animator.SetFloat("movement", -Input.GetAxis("Horizontal 2"));
            animator.SetBool("stopped", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        buttonPress();
        if (this.tag == "Player")
        {
            Debug.Log(health);
        }
    }

    void buttonPress()
    {
        if (Input.GetButtonDown("light punch") == true && canAnim && this.CompareTag("Player"))
        {
            leftHand.enabled = true;
            animator.SetBool("X", true);
            canAnim = false;
            StartCoroutine(WaitLP());
        }
        else if (Input.GetButtonDown("heavy punch") == true && canAnim && this.CompareTag("Player"))
        {
            rightHand.enabled = true;
            animator.SetBool("Y", true);
            canAnim = false;
            StartCoroutine(WaitHP());
        }
        else if (Input.GetButtonDown("light kick") == true && canAnim && this.CompareTag("Player"))
        {
            leftLeg.enabled = true;
            animator.SetBool("A", true);
            canAnim = false;
            StartCoroutine(WaitLK());
        }
        else if (Input.GetButtonDown("heavy kick") == true && canAnim && this.CompareTag("Player"))
        {
            rightLegUpper.enabled = true;
            rightLeg.enabled = true;
            animator.SetBool("B", true);
            canAnim = false;
            StartCoroutine(WaitHK());
        }
        if (Input.GetButtonDown("light punch 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            leftHand.enabled = true;
            animator.SetBool("X", true);
            canAnim = false;
            StartCoroutine(WaitLP());
        }
        else if (Input.GetButtonDown("heavy punch 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            rightHand.enabled = true;
            animator.SetBool("Y", true);
            canAnim = false;
            StartCoroutine(WaitHP());
        }
        else if (Input.GetButtonDown("light kick 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            leftLeg.enabled = true;
            animator.SetBool("A", true);
            canAnim = false;
            StartCoroutine(WaitLK());
        }
        else if (Input.GetButtonDown("heavy kick 2") == true && canAnim && this.CompareTag("Player 2"))
        {
            rightLegUpper.enabled = true;
            rightLeg.enabled = true;
            animator.SetBool("B", true);
            canAnim = false;
            StartCoroutine(WaitHK());
        }
    }
    IEnumerator WaitLP()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("X", false);
        leftHand.enabled = false;
        canAnim = true;
    }
    IEnumerator WaitHP()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Y", false);
        rightHand.enabled = false;
        canAnim = true;
    }
    IEnumerator WaitLK()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("A", false);
        leftLeg.enabled = false;
        canAnim = true;
    }
    IEnumerator WaitHK()
    {
        yield return new WaitForSeconds(1.1f);
        animator.SetBool("B", false);
        rightLegUpper.enabled = false;
        rightLeg.enabled = false;
        canAnim = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (leftHand.enabled)
        {
            enemy.GetComponent<character>().health -= 10;
        }
        if (rightHand.enabled)
        {
            enemy.GetComponent<character>().health -= 20;
        }
        if (leftLeg.enabled)
        {
            enemy.GetComponent<character>().health -= 10;
        }
        if (rightLeg.enabled)
        {
            enemy.GetComponent<character>().health -= 20;
        }
        else if (rightLegUpper.enabled)
        {
            enemy.GetComponent<character>().health -= 20;
        }
    }
}
