using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int health;
    public string enemyName;
    public int baseAttack;
    public float moveSpeed;
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Transform homePosition;
    public EnemyState currentState;

    public bool isHit;
    public bool isDead;

    public virtual void TakeDamage(int damage)
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
        Destroy(gameObject);
    }

    public void Knock(Rigidbody2D myRigidbody, float knockTime)
    {
        StartCoroutine(KnockCo(myRigidbody, knockTime));
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

