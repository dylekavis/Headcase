using System.Collections;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] Animator fullBodyAnim;
    [SerializeField] Animator headlessBodyAnim;
    [SerializeField] Camera mainCam;

    [SerializeField] PlayerPitDetection pitDetection;
    [SerializeField] HealthManager hm;

    bool isWalking = false;

    PlayerThrowing throwing;
    

    bool hasHead => throwing.HasHead();

    void Awake()
    {
        throwing = GetComponent<PlayerThrowing>();
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
        hm = GetComponent<HealthManager>();
    }

    void OnEnable()
    {
        PlayerInputManager.Instance.OnMove += HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled += CancelMovement;
        PlayerInputManager.Instance.OnPlayerLook += HandleLook;
        PlayerInputManager.Instance.OnLookCancelled += CancelLook;

        throwing.OnHeadLoss += HandleHeadLoss;
        throwing.OnHeadPickup += HandleHeadPickup;

        pitDetection.OnPitDetected += HandlePit;

        hm.OnDamageTaken += HandleDamage;
        
    }

    void OnDisable()
    {
        PlayerInputManager.Instance.OnMove -= HandleMovement;
        PlayerInputManager.Instance.OnMoveCancelled -= CancelMovement;
        PlayerInputManager.Instance.OnPlayerLook -= HandleLook;
        PlayerInputManager.Instance.OnLookCancelled -= CancelLook;

        throwing.OnHeadLoss -= HandleHeadLoss;
        throwing.OnHeadPickup -= HandleHeadPickup;

        pitDetection.OnPitDetected -= HandlePit;

        hm.OnDamageTaken -= HandleDamage;
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

    public void HandlePit(Vector2 pit)
    {
        if (hasHead)
        {
            fullBodyAnim.SetBool("hasFallen", true);
            StartCoroutine(RespawnRoutine());
        }
        else
        {
            headlessBodyAnim.SetBool("hasFallen", true);
            StartCoroutine(RespawnRoutine());
        }
    }

    public void HandleDamage()
    {
        StartCoroutine(RespawnRoutine());
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(0.4f);

        if (hasHead)
        {
            fullBodyAnim.gameObject.SetActive(false);

            fullBodyAnim.SetBool("hasFallen", false);

            int blinkAmount = 4;
            int blinks = 0;

            while (blinks < blinkAmount)
            {
                fullBodyAnim.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                fullBodyAnim.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                blinks++;
            }

            fullBodyAnim.gameObject.SetActive(true);

            yield break;
        }
        else
        {
            headlessBodyAnim.gameObject.SetActive(false);

            headlessBodyAnim.SetBool("hasFallen", false);

            int blinkAmount = 4;
            int blinks = 0;

            while (blinks < blinkAmount)
            {
                headlessBodyAnim.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
                headlessBodyAnim.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.1f);
                blinks++;
            }

            headlessBodyAnim.gameObject.SetActive(true);

            yield break;
        }
    }
} 
