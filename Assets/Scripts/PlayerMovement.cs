using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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
    private float jumpTimeCounter; //max time you can jump//
    public float jumpTime;
    private bool crashThroughEverything = false;
    public int crashThroughDuration = 10;
    public int flyingModeDuration = 20;
    public int powerUpRemainingTime { get; private set; }
    private int powerUpStartTime;
    private bool flyingMode = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bottomOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
        healthScore = 100;
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
            SceneManager.LoadScene("MainMenu");
        }

    }

    private void FixedUpdate()
    {

        animator.SetFloat("Speed", runSpeed);
        Vector2 velocity = rb.velocity;
        velocity.x = runSpeed*(1+Time.timeSinceLevelLoad*speedIncreaseFactor);

            if (jump == true && isGrounded == true && flyingMode==false)
            {
                velocity.y = jumpVelocity;
                jumpTimeCounter = jumpTime;
                jumping = true;
            }


            if (jump == true && flyingMode == true)
            {
                velocity.y = jumpVelocity;
                jumping = false;
            }
        
            else if (jumping == true && jumpTimeCounter > 0 && flyingMode==false)
            {
                velocity.y = jumpVelocity;
                jumpTimeCounter -= Time.deltaTime;
            }

            else
            {
                jumping = false;
            }
            jump = false;


        rb.velocity = velocity;


        //calculate time for power ups//
        if (crashThroughEverything == true)
        {
           powerUpRemainingTime = Mathf.Max(crashThroughDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad)- powerUpStartTime),0);
           if(powerUpRemainingTime == 0)
           {
                crashThroughEverything = false;
           }


        }

        if (flyingMode == true)
        {
            powerUpRemainingTime = Mathf.Max(flyingModeDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                flyingMode = false; print("flying mode set to false");
            }


        }



    }


   
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Collectable"))
        {
            Destroy(col.gameObject);
            PlayerScores();
        }

        if (col.gameObject.CompareTag("CrashThrough"))
        {
            Destroy(col.gameObject);
            DisableAllPowerUps();
            crashThroughEverything = true;
            powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        }


        if (col.gameObject.CompareTag("FlyingMode"))
        {
            Destroy(col.gameObject);
            DisableAllPowerUps();
            flyingMode = true; print("flying mode set to true");
            powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        }

    }



    public int PlayerScores()
    {
        playerScore++;
        return playerScore;
    }




    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.collider.gameObject.CompareTag("Obstacle") && crashThroughEverything == false)

        {
            if (healthScore > 1)
                
            {
                healthScore--;
                Destroy(col.gameObject);
            }
            else
            {
                healthScore--;
                SceneManager.LoadScene("MainMenu");

            }
        }
        else if (col.collider.gameObject.CompareTag("Obstacle") && crashThroughEverything == true)
        {
            Destroy(col.gameObject);
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {

        if (col.collider.gameObject.CompareTag("Platform") && col.relativeVelocity.y==0)
        {
            isGrounded = true;
        }

    }


    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }

    }

    private void DisableAllPowerUps()
    {

        crashThroughEverything = false;
        flyingMode = false;

    }



}
