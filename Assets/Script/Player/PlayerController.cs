using System;
using System.Collections;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float attackSpeed = 0.45f;

    private SoundManager soundManager;
    private PlayerControls playerControls;
    public PlayerState currentState;
    private Rigidbody2D myRigidbody;
    private Vector2 change;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
	public FloatValue currentHealth;
	public Signal playerHealthSignal;


	private void Awake()
    {
        playerControls = new PlayerControls();
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
        
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {

        if (playerControls.Player.Attack.triggered && currentState != PlayerState.attack
           && currentState != PlayerState.stagger)
        {
            soundManager.PlaySound2D("Woosh");
            StartCoroutine(AttackCo());
        }
    }

    private void FixedUpdate()
    {
        change = Vector2.zero;
        change = playerControls.Player.Move.ReadValue<Vector2>();

        if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(attackSpeed);
        currentState = PlayerState.walk;
    }

    void UpdateAnimationAndMove()
    {
        if (change != Vector2.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        var targetPos = myRigidbody.position + speed * Time.fixedDeltaTime * change;
        myRigidbody.MovePosition(targetPos);
    }

    public void Knock(float knockTime, float damage)
    {
        soundManager.PlaySound2D("ReceiveDamage");
        currentHealth.RuntimeValue -= damage;
		playerHealthSignal.Raise();
        //StartCoroutine(KnockCo(knockTime));
        if (currentHealth.RuntimeValue > 0)
        {

            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            soundManager.PlaySound2D("Death");
            animator.SetTrigger("death");
            if (change.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (change.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            myRigidbody.simulated = false;
            GetComponent<Collider2D>().enabled = false;
            //this.gameObject.SetActive(false);
        }
    }

    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    //public void TakeDame(float damage)
    //{
    //    currentHealth.initialValue -= damage;
    //    playerHealthSignal.Raise();
    //    if (currentHealth.initialValue <= 0)
    //    {
    //        if (change.x < 0)
    //        {
    //            spriteRenderer.flipX = true;
    //        }
    //        else if (change.x > 0)
    //        {
    //            spriteRenderer.flipX = false;
    //        }

    //        animator.SetTrigger("death");
    //        myRigidbody.simulated = false;
    //    }

    //    StartCoroutine(TakeDamageCo());
    //}

    private IEnumerator TakeDamageCo()
    {
        yield return new WaitForSeconds(0.35f);
        currentState = PlayerState.idle;
    }
}
