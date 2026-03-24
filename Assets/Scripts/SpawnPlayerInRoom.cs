using UnityEngine;

public class SpawnPlayerInRoom : MonoBehaviour
{
    public static SpawnPlayerInRoom Instance;
    [SerializeField] GameObject playerObject;

    GameObject headObject;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void OnEnable()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerObject.transform.position = this.transform.position;
            RegisterPlayer(playerObject);

            HeadController hc = playerObject.GetComponentInChildren<HeadController>(true);
            if (hc != null) RegisterHead(hc.gameObject);
        }
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
