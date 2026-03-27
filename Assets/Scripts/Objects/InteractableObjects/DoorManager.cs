using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    [SerializeField] Door[] eligableDoors;
    [SerializeField] SwitchManager switchManager;
    [SerializeField] PressurePlate plate;

    void Awake()
    {
        if (Instance != this && Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    void OnEnable()
    {
        if (switchManager != null)
        {
            switchManager.OnAllActivate += OpenEligableDoors;
            switchManager.OnAllDeactivate += CloseEligableDoors;
        }
        
        if (plate != null)
        {
            plate.OnPlateActivate += OpenEligableDoors;
            plate.OnPlateDeactivate += CloseEligableDoors;
        }
    }

    void OnDisable()
    {
        if (switchManager != null)
        {
            switchManager.OnAllActivate -= OpenEligableDoors;
            switchManager.OnAllDeactivate -= CloseEligableDoors;
        }
        
        if (plate != null)
        {
            plate.OnPlateActivate -= OpenEligableDoors;
            plate.OnPlateDeactivate -= CloseEligableDoors;
        }
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
