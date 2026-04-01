using System.Collections;
using UnityEngine;

public class BreakableCrate : MonoBehaviour
{
    [SerializeField] SpriteRenderer boxSprite;
    [SerializeField] ParticleSystem particle;
    [SerializeField] float disableTime = 0.25f;
    [SerializeField] float boxBreakMaginitude = 0.2f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ThrowableObject"))
        {
            if (collision.rigidbody.linearVelocity.sqrMagnitude > boxBreakMaginitude)
            {
                boxSprite.enabled = false;
                particle.Play();

                StartCoroutine(DisableRoutine());
            }
        }
    }

    IEnumerator DisableRoutine()
    {
        yield return new WaitForSeconds(disableTime);

        gameObject.SetActive(false);
    }
}
