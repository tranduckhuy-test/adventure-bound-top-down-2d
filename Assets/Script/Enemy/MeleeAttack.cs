using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
	[SerializeField] private float thrust;
	[SerializeField] private float nockTime;
	[SerializeField] private float damage;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
		{
			if (other.TryGetComponent<Rigidbody2D>(out var hit))
			{
				// Tính toán hướng tấn công dựa trên hướng của nhân vật hiện tại (Evil hoặc Player)
				var difference = hit.transform.position - transform.position;
				difference = difference.normalized * thrust;

				// Tính toán hướng lực để đảm bảo tấn công đúng hướng
				Vector2 forceDirection = (other.transform.position - transform.position).normalized;
				hit.AddForce(forceDirection * thrust, ForceMode2D.Impulse);

				// Nếu là Player, gọi Knock để gây sát thương
				if (other.TryGetComponent<PlayerController>(out var playerScript))
				{
					playerScript.currentState = PlayerState.stagger;
					playerScript.Knock(nockTime, damage);
				}
			}
		}
	}

}
