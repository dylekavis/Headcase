using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] int damageAmount;

    void OnTriggerEnter2D(Collider2D collision)
    {
        HealthManager healthManager = collision.GetComponent<HealthManager>();

        if (healthManager != null)
        {
            Debug.Log($"{collision.gameObject.name} was damaged for {damageAmount} hitpoints. {healthManager.GetHealth()} amount remaining!");
            healthManager.Damage(damageAmount);

            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;

            collision.attachedRigidbody.AddForce(dir * damageAmount, ForceMode2D.Impulse);
        }
    }
}
