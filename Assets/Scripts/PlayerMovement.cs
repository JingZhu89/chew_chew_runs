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
    public bool rocketMode = false;
    public float rocketYUp = 5f;
    public float rocketYDown = 4f;
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
    public float maxSpeed;
    public float freezeSpeedmultiplier;
    public float rollingSpeedmultiplier;
    private Light2D playerLt;
    private Collider2D playerCollider;

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
        playerCollider = GetComponent<Collider2D>();
        playerCollider.enabled = true;
        bottomOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        topOfScreen = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 1)).y;
        pointsJustEarnedNew = 0;
        playerLt = GetComponentInChildren<Light2D>();
        playerLt.enabled = false;
        DisableAllPowerUps();
        FindObjectOfType<AudioManager>().PlaySound("LevelThemeSound");


    }
    // Update is called once per frame
    void Update()
    {

        //jump input and condition
        if (Input.GetButtonDown("Jump") && rocketMode==false)
        {
            jump = true;
            FindObjectOfType<AudioManager>().PlaySound("ChuChuJumpSound");
        }

        if (Input.GetButtonUp("Jump") && rocketMode==false)
        {
            jumping = false;
        }

        //squeeze input and condition

        if (Input.GetButtonDown("Down") && rocketMode==false)
        {
            squeeze = true;
        }
        if (Input.GetButtonUp("Down") && rocketMode==false)
        {
            squeeze = false;
        }

        if (transform.position.y <= bottomOfScreen || transform.position.y > topOfScreen + 3)
        {
            if (!gotKilled)
            {
                DisableAllPowerUps();
                GameControl.control.AddScore(playerScore);
                GameControl.control.GetLastScore(playerScore);
                GameControl.control.GetTop5Scores();
                GameControl.control.TopScore();
                GameOver.SharedInstance.EndGame();
            }
            gotKilled = true;

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
            velocity.x = freezeSpeedmultiplier * Mathf.Min(runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor),maxSpeed);
        }

        else if (rolling == true)
        {
            velocity.x = rollingSpeedmultiplier * Mathf.Min(runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor),maxSpeed);

        }
        else if (rocketMode==true)
        {
            velocity.x = maxSpeed;
        }

        else
        {
            velocity.x = Mathf.Min(runSpeed * (1 + Time.timeSinceLevelLoad * speedIncreaseFactor), maxSpeed);
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
            FindObjectOfType<AudioManager>().PlaySound("Propeller");
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                flyingMode = false;

                playerLt.enabled = false; 
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().StopSound("Propeller");
        }


        if (freeze == true)
        {
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            if (powerUpRemainingTime == 0)
            {
                freeze = false;
                playerLt.enabled = false;
            }

        }

        if (rocketMode == true)
        {
            FindObjectOfType<AudioManager>().PlaySound("ChuChuRocketSound");
            powerUpRemainingTime = Mathf.Max(powerUpDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - powerUpStartTime), 0);
            transform.position = new Vector3(transform.position.x, Random.Range(rocketYDown, rocketYUp), transform.position.z);
 
            if (powerUpRemainingTime == 0)
            {
                rocketMode = false;
                playerCollider.enabled = true;
                rb.gravityScale = 6;
                playerLt.enabled = false;

            }

        }
        else
        {
            FindObjectOfType<AudioManager>().StopSound("ChuChuRocketSound");
        }

        if (rolling == true)
        {
            FindObjectOfType<AudioManager>().PlaySound("ChuChuRollSound");
            rollingRemainingTime = Mathf.Max(rollingDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - rollingStartTime), 0);
            if (rollingRemainingTime == 0)
                rolling = false;
        }
        else
        {
            FindObjectOfType<AudioManager>().StopSound("ChuChuRollSound");
        }

        if (atePoop == true)
        {
            atePoopRemainingTime = Mathf.Max(rollingDuration - (Mathf.RoundToInt(Time.timeSinceLevelLoad) - atePoopStartTime), 0);
            if (atePoopRemainingTime == 0)
                atePoop = false;

        }






        //determin which animation to play
        if ( flyingMode==false && crashThroughEverything==false && gotKilled==false && currentSpeed==0.0f && isGrounded==true && squeeze ==false && rolling == false && atePoop==false && rocketMode==false)
        {
            ChangePlayerAnimationState("chuchu idle");

        }
        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze == false && rolling== false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu run");

        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == false && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu run fast");
        }


        if (flyingMode == false && crashThroughEverything == false && currentSpeed <= speedThreshold1 && isGrounded == true && squeeze==true && gotKilled ==false && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu squeeze");
        }

        if ((flyingMode == false && crashThroughEverything == false && currentSpeed > speedThreshold1 && isGrounded == true && squeeze == true && gotKilled == false && rolling == false && atePoop == false && rocketMode == false) || (rolling==true && squeeze==true && isGrounded==true && gotKilled==false))
        {
            ChangePlayerAnimationState("chuchu squeeze fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed <= speedThreshold1 && jumpingUp == true && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu jumpup");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed < speedThreshold1 && jumpingDown == true && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingUp == true && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu jumpup fast");
        }

        if (flyingMode == false && crashThroughEverything == false && gotKilled == false && currentSpeed > speedThreshold1 && jumpingDown == true && rolling == false && atePoop == false && rocketMode == false)
        {
            ChangePlayerAnimationState("chuchu jumpdown fast");
        }
        

        if (gotKilled == true)
        {
            ChangePlayerAnimationState("chuchu die");
            Time.timeScale=0;
        }

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

        if (rocketMode == true)
        {
            ChangePlayerAnimationState("chuchu rocket");
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
                    FindObjectOfType<AudioManager>().PlaySound("ChuChuEatPoopSound");
                    DisableAllPowerUps();
                    atePoop = true;
                    atePoopStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                }
                else
                { FindObjectOfType<AudioManager>().PlaySound("ChuChuEatSound"); }

            }
        }

        var powerup = col.gameObject.GetComponent<PowerUp>();

        if (powerup != null)
        {
            Light2D powerupLt = col.gameObject.GetComponentInChildren<Light2D>();
            FindObjectOfType<AudioManager>().PlaySound("ChuChuPowerUpSound");

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

            if (col.gameObject.CompareTag("Rocket"))
            {
                Destroy(col.gameObject);
                DisableAllPowerUps();
                rocketMode = true;
                playerCollider.enabled = false;
                rb.gravityScale = 0;
                playerLt.enabled = true;
                powerUpStartTime = Mathf.RoundToInt(Time.timeSinceLevelLoad);
                powerUpDuration = powerup.duration;
            }


        }


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
                    DisableAllPowerUps();
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
                    DisableAllPowerUps();
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
        rocketMode = false;
        playerLt.enabled = false;


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
