using UnityEngine;

public enum DoorDirection
{
    North,
    South,
    East,
    West
}

public class Door : MonoBehaviour
{
    [SerializeField] DoorDirection doorDirection;
    [SerializeField] Collider2D doorCollider;
    [SerializeField] bool isOpen;
    [SerializeField] Animator anim;

    Vector2 direction;

    void Update()
    {
        DoorState();
    }

    public void SetOpenState(bool open)
    {
        isOpen = open;
        doorCollider.enabled = open;
    }

    void DoorState()
    {
        switch (doorDirection)
        {
            case DoorDirection.North:
                direction = new Vector2(0, 1);
                anim.SetFloat("DirX", direction.x);
                anim.SetFloat("DirY", direction.y);
                break;
            case DoorDirection.South:
                direction = new Vector2(0, -1);
                anim.SetFloat("DirX", direction.x);
                anim.SetFloat("DirY", direction.y);
                break;
            case DoorDirection.East:
                direction = new Vector2(1, 0);
                anim.SetFloat("DirX", direction.x);
                anim.SetFloat("DirY", direction.y);
                break;
            case DoorDirection.West:
                direction = new Vector2(-1, 0);
                anim.SetFloat("DirX", direction.x);
                anim.SetFloat("DirY", direction.y);
                break;
        }

        if (isOpen)
        {
            anim.SetBool("isOpen", true);
        }
        else
        {
            anim.SetBool("isOpen", false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            LoadScene.Instance.LoadNextScene();
        }
    }
}
