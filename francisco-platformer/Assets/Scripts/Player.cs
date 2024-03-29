using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float climbSpeed = 5.0f;
    [SerializeField] private Vector2 deathSeq = new Vector2(25f, 25f);
    [SerializeField] private Vector2 hurtSeq = new Vector2(15f, 15f);

    public int health = 100;

    private int jumps = 0;

    bool isAlive = true;

    private float gravityScaleAtStart;

    Rigidbody2D playerCharacter;

    Animator playerAnimator;

    CapsuleCollider2D playerBodyCollider;

    BoxCollider2D playerFeetCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();

        gravityScaleAtStart = playerCharacter.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        print("Number of jumps: " + jumps);
        //print(health);

        if (!isAlive)
        {
            return;
        }

        Run();
        FlipSprite();
        Jump();
        Climb();
        //die();
        Hurt();

    }

    private void Run()
    {
        //Value between -1 to +1
        float hmovement = Input.GetAxis("Horizontal");
        Vector2 runVelocity = new Vector2(hmovement * runSpeed, playerCharacter.velocity.y);
        playerCharacter.velocity = runVelocity;

        // playerAnimator.SetBool("run", true);

        // print(runVelocity);

        bool hSpeed = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;

        playerAnimator.SetBool("run", hSpeed);
    }

    private void FlipSprite()
    {
        // If the player is moving horizontally
        bool hMovement = Mathf.Abs(playerCharacter.velocity.x) > Mathf.Epsilon;
        if (hMovement)
        {
            //Reverse the current direction (Scale) of the X-Axis
            transform.localScale = new Vector2(Mathf.Sign(playerCharacter.velocity.x), 1f);
        }
   
    }

    private void Jump()
    {
        if (playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
        {
            jumps = 1;
        }
        if (jumps >=2)
        {
            
            if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing")))
            {
                //Will stop this function unless true
                return;
            }
        }

        if (jumps >= 2)
        {
            jumps = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            //Get new Y velocity based on a controllable variable
            Vector2 jumpVelocity = new Vector2(0.0f, jumpSpeed);
            playerCharacter.velocity += jumpVelocity;
            jumps++;
        }
    }

    private void Climb()
    {
        if (!playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerAnimator.SetBool("climb", false);

            playerCharacter.gravityScale = gravityScaleAtStart;

            //Will stop this function unless true

            return;
        }
        //"Vertical from Input Axis
        float vMovement = Input.GetAxis("Vertical");

        //X Axis needs to remain the same and we need to change Y
        Vector2 climbVelocity = new Vector2(playerCharacter.velocity.x, vMovement * climbSpeed);
        playerCharacter.velocity = climbVelocity;

        playerCharacter.gravityScale = 0.0f;

        bool vSpeed = Mathf.Abs(playerCharacter.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("climb", vSpeed);
    }

    private void Hurt()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards")))
        {
            playerCharacter.velocity = hurtSeq;
            health--;
        }

        if (health <=0)
        {
            playerAnimator.SetTrigger("die");
            playerCharacter.velocity = deathSeq;

            isAlive = false;
        }
        if (!isAlive)
        {
            FindAnyObjectByType<GameSession>().ProcessPlayerDeath();
        }
        
    }
    //private void die()
    //{
    //}
}

