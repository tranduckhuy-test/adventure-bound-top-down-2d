using UnityEngine;

public class Slime : Enemy
{
    private Rigidbody2D myRigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        CheckDistance();
        UpdateAnimation();
    }

    private void CheckDistance()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= chaseRadius && distance > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk)
            {
                MoveTowardsTarget();
                ChangeState(EnemyState.walk);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void MoveTowardsTarget()
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
        animator.SetBool("isMoving", true);

        spriteRenderer.flipX = target.position.x < transform.position.x;
    }

    private void UpdateAnimation()
    {
        animator.SetBool("isHit", isHit);
        if (isHit)
        {
            if (isDead)
            {
                animator.SetTrigger("dead");
                StartCoroutine(Die());
            }
            isHit = false;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
}
