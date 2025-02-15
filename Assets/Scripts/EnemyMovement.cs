using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D myRigidbody;
    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        animator.speed *= moveSpeed;
        if (transform.localScale.x < 0)
        {
            moveSpeed = -Mathf.Abs(moveSpeed);
        }
        else
        {
            moveSpeed = Mathf.Abs(moveSpeed);
        }
    }

    
    void Update()
    {
        myRigidbody.velocity = new Vector2(moveSpeed, 0);
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            moveSpeed = -moveSpeed;
            
        } 
    }
}
