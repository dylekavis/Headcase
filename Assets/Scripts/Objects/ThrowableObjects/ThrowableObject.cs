using System.Collections;
using UnityEngine;

public class ThrowableObject : MonoBehaviour, IThrowable
{
    public int damageAmount;

    [SerializeField] IThrowable.State state;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetState(IThrowable.State.Idle);
    }

    public void SetState(IThrowable.State state)
    {
        this.state = state;

        switch (this.state)
        {
            case IThrowable.State.Idle:
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Static;
                break;
            case IThrowable.State.Thrown:
                rb.bodyType = RigidbodyType2D.Dynamic;
                StartCoroutine(CheckStopped());
                break;
        } 
    }

    IEnumerator CheckStopped()
    {
        yield return new WaitUntil(() => rb.IsSleeping());

        SetState(IThrowable.State.Idle);
    }

    public IThrowable.State GetState()
    {
        return state;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == IThrowable.State.Idle) return;

        if (!collision.gameObject.CompareTag("NonPickUpEnemy")) return;
        if (!collision.gameObject.CompareTag("PickUpEnemy")) return;

            HealthManager hm = collision.gameObject.GetComponent<HealthManager>();

            if (hm == null) return;

            damageAmount = (int)rb.linearVelocity.sqrMagnitude;

            hm.Damage(damageAmount);

            Debug.Log($"{collision.gameObject.name} took {damageAmount} damage. {hm.GetHealth()} remains");

            Vector2 dir = (collision.gameObject.transform.position - transform.position).normalized;
            
            Rigidbody2D colRb = collision.rigidbody;
            
            if (colRb == null) return;

            colRb.AddForce(dir * damageAmount, ForceMode2D.Impulse);

            SetState(IThrowable.State.Idle);
    }

}
