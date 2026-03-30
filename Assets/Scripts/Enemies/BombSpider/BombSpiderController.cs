using System;
using System.Collections;
using UnityEngine;

public enum BombSpiderMoveState
{
    Idle,
    Chasing,
    Attack    
}

public class BombSpiderController : MonoBehaviour
{
    public event Action<Transform> OnChaseStart;
    public event Action OnChaseEnd;
    
    public event Action OnAttackStart;
    public event Action OnAttackEnd;


    [Header("Attack Variables")]
    [SerializeField] Collider2D hitbox;
    [SerializeField] float minDistanceToAttack = 1.5f;
    [SerializeField] float attackAttemptTime = 0.12f;
    [SerializeField] float attackAnimatonTime = 0.3f;
    [SerializeField] float attackCooldownTime = 2.5f;

    [Header("References")]
    [SerializeField] BombSpiderMoveState state;
    [SerializeField] DetectionRadius detectionRadius;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] ParticleSystem ps;
    [SerializeField] EnemyPitDetection pitDetection;
    [SerializeField] ThrowableEnemy throwable;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip explosionClip;
    [SerializeField] HealthManager hm;

    EnemySpawnPool spawnPool;

    bool isThrown => throwable.GetState() == IThrowable.State.Thrown;

    public bool isOnPlatform;

    //Unsearialized references
    GameObject targetToFollow;
    bool canAttack = true;

    public float GetAttackDistance() => minDistanceToAttack;

    public void RegisterSpawnPool(EnemySpawnPool pool)
    {
        spawnPool = pool;
    }
    
    void OnEnable()
    {
        detectionRadius.OnPlayerDetected += HandleDetectTarget;
        detectionRadius.OnPlayerUndetected += CancelDetectTarget;

        pitDetection.OnPitDetected += HandlePitFall;

        hm.OnDamageTaken += HandleDamage;
    }

    void OnDisable()
    {
        detectionRadius.OnPlayerDetected -= HandleDetectTarget;
        detectionRadius.OnPlayerUndetected -= CancelDetectTarget;

        pitDetection.OnPitDetected -= HandlePitFall;
        
        hm.OnDamageTaken -= HandleDamage;
    }

#region Target Detection
    void HandleDetectTarget(GameObject target)
    {
        targetToFollow = target;

        state = BombSpiderMoveState.Chasing;

        OnChaseStart?.Invoke(targetToFollow.transform);

        float distance = Vector2.Distance(transform.position, targetToFollow.transform.position);

        if (distance <= minDistanceToAttack)
        {
            state = BombSpiderMoveState.Attack;
            OnAttackStart?.Invoke();

            StartCoroutine(AttackRoutine());
        }
    }

    void CancelDetectTarget()
    {
        state = BombSpiderMoveState.Idle;

        targetToFollow = null;

        OnChaseEnd?.Invoke();
    }

#endregion

#region Attacking

    IEnumerator AttackRoutine()
    {
        if (isThrown) yield break;

        if (targetToFollow == null) yield break;

        float distance = Vector2.Distance(transform.position, targetToFollow.transform.position);

        if (distance > minDistanceToAttack)
        {
            OnAttackEnd?.Invoke();
            yield break;
        }

        yield return new WaitForSeconds(attackAttemptTime);

        OnAttackStart?.Invoke();

        canAttack = false;

        yield return new WaitForSeconds(attackAnimatonTime);

        OnAttackEnd?.Invoke();

        ps.gameObject.SetActive(true);
        ps.Play();

        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(explosionClip);
        
        yield return new WaitForSeconds(0.2f);

        hitbox.enabled = true;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);

        if (spawnPool != null)
            spawnPool.ActiveCount -= 1;
    }

    #endregion

    void HandlePitFall()
    {
        gameObject.SetActive(false);
        spawnPool.ActiveCount -= 1;
    }
    
    void HandleDamage(int damageAmount)
    {
        StartCoroutine(DamageRoutine());
    }

    IEnumerator DamageRoutine()
    {
        OnAttackStart?.Invoke();

        canAttack = false;

        yield return new WaitForSeconds(attackAnimatonTime);

        OnAttackEnd?.Invoke();

        ps.gameObject.SetActive(true);
        ps.Play();

        audioSource.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(explosionClip);
        
        yield return new WaitForSeconds(0.2f);

        hitbox.enabled = true;

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
        spawnPool.ActiveCount -= 1;
    }
}