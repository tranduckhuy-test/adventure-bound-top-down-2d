using UnityEngine;

public class EvilAttack : MonoBehaviour
{
	[SerializeField] private float thrust;
	[SerializeField] private float nockTime;
	[SerializeField] private float damage;
	private bool hasDamaged = false;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (hasDamaged) return;  // Kiểm tra nếu đã gây sát thương thì không lặp lại

		if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
		{
			if (other.TryGetComponent<Rigidbody2D>(out var hit))
			{
				var difference = hit.transform.position - transform.position;
				difference = difference.normalized * thrust;
				hit.AddForce(difference, ForceMode2D.Impulse);

				if (other.TryGetComponent<PlayerController>(out var playerScript))
				{
					playerScript.currentState = PlayerState.stagger;
					playerScript.Knock(nockTime, damage);
					hasDamaged = true;  // Đánh dấu đã gây sát thương
				}
			}
		}
	}

	private void OnEnable()
	{
		// Reset trạng thái khi collider được kích hoạt
		hasDamaged = false;
	}
}