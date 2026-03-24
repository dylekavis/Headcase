using UnityEngine;

public class CameraRegistrationWithPlayerInput : MonoBehaviour
{
    void Awake()
    {
        PlayerInputManager.Instance.RegisterCamera(GetComponent<Camera>());
    }
}
