using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerPitDetection pitDetection;
    [SerializeField] float coyoteTime;
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
        Debug.Log($"{name} fell in the pit, took 10 points of damage. {hm.GetHealth()} remains.");

        transform.position = spawnPoint;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            pitDetection.gameObject.SetActive(false);
            Debug.Log("On platform");
            this.transform.SetParent(collision.gameObject.transform);
            isOnPlatform = true;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        isOnPlatform = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            Debug.Log("Off platform");
            this.transform.SetParent(null);
            pitDetection.gameObject.SetActive(true);
            isOnPlatform = false;
        }
    }
}
