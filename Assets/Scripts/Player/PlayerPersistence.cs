using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    public static PlayerPersistence instance;
    void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
