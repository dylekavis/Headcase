using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform headTransform;

    [SerializeField] float moveSpeed;

    void Start()
    {
        playerTransform = SpawnPlayerInRoom.Instance.GetPlayer().transform;
        headTransform = SpawnPlayerInRoom.Instance.GetHead().transform;
    }

    void LateUpdate()
    {
        Vector3 midpoint = (playerTransform.position + headTransform.position) / 2f;
        transform.position = Vector2.MoveTowards(transform.position, midpoint, moveSpeed * Time.deltaTime);
    }
}
