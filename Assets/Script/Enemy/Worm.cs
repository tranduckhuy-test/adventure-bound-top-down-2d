using UnityEngine;

public class Worm : Enemy
{
    private void Awake()
    {
        health = maxHealth.initialValue;
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").transform;
        homePosition = new GameObject("HomePosition").transform;
        homePosition.position = transform.position;
    }

    private void FixedUpdate()
    {
        CheckDistance();
        UpdateAnimation();
    }
}
