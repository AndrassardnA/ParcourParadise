using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    Vector2 moveInput;
    CapsuleCollider2D cCollider;
    BoxCollider2D bCollider;
    Rigidbody2D myRigidbody;
    [Header("-------RUN-------")]
    [SerializeField] float runSpeed = 1;
    float runSpeedSetted;
    bool facingLeft;

    [Header("-------CLIMB-------")]
    [SerializeField] float climbSpeed = 1;

    [Header("-------JUMP-------")]
    [SerializeField] float preJumpTime = 1;
    [SerializeField] float coyoteTime = 0.5f;
    [SerializeField] float jumpSpeed = 1;
    float mayJump;
    [SerializeField] float jumpOffButtonSmooth;
    [SerializeField] float fallingGravityMod;
    float myGravityScale;
    bool isAlive = true;
    float preJumpTimer;
    bool bouncing;
    [Header("-------LAUNCHING-------")]
    [SerializeField] float launchX;
    [SerializeField] float launchY;
    bool islaunched;

    [Header("-------BULLET-------")]
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gun;

    public string deathBy = "nothing";
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cCollider = GetComponent<CapsuleCollider2D>();
        bCollider = GetComponent<BoxCollider2D>();
        myGravityScale = myRigidbody.gravityScale;
        mayJump = coyoteTime;
        runSpeedSetted = runSpeed;

    }

    void Update()
    {
        if (isAlive)
        {
            run();
            turning();
            climb();
            setBouncing();
            Launch();
            setPreJumpTimer();
            PreJump();
            updateCoyoteTime();
            jumpButtonOff();
            Falling();
        }
    }
    //MOVING AROUND
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }
    void run()
    {
        
       // if (!islaunched)
       // {
            Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
            myRigidbody.velocity = playerVelocity;
            if (math.abs(myRigidbody.velocity.x) > 0)
            {
                animator.SetBool("isRunning", true);
            }
            else
            {
                animator.SetBool("isRunning", false);
            }
       // }

    }
    void turning()
    {
        if (myRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingLeft = false;
        }
        else if (myRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            facingLeft = true;
        }
    }
    //JUMPING
    void Jump()
    {
        myRigidbody.velocity = new Vector2(0f, jumpSpeed);
    }
    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (isTouchingGround() || mayJump > 0)
            {
                Jump();
            }
        }
    }
    void jumpButtonOff()
    {
        if (!bouncing && !islaunched)
        {
            if (Input.GetButtonUp("Jump"))
            {
                myRigidbody.velocity -= new Vector2(0f, myRigidbody.velocity.y / jumpOffButtonSmooth);
            }
        }
    }
    void PreJump()
    {
        if (isTouchingGround() && preJumpTimer >= 0)
        {
            Jump();

        }
    }
    void setPreJumpTimer()
    {
        if (!isTouchingGround() && Input.GetButtonDown("Jump"))
        {
            preJumpTimer = preJumpTime;
        }
        else if (!isTouchingGround())
        {
            preJumpTimer -= Time.deltaTime;
        }
    }
    void updateCoyoteTime()
    {
        if (Input.GetButton("Jump"))
        {
            mayJump = -1;
        }
        else if (isTouchingGround())
        {
            mayJump = coyoteTime;
        }
        else
        {
            mayJump -= Time.deltaTime;
        }
    }
    bool isTouchingGround()
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")) /*|| bCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing"))*/)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //OTHER MEHANICS
    void Falling()
    {
        if (!isTouchingGround() && myRigidbody.velocity.y <= 0 && !bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.gravityScale = myGravityScale * fallingGravityMod;
        }
        else if (!bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.gravityScale = myGravityScale;
        }
    }
    void setBouncing()
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")))
        {
            bouncing = true;
        }
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            bouncing = false;
        }
    }
    //CLIMBING
    void climb()
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.gravityScale = 0;
            animateClimbOn();
        }
        else
        {
            animateClimbOff();
            myRigidbody.gravityScale = myGravityScale;
        }

    }
    void animateClimbOn()
    {
        if (math.abs(myRigidbody.velocity.y) > 0)
        {
            animator.SetBool("isClimbing", true);
        }
        else
        {
            animator.SetBool("isClimbing", false);
        }
    }
    void animateClimbOff()
    {
        animator.SetBool("isClimbing", false);
    }
    //FIRE
    void OnFire(InputValue value)
    {
        if (isAlive)
        {
            Instantiate(bullet, transform.position, transform.rotation);
        }
    }
    //LAUNCH
    void Launch()
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Launcher")))
        {
            islaunched = true;
            runSpeed = launchX;
            if (facingLeft)
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, launchY);
            }
            else
            {
                myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, launchY);
            }

        }
        if (isTouchingGround())
        {
            islaunched = false;
            runSpeed = runSpeedSetted;
        }
    }
    //GAME OVER
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water")
        {
            deathBy = "water";
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (cCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            deathBy = "damage";
            Die();
        }
    }
    void Die()
    {
        isAlive = false;
        if (deathBy == "damage")
        {
            animator.SetTrigger("Dying");
            myRigidbody.velocity = new Vector2(0, jumpSpeed);
        }
        if (deathBy == "water")
        {
            myRigidbody.gravityScale = 0;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x / 3f, myRigidbody.velocity.y / 3f);
        }
        runSpeed = 0;
        jumpSpeed = 0;
        Invoke("gameOver", 1);
    }
    void gameOver()
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
