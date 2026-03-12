using System.Collections;
using UnityEngine;

public enum SwitchState
{
    Base,
    Sentry,
    Active
}

public class Switch : MonoBehaviour
{
    [SerializeField] float animTime = 0.2f;

    [SerializeField] SwitchState state;
    [SerializeField] Animator anim;

    public void SetState(SwitchState newState)
    {
        state = newState;

        anim.SetInteger("SwitchState", (int)newState);

        StopAllCoroutines();

        switch (state)
        {
            case SwitchState.Base:
                StartCoroutine(BaseBuffer());
                break;
            case SwitchState.Sentry:
                StartCoroutine(SentryBuffer());
                break;
            case SwitchState.Active:
                StartCoroutine(ActiveBuffer());
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) return;
        if (collision.gameObject.CompareTag("Enemy")) return;
        
        Debug.Log($"{collision.gameObject.name} has collided with {name}");
        if (state == SwitchState.Base)
        {
            SetState(SwitchState.Sentry);
            SwitchManager.Instance.RegisterSentry();
        }
    }

    IEnumerator SentryBuffer()
    {
        yield return new WaitForSeconds(animTime);

        anim.SetBool("isSentry", true);

        yield break;
    }

    IEnumerator ActiveBuffer()
    {
        yield return new WaitForSeconds(animTime);

        anim.SetBool("isActive", true);

        yield break;
    }

    IEnumerator BaseBuffer()
    {
        yield return new WaitForSeconds(animTime);

        anim.SetBool("isSentry", false);
        anim.SetBool("isActive", false);

        yield break;
    }

    public bool IsSentry => state == SwitchState.Sentry;

}
