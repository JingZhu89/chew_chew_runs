using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float runSpeed = 6.0f;
    public float jumpVelocity = 1.0f;
    float horizontalMove = 0f;
    bool jump = false;
    Rigidbody2D rb;
    Animator animator;
    private float bottomOfScreen;
    private bool isGrounded;
    private int playerScore;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bottomOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
    }
    // Update is called once per frame
    void Update()
    {

        animator.SetBool("IsJumping", jump);
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (transform.position.y <= bottomOfScreen)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

    private void FixedUpdate()
    {

        horizontalMove = runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        Vector2 velocity = rb.velocity;
        velocity.x = horizontalMove;
        if (jump == true && isGrounded==true) { velocity.y = jumpVelocity; }
        jump = false;
        rb.velocity = velocity;
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        { Destroy(col.gameObject);
            playerScore++;
            print("Current score is" + playerScore);
        }

    }



    private void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.collider.gameObject.CompareTag("Obstacle"))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        print("Got collision with " + col.collider.name);
        print("Collider has tag " + col.collider.gameObject.tag);
        if (col.collider.gameObject.CompareTag("Platform") && col.relativeVelocity.y==0)
        {
            isGrounded = true;
            print("set is grounded true");
        }

    }


    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Platform"))
        { isGrounded = false;
            print("set is grounded false");
        }

    }



}
