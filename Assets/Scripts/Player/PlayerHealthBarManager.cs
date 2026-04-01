using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarManager : MonoBehaviour
{
    [SerializeField] Canvas healthBarCanvas;
    [SerializeField] Image healthBar;
    [SerializeField] float disableTime;

    void OnEnable()
    {
        healthBarCanvas.worldCamera = Camera.main;

        GameManager.Instance.OnDamage += HandleDamage;
    }

    void OnDisable()
    {
        GameManager.Instance.OnDamage -= HandleDamage;
    }

    void HandleDamage(int damageAmount)
    {
        healthBar.fillAmount -= damageAmount / 100f;
        StartCoroutine(DamageRoutine());
    }

    IEnumerator DamageRoutine()
    {
        healthBarCanvas.enabled = true;

        yield return new WaitForSeconds(disableTime);

        healthBarCanvas.enabled = false;
    }
} 
