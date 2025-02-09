using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    Rigidbody2D myRigidBody;
    [SerializeField] float bulletSpeed;
    GameObject player;
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        transform.localScale = player.transform.localScale;
        if (transform.localScale.x < 0)
        {
            bulletSpeed *= -1;
        }
    }


    void Update()
    {
        myRigidBody.velocity = new Vector2(bulletSpeed,0);

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (myRigidBody.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    void kill(GameObject g)
    {
        Destroy(g);
    }
}
