using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerPitDetection pitDetection;
    HealthManager hm;

    Vector2 respawnPoint;

    void Awake()
    {
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
        hm = GetComponent<HealthManager>();
    }

    void OnEnable()
    {
        pitDetection.OnPitDetected += HandlePit;
        pitDetection.OnRespawnCreated += HandleRespawn;
    }

    void OnDisable()
    {
        pitDetection.OnPitDetected -= HandlePit;
        pitDetection.OnRespawnCreated -= HandleRespawn;
    }

    void HandlePit()
    {
        hm.Damage(10);
        Debug.Log($"{name} fell in the pit, took 10 points of damage. {hm.GetHealth()} remains.");
    }

    void HandleRespawn(Vector2 spawnPoint)
    {
        transform.position = spawnPoint;
    }
}
