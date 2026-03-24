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
        hm.Damage(10);
        Debug.Log($"{name} fell in the pit, took 10 points of damage. {hm.GetHealth()} remains.");

        transform.position = spawnPoint;
    }
}
