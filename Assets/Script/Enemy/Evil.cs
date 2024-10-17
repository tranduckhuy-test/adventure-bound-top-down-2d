using System.Collections;
using UnityEngine;

public class Evil : Enemy
{
	[SerializeField] private float thrust;
	[SerializeField] private float knockTime;
	[SerializeField] private float damage;
	[SerializeField] private float attackCooldown = 0.5f; // Thời gian chờ giữa các lần tấn công

	private bool isAttacking = false; // Để kiểm tra xem có đang tấn công hay không

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

	public override void CheckDistance()
	{
		if (!target.GetComponent<Collider2D>().enabled)
		{
			MoveTowardsTarget(homePosition.position);
			ChangeState(EnemyState.idle);
			return;
		}

		float distance = Vector3.Distance(target.position, transform.position);
		if (distance <= attackRadius && !isAttacking)
		{
			StartCoroutine(Attack());
		}
		else if (distance <= chaseRadius)
		{
			if (currentState == EnemyState.idle || currentState == EnemyState.walk)
			{
				MoveTowardsTarget(target.position);
				ChangeState(EnemyState.walk);
			}
		}
		else
		{
			MoveTowardsTarget(homePosition.position);
			ChangeState(EnemyState.idle);
			animator.SetBool("isMoving", false);
		}
	}

	private IEnumerator Attack()
	{
		isAttacking = true;
		ChangeState(EnemyState.attack);
		animator.SetBool("attacking", true);

		yield return new WaitForSeconds(0.1f); // Chờ 1 chút trước khi gây sát thương để đồng bộ với animation

		// Xác định hướng của Evil so với mục tiêu (Player)
		Vector2 direction = (target.position - transform.position).normalized;

		if (target.GetComponent<PlayerController>() != null)
		{
			// Gây sát thương tùy thuộc vào hướng của Evil
			if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
			{
				if (direction.x > 0)
				{
					// Attack về phía bên phải
					Debug.Log("Attack to the right");
				}
				else
				{
					// Attack về phía bên trái
					Debug.Log("Attack to the left");
				}
			}
			else
			{
				if (direction.y > 0)
				{
					// Attack lên phía trên
					Debug.Log("Attack upwards");
				}
				else
				{
					// Attack xuống phía dưới
					Debug.Log("Attack downwards");
				}
			}

			// Gọi Knock với sát thương và thời gian tấn công
			target.GetComponent<PlayerController>().Knock(knockTime, damage);
		}

		// Chờ cho đến khi animation tấn công hoàn tất
		yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

		animator.SetBool("attacking", false);
		ChangeState(EnemyState.idle);

		// Cooldown giữa các lần tấn công
		yield return new WaitForSeconds(attackCooldown);
		isAttacking = false;
	}

}
