using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPainterCTRL : MonoBehaviour
{
    Animator animator;
    Color32 normal;
    [SerializeField] Color32 deathByEnemyMax;
    [SerializeField] Color32 deathByWaterMax;
    PlayerMovement player;
    SpriteRenderer sprite;
    [SerializeField] float fadingSpeed;

    void Start()
    {
        animator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = normal;
    }
    void Update()
    {
        dieIfDead();
    }

    void dieIfDead()
    {
        if (player.deathBy == "nothing")
        {
            return;
        }
        else if(player.deathBy == "water")
        {
            sprite.color = Color.Lerp(sprite.color, deathByWaterMax, Mathf.PingPong(Time.deltaTime*fadingSpeed, 1));
        }
        else if(player.deathBy == "damage")
        {
            sprite.color = Color.Lerp(sprite.color, deathByEnemyMax, Mathf.PingPong(Time.deltaTime*fadingSpeed, 1));
        }
    }
}
