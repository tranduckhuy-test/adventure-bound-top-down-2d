using System.Collections;
using UnityEngine;

public class Evil : Enemy
{
	private float timer;
	private Vector3 fixedScale; // Lưu scale cố định (0.5)
	[SerializeField] private float thrust;
	[SerializeField] private float nockTime;
	[SerializeField] private float damage;

	private void Awake()
	{
		health = maxHealth.initialValue;
		myRigidbody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		target = GameObject.FindWithTag("Player").transform;
		homePosition = new GameObject("HomePosition").transform;
		homePosition.position = transform.position;

		// Thiết lập scale cố định là 0.5
		fixedScale = new Vector3(0.5f, 0.5f, 0.5f);
		transform.localScale = fixedScale; // Đặt scale ban đầu là 0.5
	}

	private void Update()
	{
		// Tính khoảng cách giữa Evil và Player
		float distance = Vector2.Distance(transform.position, target.transform.position);

		if (distance < this.chaseRadius)
		{
			// Flip Evil theo trục x dựa trên vị trí của Player
			

			timer += Time.deltaTime;
			if (timer > 2)
			{
				timer = 0;
				StartCoroutine(Attack());
			}
		}
	}

	// Hàm để Flip Evil hướng về phía Player
	private void FlipTowardsPlayer()
	{
		// Kiểm tra vị trí của Player so với vị trí của Evil để flip hướng
		if (target.transform.position.x > transform.position.x)
		{
			// Nếu Player ở bên phải, mặt Evil sẽ hướng về phải
			transform.localScale = new Vector3(Mathf.Abs(fixedScale.x), fixedScale.y, fixedScale.z);
		}
		else
		{
			// Nếu Player ở bên trái, mặt Evil sẽ hướng về trái
			transform.localScale = new Vector3(-Mathf.Abs(fixedScale.x), fixedScale.y, fixedScale.z);
		}
	}

	private IEnumerator Attack()
	{
		FlipTowardsPlayer();
		// Tấn công Player
		animator.SetBool("attacking", true);

		// Bật collider khi bắt đầu tấn công
		//var attackCollider = GetComponentInChildren<Collider2D>();
		//if (attackCollider != null)
		//{
		//	attackCollider.enabled = true;  // Bật collider để va chạm với Player
		//}

		yield return new WaitForSeconds(0.1f);  // Thời gian thực hiện đòn đánh

		animator.SetBool("attacking", false);

		// Tắt collider sau khi tấn công để tránh va chạm không mong muốn
		//if (attackCollider != null)
		//{
		//	attackCollider.enabled = false;
		//}
	}


	private void FixedUpdate()
	{
		CheckDistance();
		UpdateAnimation();
	}
}
