using System;
using UnityEngine;

public class HeadCollection : MonoBehaviour
{
    public event Action<GameObject> OnHeadCollect;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Throwable"))
        {
            Debug.Log($"head collided with {collision.gameObject.name}");
            OnHeadCollect?.Invoke(collision.gameObject);
        }
    }
}
