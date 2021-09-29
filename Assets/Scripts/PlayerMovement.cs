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
    public float flyVelocity = 0.5f;
    public float speedIncreaseFactor = 1.0f;
    bool jump = false;
    bool squeeze = false;
    Rigidbody2D rb;

    private float bottomOfScreen;
    private float topOfScreen;
    private bool isGrounded;
    public int playerScore { get; private set; }
    public int pointsJustEarnedNew { get; private set; }
    private bool jumping = false;
    private float jumpTimeCounter; //max time you can jump//
    public float jumpTime;
    private bool jumpingUp;
    private bool jumpingDown;


    public int powerUpRemainingTime { get; private set; }
    private int powerUpStartTime;
    private int powerUpDuration;

    public bool flyingMode = false;
    public bool crashThroughEverything = false;
    public bool freeze = false;
    public bool atePoop = false;
    public bool rolling = false;
    private bool gotKilled;
    public int rollingRemainingTime { get; private set; }
    private int rollingStartTime;
    public int rollingDuration;
    public int atePoopRemainingTime { get; private set; }
    private int atePoopStartTime;
    public int atePoopDuration;

    Animator animator;
    public float playerAnimatorSpeedMultiplier;
    public float playerScoreMultiplier;
    private string currentState;
    public float speedThreshold1;
    public float freezeSpeedmultiplier;
    public float rollingSpeedmultiplier;
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
        bottomOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        topOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;

        playerLt = GetComponentInChildren<Light2D>();
        playerLt.enabled = false;
        DisableAllPowerUps();

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


        if (transform.position.y <= bottomOfScreen || transform.position.y>topOfScreen + 3)
        {
            if (!gotKilled)
            {
                GameControl.control.AddScore(playerScore);
                GameControl.control.GetLastScore(playerScore);
                GameControl.control.GetTop5Scores();
                GameControl.control.TopScore();
                GameOver.SharedInstance.EndGame();
            }
            gotKilled = true;

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
            velocity.x = freezeSpeedmultiplier * runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor);
        }

        else if (rolling == true)
        {
            velocity.x = rollingSpeedmultiplier * runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor);

        }

        else
        {
            velocity.x = runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor);
        }


        currentSpeed = velocity.x;
        animator.speed = currentSpeed * playerAnimatorSpeedMultiplier;


        //y velocity calculation//
        if (jump == true && isGrounded == true && flyingMode == false)
        {
            velocity.y = jumpVelocity;
            jumpTimeCounter = jumpTime;
            jumping = true;
        }

        else if (jump == true && isGrounded == true && flyingMode == true)
        {
            velocity.y = flyVelocity;
            jumping = true;
        }


        if ((jump == true || jumping==true) && flyingMode == true)
        {
            velocity.y = jumpVelocity;
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

        if (rolling == true)
        {
            rollingRemainingTime = Mathf.Max(rollingDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - rollingStartTime), 0);
            if (rollingRemainingTime == 0)
                rolling = false;

        }

        if (atePoop == true)
        {
            atePoopRemainingTime = Mathf.Max(rollingDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - atePoopStartTime), 0);
            if (atePoopRemainingTime == 0)
                atePoop = false;

        }







        //determin which animation to play
        if ( flyingMode==false && crashThroughEverything==false && gotKilled==false && currentSpeed==0.0f && isGrounded==true && squeeze ==false && rolling == false && atePoop==false)
        {
            ChangePlayerAnimationState("chuchu idle");

        }
        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze == false && rolling== false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu run");

        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == false && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu run fast");
        }


        if (flyingMode == false && crashThroughEverything == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze==true && gotKilled ==false && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu squeeze");
        }

        if ((flyingMode == false && crashThroughEverything == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == true && gotKilled == false && rolling == false && atePoop == false) || (rolling==true && squeeze==true && isGrounded==true && gotKilled==false))
        {
            ChangePlayerAnimationState("chuchu squeeze fast");
        }
        


        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && jumpingUp == true && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu jumpup");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed < speedThreshold1 && jumpingDown == true && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingUp == true && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu jumpup fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingDown == true && rolling == false && atePoop == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown fast");
        }
        

        if (gotKilled == true)
        {
            ChangePlayerAnimationState("chuchu die");
            Time.timeScale=0;
        }

        //if (flyingMode == false && crashThroughEverything == false && isGrounded ==true && atePoop == true )
        //{
        //    ChangePlayerAnimationState("chuchu run hurt");
        //}

        //if (flyingMode == false && crashThroughEverything == false && jumpingUp == true && atePoop == true)
        //{
        //    ChangePlayerAnimationState("chuchu jumpup hurt");
        //}


        //if (flyingMode == false && crashThroughEverything == false && jumpingDown == true && atePoop == true)
        //{
        //    ChangePlayerAnimationState("chuchu jumpdown hurt");
        //}

        if(rolling==true && squeeze == false && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu roll");
        }

        if (flyingMode == true && squeeze == false && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu fly 2");
        }

        if (flyingMode == true && squeeze == true && isGrounded==true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu squeeze excited fly");
        }


        if(atePoop==true && squeeze==false && isGrounded==true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu run hurt");
        }

        if (atePoop == true && jumpingUp == true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu jumpup hurt");
        }
        
        if (atePoop == true && jumpingDown == true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown hurt");
        }

        if (atePoop == true && squeeze == true && isGrounded==true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu squeeze hurt");
        }

        if (crashThroughEverything == true && squeeze == false && isGrounded==true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu run armored");
        }

        if (crashThroughEverything == true && squeeze == true && isGrounded==true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu squeeze armored");
        }

        if (crashThroughEverything == true && jumpingUp == true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu jumpup armored");
        }

        if (crashThroughEverything == true && jumpingDown == true && gotKilled == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown armored");
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
                pointsJustEarnedNew = collectable.points;
                playerScore=Mathf.Max(playerScore+collectable.points,0);
                if (col.gameObject.name.Contains("poop"))
                {
                    DisableAllPowerUps();
                    atePoop = true;
                    atePoopStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                }

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
                playerLt.enabled = true; 
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("FlyingMode"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                flyingMode = true;
                playerLt.enabled = true; 
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }

            if (col.gameObject.CompareTag("Freeze"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                freeze = true;
                playerLt.enabled = true; 
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

        if (col.gameObject.CompareTag("Obstacle"))

        {
            DisableAllPowerUps();
            rolling = true;
            rollingStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);

        }

    }
    

    private void OnCollisionEnter2D(Collision2D col)
    {
        if ((col.collider.gameObject.name.Contains ("tall floating left") || col.collider.gameObject.name.Contains("tall floating single")) && crashThroughEverything == false)
        {


            float floatingTallPlatformSpikeUpYposition = col.collider.gameObject.transform.Find("spikeup").position.y;
            float floatingTallPlatformLeftXposition = col.collider.gameObject.transform.Find("left").position.x;
            if (transform.Find("right").position.x < floatingTallPlatformLeftXposition && transform.Find("down").position.y< floatingTallPlatformSpikeUpYposition)
            {
                if (!gotKilled)
                {
                    GameControl.control.AddScore(playerScore);
                    GameControl.control.GetLastScore(playerScore);
                    GameControl.control.GetTop5Scores();
                    GameControl.control.TopScore();
                    GameOver.SharedInstance.EndGame();
                }

                gotKilled = true;
            }
        }
        else if (col.collider.gameObject.name.Contains("tall floating") && crashThroughEverything == false)
        {

            float floatingTallPlatformDownYposition = col.collider.gameObject.transform.Find("down").position.y;
            if ( transform.position.y < floatingTallPlatformDownYposition)
            {
                if (!gotKilled)
                {
                    GameControl.control.AddScore(playerScore);
                    GameControl.control.GetLastScore(playerScore);
                    GameControl.control.GetTop5Scores();
                    GameControl.control.TopScore();
                    GameOver.SharedInstance.EndGame();
                }

                gotKilled = true;
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
        atePoop = false;
        rolling = false;
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
