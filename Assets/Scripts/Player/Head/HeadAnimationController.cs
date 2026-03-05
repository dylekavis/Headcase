using UnityEngine;

[RequireComponent(typeof(HeadRotation))]
public class HeadAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;

    HeadRotation hr;

    void OnEnable()
    {
        hr = GetComponent<HeadRotation>();
        hr.OnHeadRotate += HandleRotation;
    }

    void HandleRotation(Vector2 rotateVector)
    {
        anim.SetFloat("DirectionX", rotateVector.x);
        anim.SetFloat("DirectionY", rotateVector.y);
    }
}
