using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float runSpeed = 5.0f;
    public float jumpVelocity = 1.0f;
    public float speedIncreaseFactor = 1.0f;
    bool jump = false;
    Rigidbody2D rb;
    Animator animator;
    private float bottomOfScreen;
    private bool isGrounded;
    public int playerScore { get; private set; }
    public int healthScore { get; private set; }
    private bool jumping = false;
    private float jumpTimeCounter;
    public float jumpTime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bottomOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        healthScore = 3;
    }
    // Update is called once per frame
    void Update()
    {

        animator.SetBool("IsJumping", jump);
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            jumping = false;
        }


        if (transform.position.y <= bottomOfScreen)
        {
            healthScore = 0;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

    private void FixedUpdate()
    {

        animator.SetFloat("Speed", runSpeed);
        Vector2 velocity = rb.velocity;
        velocity.x = runSpeed*(1+Time.time*speedIncreaseFactor);
        if (jump == true && isGrounded==true)
        {
            velocity.y = jumpVelocity;
            jumpTimeCounter = jumpTime;
            jumping = true;
        }
        jump = false;
        if (jumping == true && jumpTimeCounter>0)
        {
                velocity.y = jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
         }
            else { jumping = false; }
        

        rb.velocity = velocity;
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        { Destroy(col.gameObject);
            PlayerScores();
        }

    }

    public int PlayerScores()
    {
        playerScore++;
        return playerScore;
    }




    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.gameObject.CompareTag("Obstacle"))

        {
            if (healthScore > 1)


            { healthScore--; Destroy(col.gameObject); }
            else
            {
                healthScore--;
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

            }
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
