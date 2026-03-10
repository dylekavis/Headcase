using UnityEngine;

public class Rock : MonoBehaviour, IThrowable
{
    public int damageAmount;
    IThrowable throwable;

    IThrowable.State state;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetState(IThrowable.State.Idle);
    }

    void Update()
    {
        if (rb.linearVelocity == Vector2.zero) SetState(IThrowable.State.Idle);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetState() == IThrowable.State.Idle) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthManager hm = collision.gameObject.GetComponent<HealthManager>();

            if (hm == null) return;

            damageAmount = (int)rb.linearVelocity.sqrMagnitude;

            hm.Damage(damageAmount);

            Debug.Log($"{collision.gameObject.name} took {damageAmount} damage. {hm.GetHealth()} remains");

            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;
            Rigidbody2D colRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (colRb == null) return;

            colRb.AddForce(dir * damageAmount, ForceMode2D.Impulse);
        }
    }

    public void SetState(IThrowable.State state)
    {
        this.state = state;
    }

    public IThrowable.State GetState()
    {
        return state;
    }
}
