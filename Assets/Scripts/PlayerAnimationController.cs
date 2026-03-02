using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Camera mainCam;

    void OnEnable()
    {
        PlayerInputManager.Instance.OnMove += HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled += CancelMovement;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMove -= HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled -= CancelMovement;
    }

    public void HandleMovement(Vector2 moveVector)
    {
        anim.SetBool("isWalking", true);

        anim.SetFloat("AnimMoveX", moveVector.x);
        anim.SetFloat("AnimMoveY", moveVector.y);
    }

    public void CancelMovement()
    {
        anim.SetBool("isWalking", false);

        anim.SetFloat("IdleX", anim.GetFloat("AnimMoveX"));
        anim.SetFloat("IdleY", anim.GetFloat("AnimMoveY"));
    }
} 
