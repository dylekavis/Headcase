using UnityEngine;

public class SpawnPlayerInRoom : MonoBehaviour
{
    public static SpawnPlayerInRoom Instance;
    [SerializeField] GameObject playerObject;
    [SerializeField] GameObject headObject;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void OnEnable()
    {
        GameObject player = Instantiate(playerObject, transform.position, Quaternion.identity);
        GameManager.Instance.Heal(GameManager.Instance.GetMaxHealth() - GameManager.Instance.GetHealth());
        headObject = player.GetComponentInChildren<HeadController>().gameObject;
    }

    public void RegisterPlayer(GameObject player)
    {
        playerObject = player;
    }

    public void RegisterHead(GameObject head)
    {
        headObject = head;
    }

    public GameObject GetPlayer() => playerObject;
    public GameObject GetHead() => headObject;


}
