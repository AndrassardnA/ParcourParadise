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

    [Header("-------CLIMB-------")]
    [SerializeField] float climbSpeed = 1;

    [Header("-------JUMP-------")]
    [SerializeField] float preJumpTime = 1;
    [SerializeField] float coyoteTime = 0.5f;
    [SerializeField] float jumpSpeed = 1;
    [SerializeField] float mayJump;
    float myGravityScale;
    bool isAlive = true;
    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cCollider = GetComponent<CapsuleCollider2D>();
        bCollider = GetComponent<BoxCollider2D>();
        myGravityScale = myRigidbody.gravityScale;
        mayJump = coyoteTime;


    }

    void Update()
    {
        if (isAlive)
        {
            run();
            turning();
            climb();
            updateCoyoteTime();
        }



    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }
    void run()
    {
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
    }
    void updateCoyoteTime()
    {
        if (Input.GetButton("Jump"))
        {
            mayJump = -1;
        }
        else if (bCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            mayJump = coyoteTime;
        }
        else
        {
            mayJump -= Time.deltaTime;
        }
    }
    void turning()
    {
        if (myRigidbody.velocity.x > 0)
        {
            transform.localScale = new Vector3(math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (myRigidbody.velocity.x < 0)
        {
            transform.localScale = new Vector3(-math.abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
    }
    void OnJump(InputValue value)
    {
        if (isAlive)
        {
            if (bCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")) || mayJump > 0)
            {
                //if (value.isPressed)
                // {
                myRigidbody.velocity = new Vector2(0f, jumpSpeed);
                // }
            }
            else
            {
                Invoke("JumpComponent", preJumpTime);
            }

        }
    }

    void JumpComponent() //for prejump
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.velocity = new Vector2(0f, jumpSpeed);
        }
    }


    void climb()
    {
        if (bCollider.IsTouchingLayers(LayerMask.GetMask("Climbable")))
        {
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.gravityScale = 0;
            if (math.abs(myRigidbody.velocity.y) > 0)
            {
                animator.SetBool("isClimbing", true);
            }
            else
            {
                animator.SetBool("isClimbing", false);
            }
        }
        else
        {
            animator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = myGravityScale;
        }

    }
    //PRÓBA GAME OVER
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Water" || collision.tag == "Enemy")
        {
            Debug.Log("Víz");
            myGravityScale = 0;
            runSpeed = 0;
            jumpSpeed = 0;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x / 3, myRigidbody.velocity.y / 3);
            Invoke("gameOver", 1);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (cCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Die();
        }
    }
    void Die()
    {
        isAlive = false;
    }
    void gameOver()
    {
        Debug.Log("GameOver");
        SceneManager.LoadScene(0);
    }

}
