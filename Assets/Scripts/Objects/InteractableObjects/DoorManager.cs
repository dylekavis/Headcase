using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    [SerializeField] Door[] eligableDoors;
    [SerializeField] SwitchManager switchManager;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void OnEnable()
    {
        switchManager.OnAllActivate += OpenEligableDoors;
        switchManager.OnAllDeactivate += CloseEligableDoors;
    }

    void OnDisable()
    {
        switchManager.OnAllActivate -= OpenEligableDoors;
        switchManager.OnAllDeactivate -= CloseEligableDoors;
    }

    void OpenEligableDoors()
    {
        foreach (var door in eligableDoors)
        {
            door.SetOpenState(true);
        }   
    }

    void CloseEligableDoors()
    {
        foreach (var door in eligableDoors)
        {
            door.SetOpenState(false);
        }
    }
}
