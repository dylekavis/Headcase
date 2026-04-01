using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform headTransform;

    [SerializeField] float moveSpeed = 0.15f;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        playerTransform = SpawnPlayerInRoom.Instance.GetPlayer().transform;
        headTransform = playerTransform.GetComponentInChildren<HeadController>().transform;
    }

    void LateUpdate()
    {
        Vector3 midpoint = (playerTransform.position + headTransform.position) / 2f;
        transform.position = Vector3.SmoothDamp(transform.position, 
        midpoint, 
        ref velocity,
        moveSpeed);
    }
}
