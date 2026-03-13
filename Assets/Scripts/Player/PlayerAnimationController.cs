using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator fullBodyAnim;
    [SerializeField] Animator headlessBodyAnim;
    [SerializeField] Camera mainCam;

    bool isWalking = false;

    PlayerThrowing throwing;

    bool hasHead => throwing.HasHead();

    void Awake()
    {
        throwing = GetComponent<PlayerThrowing>();
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnMove += HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled += CancelMovement;
        PlayerInputManager.Instance.OnPlayerLook += HandleLook;
        PlayerInputManager.Instance.OnLookCancelled += CancelLook;

        throwing.OnHeadLoss += HandleHeadLoss;
        throwing.OnHeadPickup += HandleHeadPickup;
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMove -= HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled -= CancelMovement;
        PlayerInputManager.Instance.OnPlayerLook -= HandleLook;
        PlayerInputManager.Instance.OnLookCancelled -= CancelLook;

        throwing.OnHeadLoss -= HandleHeadLoss;
        throwing.OnHeadPickup -= HandleHeadPickup;
    }

    void HandleHeadLoss()
    {
        fullBodyAnim.gameObject.SetActive(false);
        headlessBodyAnim.gameObject.SetActive(true);
    }

    void HandleHeadPickup()
    {
        fullBodyAnim.gameObject.SetActive(true);
        headlessBodyAnim.gameObject.SetActive(false);
    }

    public void HandleMovement(Vector2 moveVector)
    {
        isWalking = true;

        if (hasHead)
        {
            fullBodyAnim.SetBool("isWalking", true);

            fullBodyAnim.SetFloat("AnimMoveX", moveVector.x);
            fullBodyAnim.SetFloat("AnimMoveY", moveVector.y);
        }
        else
        {
            headlessBodyAnim.SetBool("isWalking", true);

            headlessBodyAnim.SetFloat("AnimMoveX", moveVector.x);
            headlessBodyAnim.SetFloat("AnimMoveY", moveVector.y);
        }
    }

    public void CancelMovement()
    {
        isWalking = false;

        if (hasHead)
        {
            fullBodyAnim.SetBool("isWalking", false);

            fullBodyAnim.SetFloat("LastLookX", fullBodyAnim.GetFloat("AnimMoveX"));
            fullBodyAnim.SetFloat("LastLookY", fullBodyAnim.GetFloat("AnimMoveY"));
        }
        else
        {
            headlessBodyAnim.SetBool("isWalking", false);

            headlessBodyAnim.SetFloat("LastLookX", headlessBodyAnim.GetFloat("AnimMoveX"));
            headlessBodyAnim.SetFloat("LastLookY", headlessBodyAnim.GetFloat("AnimMoveY"));
        }
    }

    public void HandleLook(Vector2 lookDir)
    {
        if (hasHead)
        {
            fullBodyAnim.SetFloat("IdleX", lookDir.x);
            fullBodyAnim.SetFloat("IdleY", lookDir.y);
        }
        else
        {
            headlessBodyAnim.SetFloat("IdleX", lookDir.x);
            headlessBodyAnim.SetFloat("IdleY", lookDir.y);
        }
    }

    public void CancelLook()
    {
        if (hasHead)
        {
            fullBodyAnim.SetFloat("LastLookX", fullBodyAnim.GetFloat("IdleX"));
            fullBodyAnim.SetFloat("LastLookY", fullBodyAnim.GetFloat("IdleY"));
        }
        else
        {
            headlessBodyAnim.SetFloat("LastLookX", headlessBodyAnim.GetFloat("IdleX"));
            headlessBodyAnim.SetFloat("LastLookY", headlessBodyAnim.GetFloat("IdleY"));
        }

    }
} 
