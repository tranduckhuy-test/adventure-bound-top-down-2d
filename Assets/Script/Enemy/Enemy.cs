using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected float health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float respawnTime = 5f;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Rigidbody2D myRigidbody;

    public bool isHit;
    public bool isDead;

    public virtual void CheckDistance()
    {
        if (!target.GetComponent<Collider2D>().enabled)
        {
            MoveTowardsTarget(homePosition.position);
            ChangeState(EnemyState.walk);
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= chaseRadius && distance > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk)
            {
                MoveTowardsTarget(target.position);
                //MoveTowardsTarget();
                ChangeState(EnemyState.walk);
            }
        }
        else
        {
            MoveTowardsTarget(homePosition.position);
            ChangeState(EnemyState.walk);
            //animator.SetBool("isMoving", false);
        }
    }

    public virtual void MoveTowardsTarget(Vector3 destination)
    {
        Vector3 temp = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
        myRigidbody.MovePosition(temp);
        animator.SetBool("isMoving", true);

        // Flip sprite based on the movement direction
        spriteRenderer.flipX = destination.x < transform.position.x;
    }

    protected void UpdateAnimation()
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

    protected void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        isHit = true;
        if (health <= 0)
        {
            isDead = true;
        }
        StartCoroutine(TakeDamageCo());
    }

    private IEnumerator TakeDamageCo()
    {
        yield return new WaitForSeconds(0.35f);
        isHit = false;
    }

    protected IEnumerator Die()
    {
        yield return new WaitForSeconds(0.5f);

        GetComponent<Collider2D>().enabled = false;
        spriteRenderer.enabled = false;

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);

        GetComponent<Collider2D>().enabled = true;
        spriteRenderer.enabled = true;

        health = maxHealth.initialValue;
        isDead = false;

        animator.ResetTrigger("dead");
        animator.SetBool("isHit", false);
        animator.SetBool("isMoving", false);

        transform.position = homePosition.position;

        animator.Play("idle");

        yield return new WaitForSeconds(1f);
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime, float damage)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
        TakeDamage(damage);
    }

    private IEnumerator KnockCo(Rigidbody2D myRigidbody, float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = EnemyState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
}

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

