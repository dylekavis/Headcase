using UnityEngine;

public class BombSpiderAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;

    [SerializeField] BombSpiderController controller;
    [SerializeField] EnemyPitDetection pitDetection;
    [SerializeField] HealthManager hm;

    void Start()
    {
        controller = GetComponent<BombSpiderController>();    
    }

    void OnEnable()
    {
        controller.OnChaseStart += HandleMove;
        controller.OnChaseEnd += CancelMove;

        controller.OnAttackStart += HandleAttack;
        controller.OnAttackEnd += CancelAttack;

        pitDetection.OnPitDetected += HandlePit;
        pitDetection.OnPitUndetected -= EndPitAnim;   
    }

    void OnDisable()
    {
        controller.OnChaseStart -= HandleMove;
        controller.OnChaseEnd -= CancelMove;

        controller.OnAttackStart -= HandleAttack;
        controller.OnAttackEnd -= CancelAttack;

        pitDetection.OnPitDetected -= HandlePit;
        pitDetection.OnPitUndetected -= EndPitAnim;  
    }

    void HandleMove(Transform target)
    {
        anim.SetInteger("SwitchState", 1);
    }

    void CancelMove()
    {
        HandleIdle();
    }

    void HandleIdle()
    {
        anim.SetInteger("SwitchState", 0);
    }

    void HandleAttack()
    {
        anim.SetInteger("SwitchState", 2);
    }

    void CancelAttack()
    {
        HandleIdle();
    }

    void HandlePit()
    {
        anim.SetInteger("SwitchState", 3);
    }

    void EndPitAnim()
    {
        HandleIdle();
    }
}
