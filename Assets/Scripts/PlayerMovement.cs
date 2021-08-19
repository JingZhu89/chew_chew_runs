using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerMovement SharedInstance;
    public float runSpeed = 5.0f;
    public float currentSpeed;
    public float jumpVelocity = 1.0f;
    public float speedIncreaseFactor = 1.0f;
    bool jump = false;
    Rigidbody2D rb;


    private float bottomOfScreen;
    private bool isGrounded;
    public int playerScore { get; private set; }
    public int healthScore { get; private set; }
    private bool jumping = false;
    private float jumpTimeCounter; //max time you can jump//
    public float jumpTime;
    private bool jumpingUp;
    private bool jumpingDown;
       

    public int powerUpRemainingTime { get; private set; }
    private int powerUpStartTime;
    private int powerUpDuration;

    private bool flyingMode = false;
    private bool crashThroughEverything = false;
    private bool freeze = false;

    private bool gotHit;
    public int gotHitRemainingTime { get; private set; }
    private int gotHitStartTime;
    public int gotHitDuration;

    Animator animator;
    private string currentState;
    public float speedThreshold1;
    public float freezeSpeed;


    private void Awake()
    {
        SharedInstance = this;
    }

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
        
        Vector2 velocity = rb.velocity;

        if (velocity.y > 0)
        {
            jumpingUp = true;
        }
        else
        {
            jumpingUp = false;

        }



        if (velocity.y < 0)
        {
            jumpingDown = true;
        }
        else
        {
            jumpingDown = false;

        }


        //x velocity calculation//
        if (freeze == true)
        {
            velocity.x = freezeSpeed;
        }
        if (gotHit==true)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x = runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor);
        }

       

        currentSpeed = velocity.x;


        //y velocity calculation//
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

        //print("current speed is" + currentSpeed);
        //calculate time for power ups//

        if (crashThroughEverything == true)
        {
           powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad)- powerUpStartTime),0);
           if(powerUpRemainingTime == 0)
           {
                crashThroughEverything = false;
           }


        }

        if (flyingMode == true)
        {
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                flyingMode = false; print("flying mode set to false");
            }

        }

        if (gotHit == true)
        {
            gotHitRemainingTime = Mathf.Max(gotHitDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - gotHitStartTime), 0);
            if (gotHitRemainingTime == 0)
                gotHit = false;

        }



 


        //determin which animation to play
        if( flyingMode==false && crashThroughEverything==false && gotHit==false && currentSpeed==0.0f && isGrounded==true)
        {
            ChangePlayerAnimationState("chuchu idle");

        }
        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed <= speedThreshold1 && isGrounded == true)
        {
            ChangePlayerAnimationState("chuchu run");
        }

        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed > speedThreshold1 && isGrounded == true)
        {
            ChangePlayerAnimationState("chuchu run fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed <= speedThreshold1 && jumpingUp == true)
        {
            ChangePlayerAnimationState("chuchu jumpup");
        }

        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed < speedThreshold1 && jumpingDown == true)
        {
            ChangePlayerAnimationState("chuchu jumpdown");
        }

        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed > speedThreshold1 && jumpingUp == true)
        {
            ChangePlayerAnimationState("chuchu jumpup fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotHit == false && currentSpeed > speedThreshold1 && jumpingDown == true)
        {
            ChangePlayerAnimationState("chuchu jumpdown fast");
        }
        
        if (flyingMode == false && crashThroughEverything == false && isGrounded ==true && gotHit == true )
        {
            ChangePlayerAnimationState("chuchu run hurt");
        }

        if (flyingMode == false && crashThroughEverything == false && jumpingUp == true && gotHit == true)
        {
            ChangePlayerAnimationState("chuchu jumpup hurt");
        }


        if (flyingMode == false && crashThroughEverything == false && jumpingDown == true && gotHit == true)
        {
            ChangePlayerAnimationState("chuchu jumpdown hurt");
        }



        if (flyingMode == true)
        {
            ChangePlayerAnimationState("chuchu fly");
        }
        if(crashThroughEverything == true)
        {
            ChangePlayerAnimationState("chuchu roll");
        }



    }


   
    private void OnTriggerEnter2D(Collider2D col)
    {
        var collectable = col.gameObject.GetComponent<Collectable>();
        if (collectable != null)
        {
            if (col.gameObject.CompareTag("Collectable"))
            {
                Destroy(col.gameObject);
                playerScore=playerScore+collectable.points;
            }
        }

        var powerup = col.gameObject.GetComponent<PowerUp>();
        if (powerup != null)
        {
            if (col.gameObject.CompareTag("CrashThrough"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                crashThroughEverything = true;
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("FlyingMode"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                flyingMode = true; print("flying mode set to true");
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("Freeze"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                freeze = true; print("slowdown set to true");
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }


        }

        if (col.gameObject.name.Contains("cactus"))
        {
            col.gameObject.GetComponent<Animator>().SetBool("CactusGotHit", true); print("cactus got hit");
        }

        if (col.gameObject.name.Contains("spikes"))
        {
            col.gameObject.GetComponent<Animator>().SetBool("SpikeGotHit", true); print("spike got hit");
        }

        if (col.gameObject.CompareTag("Obstacle") && crashThroughEverything == false)

        {
            DisableAllPowerUps();
            gotHit = true;
            gotHitStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
            if (healthScore > 1)

            {
                healthScore--; print("obstacle hit");
            }

            else
            {
                healthScore--;
                SceneManager.LoadScene("MainMenu");

            }
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
        freeze = false;

    }


    private void ChangePlayerAnimationState(string newState)
    {

        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;

    }




}
