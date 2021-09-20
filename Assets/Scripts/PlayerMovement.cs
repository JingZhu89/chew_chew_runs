using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerMovement SharedInstance;
    public float runSpeed = 5.0f;
    public float currentSpeed;
    public float jumpVelocity = 1.0f;
    public float speedIncreaseFactor = 1.0f;
    bool jump = false;
    bool squeeze = false;
    Rigidbody2D rb;

    private float bottomOfScreen;
    private bool isGrounded;
    public int playerScore { get; private set; }

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
    private bool gotKilled;
    public int gotHitRemainingTime { get; private set; }
    private int gotHitStartTime;
    public int gotHitDuration;

    Animator animator;
    public float playerAnimatorSpeedMultiplier;
    public float playerScoreMultiplier;
    private string currentState;
    public float speedThreshold1;
    public float freezeSpeed;
    private Light2D playerLt;

    private void Awake()
    {
        SharedInstance = this;
    }


    private void Start()
    {
        StartCoroutine(DelayedStart());
        playerScore = 0;
        powerUpRemainingTime = 0;
        gotKilled = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bottomOfScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;

        playerLt = GetComponentInChildren<Light2D>();
        playerLt.enabled = false;print("playerLtdisabled");
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

            gotKilled = true;
            GameControl.control.AddScore(playerScore);
            GameControl.control.Top5Scores();
            GameControl.control.TopScore();
            GameOver.SharedInstance.EndGame();
        }

        if (Input.GetButtonDown("Down"))
        {
            squeeze = true; 
        }
        if (Input.GetButtonUp("Down"))
        {
            squeeze = false; 
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
        else if (gotHit == true)
        {
            velocity.x = 0;
        }
        else
        {
            velocity.x = runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor);
        }
  

        currentSpeed = velocity.x;
        animator.speed = currentSpeed * playerAnimatorSpeedMultiplier;


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

 

        if (crashThroughEverything == true)
        {
           powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad)- powerUpStartTime),0);
           if(powerUpRemainingTime == 0)
           {
                crashThroughEverything = false;
                playerLt.enabled = false; print("playerLtdisabled");
            }


        }

        if (flyingMode == true)
        {
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                flyingMode = false;
                playerLt.enabled = false; print("playerLtdisabled");
            }

        }


        if (freeze == true)
        {
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                freeze = false;
                playerLt.enabled = false; print("playerLtdisabled");
            }

        }

        if (gotHit == true)
        {
            gotHitRemainingTime = Mathf.Max(gotHitDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - gotHitStartTime), 0);
            if (gotHitRemainingTime == 0)
                gotHit = false;

        }






        //determin which animation to play
        if( flyingMode==false && crashThroughEverything==false && gotKilled==false && currentSpeed==0.0f && isGrounded==true && squeeze ==false)
        {
            ChangePlayerAnimationState("chuchu idle");

        }
        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze == false)
        {
            ChangePlayerAnimationState("chuchu run");

        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == false)
        {
            ChangePlayerAnimationState("chuchu run fast");
        }


        if (flyingMode == false && crashThroughEverything == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze==true && gotKilled ==false)
        {
            ChangePlayerAnimationState("chuchu squeeze");
        }

        if (flyingMode == false && crashThroughEverything == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu squeeze fast");
        }
        


        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && jumpingUp == true)
        {
            ChangePlayerAnimationState("chuchu jumpup");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed < speedThreshold1 && jumpingDown == true)
        {
            ChangePlayerAnimationState("chuchu jumpdown");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingUp == true)
        {
            ChangePlayerAnimationState("chuchu jumpup fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingDown == true)
        {
            ChangePlayerAnimationState("chuchu jumpdown fast");
        }
        

        if (gotKilled == true)
        {
            ChangePlayerAnimationState("chuchu die");
            Time.timeScale=0;
        }
        //if (flyingMode == false && crashThroughEverything == false && isGrounded ==true && gotHit == true )
        //{
        //    ChangePlayerAnimationState("chuchu run hurt");
        //}

        //if (flyingMode == false && crashThroughEverything == false && jumpingUp == true && gotHit == true)
        //{
        //    ChangePlayerAnimationState("chuchu jumpup hurt");
        //}


        //if (flyingMode == false && crashThroughEverything == false && jumpingDown == true && gotHit == true)
        //{
        //    ChangePlayerAnimationState("chuchu jumpdown hurt");
        //}

        
        if (flyingMode == true && squeeze == false)
        {
            ChangePlayerAnimationState("chuchu fly");
        }
        if(crashThroughEverything == true && squeeze == false)
        {
            ChangePlayerAnimationState("chuchu roll");
        }


        if (flyingMode == true && squeeze == true)
        {
            ChangePlayerAnimationState("chuchu squeeze excited fly");
        }
        if (crashThroughEverything == true && squeeze == true)
        {
            ChangePlayerAnimationState("chuchu squeeze excited");
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
            Light2D powerupLt = col.gameObject.GetComponentInChildren<Light2D>();

            playerLt.color = powerupLt.color;

            if (col.gameObject.CompareTag("CrashThrough"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                crashThroughEverything = true;
                playerLt.enabled = true; print("playerLt enabled");
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("FlyingMode"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                flyingMode = true;
                playerLt.enabled = true; print("playerLt enabled");
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("Freeze"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                freeze = true;
                playerLt.enabled = true; print("playerLt enabled");
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }


        }

        //if (col.gameObject.name.Contains("cactus"))
        //{
        //    col.gameObject.GetComponent<Animator>().SetBool("CactusGotHit", true); 
        //}

        //if (col.gameObject.name.Contains("spikes"))
        //{
        //    col.gameObject.GetComponent<Animator>().SetBool("SpikeGotHit", true); 
        //}

        //if (col.gameObject.CompareTag("Obstacle") && crashThroughEverything == false)

        //{
        //    DisableAllPowerUps();
        //    gotHit = true;
        //    gotHitStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
        //    if (healthScore > 1)

        //    {
        //        healthScore--; 
        //    }

        //    else
        //    {
        //        healthScore--;
        //        GameOver.SharedInstance.EndGame();

        //    }

        //}

    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.collider.gameObject.name.Contains ("tall floating left") || col.collider.gameObject.name.Contains("tall floating single")) && crashThroughEverything == false)
        {
 

            float floatingTallPlatformDownYposition = col.collider.gameObject.transform.Find("down").position.y;
            float floatingTallPlatformLeftXposition = col.collider.gameObject.transform.Find("left").position.x;
            if (transform.position.x < floatingTallPlatformLeftXposition || transform.position.y < floatingTallPlatformDownYposition)
            {
                gotKilled = true;
                GameControl.control.AddScore(playerScore);
                GameControl.control.Top5Scores();
                GameControl.control.TopScore();
                GameOver.SharedInstance.EndGame(); 
            }
        }
        else
        {
            print("collided with " + col.collider.name);
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
        playerLt.enabled = false;print("playerLT disabled");
    }


    private void ChangePlayerAnimationState(string newState)
    {

        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;

    }


    IEnumerator DelayedStart()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = 1;
    }


}
