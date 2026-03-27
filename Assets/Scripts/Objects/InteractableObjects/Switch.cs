using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum SwitchState
{
    Base,
    Sentry,
    Active
}

public class Switch : MonoBehaviour
{
    [SerializeField] float animTime = 0.2f;

    [SerializeField] Light2D lighting;
    [SerializeField] SwitchState state;
    [SerializeField] Animator anim;
    [SerializeField] SwitchManager switchManager;
 
    void Start()
    {
        lighting.enabled = false;
    }
    
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
        if (collision.gameObject.CompareTag("NonPickUpEnemy")) return;
        if (collision.gameObject.CompareTag("PickUpEnemy")) return;
        if (collision.gameObject.CompareTag("DetectionRadius")) return;
        
        Debug.Log($"{collision.gameObject.name} has collided with {name}");
        if (state == SwitchState.Base)
        {
            SetState(SwitchState.Sentry);
            switchManager.RegisterSentry();
        }
    }

    IEnumerator SentryBuffer()
    {   
        lighting.enabled = true;
        lighting.color = Color.orange;

        yield return new WaitForSeconds(animTime);

        anim.SetBool("isSentry", true);

        yield break;
    }

    IEnumerator ActiveBuffer()
    {   
        lighting.enabled = true;
        lighting.color = Color.limeGreen;

        yield return new WaitForSeconds(animTime);

        anim.SetBool("isActive", true);

        yield break;
    }

    IEnumerator BaseBuffer()
    {
        lighting.enabled = false;

        yield return new WaitForSeconds(animTime);

        anim.SetBool("isSentry", false);
        anim.SetBool("isActive", false);

        yield break;
    }

    public bool IsSentry => state == SwitchState.Sentry;

}
