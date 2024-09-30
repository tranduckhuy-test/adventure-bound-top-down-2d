using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float thrust;
    [SerializeField] private float nockTime;
    [SerializeField] private int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
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

                if (other.TryGetComponent<Enemy>(out var enemyScript) && this.gameObject.CompareTag("Player"))
                {
                    enemyScript.currentState = EnemyState.stagger;
                    enemyScript.Knock(hit, nockTime);
                    enemyScript.TakeDamage(damage);
                }
                if (other.TryGetComponent<PlayerController>(out var playerScript))
                {
                    Debug.Log("Player Hit");
                    playerScript.currentState = PlayerState.stagger;
                    playerScript.Knock(nockTime);
                    playerScript.TakeDame(damage);
                }
            }
        }
    }
}
