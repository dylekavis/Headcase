using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BombSpiderMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] ThrowableEnemy throwable;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] stepClips;

    Transform targetToChase;

    Rigidbody2D rb;
    BombSpiderController controller;

    float variableMoveSpeed;

    bool isThrown => throwable.GetState() == IThrowable.State.Thrown;
    bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<BombSpiderController>();
        variableMoveSpeed = moveSpeed;
    }

    void OnEnable()
    {
        controller.OnChaseStart += HandleChase;
        controller.OnChaseEnd += CancelChase;
        throwable.OnThrown += CancelChase;
    }

    void OnDisable()
    {
        controller.OnChaseStart -= HandleChase;
        controller.OnChaseEnd -= CancelChase;
        throwable.OnThrown -= CancelChase;
    }

    void FixedUpdate()
    {
        if (isThrown) return;

        variableMoveSpeed = moveSpeed;

        if (targetToChase != null)
        {
            float distance = Vector2.Distance(transform.position, targetToChase.position);

            if (distance > controller.GetAttackDistance())
            {
                isMoving = true;
                Vector2 moveDir = Vector2.MoveTowards(rb.position, targetToChase.position, moveSpeed * Time.fixedDeltaTime);

                rb.MovePosition(moveDir);

                StartCoroutine(StepSFXRoutine());
            }
            
            if (distance < controller.GetAttackDistance())
            {
                CancelChase();
            }
        }
    }

    void HandleChase(Transform target)
    {
        targetToChase = target;
    }

    void CancelChase()
    {
        targetToChase = null;
        variableMoveSpeed = 0;
        isMoving = false;
    }

    IEnumerator StepSFXRoutine()
    {
        while (isMoving)
        {
            if (audioSource.isPlaying) yield break;
            
            int randomStep = Random.Range(0, stepClips.Length);
            float randomPitch = Random.Range(0.9f, 1.1f);

            for (int i = 0; i < stepClips.Length; i++)
            {
                if (randomStep == i)
                {
                    audioSource.pitch = randomPitch;
                    audioSource.PlayOneShot(stepClips[i]);
                    yield return new WaitUntil(() => !audioSource.isPlaying);
                }
            }
            
            yield return new WaitForSeconds(0.1f);
        }
    }
}