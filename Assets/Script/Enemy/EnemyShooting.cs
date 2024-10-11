using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
	public GameObject bullet;
	public Transform bulletPos;
	private float timer;
	private GameObject player;
	private Animator animator;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>(); // Get the Animator component
	}

	void Update()
	{
		float distance = Vector2.Distance(transform.position, player.transform.position);

		if (distance < 7)
		{
			// Flip enemy
			if (player.transform.position.x > transform.position.x)
			{
				transform.localScale = new Vector3(1, 1, 1);
			}
			else
			{
				transform.localScale = new Vector3(-1, 1, 1);
			}

			timer += Time.deltaTime;
			if (timer > 2)
			{
				timer = 0;
				StartCoroutine(Shoot());
			}
		}
	}

	private IEnumerator Shoot()
	{
		
		// Trigger the attacking animation
		animator.SetBool("attacking", true);

		// Wait for the duration of the attack animation
		yield return new WaitForSeconds(0.1f); // Duration of your animation
		// Create the bullet at bulletPos
		//FireBullet();

		// Reset the attacking animation state after shooting
		animator.SetBool("attacking", false);
	}

	public void FireBullet()
	{
		// Create the bullet at bulletPos
		GameObject newBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
		Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();

		if (rb != null)
		{
			Vector2 shootDirection = (player.transform.position - bulletPos.position).normalized;
			rb.AddForce(shootDirection * 10f, ForceMode2D.Impulse);
		}
	}
}
