using System;
using UnityEngine;

public class HeadCollection : MonoBehaviour, IThrowable
{
    public event Action<GameObject> OnHeadCollect;

    IThrowable throwable;
    IThrowable.State state;

    public IThrowable.State GetState()
    {
        return state;
    }

    public void SetState(IThrowable.State state)
    {
        this.state = state;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Throwable") && !collision.gameObject.CompareTag("PickUpEnemy")) return;

        Debug.Log($"head collided with {collision.gameObject.name}");
        OnHeadCollect?.Invoke(collision.gameObject);
    }
}
