using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerPitDetection pitDetection;
    [SerializeField] Canvas menuCanvas;

    Vector2 respawnPoint;
    public bool isOnPlatform;

    void Awake()
    {
        pitDetection = GetComponentInChildren<PlayerPitDetection>();
    }

    void Start()
    {
        SpawnPlayerInRoom.Instance.RegisterPlayer(this.gameObject);
    }

    void OnEnable() 
    {
        pitDetection.OnPitDetected += HandlePit;
        PlayerInputManager.Instance.SettingsMenuToggle += ToggleSettings;
    }

    void OnDisable() 
    { 
        pitDetection.OnPitDetected -= HandlePit;
        PlayerInputManager.Instance.SettingsMenuToggle -= ToggleSettings;
    }

    void Update()
    {
        if (GameManager.Instance.GetHealth() <= 0)
        {
            menuCanvas.enabled = true;
            gameObject.SetActive(false);
        }
    }

    void ToggleSettings(bool open)
    {
        if (open)
        {
            Time.timeScale = 0;
            menuCanvas.enabled = true;
        }
        else
        {
            Time.timeScale = 1;
            menuCanvas.enabled = false;
        }
    }

    void HandlePit(Vector2 spawnPoint)
    {
        if (isOnPlatform) return;
        GameManager.Instance.Damage(10);
        transform.position = spawnPoint;
    }

    void EnterPlatform(Transform platform)
    {
        isOnPlatform = true;
        transform.SetParent(platform);
        pitDetection.gameObject.SetActive(false);
    }

    void ExitPlatform()
    {
        isOnPlatform = false;
        transform.SetParent(null);
        pitDetection.gameObject.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            MovingPlatform mp = collision.GetComponent<MovingPlatform>();
            Debug.Log("Got Moving Platform");

            mp.OnPlatformEnter += EnterPlatform;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
            MovingPlatform mp = collision.GetComponent<MovingPlatform>();

            mp.OnPlatformExit += ExitPlatform;
        }
    }
}
