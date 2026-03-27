using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerPitDetection pitDetection;
    HealthManager hm;

    Vector2 respawnPoint;
    public bool isOnPlatform;

    void Awake()
    {
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
        hm = GetComponent<HealthManager>();
    }

    void Start()
    {
        SpawnPlayerInRoom.Instance.RegisterPlayer(this.gameObject);
    }

    void OnEnable() 
    {
        pitDetection.OnPitDetected += HandlePit;
    }

    void OnDisable() 
    { 
        pitDetection.OnPitDetected -= HandlePit;
    }

    void HandlePit(Vector2 spawnPoint)
    {
        if (isOnPlatform) return;
        hm.Damage(10);
        transform.position = spawnPoint;
    }

    void EnterPlatform(Transform platform)
    {
        isOnPlatform = true;
        transform.SetParent(platform);
        pitDetection.gameObject.SetActive(false);
    }

    void ExitPlatform()
    {
        isOnPlatform = false;
        transform.SetParent(null);
        pitDetection.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            MovingPlatform mp = collision.GetComponent<MovingPlatform>();
            Debug.Log("Got Moving Platform");

            mp.OnPlatformEnter += EnterPlatform;
            mp.OnPlatformExit -= ExitPlatform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            MovingPlatform mp = collision.GetComponent<MovingPlatform>();

            mp.OnPlatformExit += ExitPlatform;
            mp.OnPlatformEnter -= EnterPlatform;
        }
    }
}
