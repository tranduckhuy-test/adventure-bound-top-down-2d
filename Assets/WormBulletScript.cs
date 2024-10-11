using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBulletScript : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    public float force;
    private float timer;
	[SerializeField] private float thrust;
	[SerializeField] private float nockTime;
	[SerializeField] private float damage;
	private Animator animator;
	private bool hasDamaged = false;

	// Start is called before the first frame update
	void Start()
    {
		animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2 (direction.x, direction.y).normalized * force;

        float rot = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 180);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(gameObject);
        }
    }
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (hasDamaged) return;
		if (other.gameObject.CompareTag("Breakable") && this.gameObject.CompareTag("Player"))
		{
		}

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
					animator.SetTrigger("explosion");
					hasDamaged = true;
					StartCoroutine(Explosion());
				}
			}
		}
	}

	protected IEnumerator Explosion()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}
