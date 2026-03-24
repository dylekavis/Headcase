using UnityEngine;

[RequireComponent(typeof(HeadRotation))]
public class HeadAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] HeadController hc;

    HeadRotation hr;

    void OnEnable()
    {
        hr = GetComponent<HeadRotation>();
        hr.OnHeadRotate += HandleRotation;

        hc.OnPitFall += HandlePit;
        hc.OnPitCancel += CancelPit;
    }

    void OnDisable()
    {
        hr.OnHeadRotate -= HandleRotation;

        hc.OnPitFall -= HandlePit;
        hc.OnPitCancel -= CancelPit;
    }

    void HandleRotation(Vector2 rotateVector)
    {
        anim.SetFloat("DirectionX", rotateVector.x);
        anim.SetFloat("DirectionY", rotateVector.y);
    }

    void HandlePit()
    {
        anim.SetInteger("SwitchState", 1);
    }

    void CancelPit()
    {
        anim.SetInteger("SwitchState", 0);
    }
}
